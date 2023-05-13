using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

public class CpuLoadGenerator
{
    private BackgroundWorker _worker;
    private Stopwatch _stopwatch;
    private int _percentage;
    private bool _stopRequested;

    public CpuLoadGenerator()
    {
        _worker = new BackgroundWorker();
        _worker.DoWork += DoWork;
        _worker.RunWorkerCompleted += RunWorkerCompleted;
    }

    public void Start(int percentage)
    {
        _percentage = percentage;
        _stopwatch = Stopwatch.StartNew();
        _stopRequested = false;

        _worker.RunWorkerAsync();
    }

    public void Stop()
    {
        _stopRequested = true;
    }

    private void DoWork(object sender, DoWorkEventArgs e)
    {
        int threadCount = Environment.ProcessorCount * _percentage / 100;
        Thread[] threads = new Thread[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            threads[i] = new Thread(() =>
            {
                while (!_stopRequested)
                {
                    for (int j = 0; j < 1000000; j++)
                    {
                        double result = Math.Sqrt(j);
                    }
                }
            });

            threads[i].Start();
        }

        while (!_stopRequested)
        {
            // Sleep for 10ms to give other threads a chance to run
            Thread.Sleep(80);
        }

        foreach (Thread thread in threads)
        {
            thread.Interrupt();
        }
    }

    private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        _stopwatch.Stop();
        Debug.WriteLine($"CpuLoadGenerator: Stopped after {_stopwatch.ElapsedMilliseconds}ms");
    }
}