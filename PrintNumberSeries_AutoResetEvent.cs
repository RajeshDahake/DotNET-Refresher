using System;
using System.Threading;

class NumberSeriesPrinter
{
    private static int currentNumber = 1;
    private const int MaxNumber = 20;

    private static readonly AutoResetEvent thread1Event = new AutoResetEvent(true);
    private static readonly AutoResetEvent thread2Event = new AutoResetEvent(false);
    private static readonly AutoResetEvent thread3Event = new AutoResetEvent(false);

    static void PrintNumbers(
        AutoResetEvent currentEvent,
        AutoResetEvent nextEvent)
    {
        while (true)
        {
            currentEvent.WaitOne();

            if (currentNumber > MaxNumber)
            {
                nextEvent.Set();
                break;
            }

            Console.WriteLine(
                $"{Thread.CurrentThread.Name} : {currentNumber}");

            currentNumber++;

            nextEvent.Set();
        }
    }

    static void Main()
    {
        Thread thread1 = new Thread(() => PrintNumbers(thread1Event, thread2Event));
        Thread thread2 = new Thread(() => PrintNumbers(thread2Event, thread3Event));
        Thread thread3 = new Thread(() => PrintNumbers(thread3Event, thread1Event));

        thread1.Name = "Thread-1";
        thread2.Name = "Thread-2";
        thread3.Name = "Thread-3";

        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();

        Console.WriteLine("Completed");
    }
}
