using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDesk.Client
{
    class ClientEvent
    {
        public delegate void MessageServer(object serverResponse);
        static public event MessageServer OnMessageServer;
        public delegate void MessageServerData(object serverResponseData);
        static public event MessageServerData OnMessageServerData;
        static public void SendMessage(Tuple<Dictionary<string, object>, Dictionary<string, string>, Dictionary<string, long>> serverResponse)
        {
            OnMessageServer?.Invoke(serverResponse);
        }
        static public void SendMessage(List<string> serverResponse)
        {
            OnMessageServer?.Invoke(serverResponse);
        }

        static public void SendMessageDataTable(DataTable dataTable)
        {
            OnMessageServerData?.Invoke(dataTable);
        }

        static public void SendMessageData(Dictionary<string, object> serverResponseData)
        {
            OnMessageServerData?.Invoke(serverResponseData);
        }
    }
}
