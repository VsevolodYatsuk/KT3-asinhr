using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int resourceCount = 3; // количество доступных ресурсов
        SemaphoreSlim semaphore = new SemaphoreSlim(resourceCount);

        for (int i = 1; i <= 10; i++)
        {
            int threadNumber = i;
            new Thread(() =>
            {
                Console.WriteLine($"Поток {threadNumber} ожидает доступа к ресурсу.");
                semaphore.Wait();
                try
                {
                    Console.WriteLine($"Поток {threadNumber} получил доступ к ресурсу.");
                    Thread.Sleep(1000); // имитация работы с ресурсом
                }
                finally
                {
                    Console.WriteLine($"Поток {threadNumber} освобождает ресурс.");
                    semaphore.Release();
                }
            }).Start();
        }
    }
}
