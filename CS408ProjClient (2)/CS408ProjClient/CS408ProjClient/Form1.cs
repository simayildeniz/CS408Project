using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//SORUNLAR
//1. Önce server uygulaması çalışınca co nnection kurulduktan sonra server zorla kapanınca hata geliyor.

namespace CS408ProjClient
{
    public partial class Form1 : Form
    {
     
        bool terminating = false;
        bool connected = false;
        string filePath = "";
        Socket clientSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = IPInput.Text;

            int portNum;
            if (Int32.TryParse(PortInput.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    connectButton.Enabled = false;
                    disconnectButton.Enabled = true;

                    Byte[] myUserName = new Byte[64];
                    string userName = userNameInput.Text;
                    myUserName = Encoding.ASCII.GetBytes(userName);
                    clientSocket.Send(myUserName);

                    Byte[] userNameAck = new Byte[64];
                    clientSocket.Receive(userNameAck);
 
                    string UserNameAck = Encoding.Default.GetString(userNameAck).Trim('\0');

                    if (UserNameAck == "connected")
                    {
                        connected = true;
                        myLog.AppendText("Connected to the server!\n");
                        button1.Enabled = true; // browse is enabled       
                        Thread receiveThread = new Thread(Receive);
                        receiveThread.Start();
                    }
                    else if (UserNameAck == "duplicate")
                    {
                        myLog.AppendText("Connection failed due to already existing username...");
                        disconnectButton.Enabled = false;  
                        connectButton.Enabled = true;
                        connected = false;
                    }                   

                }
                catch
                {
                    myLog.AppendText("Could not connect to the server!\n");
                    disconnectButton.Enabled = false;
                    connectButton.Enabled = true;
                }
            }
            else
            {
                myLog.AppendText("Check the port\n");
            }
        }
       
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //send message to server that you are disconnected
            

            if (connected)
            {
                Byte[] isDisconnect = new Byte[64];
                string disconnectCode = "disconnected";
                isDisconnect = Encoding.ASCII.GetBytes(disconnectCode);
                clientSocket.Send(isDisconnect);
                connectButton.Enabled = false;
                disconnectButton.Enabled = false;
                connected = false;
                clientSocket.Close();
            }
            //connect etmeden önce form kapanınca client hata veriyor bu satırda

            
            //myLog.AppendText("You are disconnected...");
            

            terminating = true;
            Environment.Exit(0);
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {

            Byte[] isDisconnect = new Byte[64];
            string disconnectCode = "disconnected";
            isDisconnect = Encoding.ASCII.GetBytes(disconnectCode);
            clientSocket.Send(isDisconnect);

            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
            myLog.AppendText("You are disconnected...");

            connected = false;
            clientSocket.Close(); 
 
            terminating = true;
        }

        private void button1_Click(object sender, EventArgs e) //browse button
        {
            using (var opnDlg = new OpenFileDialog())
            {
                try
                {
                    if (opnDlg.ShowDialog() == DialogResult.OK)
                    {
                        filePath = opnDlg.FileName;
                        filePathInput.AppendText(filePath);
                        button2.Enabled = true; //upload is enabled now
                    }
                }
                catch // burada try catch lazim mi dunno
                {
                    myLog.AppendText("Wrong Directory Entered\n");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //send button
        {
            //Kerem's suggestion:
            //1. dosyanin icerigini oku string olarak
            //byte arraylere bol
            //for loop icinde tek tek yolla
            try
            {
                string[] directoryList = filePathInput.Text.Split('/');
                string fileName = directoryList[directoryList.Length - 1];

                
                myLog.AppendText("Uploading File...\n");
                string headerText = "incomingFile ";
                string contentText = System.IO.File.ReadAllText(@filePath);//doesnt work for some reason

                int fileSize = headerText.Length + fileName.Length + contentText.Length + 8;
                string textToSend = headerText + "/" + fileSize + "/" + fileName + "/" + contentText ;
                Byte[] outGoingText = new Byte[fileSize];
                outGoingText = Encoding.ASCII.GetBytes(textToSend);
                clientSocket.Send(outGoingText);
                myLog.AppendText("File is sent successfully...\n");
            }
            catch (Exception E)
            {
                myLog.AppendText("There has been an error while uploading the file...\n");
                throw;
            }
        }

        private void Receive()
        {
           while (connected)
           {
               try
               {
                    /*//added today
                   Byte[] isDisconnect = new Byte[64];
                   string disconnectCode = "disconnected";
                   isDisconnect = Encoding.ASCII.GetBytes(disconnectCode);
                   clientSocket.Send(isDisconnect);
                    //finish add*/

                   Byte[] buffer = new Byte[256];
                   clientSocket.Receive(buffer);

                   string incomingMessage = Encoding.Default.GetString(buffer);
                   incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                   myLog.AppendText("Server's message: " + incomingMessage + "\n");

                    if (incomingMessage == "duplicate")
                    {
                        myLog.AppendText("Connection failed due to already existing username...\n");
                        disconnectButton.Enabled = false;
                        connectButton.Enabled = true;

                        connected = false;
                    }
               }
               catch
               {
                   if (!terminating)
                   {
                        myLog.AppendText("The server has disconnected\n");
                        connectButton.Enabled = false;
                        disconnectButton.Enabled = false;
                        button1.Enabled = false;
                        button2.Enabled = false;      
                        myLog.AppendText("Please try again by another client socket...\n");
                    }

                   clientSocket.Close();
                   connected = false;
               }
           }
       } 
    }
}