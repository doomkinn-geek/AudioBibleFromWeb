namespace SortBibleFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Список книг Нового Завета
            string[] books = new string[]
            {
            "Евангелие от Матфея",
            "Евангелие от Марка",
            "Евангелие от Луки",
            "Евангелие от Иоанна",
            "Деяния святых апостолов",
            "Послание Иакова",
            "1-е послание Петра",
            "2-е послание Петра",
            "1-е послание Иоанна",
            "2-е послание Иоанна",
            "3-е послание Иоанна",
            "Послание Иуды",
            "Послание к Римлянам",
            "1-е послание к Коринфянам",
            "2-е послание к Коринфянам",
            "Послание к Галатам",
            "Послание к Ефесянам",
            "Послание к Филиппийцам",
            "Послание к Колоссянам",
            "1-е послание к Фессалоникийцам",
            "2-е послание к Фессалоникийцам",
            "1-е послание к Тимофею",
            "2-е послание к Тимофею",
            "Послание к Титу",
            "Послание к Филимону",
            "Послание к Евреям",
            "Откровение Иоанна Богослова"
            };

            // Путь к папке с исходными файлами
            string sourceDirectory = @"d:\\Temp\\audio_new_tastament\\";

            foreach (string file in Directory.GetFiles(sourceDirectory, "*.mp3"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string[] parts = fileName.Split('-');

                if (parts.Length != 2)
                {
                    Console.WriteLine($"Файл {fileName} имеет неправильный формат имени.");
                    continue;
                }

                if (!int.TryParse(parts[1], out int bookIndex) || bookIndex < 0 || bookIndex >= books.Length)
                {
                    Console.WriteLine($"Файл {fileName} имеет неправильный номер книги.");
                    continue;
                }

                string bookName = books[bookIndex];
                string targetDirectory = Path.Combine(sourceDirectory, bookName);

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                string targetFile = Path.Combine(targetDirectory, Path.GetFileName(file));

                File.Move(file, targetFile);

                // Изменение метаданных MP3 файла
                var mp3 = TagLib.File.Create(targetFile);
                mp3.Tag.Performers = new string[] { bookName };
                mp3.Save();
            }
        }
    }
}