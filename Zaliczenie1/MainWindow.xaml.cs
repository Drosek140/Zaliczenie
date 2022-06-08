using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;

namespace Zaliczenie1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-A4M3759K\SQLEXPRESS;Initial Catalog=Sample_Database;Integrated Security=True;");

        private void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT UserID,Name,LastName,Age,City.City,Type_of_Employment.Type_of_employment FROM((DataUser INNER JOIN City ON DataUser.City = City.ID) JOIN Type_of_Employment ON DataUser.Type_of_employment = Type_of_Employment.ID); ", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;

        }
        public void ClearData()
        {
            Name.Clear();
            Age.Clear();
            LastName.Clear();
            Pesel.Clear();
            Search.Clear();

        }
        public bool IsValid()
        {
            if(Name.Text == string.Empty)
            {
                MessageBox.Show("Imię jest wymagane!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (LastName.Text == string.Empty)
            {
                MessageBox.Show("Nazwisko jest wymagane!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Age.Text == string.Empty)
            {
                MessageBox.Show("Wiek jest wymagany!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Pesel.Text == string.Empty)
            {
                MessageBox.Show("Pesel jest wymagany!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
        private void InsertDataBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-A4M3759K\SQLEXPRESS;Initial Catalog=Sample_Database;Integrated Security=True;");
            try {
                if (IsValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO DataUser VALUES (@Name, @Lastname, @Age, @City )",con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", Name.Text);
                    cmd.Parameters.AddWithValue("@Lastname", LastName.Text);
                    cmd.Parameters.AddWithValue("@Age", Age.Text);
                    cmd.Parameters.AddWithValue("@City", City.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("Poprawnie dodano!", "Zapisane", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteDataBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from DataUser where UserID = " + Search.Text + "", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Poprawnie usunięto rekord", "Usunięte", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                ClearData();
                LoadGrid();
                con.Close();
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Nie udało się usunąć"+" "+ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdateDataBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Update DataUser set Name = '" + Name.Text + "',Lastname = '" + LastName.Text + "',Age = '" + Age.Text + "',Pesel = '" + Pesel.Text + "' WHERE ID = '"+Search.Text+"'",con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Poprawnie zmieniono rekord", "Zmienione", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                ClearData();
                LoadGrid();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Nie udało się usunąć" + ex.Message);
            }
            finally
            {
                con.Close();
                ClearData();
                LoadGrid();
            }
        }
    }
}
