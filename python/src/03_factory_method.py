from abc import ABC, abstractmethod


class IService(ABC):
    @abstractmethod
    def get_required_documents(self): ...


class ServiceOne(IService):
    __required_documents = ["passport", "id number"]

    def get_required_documents(self):
        return self.__required_documents


class ServiceTwo(IService):
    __required_documents = ["passport", "id number", "wedding certificate"]

    def get_required_documents(self):
        return self.__required_documents


class ServiceOrdering(ABC):
    @staticmethod
    @abstractmethod
    def order(): ...


class ServiceOneOrdering(ServiceOrdering):
    @staticmethod
    def order():
        return ServiceOne()


class ServiceTwoOrdering(ServiceOrdering):
    @staticmethod
    def order():
        return ServiceTwo()


if __name__ == "__main__":
    def print_required_documents(service: IService):
        print(service.get_required_documents())


    service1: IService = ServiceOneOrdering().order()
    service2: IService = ServiceTwoOrdering().order()
    print("ServiceOne required documents:")
    print_required_documents(service1)
    print("ServiceTwo required documents:")
    print_required_documents(service2)
