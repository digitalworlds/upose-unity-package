using edu.ufl.digitalworlds.upose;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;



public class StartMediaPipe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    System.Diagnostics.Process activecheck;
    System.Diagnostics.Process activecheckassist;
    private bool test = false;
    ServerUDP server_1;
    string finalpath;
    private UdpClient client;


    public string IP = "127.0.0.1"; // Target IP
    public int port = 52734;        // Target Port
    public int portlisten = 52735;        // Target Port

    public static bool continueon = false;

    bool killpipe = false;


    public void SendData2(string message)
    {
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPAddress serverAddr = IPAddress.Loopback;

        IPEndPoint endPoint = new IPEndPoint(serverAddr, port);

        string text = message;
        byte[] send_buffer = Encoding.ASCII.GetBytes(text);

        sock.SendTo(send_buffer, endPoint);
    }

    public void SendData(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, IP, port);
            Debug.Log("Sent: " + message + " to " + IP + ":" + port);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }


    System.Diagnostics.ProcessStartInfo startInfo;
    System.Diagnostics.ProcessStartInfo endInfo;
    void SetUP()
    {
        startInfo = new System.Diagnostics.ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = "/k title mediapipe&&" + "cd " + System.IO.Path.GetFullPath("Packages/edu.ufl.digitalworlds.upose/Python") + " && conda activate mediapipe && python run_mediapipe_automatic.py",
            
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = true,
            CreateNoWindow = true
        };


    }
    void Start()
    {

        Application.runInBackground = true;
        finalpath = Path.Combine(Application.streamingAssetsPath, "sharedinfo.txt");
        Thread t = new Thread(new ThreadStart(CheckProcess));
        t.Start();
    }

    void CheckProcess()
    {
        ServerUDP server = new ServerUDP(IP, portlisten);
        server.Connect();
        server.StartListeningAsync();
        while (true)
        {
            if (server.HasMessage())
            {
                string activecheck = server.GetMessage();
                if (killpipe)
                {

                    endInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        Arguments = "/k cd %userprofile%&&taskkill /PID " + activecheck + " /T /F",
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        UseShellExecute = false,
                        CreateNoWindow = false
                    };
                    activecheckassist = System.Diagnostics.Process.Start(endInfo);
                    killpipe = false;
                }
                Debug.Log(activecheck + " is the process id number");
            }
        }
    }
    int number = 0;
    private void FixedUpdate()
    {
        if (test)
        {
            number++;
            if (number > 10000)
            {
                number = 0;
            }
            SendData(number.ToString());
        }

    }
    public void startmediapipe()
    {
        client = new UdpClient();
        SetUP();
        activecheck = System.Diagnostics.Process.Start(startInfo);
        test = true;
        killpipe = false;
        SendData("Start");




    }
    public void endmediapipe()
    {
        killpipe = true;


    }

}
[CustomEditor(typeof(StartMediaPipe))]
public class AutoManagerGearEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        StartMediaPipe server_1 = (StartMediaPipe)target;
        if (GUILayout.Button("Start MediaPipe"))
        {
            server_1.startmediapipe();
        }
        if (GUILayout.Button("End MediaPipe"))
        {
            server_1.endmediapipe();
        }

    }


}