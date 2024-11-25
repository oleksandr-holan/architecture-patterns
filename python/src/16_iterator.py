from abc import ABC, abstractmethod


class Component(ABC):
    @abstractmethod
    def __repr__(self) -> str: ...


class Composite(Component):
    def __init__(self, *args):
        self.children = args

    def __iter__(self):
        return CompositeIterator(self)


class Div(Composite):
    def __repr__(self):
        return "div"


class P(Composite):
    def __repr__(self):
        return "p"


class CompositeIterator:
    def __init__(self, root):
        self.stack = [iter([root])]

    def __iter__(self):
        return self

    def __next__(self):
        while self.stack:
            try:
                component = next(self.stack[-1])
                if isinstance(component, Composite):
                    self.stack.append(iter(component.children))
                return component
            except StopIteration:
                self.stack.pop()
        raise StopIteration()


tags = Div(
    P(
        Div(),
        P(),
    ),
    Div(
        P(),
    ),
)


def to_html(component, indent=0):
    result = " " * indent * 4 + f"<{component}>\n"

    if isinstance(component, Composite):
        for child in component.children:
            result += to_html(child, indent + 1)

    result += " " * indent * 4 + f"</{component}>\n"
    return result


print(to_html(tags))
print("Flattened: ", *tags)
