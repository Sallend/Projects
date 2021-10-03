using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace HelpDesk.Client
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            checkBoxAutorun.IsChecked = RegeditApp.GetStatusAutorun("HelpDesk");
            textBoxAddress.Text = RegeditApp.KeyValue("HelpDesk", "IPAddressServer");
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            RegeditApp.SetAutorunValue("HelpDesk", (bool)checkBoxAutorun.IsChecked);
            RegeditApp.KeyValue("HelpDesk", "IPAddressServer", textBoxAddress.Text);
        }
    }
}
