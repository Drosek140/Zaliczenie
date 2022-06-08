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
using System.Data;

namespace Zaliczenie1
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-A4M3759K\SQLEXPRESS;Initial Catalog=Sample_Database;Integrated Security=True;");
        public bool IsValid()
        {
            if (UserLogin.Text == string.Empty)
            {
                MessageBox.Show("Wprowadz Login!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (UserPassword.Password == string.Empty)
            {
                MessageBox.Show("Wprowadz Hasło!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
           
            return true;
        }
        public void handleLoginUser(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                String query = "SELECT COUNT(1) FROM Login_User WHERE Login = @Login AND Password=@Password";
                SqlCommand sqlCmd = new SqlCommand(query, con);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Login", UserLogin.Text);
                sqlCmd.Parameters.AddWithValue("@Password", UserPassword.Password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nie ma takiego użytkownika!");
                }

            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    bool exists = false;
                    SqlCommand cmd = new SqlCommand("Select count(*) from Login_User where Login=@Login", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Login", UserLogin.Text);
                    exists = (int)cmd.ExecuteScalar() > 0;
                    
                    if (exists)
                    {
                        MessageBox.Show(string.Format("Użytkownik {0} już istnieje",UserLogin.Text));
                    }
                    else
                    {
                        SqlCommand comand = new SqlCommand("INSERT INTO Login_User VALUES (@Login, @Password)", con);
                        comand.CommandType = CommandType.Text;
                        comand.Parameters.AddWithValue("@Login", UserLogin.Text);
                        comand.Parameters.AddWithValue("@Password", UserPassword.Password);                      
                        comand.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Poprawnie zarejestrowano! Kliknij przycisk Zaloguj!", "Rejestracja", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}