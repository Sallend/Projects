using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelpDesk.Server
{
    internal class Server
    {
        public Server(int port = 0)
        {
            if (port != 0) this.port = port;
            file = new StreamWriter(new FileStream(@"c:\helpDeskServer.log", System.IO.FileMode.Append));
            file.WriteLine("HelpDeskServer стартовал " + DateTime.Now.ToString());
            file.Flush();

            ThreadStart start = new ThreadStart(startserver);
            Thread receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void startserver()
        {
            int bufferSize = 1024;
            byte[] buffer = null;
            byte[] header = null;
            string headerStr = "";
            int filesize = 0;
            string[] incomingHeaderData;
            string command;
            string filename;

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                try
                {
                    Socket socket = listener.AcceptSocket();
                    header = new byte[bufferSize];

                    socket.Receive(header);

                    headerStr = Encoding.GetEncoding(1251).GetString(header);                    
                    filesize = Convert.ToInt32(headerStr);

                    int bufferCount = Convert.ToInt32(Math.Ceiling((double)filesize / (double)bufferSize));

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

                    Dictionary<long, List<Dictionary<long, object>>> obj = (Dictionary<long, List<Dictionary<long, object>>>)new BinaryFormatter().Deserialize(fs);
                    foreach (long key in obj.Keys)
                    {
                        switch (key)
                        {
                            case 1://системный код интерактивной работы с сервером напрямую (резерв)                            
                                break;
                            case 2: // HelpDesk
                                InteractService iService = new InteractService(obj[key]);
                                break;
                        }
                    }

                    file.WriteLine("Данные успешно приняты " + DateTime.Now.ToString());
                    file.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    file.Flush();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    file.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    file.WriteLine("Данные не приняты " + DateTime.Now.ToString());
                    file.WriteLine(ex.ToString());
                    file.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    file.Flush();
                }

            }
        }

        public static void sendDataToServer(object myObj, IPAddress ServerIP, int portClient) //test
        {

            int bufferSize = 1024;
            byte[] buffer = null;
            byte[] header = null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream fs = new MemoryStream();
            bf.Serialize(fs, myObj);
            fs.Position = 0;


            int bufferCount = Convert.ToInt32(Math.Ceiling((double)fs.Length / (double)bufferSize));

            TcpClient tcpClient = new TcpClient(ServerIP.ToString(), portClient);
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

        private StreamWriter file;
        private int port = 500; //def 500
    }
}
