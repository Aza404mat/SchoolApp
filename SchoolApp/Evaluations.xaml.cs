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
    /// Логика взаимодействия для Evaluations.xaml
    /// </summary>
    public partial class Evaluations : Window
    {
        private SqlConnection sqlConnection = null;
        public Evaluations()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
            sqlConnection.Open();
        }

        public class DataAll
        {
            public int IdStudent { get; set; }
            public string FcsStudent { get; set; }
            public string ClassStudent { get; set; }
            public string Evaluations { get; set; }
            public double AverageScore { get; set; }
        }

        public void LoadedAll()
        {
            Data.Items.Clear();
            sfcs.Text = "";
            sclass.Text = "";
            evaluations.Text = "";

            SqlDataReader dataReader = null;
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT Id, FullName, Class, AverageScore, Evaluations FROM Evaluations ",
                    sqlConnection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    DataAll tempData = new DataAll();
                    tempData.IdStudent = Convert.ToInt32(dataReader["Id"]);
                    tempData.FcsStudent = Convert.ToString(dataReader["FullName"]);
                    tempData.ClassStudent = Convert.ToString(dataReader["Class"]);
                    tempData.AverageScore = Convert.ToDouble(dataReader["AverageScore"]);
                    tempData.Evaluations = Convert.ToString(dataReader["Evaluations"]);
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

        private void Button_AddStudent(object sender, RoutedEventArgs e)
        {
            if (sfcs.Text == "" || sclass.Text == "" || evaluations.Text == "")
            {
                MessageBox.Show("Поле не может быть пустым");
                return;
            }

            List<int> evaluationsList = new List<int>();
            double AverageScore = 0;

            string[] evaluationsAll = evaluations.Text.Split(' ');

            foreach (var eAll in evaluationsAll)
            {
                AverageScore += Convert.ToDouble(eAll);
                evaluationsList.Add(Convert.ToInt32(eAll));
            }
            AverageScore = AverageScore / Convert.ToDouble(evaluationsList.Count);
            MessageBox.Show($"{AverageScore:N2}");

            SqlCommand command = new SqlCommand(
                "INSERT INTO [Evaluations] (FullName, Class, AverageScore, Evaluations) " +
                $"VALUES (N'{sfcs.Text}', N'{sclass.Text}', '{AverageScore:N2}', N'{evaluations.Text}')",
                sqlConnection);
            command.ExecuteNonQuery();

            MessageBox.Show("Успешно добавлено в базу!");
            LoadedAll();
        }
    }
}
