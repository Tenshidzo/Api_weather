using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly string apiKey = "1eaac2b2d4553a18d869c6d275bb28c1"; // Ваш API ключ с OpenWeatherMap
    private static readonly string[] cities = new string[] {
        "Kyiv", "Kharkiv", "Odesa", "Dnipro", "Lviv", "Zaporizhzhia",
        "Kryvyi Rih", "Mykolaiv", "Mariupol", "Vinnytsia", "Kherson",
        "Chernihiv", "Poltava", "Cherkasy", "Sumy", "Zhytomyr",
        "Chernivtsi", "Kropyvnytskyi", "Ternopil", "Rivne", "Lutsk",
        "Uzhhorod", "Ivano-Frankivsk"
    };

    static async Task Main(string[] args)
    {
        Console.WriteLine("Выберите областной центр Украины для получения прогноза погоды:");

        for (int i = 0; i < cities.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {cities[i]}");
        }

        // Получение выбора пользователя
        int choice = int.Parse(Console.ReadLine());

        if (choice < 1 || choice > cities.Length)
        {
            Console.WriteLine("Неверный выбор. Попробуйте снова.");
            return;
        }

        string selectedCity = cities[choice - 1];

        // Запрос погоды для выбранного города
        await GetWeatherForCity(selectedCity);
    }

    private static async Task GetWeatherForCity(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Формирование запроса к OpenWeatherMap
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";

                // Получение ответа от API
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Парсинг JSON-ответа
                JObject weatherData = JObject.Parse(responseBody);

                // Извлечение информации
                string weatherDescription = weatherData["weather"][0]["description"].ToString();
                double temperature = double.Parse(weatherData["main"]["temp"].ToString());
                double feelsLike = double.Parse(weatherData["main"]["feels_like"].ToString());
                double humidity = double.Parse(weatherData["main"]["humidity"].ToString());
                double windSpeed = double.Parse(weatherData["wind"]["speed"].ToString());

                // Вывод данных на экран
                Console.WriteLine($"Прогноз погоды для города {city}:");
                Console.WriteLine($"Описание: {weatherDescription}");
                Console.WriteLine($"Температура: {temperature}°C");
                Console.WriteLine($"Ощущается как: {feelsLike}°C");
                Console.WriteLine($"Влажность: {humidity}%");
                Console.WriteLine($"Скорость ветра: {windSpeed} м/с");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Ошибка при получении данных: " + e.Message);
            }
        }
    }
}
