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
using System.Windows.Shapes;

namespace SchoolApp
{
    /// <summary>
    /// Логика взаимодействия для Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        private SqlConnection sqlConnection = null;
        public Schedule()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
            sqlConnection.Open();
        }

        public class DataAll
        {
            public string Day { get; set; }
            public int Lesson { get; set; }
            public string Subject { get; set; }
            public string Sclass { get; set; }
            public int Office { get; set; }
        }

        public void LoadedAll()
        {
            Data.Items.Clear();
            day.Text = "";
            lesson.Text = "";
            sclass.Text = "";
            subject.Text = "";
            office.Text = "";

            SqlDataReader dataReader = null;
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT Id, Day, Lesson, Subject, Class, Office FROM Schedule ",
                    sqlConnection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    DataAll tempData = new DataAll();
                    tempData.Day = Convert.ToString(dataReader["Day"]);
                    tempData.Lesson = Convert.ToInt32(dataReader["Lesson"]);
                    tempData.Subject = Convert.ToString(dataReader["Subject"]);
                    tempData.Sclass = Convert.ToString(dataReader["Class"]);
                    tempData.Office = Convert.ToInt32(dataReader["Office"]);
                    Data.Items.Add(tempData);
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

        private void Button_AddSchedule(object sender, RoutedEventArgs e)
        {
            if (day.Text == "" || sclass.Text == "" || lesson.Text == "" ||
                subject.Text == "" || office.Text == "")
            {
                MessageBox.Show("Поле не может быть пустым");
                return;
            }

            SqlCommand command = new SqlCommand(
                "INSERT INTO [Schedule] (Day, Lesson, Subject, Class, Office) " +
                $"VALUES (N'{day.Text}', N'{lesson.Text}', N'{subject.Text}', " + 
                $"N'{sclass.Text}', N'{office.Text}')",
                sqlConnection);
            command.ExecuteNonQuery();

            MessageBox.Show("Успешно добавлено в базу!");
            LoadedAll();
        }
    }
}
