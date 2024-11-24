import logging
from functools import wraps

# Configure logging
logging.basicConfig(
    level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s"
)


class LoggingProxy:
    def __init__(self, target):
        self._target = target

    def __getattr__(self, name):
        attr = getattr(self._target, name)
        if callable(attr):
            return self._log_and_call(attr, name)
        return attr

    def _log_and_call(self, method, method_name):
        @wraps(method)
        def wrapper(*args, **kwargs):
            args_repr = [repr(a) for a in args]
            kwargs_repr = [f"{k}={v!r}" for k, v in kwargs.items()]
            signature = ", ".join(args_repr + kwargs_repr)

            logging.info(
                f"Calling {self._target.__class__.__name__}.{method_name}({signature})"
            )

            try:
                result = method(*args, **kwargs)
                logging.info(
                    f"{self._target.__class__.__name__}.{method_name} returned {result!r}"
                )
                return result
            except Exception as e:
                logging.exception(
                    f"Exception in {self._target.__class__.__name__}.{method_name}: {str(e)}"
                )
                raise

        return wrapper


class Calculator:
    def add(self, a, b):
        return a + b

    def divide(self, a, b):
        return a / b


calc = LoggingProxy(Calculator())

result = calc.add(5, 3)
print(f"5 + 3 = {result}")

try:
    result = calc.divide(10, 0)
except ZeroDivisionError:
    print("Caught ZeroDivisionError")

try:
    calc.multiply(4, 2)
except AttributeError:
    print("Caught AttributeError")
