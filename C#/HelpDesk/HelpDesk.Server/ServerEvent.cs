using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Server
{
    class ServerEvent
    {
        public delegate void MessageConsoleEvent(string serverResponse);
        static public event MessageConsoleEvent OnMessageConsiole;
     
        static public void SendMessageConsole(string messageConsole)
        {
            OnMessageConsiole?.Invoke(messageConsole);
        }        
    }
}
