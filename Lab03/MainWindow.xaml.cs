using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Lab03
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Data Source=LAB1504-25\\SQLEXPRESS;Initial Catalog=Tecsupdb;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnListDataTable_Click(object sender, RoutedEventArgs e)
        {
            List<Student> students = GetStudentsUsingDataTable();
            dataGrid.ItemsSource = students;
        }

        private void btnListDataReader_Click(object sender, RoutedEventArgs e)
        {
            List<Student> students = GetStudentsUsingDataReader();
            dataGrid.ItemsSource = students;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text;
            List<Student> students = SearchStudentsByName(searchText);
            dataGrid.ItemsSource = students;
        }

        private List<Student> GetStudentsUsingDataTable()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(row["StudentId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    });
                }
            }

            return students;
        }

        private List<Student> GetStudentsUsingDataReader()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    });
                }
            }

            return students;
        }

        private List<Student> SearchStudentsByName(string name)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE FirstName LIKE @Name OR LastName LIKE @Name", connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(row["StudentId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    });
                }
            }

            return students;
        }
    }
}
