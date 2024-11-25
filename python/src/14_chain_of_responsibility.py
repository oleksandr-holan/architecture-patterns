import string
from abc import ABC, abstractmethod
from dataclasses import dataclass


def shares_any_character(s1: str, s2: str) -> bool:
    return any(char in s1 for char in s2)


class IValidate(ABC):
    @abstractmethod
    def handle(self, s: str): ...


class BaseValidator(IValidate):
    def __init__(self, next: IValidate = None):
        self.next = next

    def handle(self, s: str):
        if self.next:
            self.next.handle(s)


class NonEmpty(BaseValidator):
    def handle(self, s: str):
        if not s:
            raise Exception("Can't be empty")
        super().handle(s)


class HasLowerLatters(BaseValidator):
    def handle(self, s: str):
        if not shares_any_character(s, string.ascii_lowercase):
            raise Exception("Must have lower letters")
        super().handle(s)


class HasUpperLatters(BaseValidator):
    def handle(self, s: str):
        if not shares_any_character(s, string.ascii_uppercase):
            raise Exception("Must have upper letters")
        super().handle(s)


@dataclass
class Input:
    text: str = None
    error: str = None


if __name__ == "__main__":
    validator = NonEmpty(HasLowerLatters(HasUpperLatters()))
    inputs = [Input(text=s) for s in ("lower", "UPPER", "lowerUPPER")]

    for inp in inputs:
        try:
            validator.handle(inp.text)
            print(f"{inp.text} is valid!")
        except Exception as e:
            inp.error = e
            print(f"{inp.text} {inp.error}")
