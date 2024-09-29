namespace _06_objectPool;

public class Agent(
    string? name = null,
    string? image = null,
    int cpuCores = 1,
    int memory = 512)
{
    public string? Name = name;
    public string? Image = image;
    public int CpuCores = cpuCores;
    public int Memory = memory;
}

public sealed class PipelineAgentsPool
{
    private static readonly Lazy<PipelineAgentsPool> Lazy =
        new(() => new PipelineAgentsPool());

    private static PipelineAgentsPool? _instance;
    private static readonly object Lock = new();

    // Private properties
    private Queue<Agent> _agentPool;

    private PipelineAgentsPool()
    {
    }

    // Public method to get instance and set properties
    public static PipelineAgentsPool GetInstance(int agentsCount)
    {
        if (_instance != null) return _instance;
        lock (Lock)
        {
            _instance ??= new PipelineAgentsPool();
            _instance._agentPool =
                new Queue<Agent>(Enumerable.Repeat(new Agent(), agentsCount));
        }

        return _instance;
    }

    public Agent GetAgent()
    {
        if (_agentPool is { Count: 0 })
        {
            throw new Exception("No agents left");
        }

        return _agentPool.Dequeue();
    }

    public void PutAgent(Agent agent)
    {
        _agentPool.Enqueue(agent);
    }
}

internal static class Program
{
    private static void Main()
    {
        var pool = PipelineAgentsPool.GetInstance(3);
        pool = PipelineAgentsPool.GetInstance(10);
        // pool = new PipelineAgentsPool(10);
        Agent? agent1 = null;
        Agent? agent2;
        Agent? agent3;
        Agent? agent4;
        try
        {
            agent1 = pool.GetAgent();
            agent2 = pool.GetAgent();
            agent3 = pool.GetAgent();
            agent4 = pool.GetAgent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        pool.PutAgent(agent1!);
        Console.WriteLine("Put agent back");
        agent4 = pool.GetAgent();
        Console.WriteLine("Got agent4");
    }
}