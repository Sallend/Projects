using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace HelpDesk.Client
{
    class MonitorServer
    {       
        public MonitorServer()// string clientPort
        {
            //string[] ipServerPort = serverIP.Split(':');
            //_serverIP = ipServerPort[0];
            //port = Convert.ToInt32(ipServerPort[1]);
        }

        public void Start()
        {

            int bufferSize = 1024;
            byte[] buffer = null;
            byte[] header = null;
            string headerStr = "";
            int filesize = 0;

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (enabled)
            {
                //Thread.Sleep(1000);
                Socket socket = listener.AcceptSocket();
                header = new byte[bufferSize];

                socket.Receive(header);

                headerStr = Encoding.ASCII.GetString(header);

                //Get filesize from header
                filesize = Convert.ToInt32(headerStr);


                int bufferCount = Convert.ToInt32(Math.Ceiling((double)filesize / (double)bufferSize));

                //System.Windows.Forms.MessageBox.Show(header.Length.ToString() + "|" + filesize + "|" + bufferSize +"|" + bufferCount.ToString());
                MemoryStream fs = new MemoryStream();
                //fs.Position = 0;
                while (filesize > 0)
                {

                    buffer = new byte[bufferSize];

                    int size = socket.Receive(buffer, SocketFlags.Partial);

                    fs.Write(buffer, 0, size);

                    filesize -= size;

                }
                fs.Position = 0;

                Dictionary<string, List<object>> response = (Dictionary<string, List<object>>)new BinaryFormatter().Deserialize(fs);
                string command = response.Keys.ToArray()[0];
                List<object> param;
                switch (command)
                {
                    case "getServerStatusErr": // !!
                        param = response[command];
                        ClientEvent.SendMessageDataTable((DataTable)param[0]);
                        break;
                    case "getStatusHelpDeskInfo": // !!
                        param = response[command];
                        ClientEvent.SendMessage((List<string>)param[0]);
                        break;
                }
            }
        }

        public void Stop()
        {
            enabled = false;
        }
                
        public static void sendDataToServer(object myObj, string[] ipServerPort)
        {
            int bufferSize = 1024;
            byte[] buffer = null;
            byte[] header = null;
            //   BinaryFormatter.BinaryConverter bf = new BinaryFormatter.BinaryConverter();
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream fs = new MemoryStream();
            bf.Serialize(fs, myObj);
            //   bf.Serialize(myObj, fs);
            //  byte[] fdf =   bf.Serialize(myObj);
            fs.Position = 0;

            int bufferCount = Convert.ToInt32(Math.Ceiling((double)fs.Length / (double)bufferSize));
            
            TcpClient tcpClient = new TcpClient(ipServerPort[0], Convert.ToInt32(ipServerPort[1]));
            tcpClient.SendTimeout = 600000;
            tcpClient.ReceiveTimeout = 600000;

            string headerStr = fs.Length.ToString();
            header = new byte[bufferSize];
            Array.Copy(Encoding.ASCII.GetBytes(headerStr), header, Encoding.ASCII.GetBytes(headerStr).Length);

            tcpClient.Client.Send(header);

            for (int i = 0; i < bufferCount; i++)
            {
                buffer = new byte[bufferSize];
                int size = fs.Read(buffer, 0, bufferSize);

                tcpClient.Client.Send(buffer, size, SocketFlags.Partial);

            }
            tcpClient.Client.Close();
            fs.Close();
        }      

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private static string _serverIP = string.Empty;
        private static string _pletorFolder = string.Empty;
        public static int port = 777;

        object obj = new object();
        bool enabled = true;

    }
}
