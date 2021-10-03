using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Client
{
    class RegeditApp
    {
        public static bool SetAutorunValue(string name, bool autorun)
        {
            string ExePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                    reg.SetValue(name, ExePath);
                else
                    reg.DeleteValue(name);

                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool GetStatusAutorun(string name)
        {
            RegistryKey currentUsers = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
            if (currentUsers.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\").GetValue(name) != null)
            {
               return true;
            }
            else
            {
                return false;
            }
        }
       
        public static string KeyValue(string appName, string key, string value = "")
        {
            if (appName != String.Empty && key != String.Empty)
            {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\" + appName + "\\");
                object obj = reg.GetValue(key);
                if (obj == null || value != "")
                {
                    reg.SetValue(key, value);
                    reg.Close();
                }
                else
                {
                    return (string)obj;
                }
            }
            return "";
        }

    }
}
