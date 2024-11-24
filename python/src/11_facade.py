import bcrypt


class PasswordHasher:
    def __init__(self, rounds=12):
        self.rounds = rounds

    def get_password_hash(self, password):
        salt = bcrypt.gensalt(rounds=self.rounds)
        return bcrypt.hashpw(password.encode("utf-8"), salt)

    def verify_password(self, plain_password, hashed_password) -> bool:
        return bcrypt.checkpw(plain_password.encode("utf-8"), hashed_password)


if __name__ == "__main__":
    hasher = PasswordHasher()

    original_password = "mysecretpassword123"

    hashed_password = hasher.get_password_hash(original_password)
    print(f"Hashed password: {hashed_password}")

    is_correct = hasher.verify_password(original_password, hashed_password)
    print(f"Is password correct? {is_correct}")

    wrong_password = "wrongpassword456"
    is_incorrect = hasher.verify_password(wrong_password, hashed_password)
    print(f"Is wrong password correct? {is_incorrect}")
