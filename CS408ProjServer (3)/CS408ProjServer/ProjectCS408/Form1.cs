using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;//imported for file not sure if it works //not file but FOLDER
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProjectCS408
{
    public struct myClient
    {
        public Socket clientSocket;
        public string userName;
        public int numOfFiles;
        //List<String> fileNames = new List<String>();

        public myClient(Socket clientSocket, string userName)
        {
            this.clientSocket = clientSocket;
            this.userName = userName;
            this.numOfFiles = 0;
        }

    };

    public partial class Form1 : Form
    {

        //client upload edecegi dosyayi secemk icin browse
        //server browses to choose loaction for incoming txt files to be loaded.

        //create database:
        //StreamReader streamreader = new StreamReader(@"db.txt");
        string path = "";
        //StreamWriter sw = File.CreateText(path);



        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //byte array
        List<myClient> myClients = new List<myClient>();
        //in order to keep clients:
        List<Socket> clientSockets = new List<Socket>();//not necessary
        List<String> userNames = new List<string>();//not necesarry
        // since initially I am not listening any port:
        bool terminating = false;
        bool listening = false;
        int serverPort;



        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //disconnnect here TODO
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }


        private void connectButton_Click(object sender, EventArgs e)  //listen button
        {
            if (Int32.TryParse(portInput.Text, out serverPort)) // returns boolean
            {
                // bind new socket to port
                // in order to listen to a port:
                // using System.Net
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10); // 10 clients at max can listen

                listening = true;
                //button_listen.Enabled = false;

                //accepts clients that are trying to connect
                // socket function accept
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                myLogOutput.AppendText("Started listening on port: " + serverPort + "\n");
                
            }

            else
            {
                myLogOutput.AppendText("Please check port number \n");
            }
        }

        private void Accept()
        {
            while (listening)
            {
                try
                {
                    //newly connected client socket:
                    //myClient myClient = new myClient();
                    //Socket newClient = serverSocket.Accept(); // This thread is blocked if there are no clients
                    //clientSockets.Add(newClient);
                    bool unique = true;

                    Socket clientSocket = serverSocket.Accept();
                    byte[] username = new Byte[64]; 
                    clientSocket.Receive(username);
                    string userName = Encoding.Default.GetString(username);
                    userName = userName.Trim('\0');

                    //checking for uniqueness
                    for (int i = 0; i < myClients.Count; i++)
                    {
                        if (myClients[i].userName.Equals(userName) ) // different than ==  // more stable
                        {
                            unique = false;
                        }
                    }

                    
                    if (unique)
                    {
                        userName = userName.Trim('\0');
                        myClient newClient = new myClient(clientSocket, userName);
                        myClients.Add(newClient);

                        Byte[] connectedMessage = new Byte[64];
                        string connectedCode = "connected";
                        connectedMessage = Encoding.ASCII.GetBytes(connectedCode);
                        clientSocket.Send(connectedMessage);

                        myLogOutput.AppendText("A client has connected with user name " + newClient.userName);//FOR SOME REASON WONT PRINT AFTER THE USERNAME                      
                        myLogOutput.AppendText("\n");

                        Thread receiveThread = new Thread(() => Receive(ref newClient)); // we take the usernames in receive func. 
                        receiveThread.Start();

                    }
                    else
                    {
                        myLogOutput.AppendText("A client failed to connect, duplicate user name\n");
                        terminating = true;

                        //addes by simay
                        Byte[] isDuplicate = new Byte[64];
                        string duplicateCode = "duplicate";
                        isDuplicate = Encoding.ASCII.GetBytes(duplicateCode);
                        clientSocket.Send(isDuplicate);
                        //finish
                        clientSocket.Close();//close socket
                    }
                }
                catch
                {
                    if (terminating) // disconnect button of any OR closed x
                    {
                        listening = false;
                        myClients.RemoveAt(myClients.Count-1);// doesnt work
                    }
                    else
                    {
                        myLogOutput.AppendText("The socket stopped working!\n");
                    }
                }
            }
        }

        private void Receive(ref myClient thisClient)
        {
            bool connected = true;
            while (connected && !terminating)
            {
                try
                {
                    // since we are getting byte as input:
                    
                    Byte[] buffer = new Byte[256];//what will the size be
                    thisClient.clientSocket.Receive(buffer);

                    //added by simay
                    //these are our headers.
                    string disconnectMessage = Encoding.Default.GetString(buffer);
                    disconnectMessage = disconnectMessage.Trim('\0');
                    //myLogOutput.AppendText(disconnetMessage);
                    if (disconnectMessage != "disconnected" && !disconnectMessage.StartsWith("incomingFile") ) //&& disconnetMessage != "duplicate") //updated by Simay
                    {
                        string userName = Encoding.Default.GetString(buffer);
                        thisClient.userName = userName;
                    }
                    //finish
                    else if (disconnectMessage.StartsWith("incomingFile "))
                    {
                        disconnectMessage = disconnectMessage.Substring(13, disconnectMessage.Length - 13);
                        int bufferSize = Int32.Parse(disconnectMessage.Split('/')[1]);
                        string fileName = disconnectMessage.Split('/')[2];
                        fileName = fileName.Split('\\')[fileName.Split('\\').Length-1]; // receive file name backslaha göre split edip almamız lazım!!!!TODO
                        string fileContent = disconnectMessage.Split('/')[3]; // receive file content
                        

                        //created txt after we browse the button.
                        //int numofBytes =thisClient.clientSocket.Receive(buffer);
                        for (int i = 0; i < (bufferSize/256); i++)
                        {
                            thisClient.clientSocket.Receive(buffer);
                            string cntmessage = Encoding.Default.GetString(buffer);
                            fileContent += cntmessage;
                            Array.Clear(buffer, 0, buffer.Length);
                        }
                        //clean buffer??
                        Array.Clear(buffer, 0, buffer.Length);
                        //add the name to database

                        //txt file name = username + filename 
                        if (thisClient.numOfFiles > 0)
                        {
                            fileName = thisClient.userName  + fileName.Substring(0,fileName.Length-4) + "-"+ Convert.ToString(thisClient.numOfFiles) + ".txt";
                            thisClient.numOfFiles +=1;
                        }
                        else
                        {
                            fileName = thisClient.userName + fileName;
                            thisClient.numOfFiles += 1;
                        }

                        myLogOutput.AppendText("File from: " + thisClient.userName + " arrived with filename: " + fileName +"\n");

                        // Write to database the usernames and their filenames.

                        StreamWriter w;
                        using ( w = File.AppendText(path + "\\Database.txt"))
                        {                           
                            string s = thisClient.userName + " " + fileName;
                            w.WriteLine(s);
                            //w.
                        }

                        try
                        {
                            if (!File.Exists(path))
                            {
                                // Create a file to write sent data to.
                                using (StreamWriter sw = File.CreateText(path + "\\" + fileName))
                                {
                                    sw.WriteLine(fileContent);
                                }
                            }
                            else //file already exists
                            {
                                
                            }
                            
                        }
                        catch
                        {
                            myLogOutput.AppendText("Couldn't open sent file as new text file... \n");
                        }

                    }
                    else
                    {
                        myLogOutput.AppendText("A client has disconnected with user name " + thisClient.userName);
                        myLogOutput.AppendText("\n");//weird fix but ok
                        thisClient.clientSocket.Close();
                        connected = false;
                        myClients.Remove(thisClient);
                    }                                                 
                }
                catch
                {
                    // if client cannot send a message for some reason
                    if (!terminating)
                    {
                        myLogOutput.AppendText("A client has disconnected with user name " + thisClient.userName);
                        myLogOutput.AppendText("\n");//weird fix but ok
                        myClients.RemoveAt(myClients.Count - 1);
                    }
                    thisClient.clientSocket.Close(); // dont forget
                    connected = false;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();          
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;
                textBoxDirectory.AppendText(path);
            }

            try
            {
                if (!File.Exists(path)) // folder path
                {
                    // Create a file to write to.
                    if(!File.Exists(path + "\\Database.txt" )) // folder path + database.txt
                    {
                        using (StreamWriter sw = File.CreateText(path + "\\Database.txt"))
                        {
                            /*sw.WriteLine("Hello");
                            sw.WriteLine("And");
                            sw.WriteLine("Welcome");*/
                        }
                    }                                       
                }
                else
                {

                }                
            }
            catch 
            {
                myLogOutput.AppendText("Couldn't open database \n");
            }

            buttonBrowse.Enabled = false;
            listenButton.Enabled = true;
        }
    }
}