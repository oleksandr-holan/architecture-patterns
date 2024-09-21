namespace _02_prototype;

public class Appointment
{
    public int Uid;
    public required string NotaryPublic;
    public DateTime Time;
    public required List<string> Services;

    public Appointment ShallowCopy()
    {
        return (Appointment)MemberwiseClone();
    }

    public Appointment DeepCopy()
    {
        var clone = (Appointment)MemberwiseClone();
        clone.Services = [..Services];
        return clone;
    }
}

internal static class Program
{
    private static void Main()
    {
        // Create original appointment
        var appointment = new Appointment
        {
            Uid = 23,
            NotaryPublic = "Olesya Kindryk",
            Time = new DateTime(2024, 9, 10, 13, 30, 0),
            Services = ["Witnessing Signatures"]
        };

        // Perform shallow copy
        var shallowCopiedAppointment = appointment.ShallowCopy();
        shallowCopiedAppointment.Services.Add("Administering Oaths and Affirmations");

        // Check if shallow copy affected the original
        Console.WriteLine(
            appointment.Services.Contains("Administering Oaths and Affirmations")
                ? "Shallow copy is a reference!"
                : "Shallow copy is not a reference!");

        // Perform deep copy
        var deepCopiedAppointment = appointment.DeepCopy();
        deepCopiedAppointment.Services.Add("Certifying Copies");

        // Check if deep copy affected the original
        Console.WriteLine(appointment.Services.Contains("Certifying Copies")
            ? "Deep copy is a reference!"
            : "Deep copy is not a reference!");
    }
}