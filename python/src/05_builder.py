from abc import ABC, abstractmethod
from dataclasses import dataclass


@dataclass
class Component(ABC):
    manufacturer: str


@dataclass
class Motherboard(Component):
    chipset: str


@dataclass
class CPU(Component):
    cores: int


@dataclass
class GPU(Component):
    memory_frequency: int


@dataclass
class PSU(Component):
    power: int


@dataclass
class Case(Component):
    form_factor: str


@dataclass
class RAM(Component):
    frequency: int


@dataclass
class Storage(ABC, Component):
    capacity: int


@dataclass
class SSD(Storage):
    memory_type: int


@dataclass
class HDD(Storage):
    rpm: int


@dataclass
class Cooler(Component):
    height: int


@dataclass
class Fan(Component):
    size: int


@dataclass
class PC:
    motherboard: Motherboard
    cpu: CPU
    gpu: GPU
    psu: PSU
    ram: RAM
    storage: Storage
    cooler: Cooler
    fans: list[Fan]
    def __init__(self):
        ...


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
    def __init__(self):
        self._pc = None
        self.motherboard = None


    def reset(self):
        self._pc = PC()

    def set_motherboard(self, motherboard):
        self.motherboard = motherboard