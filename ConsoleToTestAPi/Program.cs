using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleToTestAPi
{
    class Program
    {
        static async  Task Main(string[] args)
        {


            Console.WriteLine("Wait For 5 Seconds");

            Task.Delay(50000);


            Console.WriteLine("Now Sending Request");

            HttpClient httpClient = new HttpClient();

            var stopwatch = Stopwatch.StartNew();

            var Task1 = await GetTask1(httpClient);
            var Task2 = await GetTask2(httpClient);
            var Task3 = await GetTask3(httpClient);


            Console.WriteLine($"Total Time Taken To Process : {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"{Task1} , {Task2} , {Task3} ");
        }

        public static async Task<int> GetTask1(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("http://localhost:5000/weatherforecast/get1");
            var data = JsonSerializer.Deserialize<ResponseData>(response);

            return data!.Data;
        }


        public static async Task<int> GetTask2(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("http://localhost:5000/weatherforecast/get2");
            var data = JsonSerializer.Deserialize<ResponseData>(response);

            return data!.Data;
        }


        public static async Task<int> GetTask3(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("http://localhost:5000/weatherforecast/get3");
            var data = JsonSerializer.Deserialize<ResponseData>(response);

            return data!.Data;
        }
    }

    class ResponseData
    {
        public int Data { get; set; }
    }
}
