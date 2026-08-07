import socket
import time
import threading

class ClientUDP_Listener(threading.Thread):

    def run(self):
        self.connect()

    def __init__(self,ip,port, autoReconnect = True) -> None:
        threading.Thread.__init__(self)
        self.ip = ip
        self.port = port
        self.autoReconnect = autoReconnect
        self.connected = False
        pass

    def isConnected(self):
        return self.connected

    def sendMessage(self,message):
        try:
            message = str('%s<EOM>'%message).encode('utf-8')
            self.socket.send(message)
        except ConnectionRefusedError as ex:
            print("Connection refused. Is server running?")
            self.disconnect()
        except ConnectionResetError as ex:
            print("Server was disconnected...")
            self.disconnect()
    
    def readMessage(self):
        socks=socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
        socks.bind((self.ip,self.port))
        socks.setblocking(0)
        try:
            
            data= socks.recv(1024)
            return data.decode()
        except BlockingIOError as ex:
            return 'failed'
    def clearbuffer(self):
        socks=socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
        socks.bind((self.ip,self.port))
        socks.setblocking(0)
        while True:
            try:
                socks.recv(65535)
            except BlockingIOError as ex:
                return "successclear"
        

    def disconnect(self):
        self.connected = False
        self.socket.close()
        if(self.autoReconnect):
            time.sleep(1)
            self.connect()

    def connect(self):
        try:
            self.socket = socket.socket(socket.AF_INET, 
                                        socket.SOCK_DGRAM)     
            print("Attempting Connection...")
            self.socket.connect((self.ip, self.port))
            print("Will send messages to "+str(self.socket.getpeername()))
            self.connected = True
        except ConnectionRefusedError as ex:
            print("Connection refused. Is server running?")
            self.disconnect()
        except ConnectionResetError as ex:
            print("Server was disconnected...")
            self.disconnect()
        