namespace _03_factoryMethod;

internal abstract class ServiceFactory
{
    public abstract IService OrderService();
}

internal class TransactionCertificationFactory : ServiceFactory
{
    public override IService OrderService()
    {
        return new TransactionCertification();
    }
}

internal class InheritanceCertificateIssuanceFactory : ServiceFactory
{
    public override IService OrderService()
    {
        return new InheritanceCertificateIssuance();
    }
}

public interface IService
{
    List<string> GetRequiredDocuments();
}

internal class TransactionCertification : IService
{
    public List<string> GetRequiredDocuments()
    {
        return ["Passport", "transaction document"];
    }
}

internal class InheritanceCertificateIssuance : IService
{
    public List<string> GetRequiredDocuments()
    {
        return
        [
            "Death certificate", "passport", "identification number", "will (if any)",
            "documents confirming kinship (birth certificate, marriage certificate)"
        ];
    }
}

internal static class TestServiceFactory
{
    public static void Start()
    {
        Console.WriteLine("App: Launched with the TransactionCertificationFactory.");
        PrintRequiredDocuments(new TransactionCertificationFactory());

        Console.WriteLine("");

        Console.WriteLine(
            "App: Launched with the InheritanceCertificateIssuanceFactory.");
        PrintRequiredDocuments(new InheritanceCertificateIssuanceFactory());
    }

    private static void PrintRequiredDocuments(ServiceFactory serviceFactory)
    {
        Console.WriteLine("The required documents are: " +
                          string.Join(", ",
                              serviceFactory.OrderService().GetRequiredDocuments()));
    }
}

internal static class Program
{
    private static void Main()
    {
        TestServiceFactory.Start();
    }
}