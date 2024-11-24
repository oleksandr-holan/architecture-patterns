from abc import ABC, abstractmethod


class ICoffee(ABC):
    @abstractmethod
    def get_cost(self):
        pass

    @abstractmethod
    def get_description(self):
        pass


class Coffee(ICoffee):
    def get_cost(self):
        return 2.0

    def get_description(self):
        return "Simple coffee"


class CoffeeDecorator(ICoffee):
    def __init__(self, coffee):
        self._coffee = coffee

    def get_cost(self):
        return self._coffee.get_cost()

    def get_description(self):
        return self._coffee.get_description()


class Milk(CoffeeDecorator):
    def __init__(self, coffee):
        super().__init__(coffee)

    def get_cost(self):
        return super().get_cost() + 0.5

    def get_description(self):
        return super().get_description() + ", milk"


class Sugar(CoffeeDecorator):
    def __init__(self, coffee):
        super().__init__(coffee)

    def get_cost(self):
        return super().get_cost() + 0.2

    def get_description(self):
        return super().get_description() + ", sugar"


class Vanilla(CoffeeDecorator):
    def __init__(self, coffee):
        super().__init__(coffee)

    def get_cost(self):
        return super().get_cost() + 0.7

    def get_description(self):
        return super().get_description() + ", vanilla"


if __name__ == "__main__":
    coffee = Coffee()
    print(f"Cost: ${coffee.get_cost():.2f}, Description: {coffee.get_description()}")

    coffee_with_milk = Milk(coffee)
    print(
        f"Cost: ${coffee_with_milk.get_cost():.2f}, Description: {coffee_with_milk.get_description()}"
    )

    coffee_with_milk_and_sugar = Sugar(coffee_with_milk)
    print(
        f"Cost: ${coffee_with_milk_and_sugar.get_cost():.2f}, Description: {coffee_with_milk_and_sugar.get_description()}"
    )

    fancy_coffee = Vanilla(Milk(Sugar(Coffee())))
    print(
        f"Cost: ${fancy_coffee.get_cost():.2f}, Description: {fancy_coffee.get_description()}"
    )
