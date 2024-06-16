using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Встановлення кодування консолі в UTF-8 для коректного відображення українських символів
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Виконання завдань
        Console.WriteLine("Виберіть завдання для виконання (1-4):");
        Console.WriteLine("Завдання 1: Знайти дати у форматі дд.мм.рррр.");
        Console.WriteLine("Завдання 2: Перевірити наявність слова у тексті");
        Console.WriteLine("Завдання 3: Вилучити слова з подвоєнням літер");
        Console.WriteLine("Завдання 4: Робота із двійковими числами (степені числа 3)");
        int task = int.Parse(Console.ReadLine());

        switch (task)
        {
            case 1:
                
                Task1();
                break;
            case 2:
                
                Task2();
                break;
            case 3:
                
                Task3();          
                break;
            case 4:
               
                Task4();
                break;
            default:
                Console.WriteLine("Невірний вибір. Виберіть завдання від 1 до 4.");
                break;
        }
    }

    static void Task1()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string inputFilePath = Path.Combine(currentDirectory, "input1.txt");
        string outputFilePath = Path.Combine(currentDirectory, "output1.txt");

        string text = File.ReadAllText(inputFilePath);
        string datePattern = @"\b(0?[1-9]|[12][0-9]|3[01])\.(0?[1-9]|1[012])\.(19[0-9]{2}|20[0-9]{2})\b";
        Regex dateRegex = new Regex(datePattern);

        MatchCollection matches = dateRegex.Matches(text);
        List<string> dates = new List<string>();

        foreach (Match match in matches)
        {
            dates.Add(match.Value);
        }

        File.WriteAllLines(outputFilePath, dates);
        Console.WriteLine($"Знайдено {dates.Count} дат.");

        Console.WriteLine("Введіть дату для заміни (дд.мм.рррр):");
        string oldDate = Console.ReadLine();

        Console.WriteLine("Введіть нову дату (дд.мм.рррр):");
        string newDate = Console.ReadLine();

        text = text.Replace(oldDate, newDate);
        File.WriteAllText(Path.Combine(currentDirectory, "updated_" + inputFilePath), text);

        Console.WriteLine("Дати було успішно замінено.");
    }


    static void Task2()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string inputFilePath = Path.Combine(currentDirectory, "input2.txt");
        string outputFilePath = Path.Combine(currentDirectory, "output2.txt");

        string text = File.ReadAllText(inputFilePath).Trim();

        Console.WriteLine("Вміст файлу:");
        Console.WriteLine(text);

        Console.WriteLine("Введіть слово для пошуку:");
        string searchWord = Console.ReadLine().Trim();

        Console.WriteLine($"Пошук слова: '{searchWord}'");

        if (string.IsNullOrWhiteSpace(searchWord))
        {
            Console.WriteLine("Слово для пошуку не було введено.");
            return;
        }

        bool containsWord = text.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
        string result = containsWord
            ? $"Слово \"{searchWord}\" міститься у тексті."
            : $"Слово \"{searchWord}\" не міститься у тексті.";

        File.WriteAllText(outputFilePath, result);
        Console.WriteLine(result);
    }

    static void Task3()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string inputFilePath = Path.Combine(currentDirectory, "input3.txt");
        string outputFilePath = Path.Combine(currentDirectory, "output3.txt");

        string text = File.ReadAllText(inputFilePath);

        string doubleLetterPattern = @"\b\w*(\w)\1\w*\b";
        Regex regex = new Regex(doubleLetterPattern);

        List<string> removedWords = new List<string>();
        List<string> remainingWords = new List<string>();

        foreach (string word in text.Split(new char[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (regex.IsMatch(word))
            {
                removedWords.Add(word);
            }
            else
            {
                remainingWords.Add(word);
            }
        }

        string removedWordsLine = string.Join(" ", removedWords);
        string updatedText = string.Join(" ", remainingWords);

        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            writer.WriteLine("Вилучені слова:");
            writer.WriteLine(removedWordsLine);
            writer.WriteLine();
            writer.WriteLine("Текст після вилучення слів:");
            writer.WriteLine(updatedText);
        }

        Console.WriteLine("Вилучені слова:");
        Console.WriteLine(removedWordsLine);
        Console.WriteLine();
        Console.WriteLine("Текст після вилучення слів:");
        Console.WriteLine(updatedText);
    }

    static void Task4()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(currentDirectory, "powers_of_3.bin");

        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            for (int i = 0; i <= 9; i++)
            {
                writer.Write(Math.Pow(3, i));
            }
        }

        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            int index = 0;
            Console.WriteLine("Компоненти файлу з парним порядковим номером:");
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                double value = reader.ReadDouble();
                if (index % 2 == 1)
                {
                    Console.WriteLine($"Порядковий номер {index}: {value}");
                }
                index++;
            }
        }
    }
}
