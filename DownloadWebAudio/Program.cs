using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace DownloadWebAudio
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var url = "https://knigavuhe.org/book/biblija-novyjj-zavet-sinodalnyjj-perevod/"; // URL страницы
            var html = await client.GetStringAsync(url); // Получаем HTML-код страницы

            var regex = new Regex(@"new BookPlayer\(\d+, (\[.*?\])"); // Регулярное выражение для поиска JSON-данных
            var match = regex.Match(html); // Ищем JSON-данные в HTML-коде

            if (match.Success)
            {
                var jsonData = match.Groups[1].Value; // Извлекаем JSON-данные
                var jsonArray = JArray.Parse(jsonData); // Преобразуем JSON-данные в JArray

                foreach (var jsonObject in jsonArray)
                {
                    var audioUrl = (string)jsonObject["url"]; // Получаем URL аудиофайла
                    if (audioUrl != null)
                    {
                        Console.WriteLine($"Found audio file: {audioUrl}");
                        await DownloadAudioFile(audioUrl); // Скачиваем аудиофайл
                    }
                }
            }
        }
        static async Task DownloadAudioFile(string url)
        {
            var fileName = Path.GetFileName(new Uri(url).LocalPath); // Получаем имя файла из URL
            var bytes = await client.GetByteArrayAsync(url); // Скачиваем файл
            await File.WriteAllBytesAsync(fileName, bytes); // Сохраняем файл на диск
            Console.WriteLine($"Downloaded: {fileName}");
        }
    }
}