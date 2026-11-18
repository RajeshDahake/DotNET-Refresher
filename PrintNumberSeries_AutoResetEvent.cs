using System;
using System.Threading;

class NumberSeriesPrinter
{
    private static int number = 1;
    private static int max = 20;

    static AutoResetEvent ev1 = new AutoResetEvent(true);
    static AutoResetEvent ev2 = new AutoResetEvent(false);
    static AutoResetEvent ev3 = new AutoResetEvent(false);

    static void Print(
        AutoResetEvent currentEvent,
        AutoResetEvent nextEvent)
    {
        while (true)
        {
            currentEvent.WaitOne();

            if (number > max)
            {
                nextEvent.Set();
                break;
            }

            Console.WriteLine(
                $"{Thread.CurrentThread.Name} : {number}");

            number++;

            nextEvent.Set();
        }
    }

    static void Main()
    {
        Thread t1 = new Thread(() => Print(ev1, ev2));
        Thread t2 = new Thread(() => Print(ev2, ev3));
        Thread t3 = new Thread(() => Print(ev3, ev1));

        t1.Name = "Thread-1";
        t2.Name = "Thread-2";
        t3.Name = "Thread-3";

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine("Completed");
    }
}
