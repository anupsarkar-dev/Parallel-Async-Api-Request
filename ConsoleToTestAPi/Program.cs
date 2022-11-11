using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleToTestAPi
{
    class Program
    {
        static async  Task Main(string[] args)
        {


            Console.WriteLine("Press anykey to send API request ...");

            Console.Read();


            Console.WriteLine("Sending Request .....");

            //  await AsynchronousCall(); // Will Take Almost 7 Seconds



            //try
            //{

            //    await ParrallelTaskUsingAsync();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}



            try
            {

                var stopwatch = Stopwatch.StartNew();

                var httpClient = new HttpClient();


                var apiCall1Task = Api1(httpClient);
                var apiCall2Task = Api2(httpClient);
                var apiCall3Task = Api3(httpClient);

                var tasks = new[] { apiCall1Task, apiCall2Task, apiCall3Task };


                await Task.WhenAll(tasks).WithAggregatedExceptions();

                // task is already processed , just get the result
                var apiCall1 = apiCall1Task.Result;
                var apiCall2 = apiCall2Task.Result;
                var apiCall3 = apiCall3Task.Result;

                Console.WriteLine($"Total Time Taken To Process Request : {stopwatch.ElapsedMilliseconds}ms");

                Console.WriteLine($"Date Returned : {apiCall1} , {apiCall2} , {apiCall3} ");

            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Canceled");
            }
            catch (AggregateException exception)
            {
                //Console.WriteLine("2 or more exceptions");
                // Now the exception that we caught here is an AggregateException, 
                // with two inner exceptions:
                //var aggregate = exception as AggregateException;

                foreach (var ex in exception.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine($"Just a single exception: ${exception.Message}");
            }


        }

        private static async Task AsynchronousCall()
        {
            HttpClient httpClient = new HttpClient();

            var stopwatch = Stopwatch.StartNew();

            var apiCall1 = await Api1(httpClient);
            var apiCall2 = await Api2(httpClient);
            var apiCall3 = await Api3(httpClient);


            Console.WriteLine($"Total Time Taken To Process Request : {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Date Returned : {apiCall1} , {apiCall2} , {apiCall3} ");
        }

        private static async Task ParrallelTaskUsingAsync()
        {
            HttpClient httpClient = new HttpClient();

            var stopwatch = Stopwatch.StartNew();

            var apiCall1Task =  Api1(httpClient);
            var apiCall2Task =  Api2(httpClient);
            var apiCall3Task =  Api3(httpClient);

            var tasks = new[] { apiCall1Task, apiCall2Task, apiCall3Task };

            try
            {
               await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                var exceptions = tasks.Where(t => t.Exception != null)
                                      .Select(t => t.Exception.Message);

                Console.WriteLine(exceptions.ToString());
            }

           

            //try
            //{
            //    await t;
            //}
            //catch { };

            //if(t.Status == TaskStatus.RanToCompletion)
            //{
            //    Console.WriteLine("All Api Call Successfull!");

            //}else if( t.Status ==  TaskStatus.Faulted)
            //{
            //    Console.WriteLine("{0} task failed! " , TaskStatus.Faulted);
            //}


            // task is already processed , just get the result
            var apiCall1 = apiCall1Task.Result;
            var apiCall2 = apiCall2Task.Result;
            var apiCall3 = apiCall3Task.Result;

            Console.WriteLine($"Total Time Taken To Process Request : {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Date Returned : {apiCall1} , {apiCall2} , {apiCall3} ");

            //var taskCompletetionSource = new TaskCompletionSource<int>();

            //taskCompletetionSource.TrySetException(new Exception[]
            //{
            //    new ("This Went Wrong First!"),
            //    new ("This Went Wront Second!"),
            //    new ("This Went Wront Third!")
            //});
            //var result = TaskExt.WhenAll(taskCompletetionSource.Task);

        }

        public static async Task<int> Api1(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("http://localhost:5000/weatherforecast/get1");
            var data = JsonSerializer.Deserialize<ResponseData>(response);

            return data!.data;
        }


        public static async Task<int> Api2(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("http://localhost:5000/weatherforecast/get2");
            var data = JsonSerializer.Deserialize<ResponseData>(response);

            return data!.data;
        }


        public static async Task<int> Api3(HttpClient httpClient)
        {
            var response = await httpClient.GetStringAsync("http://localhost:5000/weatherforecast/get3");
            var data = JsonSerializer.Deserialize<ResponseData>(response);

            return data!.data;
        }
    }

    class ResponseData
    {
        public int data { get; set; }
    }


    public static class TaskExt2
    {
        /// <summary>
        /// A workaround for getting all of AggregateException.InnerExceptions with try/await/catch
        /// </summary>
        public static Task WithAggregatedExceptions(this Task @this)
        {
            // using AggregateException.Flatten as a bonus
            return @this.ContinueWith(
                continuationFunction: anteTask =>
                    anteTask.IsFaulted &&
                    anteTask.Exception is AggregateException ex &&
                    (ex.InnerExceptions.Count > 1 || ex.InnerException is AggregateException) ?
                    Task.FromException(ex.Flatten()) : anteTask,
                cancellationToken: CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                scheduler: TaskScheduler.Default).Unwrap();
        }
    }
}
