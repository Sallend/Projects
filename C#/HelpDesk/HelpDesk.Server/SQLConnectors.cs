using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Server
{
    public class SQLConnectors
    {
        public static SqlConnection sqlConnection()
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=HelpDesk;Persist Security Info=True;User ID=test;Password=test");
                con.Open();
                return con;
            }
            catch
            {
                return new SqlConnection();
            }
        }

        public static DataTable getHelp()
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = sqlConnection();
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM help", connection))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    dataTable.Load(dr);
                    dr.Close();
                }
                return dataTable;

            }
            catch (Exception ex)
            {
                StreamWriter log = new StreamWriter(new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ErrSQL.txt", FileMode.Append));
                log.WriteLine(ex);
                log.Flush();
                log.Close();
                connection.Close();
                return new DataTable();
            }
        }
    }
}
