using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        int resourceCount = 3; 
        SemaphoreSlim semaphore = new SemaphoreSlim(resourceCount);

        for (int i = 1; i <= 10; i++)
        {
            int threadNumber = i;
            Task.Run(async () =>
            {
                Console.WriteLine($"Поток {threadNumber} ожидает доступа к ресурсу.");
                await semaphore.WaitAsync();
                try
                {
                    Console.WriteLine($"Поток {threadNumber} получил доступ к ресурсу.");
                    await Task.Delay(1000); // имитация работы с ресурсом
                }
                finally
                {
                    Console.WriteLine($"Поток {threadNumber} освобождает ресурс.");
                    semaphore.Release();
                }
            });
        }
        await Task.Delay(5000);
    }
}