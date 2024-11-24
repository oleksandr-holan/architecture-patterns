from abc import ABC, abstractmethod


class PaymentProcessor(ABC):
    @abstractmethod
    def process_payment(self, amount: float) -> bool:
        pass

    @abstractmethod
    def refund_payment(self, amount: float) -> bool:
        pass


class StripeProcessor(PaymentProcessor):
    def process_payment(self, amount: float) -> bool:
        print(f"Processing ${amount} payment via Stripe")
        return True

    def refund_payment(self, amount: float) -> bool:
        print(f"Refunding ${amount} via Stripe")
        return True


class PayPalProcessor(PaymentProcessor):
    def process_payment(self, amount: float) -> bool:
        print(f"Processing ${amount} payment via PayPal")
        return True

    def refund_payment(self, amount: float) -> bool:
        print(f"Refunding ${amount} via PayPal")
        return True


class BankAccount(ABC):
    def __init__(self, processor: PaymentProcessor):
        self.processor = processor

    @abstractmethod
    def make_payment(self, amount: float) -> bool:
        pass

    @abstractmethod
    def request_refund(self, amount: float) -> bool:
        pass


class CheckingAccount(BankAccount):
    def make_payment(self, amount: float) -> bool:
        print("Payment from Checking Account")
        return self.processor.process_payment(amount)

    def request_refund(self, amount: float) -> bool:
        print("Refund to Checking Account")
        return self.processor.refund_payment(amount)


class SavingsAccount(BankAccount):
    def make_payment(self, amount: float) -> bool:
        if amount > 1000:
            print("Error: Cannot make payments over $1000 from Savings Account")
            return False
        print("Payment from Savings Account")
        return self.processor.process_payment(amount)

    def request_refund(self, amount: float) -> bool:
        print("Refund to Savings Account")
        return self.processor.refund_payment(amount)


# Client code
if __name__ == "__main__":
    stripe_processor = StripeProcessor()
    paypal_processor = PayPalProcessor()

    checking_stripe = CheckingAccount(stripe_processor)
    savings_paypal = SavingsAccount(paypal_processor)
    checking_paypal = CheckingAccount(paypal_processor)

    checking_stripe.make_payment(100)
    savings_paypal.make_payment(500)
    checking_paypal.make_payment(750)

    savings_paypal.make_payment(1500)

    checking_stripe.request_refund(50)
    savings_paypal.request_refund(200)
