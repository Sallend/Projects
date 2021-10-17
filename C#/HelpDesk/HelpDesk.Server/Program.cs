using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerEvent.OnMessageConsiole += ServerEvent_MessageConsole;
            Console.WriteLine("Для остановки сервера закройте консоль или введите команду “exit”.\n");
            server = new Server();
            while(!exit)
            {
              if (Console.ReadLine() == "exit")  exit = true;
            }
        }

        private static void ServerEvent_MessageConsole(string message)
        {
            Console.WriteLine(message);
        }

        private static Server server;
        private static bool exit = false;
    }
}
