using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HelpDesk.Client
{    
    public partial class RequestStatusWindows : Window
    {
        public RequestStatusWindows(string ipServer)
        {
            ipServerPort = ipServer;
            InitializeComponent();
            getServerErrStatus();
            LableUser.Content += System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper();
            LabelPCName.Content += System.Net.Dns.GetHostName().ToUpper();
            ClientEvent.OnMessageServerData += ServerEvent_OnMessageServerData;
        }

        private void detailedInformation(object sender, RoutedEventArgs e)
        {

        }
        private void ServerEvent_OnMessageServerData(object serverResponseData)
        {
            DataTable table = serverResponseData as DataTable;
            bool status = false;
            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    dataTable = table;
                    status = true;
                }
            }
            if (status)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                {
                    labelStatus.Visibility = System.Windows.Visibility.Hidden;
                    dataGridErrStatus.ItemsSource = dataTable.DefaultView;
                }));
            }
            else
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                {
                    labelStatus.Content = "Заявок текущего пользователя OC не найдено!";
                }));
            }
        }


        private void getServerErrStatus()
        {
            List<object> list = new List<object>();
            list.Add(System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper());//name user
            list.Add(System.Net.Dns.GetHostName().ToUpper());//pc name или IP
            list.Add(MonitorServer.port);//port
            try
            {
                DialogServer.sendRequest(ipServerPort, "getStatusHelpDeskClient", list);
            }
            catch
            {
                labelStatus.Content = "Сервер недоступен.";
            }
        }

        static private string ipServerPort = string.Empty;
        private DataTable dataTable = new DataTable();

    }
}
