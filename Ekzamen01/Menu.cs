using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekzamen01
{
    public class Menu
    {
        public static void ShowMyMenu(Dict dict)
        {
            bool engRus, rusEng;
            Dict.SearchDictionary(out engRus, out rusEng);
            Console.Clear();
            Console.WriteLine("СЛОВАРИ v1.0");
            Console.WriteLine("\nДля продолжения нажмите любую кнопку...");
            Console.ReadKey();

            char choice = default;
            ShowBeginMenu(ref dict, engRus, rusEng, ref choice);

            char choiceMenu;
            do
            {
                if (choice == '3')
                {
                    Console.Clear();
                    break;
                }
                Console.Clear();
                Console.WriteLine("1 - Добавить слово и перевод в словарь");
                Console.WriteLine("2 - Изменить слово с уже существующими переводами");
                Console.WriteLine("3 - Изменить один из вариантов перевода слова");
                Console.WriteLine("4 - Удалить слово со всеми переводами");
                Console.WriteLine("5 - Удалить один из переводов слова (последний перевод не удаляется)");
                Console.WriteLine("6 - Показать все переводы слова");
                Console.WriteLine("7 - Экспорт слова и его переводов в файл");
                Console.WriteLine("8 - Сохранение словаря и выход в главное меню");
                choiceMenu = Console.ReadKey().KeyChar;
                switch (choiceMenu)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine("Введите слово:");
                        string word = Console.ReadLine();
                        Console.WriteLine("Введите перевод:");
                        string translation = Console.ReadLine();
                        dict.Add(word, translation);
                        break;
                    case '2':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        string oldWord = Console.ReadLine();
                        Console.WriteLine("Введите новое слово:");
                        string newWord = Console.ReadLine();
                        dict.ChangeWord(oldWord, newWord);
                        break;
                    case '3':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        word = Console.ReadLine();
                        Console.WriteLine("Введите старый вариант перевода:");
                        string oldTranslation = Console.ReadLine();
                        Console.WriteLine("Введите новый вариант перевода:");
                        string newTranslation = Console.ReadLine();
                        dict.ChangeTranslation(word, oldTranslation, newTranslation);
                        break;
                    case '4':
                        Console.Clear();
                        Console.WriteLine("Введите удаляемое слово:");
                        word = Console.ReadLine();
                        dict.DeleteWord(word);
                        break;
                    case '5':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        word = Console.ReadLine();
                        Console.WriteLine("Введите удаляемый перевод слова:");
                        string delTranslation = Console.ReadLine();
                        dict.DeleteTranslation(word, delTranslation);
                        break;
                    case '6':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово:");
                        word = Console.ReadLine();
                        dict.SearchTranslations(word);
                        break;
                    case '7':
                        Console.Clear();
                        Console.WriteLine("Введите существующее слово для экспорта в файл:");
                        word = Console.ReadLine();
                        dict.ExportWordToFile(word);
                        break;
                    case '8':
                        dict.WriteDictionaryToFile();
                        ShowBeginMenu(ref dict, engRus, rusEng, ref choice);
                        break;
                    default:
                        continue;
                }
            }
            while (true);
        }

        public static void ShowBeginMenu(ref Dict dict, bool engRus, bool rusEng, ref char choice)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Выберите словарь для работы:");
                Console.WriteLine("1 - Англо-русский");
                Console.WriteLine("2 - Русско-английский");
                Console.WriteLine("\n3 - Выход из программы");
                choice = Console.ReadKey().KeyChar;
                switch (choice)
                {
                    case '1':
                        if (engRus == false)
                        {
                            char choiceCreate;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Словарь не найден.");
                                Console.WriteLine("1 - Создать новый словарь");
                                Console.WriteLine("2 - Вернуться назад");
                                choiceCreate = Console.ReadKey().KeyChar;
                                switch (choiceCreate)
                                {
                                    case '1':
                                        dict = new Dict(Language.English, Language.Russian);
                                        break;
                                    case '2':
                                        ShowBeginMenu(ref dict, engRus, rusEng, ref choice);
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            while (choiceCreate != '1' && choiceCreate != '2');
                        }
                        else
                        {
                            dict = new Dict(Language.English, Language.Russian);
                        }
                        break;
                    case '2':
                        if (rusEng == false)
                        {
                            char choiceCreate;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Словарь не найден.");
                                Console.WriteLine("1 - Создать новый словарь");
                                Console.WriteLine("2 - Вернуться назад");
                                choiceCreate = Console.ReadKey().KeyChar;
                                switch (choiceCreate)
                                {
                                    case '1':
                                        dict = new Dict(Language.Russian, Language.English);
                                        break;
                                    case '2':
                                        ShowBeginMenu(ref dict, engRus, rusEng, ref choice);
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            while (choiceCreate != '1' && choiceCreate != '2');
                        }
                        else
                        {
                            dict = new Dict(Language.Russian, Language.English);
                        }
                        break;
                    case '3':
                        break;
                    default:
                        continue;
                }

            }
            while (choice != '1' && choice != '2' && choice != '3');
        }
    }
}