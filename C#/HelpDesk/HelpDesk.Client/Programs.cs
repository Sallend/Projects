using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace HelpDesk.Client
{
    static class Program
    {
        public const string nameApp = "HelpDesk";
        private static string serverIP = string.Empty; // save regedit
        private static MonitorServer _monitor;
        public static NotifyIcon notifyIcon;
        [STAThread]
        static void Main()
        {
            if (System.Diagnostics.Process.GetProcessesByName(Application.ProductName).Length > 1)
            {
                MessageBox.Show("Приложение уже запущено :)");
                return;
            }
            else
            {
                Application.EnableVisualStyles();
                serverIP = RegeditApp.KeyValue("HelpDesk", "IPAddressServer");

                _monitor = new MonitorServer();
                Thread _monitorThread = new Thread(new ThreadStart(_monitor.Start));
                _monitorThread.SetApartmentState(ApartmentState.STA);
                _monitorThread.Name = "HepDesk";
                _monitorThread.Start();


                Application.SetCompatibleTextRenderingDefault(false);

                //using (NotifyIcon icon = new NotifyIcon())
                //{
                notifyIcon = new NotifyIcon();
                notifyIcon.Icon = HelpDesk.Client.Properties.Resources.HelpDesk1;
                //icon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                        new MenuItem("Новая заявка", onClickNewMessageForms),
                        new MenuItem("Посмотреть статус заявок", onClickMonitorForms),
                        new MenuItem("Ножницы Windows", clickWindowScissors),
                        new MenuItem("Настройки", (s, e) => {new SettingsWindow().ShowDialog();}),
                        new MenuItem("Выход", onClickExit),
                    }); ;
                notifyIcon.Visible = true;

                notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

                ClientEvent.OnMessageServer += ServerEvent_OnMessageServer;

                Application.Run();
                notifyIcon.Visible = false;


                //}
            }
        }

        private static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                onClickNewMessageForms(null, null);
            }
        }

        private static void ServerEvent_OnMessageServer(object serverResponse)
        {
            List<string> lictAlerts = serverResponse as List<string>;

            //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            //{
            if (lictAlerts[3] == System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper())
            {
                DisplaynotifyMessageSentStatusErr(lictAlerts);
            }
            //}));
        }

        private static void DisplaynotifyMessageSentStatusErr(List<string> infoStatus) 
        {
            Program.notifyIcon.BalloonTipTitle = "Изменение статуса HelpDesk";
            Program.notifyIcon.BalloonTipText = $"Рег. номер:{infoStatus[0]} \nТема: {infoStatus[1]} \nСтатус: {infoStatus[2]}";
            Program.notifyIcon.ShowBalloonTip(150);
        }

        private static void onClickNewMessageForms(object s, EventArgs e)
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "NewSupportWindows", out createdNew))
            {
                if (createdNew)
                {
                    System.Windows.Forms.Application.EnableVisualStyles();
                    new NewSupportWindows(serverIP).ShowDialog();
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
        }


        private static void onClickMonitorForms(object s, EventArgs e)
        {
            //bool createdNew = true;
            //using (Mutex mutex = new Mutex(true, "HelpDeskStatus", out createdNew))
            //{
            //    if (createdNew)
            //    {
            //        System.Windows.Forms.Application.EnableVisualStyles();
            //        new **(serverIP).ShowDialog();// добав форму статуса заявок 
            //    }
            //    else
            //    {
            //        Process current = Process.GetCurrentProcess();
            //        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            //        {
            //            if (process.Id != current.Id)
            //            {
            //                SetForegroundWindow(process.MainWindowHandle);
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        private static void clickWindowScissors(object s, EventArgs e)
        {
            Process.Start(@"C:\\Windows\\sysnative\\SnippingTool.exe");
        }

        private static void onClickExit(object s, EventArgs e)
        {
            _monitor.Stop();
            Thread.Sleep(1000);
            Application.Exit();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}