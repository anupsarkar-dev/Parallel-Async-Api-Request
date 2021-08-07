using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleToTestAPi
{
    public class TaskExt
    {
        public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
        {
            var allTasks = Task.WhenAll(tasks);

            try
            {
                return await allTasks;

            }catch(Exception)
            {

            }

            throw allTasks.Exception ?? throw new Exception("Uncought Exception");
        }
    }
}
