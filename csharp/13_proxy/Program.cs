using System.Collections.Concurrent;
using System.Diagnostics;

namespace _13_proxy;

internal static class Program
{
    private static async Task Main()
    {
        const int threadCount = 100;
        const int writesPerThread = 1000;
        const string filePath = "output.txt";

        if (File.Exists(filePath))
            File.Delete(filePath);

        var stopwatch = Stopwatch.StartNew();

        using (var writer = new ThreadSafeFileWriter(filePath))
        {
            var tasks = new List<Task>();

            for (var i = 0; i < threadCount; i++)
            {
                var threadId = i;
                tasks.Add(Task.Run(() =>
                {
                    for (var j = 0; j < writesPerThread; j++)
                    {
                        writer.AppendAllText($"Thread {threadId}, Write {j}");
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        stopwatch.Stop();

        Console.WriteLine($"Writing completed in {stopwatch.ElapsedMilliseconds} ms");

        // Verify the results
        var lines = await File.ReadAllLinesAsync(filePath);
        Console.WriteLine($"Total lines written: {lines.Length}");
        Console.WriteLine($"Expected lines: {threadCount * writesPerThread}");

        var distinctThreads = lines.Select(l => int.Parse(l.Split(',')[0].Split(' ')[1])).Distinct().Count();
        Console.WriteLine($"Distinct threads that wrote to the file: {distinctThreads}");

        if (lines.Length == threadCount * writesPerThread && distinctThreads == threadCount)
        {
            Console.WriteLine("Test passed successfully!");
        }
        else
        {
            Console.WriteLine("Test failed. The number of lines or distinct threads doesn't match the expected values.");
        }
    }
}

internal class ThreadSafeFileWriter : IDisposable
{
    private readonly ConcurrentQueue<string> _writeQueue = new();
    private readonly AutoResetEvent _queueNotEmpty = new(false);
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Task _writerTask;
    private readonly string _filePath;


    public ThreadSafeFileWriter(string filePath)
    {
        _filePath = filePath;
        _writerTask = Task.Run(WriterThread);
    }

    public void WriteAllText(string contents)
    {
        EnqueueWrite(contents, false);
    }

    public void AppendAllText(string contents)
    {
        EnqueueWrite(contents, true);
    }

    public void WriteAllLines(string[] lines)
    {
        EnqueueWrite(string.Join(Environment.NewLine, lines), false);
    }

    public void AppendAllLines(string[] lines)
    {
        EnqueueWrite(string.Join(Environment.NewLine, lines), true);
    }

    private void EnqueueWrite(string data, bool append)
    {
        _writeQueue.Enqueue($"{(append ? "APPEND:" : "WRITE:")}{data}");
        _queueNotEmpty.Set();
    }

    private void WriterThread()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            _queueNotEmpty.WaitOne();
            while (_writeQueue.TryDequeue(out var item))
            {
                try
                {
                    var parts = item.Split([':'], 2);
                    var isAppend = parts[0] == "APPEND";
                    var data = parts[1];

                    if (isAppend)
                    {
                        File.AppendAllText(_filePath, data + Environment.NewLine);
                    }
                    else
                    {
                        File.WriteAllText(_filePath, data);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as appropriate for your application
                    Console.WriteLine($"Error writing to file: {ex.Message}");
                }
            }
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _queueNotEmpty.Set();
        _writerTask.Wait(); 
        _queueNotEmpty.Dispose();
        _cancellationTokenSource.Dispose();
    }
}