# filepath: e:\UnityLearn\QuantSignalServer\Test.py
import requests
import urllib3
import time

urllib3.disable_warnings()  # 忽略 InsecureRequestWarning

class QuantSignalClient:
    def __init__(self, base_url):
        self.base_url = base_url
        self.token = None

    def register(self, username, password):
        url = f"{self.base_url}/api/Auth/register"
        resp = requests.post(url, json={"username": username, "password": password}, verify=False)
        self._print_response(resp)
        return resp

    def login(self, username, password):
        url = f"{self.base_url}/api/Auth/login"
        resp = requests.post(url, json={"username": username, "password": password}, verify=False)
        data = resp.json()
        if data.get("token"):
            self.token = data["token"]
            print("登录成功，token已保存")
        else:
            print("登录失败:", data)
        self._print_response(resp)
        return resp

    def _auth_headers(self):
        return {"Authorization": f"Bearer {self.token}"} if self.token else {}

    def register_strategy(self, name, forward_targets):
        url = f"{self.base_url}/api/Strategy"
        data = {"name": name, "forwardTargets": forward_targets}
        resp = requests.post(url, json=data, headers=self._auth_headers(), verify=False)
        self._print_response(resp)
        return resp

    def send_signal(self, strategy_name, signal_data):
        url = f"{self.base_url}/api/Signal/{strategy_name}"
        resp = requests.post(url, json=signal_data, headers=self._auth_headers(), verify=False)
        self._print_response(resp)
        return resp

    def get_signals(self, strategy_name):
        url = f"{self.base_url}/api/Signal/{strategy_name}"
        resp = requests.get(url, headers=self._auth_headers(), verify=False)
        self._print_response(resp)
        return resp
    
    def _print_response(self, resp):
        print(f"状态码: {resp.status_code}, Content-Type: {resp.headers.get('Content-Type')}")
        print("请求头:", resp.request.headers)
        try:
            print(resp.json())
        except Exception:
            print(resp.text)
    
if __name__ == "__main__":
    client = QuantSignalClient("https://localhost:63737")

    # 注册
    client.register("testuser", "testpassword")
    time.sleep(1)
    # 登录
    client.login("testuser", "testpassword")
    time.sleep(1)
    # 注册策略
    client.register_strategy("demo_strategy", ["http://target1.com", "http://target2.com"])
    time.sleep(1)
    # 发送交易信号
    client.send_signal("demo_strategy", {"symbol": "BTCUSDT", "action": "buy", "price": 30000})
    time.sleep(1)
    # 获取交易信号
    client.get_signals("demo_strategy")