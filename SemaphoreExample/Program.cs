using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly Dictionary<int, SemaphoreSlim> prioritySemaphores = new Dictionary<int, SemaphoreSlim>();

    static async Task Main(string[] args)
    {
        int resourceCount = 3;

        prioritySemaphores[1] = new SemaphoreSlim(resourceCount);
        prioritySemaphores[2] = new SemaphoreSlim(0);

        List<Task> tasks = new List<Task>();

        for (int i = 1; i <= 10; i++)
        {
            int threadNumber = i;
            int priority = i % 2 == 0 ? 2 : 1;
            tasks.Add(RunThreadWithPriority(threadNumber, priority));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine("Все потоки завершили работу.");
    }

    static async Task RunThreadWithPriority(int threadNumber, int priority)
    {
        Console.WriteLine($"Поток {threadNumber} с приоритетом {priority} ожидает доступа к ресурсу.");
        await prioritySemaphores[priority].WaitAsync();
        try
        {
            Console.WriteLine($"Поток {threadNumber} с приоритетом {priority} получил доступ к ресурсу.");
            await Task.Delay(1000); 
        }
        finally
        {
            Console.WriteLine($"Поток {threadNumber} с приоритетом {priority} освобождает ресурс.");
            prioritySemaphores[priority].Release();
            if (priority == 1) 
            {
                prioritySemaphores[2].Release();
            }
        }
    }
}