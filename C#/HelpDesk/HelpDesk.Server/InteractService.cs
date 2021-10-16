using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Server
{
    internal class InteractService
    {       
        public InteractService(List<Dictionary<long, object>> sendingData)
        {            
            Dictionary<string, object> command = (Dictionary<string, object>)sendingData[0][sendingData[0].Keys.ToArray()[0]];
            controller(command);
        }

        private void controller(Dictionary<string, object> command)
        {
            List<object> objInfo;
            string key = command.Keys.ToArray()[0];
            switch (key)
            {
                case "getObjFromBase":
                    objInfo = (List<object>)command[key];
                    sendObjFromBase(objInfo);
                    break; 
                case "getStatusHelpDeskClient":
                    objInfo = (List<object>)command[key];

                    break;               
            }
        }

        private void sendObjFromBase(List<object> objInfo)
        {   
            Dictionary<string, string> attrListValue = (Dictionary<string, string>)objInfo[0]; //list: attribute, value
            Dictionary<string, byte[]> fileByteList = (Dictionary<string, byte[]>)objInfo[1]; //file
            


            Dictionary<string, List<object>> response = new Dictionary<string, List<object>>();
            // Кастомная передача 
            Dictionary<string, object> objectList = new Dictionary<string, object>();
            //objectList.Add("IPUser", userIP.ToString());

            //Tuple<Dictionary<string, object>, Dictionary<string, string>, Dictionary<string, long>> resultSet = new Tuple<Dictionary<string, object>, Dictionary<string, string>, Dictionary<string, long>>(objectList, namesOfFilesAndTypes, namesAndObjID);
            //response.Add("filesIDFromIPS", new List<object>() { resultSet });
            //Server.sendDataToServer(response, userIP, port);

        }

    }
}
