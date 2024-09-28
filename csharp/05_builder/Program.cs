namespace _05_builder;

using System;
using System.Collections.Generic;

public abstract class Component
{
    public string? Manufacturer { get; set; }

    public override string ToString() => $"{GetType().Name} ({Manufacturer})";
}

public class Motherboard : Component
{
    public string? Chipset { get; set; }

    public override string ToString() => $"{base.ToString()} - Chipset: {Chipset}";
}

public class Cpu : Component
{
    public int Cores { get; set; }

    public override string ToString() => $"{base.ToString()} - Cores: {Cores}";
}

public class Gpu : Component
{
    public int MemoryFrequency { get; set; }

    public override string ToString() =>
        $"{base.ToString()} - Memory Frequency: {MemoryFrequency} MHz";
}

public abstract class Psu : Component
{
    public int Power { get; set; }

    public override string ToString() => $"{base.ToString()} - Power: {Power}W";
}

public class Ram : Component
{
    public int Frequency { get; set; }

    public override string ToString() =>
        $"{base.ToString()} - Frequency: {Frequency} MHz";
}

public abstract class Storage : Component
{
    public int Capacity { get; set; }

    public override string ToString() => $"{base.ToString()} - Capacity: {Capacity}GB";
}

public class Ssd : Storage
{
    public int MemoryType { get; set; }

    public override string ToString() =>
        $"{base.ToString()} - Memory Type: {MemoryType}";
}

public class Hdd : Storage
{
    public int Rpm { get; set; }

    public override string ToString() => $"{base.ToString()} - RPM: {Rpm}";
}

public class Cooler : Component
{
    public int Height { get; set; }

    public override string ToString() => $"{base.ToString()} - Height: {Height}mm";
}

public class Fan : Component
{
    public int Size { get; set; }

    public override string ToString() => $"{base.ToString()} - Size: {Size}mm";
}

public abstract class Pc
{
    public Motherboard? Motherboard { get; set; }
    public Cpu? Cpu { get; set; }
    public Gpu? Gpu { get; set; }
    public Psu? Psu { get; set; }
    public Ram? Ram { get; set; }
    public Storage? Storage { get; set; }
    public Cooler? Cooler { get; set; }
    public List<Fan>? Fans { get; set; }

    public override string ToString()
    {
        return $"PC Configuration:\n" +
               $"  Motherboard: {Motherboard}\n" +
               $"  CPU: {Cpu}\n" +
               $"  GPU: {Gpu}\n" +
               $"  PSU: {Psu}\n" +
               $"  RAM: {Ram}\n" +
               $"  Storage: {Storage}\n" +
               $"  Cooler: {Cooler}\n" +
               $"  Fans: {(Fans?.Count > 0 ? string.Join(", ", Fans) : "None")}";
    }
}

public class OfficePc : Pc
{
    public string DoWork()
    {
        return $"Imma work on {this}!";
    }
}

public class GamingPc : Pc
{
    public string PlayGames()
    {
        return $"Imma play games on {this}!";
    }
}

public abstract class PcBuilder
{
    public abstract Pc Build();
    public abstract PcBuilder SetMotherboard(Motherboard? motherboard);
    public abstract PcBuilder SetCpu(Cpu? cpu);
    public abstract PcBuilder SetGpu(Gpu? gpu);
    public abstract PcBuilder SetPsu(Psu? psu);
    public abstract PcBuilder SetRam(Ram? ram);
    public abstract PcBuilder SetStorage(Storage? storage);
    public abstract PcBuilder SetCooler(Cooler? cooler);
    public abstract PcBuilder SetFans(List<Fan>? fans);
}

public class GamingPcBuilder : PcBuilder
{
    private GamingPc _pc = new();

    public override Pc Build()
    {
        var pc = _pc;
        _pc = new GamingPc();
        return pc;
    }

    public override PcBuilder SetMotherboard(Motherboard? motherboard)
    {
        _pc.Motherboard = motherboard;
        return this;
    }

    public override PcBuilder SetCpu(Cpu? cpu)
    {
        _pc.Cpu = cpu;
        return this;
    }

    public override PcBuilder SetGpu(Gpu? gpu)
    {
        _pc.Gpu = gpu;
        return this;
    }

    public override PcBuilder SetPsu(Psu? psu)
    {
        _pc.Psu = psu;
        return this;
    }

    public override PcBuilder SetRam(Ram? ram)
    {
        _pc.Ram = ram;
        return this;
    }

    public override PcBuilder SetStorage(Storage? storage)
    {
        _pc.Storage = storage;
        return this;
    }

    public override PcBuilder SetCooler(Cooler? cooler)
    {
        _pc.Cooler = cooler;
        return this;
    }

    public override PcBuilder SetFans(List<Fan>? fans)
    {
        if (fans is { Count: < 2 })
            throw new ArgumentException("There should be at least two fans!");
        _pc.Fans = fans;
        return this;
    }
}

public class OfficePcBuilder : PcBuilder
{
    private OfficePc _pc = new();

    public override Pc Build()
    {
        var pc = _pc;
        _pc = new OfficePc();
        return pc;
    }

    public override PcBuilder SetMotherboard(Motherboard? motherboard)
    {
        _pc.Motherboard = motherboard;
        return this;
    }

    public override PcBuilder SetCpu(Cpu? cpu)
    {
        _pc.Cpu = cpu;
        return this;
    }

    public override PcBuilder SetGpu(Gpu? gpu)
    {
        if (!OfficeComponents.Gpu.Contains(gpu))
            throw new ArgumentException(
                "It seems your coworker is trying to fool you and buy a gaming pc to play games at work");
        _pc.Gpu = gpu;
        return this;
    }

    public override PcBuilder SetPsu(Psu? psu)
    {
        _pc.Psu = psu;
        return this;
    }

    public override PcBuilder SetRam(Ram? ram)
    {
        _pc.Ram = ram;
        return this;
    }

    public override PcBuilder SetStorage(Storage? storage)
    {
        _pc.Storage = storage;
        return this;
    }

    public override PcBuilder SetCooler(Cooler? cooler)
    {
        _pc.Cooler = cooler;
        return this;
    }

    public override PcBuilder SetFans(List<Fan>? fans)
    {
        _pc.Fans = fans;
        return this;
    }
}

public static class OfficeComponents
{
    public static readonly List<Gpu?> Gpu =
        [new() { Manufacturer = "AMD", MemoryFrequency = 1000 }];
}

internal static class Program
{
    private static void Main()
    {
        var cpu = new Cpu { Manufacturer = "AMD", Cores = 8 };
        var officeGpu = OfficeComponents.Gpu[0];
        var gamingGpu = new Gpu { Manufacturer = "Nvidia", MemoryFrequency = 7500 };
        var gamingPcBuilder = new GamingPcBuilder();
        var officePcBuilder = new OfficePcBuilder();

        var gamingPc = gamingPcBuilder
            .SetCpu(cpu)
            .SetFans([new Fan { Size = 120 }, new Fan { Size = 120 }])
            .Build();

        Console.WriteLine(gamingPc);

        var officePc = officePcBuilder
            .SetFans([new Fan { Size = 90 }, new Fan { Size = 90 }])
            .SetGpu(officeGpu)
            .Build();

        Console.WriteLine(officePc);

        try
        {
            officePc = officePcBuilder
                .SetFans([new Fan { Size = 90 }, new Fan { Size = 90 }])
                .SetGpu(gamingGpu)
                .Build();

            Console.WriteLine(officePc);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}