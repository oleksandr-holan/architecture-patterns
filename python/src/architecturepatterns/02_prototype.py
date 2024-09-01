import copy
from datetime import datetime


class Appointment:
    def __init__(self, uid: int, notary_public: str, time: datetime, services: list):
        self.uid = uid
        self.notary_public = notary_public
        self.time = time
        self.services = services

    def __copy__(self):
        notary_public = copy.copy(self.notary_public)
        time = copy.copy(self.time)
        services = copy.copy(self.services)

        new = self.__class__(
            self.uid, notary_public, time, services
        )
        new.__dict__.update(self.__dict__)

        return new

    def __deepcopy__(self, memo=None):
        if memo is None:
            memo = {}

        notary_public = copy.deepcopy(self.notary_public, memo)
        time = copy.deepcopy(self.time, memo)
        services = copy.deepcopy(self.services, memo)

        new = self.__class__(
            self.uid, notary_public, time, services
        )
        new.__dict__ = copy.deepcopy(self.__dict__, memo)

        return new


if __name__ == "__main__":
    appointment = Appointment(23, "Olesya Kindryk", datetime(2024, 9, 10, 13, 30), ["Witnessing Signatures"])

    shallow_copied_appointment = copy.copy(appointment)

    shallow_copied_appointment.services.append("Administering Oaths and Affirmations")
    if appointment.services[-1] == "Administering Oaths and Affirmations":
        print("Shallow copy is a pointer!")
    else:
        print("Shallow copy is not a pointer!")

    deep_copied_appointment = copy.deepcopy(appointment)

    deep_copied_appointment.services.append("Certifying Copies")
    if appointment.services[-1] == "Certifying Copies":
        print("Deep copy is a pointer!")
    else:
        print("Deep copy is not a pointer!")
