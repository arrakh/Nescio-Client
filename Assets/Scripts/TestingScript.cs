using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    TcpClient clientSocket = new TcpClient();

    StreamReader reader;
    StreamWriter writer;

    [SerializeField] private TMP_Text textbox;
    [SerializeField] private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        textbox.text = "";
        StartCoroutine(ConnectCoroutine("127.0.0.1", 3000));
    }

    public void SendData()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            try
            {
                //byte[] outStream = Encoding.ASCII.GetBytes(inputField.text);

                //Debug.Log("Sent: " + inputField.text);
                //textbox.text += "Client sends: " + inputField.text + Environment.NewLine;

                //serverStream.Write(outStream, 0, outStream.Length);
                //serverStream.Flush();

                //byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
                //serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                //string returnData = Encoding.ASCII.GetString(inStream);

                //textbox.text += "Server recieves sends: " + returnData + Environment.NewLine;
                //Debug.Log("Receives: " + returnData);

                //inputField.text = "";

                writer.WriteLine(inputField.text);
                textbox.text += "Client sends: " + inputField.text + Environment.NewLine;
                writer.Flush();

                textbox.text += "Recieved from server: " + reader.ReadLine() + Environment.NewLine;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            textbox.text += "<color=\"red\">Error! Input field is Empty!</color>" + Environment.NewLine;
        }
    }
    
    private void OnDisable()
    {
        reader.Close();
        writer.Close();
        clientSocket.Close();
    }

    IEnumerator ConnectCoroutine(string address, int port)
    {
        clientSocket.Connect(address, port);
        reader = new StreamReader(clientSocket.GetStream());
        writer = new StreamWriter(clientSocket.GetStream());

        yield return new WaitUntil(() => clientSocket.Connected);
        //Client is connected
        Debug.Log("Client Connected!");
    }
}
