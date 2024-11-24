import json
from abc import ABC, abstractmethod

import requests


class APIClient(ABC):
    @abstractmethod
    def get(self, url, headers=None):
        pass

    @abstractmethod
    def post(self, url, data, headers=None):
        pass


class RequestsAdapter(APIClient):
    def __init__(self, session=None):
        self.session = session or requests.Session()

    def get(self, url, headers=None):
        response = self.session.get(url, headers=headers)
        return self._adapt_response(response)

    def post(self, url, data, headers=None):
        response = self.session.post(url, json=data, headers=headers)
        return self._adapt_response(response)

    def _adapt_response(self, response):
        return {
            "status_code": response.status_code,
            "headers": dict(response.headers),
            "body": response.json()
            if response.headers.get("Content-Type") == "application/json"
            else response.text,
        }


def fetch_user_data(api_client, user_id):
    response = api_client.get(f"https://jsonplaceholder.typicode.com/users/{user_id}")
    if response["status_code"] == 200:
        return response["body"]
    else:
        raise Exception(f"Failed to fetch user data: {response['status_code']}")


def create_user(api_client: APIClient, user_data):
    response = api_client.post("https://graph.microsoft.com/v1.0/users", data=user_data)
    if response["status_code"] == 201:
        return response["body"]
    else:
        raise Exception(f"Failed to create user: {response['status_code']}")


if __name__ == "__main__":
    api_client = RequestsAdapter()

    try:
        user = fetch_user_data(api_client, 1)
        print(f"Fetched user: {user}")
    except Exception as e:
        print(str(e))

    data = {
        "accountEnabled": True,
        "displayName": "Adele Vance",
        "mailNickname": "AdeleV",
        "userPrincipalName": "AdeleV@contoso.com",
        "passwordProfile": {
            "forceChangePasswordNextSignIn": True,
            "password": "xWwvJ]6NMw+bWH-d",
        },
    }
    data_json = json.dumps(data)
    try:
        new_user = create_user(api_client, user_data=data_json)
        print(f"Created user: {new_user}")
    except Exception as e:
        print(str(e))
