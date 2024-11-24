from abc import ABC, abstractmethod


class Item(ABC):
    @abstractmethod
    def get_price(self) -> float:
        pass

    @abstractmethod
    def display(self, indent: str = "") -> None:
        pass


class Composition(Item):
    def __init__(self, name: str):
        self.name = name
        self.components: list[Item] = []

    def add(self, component: Item) -> None:
        self.components.append(component)

    def remove(self, component: Item) -> None:
        self.components.remove(component)

    def get_price(self) -> float:
        return sum(component.get_price() for component in self.components)

    def display(self, indent: str = "") -> None:
        print(f"{indent}{self.name}:")
        for component in self.components:
            component.display(indent + "  ")


class PCItem(Item):
    def __init__(self, name: str, price: float, category: str):
        self.name = name
        self.price = price
        self.category = category

    def get_price(self) -> float:
        return self.price

    def display(self, indent: str = "") -> None:
        print(f"{indent}{self.name} ({self.category}): ${self.price:.2f}")


class PCBuild(Composition):
    def __init__(self, name: str):
        super().__init__(name)

    def add(self, component: PCItem) -> None:
        super().add(component)


class Order(Composition):
    def __init__(self):
        super().__init__("Order")


if __name__ == "__main__":
    order = Order()

    order.add(PCItem("Keyboard", 49.99, "Keyboards"))
    order.add(PCItem("Mouse", 29.99, "Mice"))

    pc_build = PCBuild("Gaming PC")
    pc_build.add(PCItem("CPU", 299.99, "Processor"))
    pc_build.add(PCItem("GPU", 499.99, "Graphics Card"))
    pc_build.add(PCItem("RAM", 89.99, "Memory"))
    pc_build.add(PCItem("SSD", 119.99, "Storage"))

    order.add(pc_build)

    order.add(PCItem("Monitor", 199.99, "Monitors"))

    order.display()

    total_price = order.get_price()
    print(f"\nTotal Price: ${total_price:.2f}")
