namespace _11_facade;

public class PasswordHasher(int workFactor = 12)
{
    public string GetPasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}

internal static class Program
{
    private static void Main()
    {
        var hasher = new PasswordHasher();

        const string originalPassword = "mysecretpassword123";

        var hashedPassword = hasher.GetPasswordHash(originalPassword);
        Console.WriteLine($"Hashed password: {hashedPassword}");

        var isCorrect = PasswordHasher.VerifyPassword(originalPassword, hashedPassword);
        Console.WriteLine($"Is password correct? {isCorrect}");

        const string wrongPassword = "wrongpassword456";
        var isIncorrect = PasswordHasher.VerifyPassword(wrongPassword, hashedPassword);
        Console.WriteLine($"Is wrong password correct? {isIncorrect}");
    }
}