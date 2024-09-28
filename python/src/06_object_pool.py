from dataclasses import dataclass
import importlib
from abc import ABC, abstractmethod

singleton = importlib.import_module("01_singleton", package=None)


@dataclass
class PipelineAgent:
    image: str = None
    cpu_cores: int = None
    ram_capacity: int = None


class PipelineAgentsPool(metaclass=singleton.SingletonMeta):
    def __init__(self, agents_number):
        self._agens = [PipelineAgent() for _ in range(agents_number)]

    def get_agent(self):
        if not self._agens:
            raise Exception("No agents left")
        return self._agens.pop()

    def put_agent(self, agent: PipelineAgent):
        self._agens.append(agent)


if __name__ == '__main__':
    agents_pool = PipelineAgentsPool(3)
    try:
        agent1 = agents_pool.get_agent()
        agent2 = agents_pool.get_agent()
        agent3 = agents_pool.get_agent()
        agent4 = agents_pool.get_agent()
    except Exception as e:
        print(e)
