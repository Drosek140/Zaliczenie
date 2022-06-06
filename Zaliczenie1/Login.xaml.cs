using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using Zaliczenie1;

namespace Zaliczenie1
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public void handleLoginUser(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=LAPTOP-A4M3759K\SQLEXPRESS;Initial Catalog=Sample_Database;Integrated Security=True");
            try
            {
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String query = "SELECT COUNT(1) FROM User_Login WHERE Login = @UserName AND Password=@UserPassword";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Username", UserLogin.Text);
                sqlCmd.Parameters.AddWithValue("@UserPassword", UserPassword.Text);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User Invalid!");
                }

            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

    }
}