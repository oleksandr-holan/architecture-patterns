from abc import ABC, abstractmethod
from dataclasses import dataclass
from enum import Enum


@dataclass
class Component(ABC):
    manufacturer: str = None


@dataclass
class Motherboard(Component):
    chipset: str = None


@dataclass
class CPU(Component):
    cores: int = None


@dataclass
class GPU(Component):
    memory_frequency: int = None


@dataclass
class PSU(Component):
    power: int = None


@dataclass
class Case(Component):
    form_factor: str = None


@dataclass
class RAM(Component):
    frequency: int = None


@dataclass
class Storage(Component, ABC):
    capacity: int = None


@dataclass
class SSD(Storage):
    memory_type: int = None


@dataclass
class HDD(Storage):
    rpm: int = None


@dataclass
class Cooler(Component):
    height: int = None


@dataclass
class Fan(Component):
    size: int = None


@dataclass
class PC(ABC):
    motherboard: Motherboard = None
    cpu: CPU = None
    gpu: GPU = None
    psu: PSU = None
    ram: RAM = None
    storage: Storage = None
    cooler: Cooler = None
    fans: list[Fan] = None


@dataclass
class OfficePC(PC):
    def do_work(self):
        return f"Imma work on {self}!"


@dataclass
class GamingPC(PC):
    def play_games(self):
        return f"Imma play games on {self}!"


# @dataclass
class PCBuilder(ABC):

    @abstractmethod
    def build(self): ...

    @abstractmethod
    def set_motherboard(self, motherboard): ...

    @abstractmethod
    def set_cpu(self, cpu): ...

    @abstractmethod
    def set_gpu(self, gpu): ...

    @abstractmethod
    def set_psu(self, psu): ...

    @abstractmethod
    def set_ram(self, ram): ...

    @abstractmethod
    def set_storage(self, storage): ...

    @abstractmethod
    def set_cooler(self, cooler): ...

    @abstractmethod
    def set_fans(self, fans): ...


class GamingPCBuilder(PCBuilder):
    def set_fans(self, fans: list[Fan]):
        if len(fans) < 2:
            raise ValueError("There should be at least two fans!")
        self._pc.fans = fans
        return self

    def set_cooler(self, cooler: Cooler):
        self._pc.cooler = cooler
        return self

    def set_storage(self, storage: Storage):
        self._pc.storage = storage
        return self

    def set_ram(self, ram: RAM):
        self._pc.ram = ram
        return self

    def set_psu(self, psu: PSU):
        self._pc.psu = psu
        return self

    def set_gpu(self, gpu: GPU):
        self._pc.gpu = gpu
        return self

    def set_cpu(self, cpu: CPU):
        self._pc.cpu = cpu
        return self

    def set_motherboard(self, motherboard: Motherboard):
        self._pc.motherboard = motherboard
        return self

    def build(self):
        pc = self._pc
        self.__init__()
        return pc

    def __init__(self):
        self._pc = GamingPC()


class OfficePCBuilder(PCBuilder):
    def set_fans(self, fans: list[Fan]):
        self._pc.fans = fans
        return self

    def set_cooler(self, cooler: Cooler):
        self._pc.cooler = cooler
        return self

    def set_storage(self, storage: Storage):
        self._pc.storage = storage
        return self

    def set_ram(self, ram: RAM):
        self._pc.ram = ram
        return self

    def set_psu(self, psu: PSU):
        self._pc.psu = psu
        return self

    def set_gpu(self, gpu: GPU):
        if gpu not in OfficeComponents.GPU.value:
            raise ValueError(
                "It seems your coworker is trying to fool you and by a gaming pc to play games at work")
        self._pc.gpu = gpu
        return self

    def set_cpu(self, cpu: CPU):
        self._pc.cpu = cpu
        return self

    def set_motherboard(self, motherboard: Motherboard):
        self._pc.motherboard = motherboard
        return self

    def build(self):
        pc = self._pc
        self.__init__()
        return pc

    def __init__(self):
        self._pc = OfficePC()


class OfficeComponents(Enum):
    GPU: list[GPU] = [GPU("AMD", 1000)]


if __name__ == "__main__":
    cpu = CPU("AMD", 8)
    office_gpu = OfficeComponents.GPU.value[0]
    gaming_gpu = GPU("Nvidia", 7500)
    gaming_pc_builder = GamingPCBuilder()
    office_pc_builder = OfficePCBuilder()

    gaming_pc = (gaming_pc_builder
                 .set_cpu(cpu)
                 .set_fans([Fan(size=120), Fan(size=120)])
                 .build()
                 )

    print(gaming_pc)

    office_pc = (office_pc_builder
                 .set_fans([Fan(size=90), Fan(size=90)])
                 .set_gpu(office_gpu)
                 .build()
                 )

    print(office_pc)

    office_pc = (office_pc_builder
                 .set_fans([Fan(size=90), Fan(size=90)])
                 .set_gpu(gaming_gpu)
                 .build()
                 )

    print(office_pc)
