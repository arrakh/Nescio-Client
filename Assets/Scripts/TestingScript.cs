using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    TcpClient clientSocket = new TcpClient();

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
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(inputField.text);

            textbox.text += "Client sends: " + inputField.text + Environment.NewLine;

            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returnData = Encoding.ASCII.GetString(inStream); 

            textbox.text += "Server recieves sends: " + returnData + Environment.NewLine;
        }
        else
        {
            textbox.text += "<color=\"red\">Error! Input field is Empty!</color>" + Environment.NewLine;
        }
    }

    private void OnDisable()
    {
        clientSocket.Close();
    }

    IEnumerator ConnectCoroutine(string address, int port)
    {
        clientSocket.ConnectAsync(address, port);
        yield return new WaitUntil(() => clientSocket.Connected);
        //Client is connected
        Debug.Log("Client Connected!");
    }
}
