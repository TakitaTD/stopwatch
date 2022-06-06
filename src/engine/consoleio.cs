using System;
namespace Stopwatch.engine;
using System.Threading;
class CountThread
{
    private int num;
    public bool paused;

    public bool setPaused(bool val)
    {
        paused = val;
        return paused;
    }
    public CountThread()
    {

        if (File.Exists(@"./stopwatch_time.txt"))
        {
            try
            {
                num = Convert.ToInt32(File.ReadAllText(@"./stopwatch_time.txt"));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("FUCK: stopwatch_time.txt may be corrupted or tampered with. Would you like to reset the file?");
            }
        }
        else
        {
            File.Create(@"./stopwatch_time.txt");
            File.WriteAllText(@"./stopwatch_time.txt", "1");
        }
    }
    public void count()
    {
        while (!paused)
        {
            Console.Clear();
            Console.Write("Time elapsed: " + TimeSpan.FromSeconds(num).ToString().Replace("0000", ""));
            Thread.Sleep(1000);
            num++;
            File.WriteAllTextAsync(@"./stopwatch_time.txt", num.ToString());
        }
    }
}

public class consoleio
{
    private int num = 0;
    private bool paused = false;
    private CountThread countThread;

    private async void Effect()
    {
        if (!paused)
        {
            num++;
            await File.WriteAllTextAsync(@"./stopwatch_time.txt", num.ToString());
        }
    }
    public async void Init()
    {
        countThread = new CountThread();
        Thread thr = new Thread(new ThreadStart(countThread.count));
        thr.Start();

        while (true)
        {
            ConsoleKey topLevelKey = Console.ReadKey().Key;

            if (topLevelKey == ConsoleKey.Spacebar)
            {
                countThread.setPaused(true);
                Console.Write("(PAUSED)");
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Spacebar) Init();
                else if (key == ConsoleKey.Q) { Console.WriteLine("\nExiting..."); Environment.Exit(0); };
            }
            else if (topLevelKey == ConsoleKey.Q)
            {
                Console.WriteLine("\nExiting..."); Environment.Exit(0);
            }
            else
            {
                Init();
            }
        }

    }
}