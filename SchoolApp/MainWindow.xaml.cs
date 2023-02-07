using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace SchoolApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection sqlConnection = null;
        public MainWindow()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
            sqlConnection.Open();
            RefreshAll();
        }

        public class DataAll
        {
            public int IdStudent { get; set; }
            public int IdTeacher{ get; set; }
            public string FcsStudent { get; set; }
            public string FcsTeacher { get; set; }
            public string ClassStudent { get; set; }
            public string ItemTeacher { get; set; }
            public string OmissionsStudent { get; set; }
        }

        private void RefreshAll()
        {
            Student_Data.Items.Clear();
            Teacher_Data.Items.Clear();

            tfcs.Text = "";
            titem.Text = "";
            sfcs.Text = "";
            sclass.Text = "";
            omissions.Text = "";

            SqlDataReader dataReader = null;
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT Id, FullName, Class, Omissions FROM Students ",
                    sqlConnection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    DataAll tempData = new DataAll();
                    tempData.IdStudent = Convert.ToInt32(dataReader["Id"]);
                    tempData.FcsStudent = Convert.ToString(dataReader["FullName"]);
                    tempData.ClassStudent = Convert.ToString(dataReader["Class"]);
                    tempData.OmissionsStudent = Convert.ToString(dataReader["Omissions"]);
                    Student_Data.Items.Add(tempData);
                }

                dataReader.Close();
                SqlCommand commandTeacher = new SqlCommand(
                    "SELECT Id, FullName, Item FROM Teachers ",
                    sqlConnection);
                dataReader = commandTeacher.ExecuteReader();

                while (dataReader.Read())
                {
                    DataAll tempData = new DataAll();
                    tempData.IdTeacher = Convert.ToInt32(dataReader["Id"]);
                    tempData.FcsTeacher = Convert.ToString(dataReader["FullName"]);
                    tempData.ItemTeacher = Convert.ToString(dataReader["Item"]);
                    Teacher_Data.Items.Add(tempData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        private void Button_AddStudent(object sender, RoutedEventArgs e)
        {
            if (sfcs.Text == "" || sclass.Text == "" || omissions.Text == "")
            {
                MessageBox.Show("Поле не может быть пустым");
                return;
            }

            SqlCommand command = new SqlCommand(
                "INSERT INTO [Students] (FullName, Class, Omissions) " +
                $"VALUES (N'{sfcs.Text}', N'{sclass.Text}', N'{omissions.Text}')",
                sqlConnection);
            command.ExecuteNonQuery();

            MessageBox.Show("Успешно добавлено в базу!");
            RefreshAll();
        }

        private void Button_AddTeacher(object sender, RoutedEventArgs e)
        {
            if (tfcs.Text == "" || titem.Text == "")
            {
                MessageBox.Show("Поле не может быть пустым");
                return;
            }

            SqlCommand command = new SqlCommand(
                "INSERT INTO [Teachers] (FullName, Item) " +
                $"VALUES (N'{tfcs.Text}', N'{titem.Text}')",
                sqlConnection);
            command.ExecuteNonQuery();

            MessageBox.Show("Успешно добавлено в базу!");
            RefreshAll();
        }

        private void Button_Evaluations(object sender, RoutedEventArgs e)
        {
            Evaluations form = new Evaluations();
            form.LoadedAll();
            form.Show();
        }

        private void Button_Schedule(object sender, RoutedEventArgs e)
        {
            Schedule form = new Schedule();
            form.LoadedAll();
            form.Show();
        }
    }
}
