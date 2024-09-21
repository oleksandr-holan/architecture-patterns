from abc import ABC, abstractmethod


class IClothes(ABC):
    @abstractmethod
    def order_modern(self): ...

    @abstractmethod
    def order_classic(self): ...


class Pants(IClothes):
    def order_modern(self):
        return ModernPants()

    def order_classic(self):
        return ClassicPants()


class Shirt(IClothes):
    def order_modern(self):
        return ModernShirt()

    def order_classic(self):
        return ClassicShirt()


class IModern(ABC):
    @abstractmethod
    def get_style(self):
        ...


class IClassic(ABC):
    @abstractmethod
    def get_style(self):
        ...

    @abstractmethod
    def match_with(self, modern_thing: IModern):
        ...


class ModernPants(IModern):
    def get_style(self):
        return "Slim fit, low-rise pants"


class ModernShirt(IModern):
    def get_style(self):
        return "Fitted, short-sleeved shirt"


class ClassicPants(IClassic):
    def get_style(self):
        return "Straight-leg, high-waisted pants"

    def match_with(self, modern_thing: IModern):
        modern_style = modern_thing.get_style()
        return f"{self.get_style()} paired with {modern_style}"


class ClassicShirt(IClassic):
    def get_style(self):
        return "Button-down, long-sleeved shirt"

    def match_with(self, modern_thing: IModern):
        modern_style = modern_thing.get_style()
        return f"{self.get_style()} paired with {modern_style}"


if __name__ == '__main__':
    def order_modern_clothes(clothes: IClothes):
        modern_clothes = clothes.order_modern()
        print(modern_clothes.get_style())
        return modern_clothes
    
    def order_classic_clothes(clothes: IClothes):
        classic_clothes = clothes.order_classic()
        print(classic_clothes.get_style())
        return classic_clothes


    def pair_classic_with_modern(classic_clothes: IClassic, modern_clothes: IModern):
        match = classic_clothes.match_with(modern_clothes)
        print(match)
        return match


    modern_pants = order_modern_clothes(Pants())
    modern_shirt = order_modern_clothes(Shirt())
    
    classic_pants = order_classic_clothes(Pants())
    classic_shirt = order_classic_clothes(Shirt())

    pair_classic_with_modern(classic_shirt, modern_pants)
    pair_classic_with_modern(classic_shirt, modern_shirt)
