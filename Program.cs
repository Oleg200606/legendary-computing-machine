using System;
using System.IO;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в консольный проводник!");

        ExplorerMenu.Start();
    }
}

static class ExplorerMenu
{
    private static string currentPath;

    public static void Start()
    {
        while (true)
        {
            DisplayDrives();
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }

            if (char.IsLetter(key.KeyChar))
            {
                char driveLetter = char.ToUpper(key.KeyChar);
                ExploreDrive(driveLetter);
            }
        }
    }

    private static void DisplayDrives()
    {
        Console.Clear();
        Console.WriteLine("Выберите диск:");

        // В macOS нет явного понятия дисков (как в Windows), поэтому просто отображаем корневые директории.
        string[] rootDirectories = Directory.GetDirectories("/");
        for (int i = 0; i < rootDirectories.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {rootDirectories[i]}");
        }
    }

    private static void ExploreDrive(char driveLetter)
    {
        currentPath = $"/";
        ExplorePath();
    }

    private static void ExplorePath()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Текущий путь: {currentPath}");

            DisplayContent();

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.Escape)
            {
                if (currentPath == "/") // at the root
                {
                    break;
                }
                else
                {
                    currentPath = Directory.GetParent(currentPath).FullName;
                }
            }
            else if (char.IsDigit(key.KeyChar))
            {
                int selectedIndex = int.Parse(key.KeyChar.ToString()) - 1;

                if (selectedIndex >= 0 && selectedIndex < Directory.GetDirectories(currentPath).Length)
                {
                    currentPath = Directory.GetDirectories(currentPath)[selectedIndex];
                }
                else if (selectedIndex >= Directory.GetDirectories(currentPath).Length &&
                         selectedIndex < Directory.GetDirectories(currentPath).Length + Directory.GetFiles(currentPath).Length)
                {
                    string selectedFile = Directory.GetFiles(currentPath)[selectedIndex - Directory.GetDirectories(currentPath).Length];
                    OpenFile(selectedFile);
                }
            }
        }
    }

    private static void DisplayContent()
    {
        int index = 1;

        string[] directories = Directory.GetDirectories(currentPath);
        foreach (var directory in directories)
        {
            Console.WriteLine($"{index}. [Папка] {Path.GetFileName(directory)}");
            index++;
        }

        string[] files = Directory.GetFiles(currentPath);
        foreach (var file in files)
        {
            Console.WriteLine($"{index}. [Файл] {Path.GetFileName(file)}");
            index++;
        }

        Console.WriteLine("Нажмите Escape для возврата.");
    }

    private static void OpenFile(string filePath)
    {
        // Добавьте код для открытия файла здесь.
        Console.WriteLine($"Открываем файл: {filePath}");
        Process.Start("open", filePath); // открыть файл с помощью стандартного приложения
        Console.ReadKey();
    }
}
