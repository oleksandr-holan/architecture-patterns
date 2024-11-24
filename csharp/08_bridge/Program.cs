namespace _08_bridge;

public abstract class PaymentProcessor
{
    public abstract bool ProcessPayment(float amount);
    public abstract bool RefundPayment(float amount);
}

internal class StripeProcessor : PaymentProcessor
{
    public override bool ProcessPayment(float amount)
    {
        Console.WriteLine($"Processing ${amount} payment via Stripe");
        return true;
    }

    public override bool RefundPayment(float amount)
    {
        Console.WriteLine($"Refunding ${amount} via Stripe");
        return true;
    }
}

internal class PayPalProcessor : PaymentProcessor
{
    public override bool ProcessPayment(float amount)
    {
        Console.WriteLine($"Processing ${amount} payment via PayPal");
        return true;
    }

    public override bool RefundPayment(float amount)
    {
        Console.WriteLine($"Refunding ${amount} via PayPal");
        return true;
    }
}

internal abstract class BankAccount(PaymentProcessor processor)
{
    protected readonly PaymentProcessor Processor = processor;

    public abstract bool MakePayment(float amount);
    public abstract bool RequestRefund(float amount);
}

internal class CheckingAccount(PaymentProcessor processor) : BankAccount(processor)
{
    public override bool MakePayment(float amount)
    {
        Console.WriteLine("Payment from Checking Account");
        return Processor.ProcessPayment(amount);
    }

    public override bool RequestRefund(float amount)
    {
        Console.WriteLine("Refund to Checking Account");
        return Processor.RefundPayment(amount);
    }
}

internal class SavingsAccount(PaymentProcessor processor) : BankAccount(processor)
{
    public override bool MakePayment(float amount)
    {
        if (amount > 1000)
        {
            Console.WriteLine("Error: Cannot make payments over $1000 from Savings Account");
            return false;
        }
        Console.WriteLine("Payment from Savings Account");
        return Processor.ProcessPayment(amount);
    }

    public override bool RequestRefund(float amount)
    {
        Console.WriteLine("Refund to Savings Account");
        return Processor.RefundPayment(amount);
    }
}

internal static class Program
{
    public static void Main()
    {
        PaymentProcessor stripeProcessor = new StripeProcessor();
        PaymentProcessor paypalProcessor = new PayPalProcessor();

        BankAccount checkingStripe = new CheckingAccount(stripeProcessor);
        BankAccount savingsPaypal = new SavingsAccount(paypalProcessor);
        BankAccount checkingPaypal = new CheckingAccount(paypalProcessor);

        checkingStripe.MakePayment(100);
        savingsPaypal.MakePayment(500);
        checkingPaypal.MakePayment(750);

        savingsPaypal.MakePayment(1500);

        checkingStripe.RequestRefund(50);
        savingsPaypal.RequestRefund(200);
    }
}