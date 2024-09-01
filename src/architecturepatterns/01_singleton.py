from threading import Lock, Thread
from dataclasses import dataclass


class SingletonMeta(type):
    _instances = {}
    _lock: Lock = Lock()

    def __call__(cls, *args, **kwargs):
        with cls._lock:
            if cls not in cls._instances:
                instance = super().__call__(*args, **kwargs)
                cls._instances[cls] = instance
        return cls._instances[cls]


@dataclass
class Notary(metaclass=SingletonMeta):
    uid: int
    name: str
    phone: str
    email: str
    address: str


def test_singleton(uid: int, name: str, phone: str, email: str, address: str) -> None:
    notary = Notary(uid, name, phone, email, address)
    print(notary)


if __name__ == "__main__":
    process1 = Thread(target=test_singleton,
                      args=(5, "FOO", "05498563", "me@email.com", "address"))
    process2 = Thread(target=test_singleton,
                      args=(8, "BAR", "09743234", "me@email.com", "address"))
    process1.start()
    process2.start()
