using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ekzamen01
{
    public enum Language
    {
        Unknown = 0,
        Russian = 1,
        English = 2
    }

    public class Dict
    {
        public Language LanguageFrom;
        public Language LanguageTo;
        public NameValueCollection NameValueColl;

        public Dict(Language from, Language to)
        {
            try
            {
                if (from == to)
                    throw new Exception("Язык перевода должен быть отличный от первоначального!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Menu.ShowMyMenu(this);
            }
            LanguageFrom = from;
            LanguageTo = to;
            NameValueColl = new NameValueCollection();
            ReadDictionaryFromFile(LanguageFrom, LanguageTo);
        }

        private Language CheckSymbolLanguage(int symbol)
        {
            if ((symbol >= 1040 && symbol <= 1103) || symbol == 1025 || symbol == 1105)
                return Language.Russian;
            else if ((symbol >= 65 && symbol <= 90) || symbol >= 97 && symbol <= 122)
                return Language.English;
            return Language.Unknown;
        }

        private Language CheckWordLanguage(string word)
        {
            Language current = CheckSymbolLanguage(word[0]);
            if (current == Language.Unknown)
            {
                Console.WriteLine("В слове присутствует недопустимый символ или язык нераспознан!");
                Console.ReadKey();
                return Language.Unknown;
            }

            Language tmp;
            for (int i = 1; i < word.Length; i++)
            {
                tmp = CheckSymbolLanguage(word[i]);
                if (current != tmp)
                {
                    Console.WriteLine("В слове присутствует недопустимый символ или язык нераспознан!");
                    Console.ReadKey();
                    return Language.Unknown;
                }
            }
            return current;
        }

        private bool IsCorrectLanguage(string word, string translation)
        {
            if ((CheckWordLanguage(word) == LanguageFrom) && (CheckWordLanguage(translation) == LanguageTo))
                return true;
            return false;
        }

        public void Add(string word, string translation)
        {
            bool isCorrectToAdd = true;
            try
            {
                if (word.Length == 0 || translation.Length == 0)
                {
                    isCorrectToAdd = false;
                    throw new Exception("В слове и переводе должны быть символы!");
                }
                if (IsCorrectLanguage(word, translation) == false)
                {
                    isCorrectToAdd = false;
                    throw new Exception("Несоответствие языка слова и его перевода текущему словарю!");
                }
                string[] arrValues = NameValueColl.GetValues(word);
                if (arrValues != null)
                {
                    for (int i = 0; i < arrValues.Length; i++)
                    {
                        if (arrValues[i].ToLower() == translation.ToLower())
                        {
                            isCorrectToAdd = false;
                            throw new Exception("Данный перевод слова уже существует!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            if (isCorrectToAdd == true)
                NameValueColl.Add(word, translation);
        }

        public void ChangeWord(string oldWord, string newWord)
        {
            string[] values = NameValueColl.GetValues(oldWord);
            if (values == null)
            {
                Console.WriteLine("Слово не найдено или переводов данного слова нет!");
                Console.ReadKey();
                return;
            }

            DeleteWord(oldWord);
            for (int i = 0; i < values.Length; i++)
                Add(newWord, values[i]);
        }

        public void ChangeTranslation(string word, string oldTranslation, string newTranslation)
        {
            DeleteTranslation(word, oldTranslation);
            Add(word, newTranslation);
        }

        public void DeleteWord(string word)
        {
            NameValueColl.Remove(word);
        }

        public void DeleteTranslation(string word, string delTranslation)
        {
            string[] values = NameValueColl.GetValues(word);

            if (values == null || values.Length == 1)
            {
                Console.WriteLine("Невозможно удалить последний вариант перевода или переводов(слова) вообще не существует!");
                Console.ReadKey();
                return;
            }
            if (values.Contains(delTranslation.ToLower()))
            {
                Console.WriteLine("Данного варианта перевода не существует!");
                Console.ReadKey();
                return;
            }

            DeleteWord(word);
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (values[i] != delTranslation)
                    NameValueColl.Add(word, values[i]);
            }
        }

        public void SearchTranslations(string word)
        {
            string[] values = NameValueColl.GetValues(word);
            if (values == null)
            {
                Console.WriteLine("Не найдено переводов данного слова!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"{word.ToUpper()} - {NameValueColl[word].ToLower()}");
            Console.ReadKey();
        }

        public void ReadDictionaryFromFile(Language from, Language to)
        {
            string path = default;
            if (from == Language.English && to == Language.Russian)
                path = "Eng-Rus.txt";
            else if (from == Language.Russian && to == Language.English)
                path = "Rus-Eng.txt";
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream, Encoding.Default))
                {
                    Regex regexFrom = new Regex(@"[A-z](\w*)");
                    Regex regexTo = new Regex(@"[А-я](\w*)");
                    if (from == Language.Russian && to == Language.English)
                    {
                        regexFrom = new Regex(@"[А-я](\w*)");
                        regexTo = new Regex(@"[A-z](\w*)");
                    }
                    while (!reader.EndOfStream)
                    {
                        string currentLine = reader.ReadLine();
                        Match matchFrom = regexFrom.Match(currentLine);
                        Match matchTo = regexTo.Match(currentLine);
                        Add(matchFrom.Value, matchTo.Value);
                    }
                }
            }
        }

        public void WriteDictionaryToFile()
        {
            string path = default;
            if (LanguageFrom == Language.English && LanguageTo == Language.Russian)
                path = "Eng-Rus.txt";
            else if (LanguageFrom == Language.Russian && LanguageTo == Language.English)
                path = "Rus-Eng.txt";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.Default))
            {
                for (int i = 0; i < NameValueColl.Count; i++)
                {
                    string[] values = NameValueColl.GetValues(i);
                    foreach (string item in values)
                        writer.WriteLine($"{NameValueColl.GetKey(i)} - {item}");
                }
            }
        }

        public void ExportWordToFile(string word)
        {
            string[] values = NameValueColl.GetValues(word);
            if (values == null)
            {
                Console.WriteLine("Не найдено переводов данного слова!");
                Console.ReadKey();
                return;
            }
            string path = "Export.txt";
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
                {
                    foreach (string item in values)
                        writer.WriteLine($"{word} - {item}");
                }
            }
        }

        public static void SearchDictionary(out bool engRus, out bool rusEng)
        {
            string path = "Eng-Rus.txt";
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
                engRus = true;
            else
                engRus = false;
            path = "Rus-Eng.txt";
            fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
                rusEng = true;
            else
                rusEng = false;
        }
    }
}
