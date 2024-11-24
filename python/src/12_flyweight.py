import string


class CharacterStyle:
    def __init__(self, font, size, bold, italic):
        self.font = font
        self.size = size
        self.bold = bold
        self.italic = italic


class Character:
    def __init__(self, char, style):
        self.char = char
        self.style = style

    def render(self):
        bold = "bold" if self.style.bold else "normal"
        italic = "italic" if self.style.italic else "normal"
        print(
            f"Rendering '{self.char}' in {self.style.font}, size {self.style.size}, {bold}, {italic}"
        )


class CharacterFactory:
    def __init__(self):
        self.characters = {}

    def get_character(self, char, style):
        key = (char, style.font, style.size, style.bold, style.italic)
        if key not in self.characters:
            self.characters[key] = Character(char, style)
        return self.characters[key]


class Document:
    def __init__(self):
        self.characters = []
        self.factory = CharacterFactory()

    def add_character(self, char, font, size, bold=False, italic=False):
        style = CharacterStyle(font, size, bold, italic)
        character = self.factory.get_character(char, style)
        self.characters.append(character)

    def render(self):
        for character in self.characters:
            character.render()


if __name__ == "__main__":
    document = Document()

    text = "Some text with repeating characters!"
    for char in text:
        if char in string.ascii_uppercase:
            document.add_character(char, "Arial", 12, bold=True)
        elif char in string.ascii_lowercase:
            document.add_character(char, "Times New Roman", 10)
        else:
            document.add_character(char, "Courier", 11, italic=True)

    document.render()

    print(f"\nNumber of unique character objects: {len(document.factory.characters)}")
    print(f"\nNumber of characters: {len(document.characters)}")
