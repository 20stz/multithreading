using System;
using System.Threading;
using System.Threading.Tasks;

namespace multithreading
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            SimpleThreadExample simpleThreadExample = new SimpleThreadExample();

            Task t1 = simpleThreadExample.DoSometh("t1");
            Task t2 = simpleThreadExample.DoSometh("t2");
            Task t3 = simpleThreadExample.DoSometh("t3");
            Task t4 = simpleThreadExample.DoSometh("t4");

            Task.Run(() => t1);
            //Task.Run(() => t2);
            Task.Run(() => t3);
            Task.Run(() => t4);

            Task.WaitAll(t1);

            await t2;

            return 1;
        }
    }
}


public class SimpleThreadExample
{
    public void StartMultipleThread()
    {
        DateTime startTime = DateTime.Now;

        Thread t1 = new Thread(() =>
        {
            int numberOfSeconds = 0;
            while (numberOfSeconds < 5)
            {
                Thread.Sleep(1000);

                numberOfSeconds++;
            }

            Console.WriteLine("I ran for 5 seconds");
        });

        Thread t2 = new Thread(() =>
        {
            int numberOfSeconds = 0;
            while (numberOfSeconds < 8)
            {
                Thread.Sleep(1000);

                numberOfSeconds++;
            }

            Console.WriteLine("I ran for 8 seconds");
        });


        //parameterized thread
        Thread t3 = new Thread(p =>
        {
            int numberOfSeconds = 0;
            while (numberOfSeconds < Convert.ToInt32(p))
            {
                Thread.Sleep(1000);

                numberOfSeconds++;
            }

            Console.WriteLine("I ran for {0} seconds", numberOfSeconds);
        });

        t1.Start();
        t2.Start();
        //passing parameter to parameterized thread
        t3.Start(20);

        //wait for t1 to finish
        t1.Join();

        //wait for t2 to finish
        t2.Join();

        //wait for t3 to finish
        t3.Join();


        Console.WriteLine("All Threads Exited in {0} secods", (DateTime.Now - startTime).TotalSeconds);
    }

    public async Task DoSometh(string name)
    {
        Console.WriteLine($"entering task {name} {Task.CurrentId}" );
        await Task.Delay(10000);
        Console.WriteLine($"exiting do someth {name} {Task.CurrentId}");
    }

    public async void DoVoid(string name)
    {
        Console.WriteLine($"entering task {Task.CurrentId}");
        await Task.Delay(10000);
        Console.WriteLine($"exiting do someth {Task.CurrentId}");
    }
}

public class DestroyThreadExample
{
    public bool IsCancelled { get; set; }

    public Thread MyThread { get; set; }

    public void StartThread()
    {
        MyThread = new Thread(() =>
        {
            int numberOfSeconds = 0;
            while (numberOfSeconds < 8)
            {
                if (IsCancelled == false)
                {
                    break;
                }

                Thread.Sleep(1000);

                numberOfSeconds++;
            }

            Console.WriteLine("I ran for {0} seconds", numberOfSeconds);
        });
    }

    //Destroy thread
    public void Abort()
    {
        MyThread.Abort();
    }

    //Graceful exit
    public void GracefulAbort()
    {
        IsCancelled = true;
    }
}