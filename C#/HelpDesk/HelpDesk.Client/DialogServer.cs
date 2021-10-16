using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Client
{
    class DialogServer
    {
        public static void sendRequest(string ipServer, string commandName, List<object> objInfo)
        {
            string[] ipServerPort = ipServer.Split(':');
            Dictionary<long, List<Dictionary<long, object>>> request = new Dictionary<long, List<Dictionary<long, object>>>();
            List<Dictionary<long, object>> part1 = new List<Dictionary<long, object>>();
            Dictionary<long, object> part2 = new Dictionary<long, object>();
            Dictionary<string, object> part3 = new Dictionary<string, object>();

            part3.Add(commandName, objInfo);

            part2.Add(1, part3);
            part1.Add(part2);
            request.Add(666666666666666, part1);

            MonitorServer.sendDataToServer(request, ipServerPort); //IP и Port сервера
        }
    }
}
