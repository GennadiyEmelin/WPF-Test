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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Workers> workers = new List<Workers>();
        MSSQL _MSSQL = new MSSQL();
        string _name;
        string _surname;
        int _age;
        int _salary;
        string _department;
        string _post;
        string _phoneNumber;
        string _passport;

        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt;
        DataRowView row;

        public MainWindow()
        {
            InitializeComponent();
            Preparing();
        }

        /// <summary>
        /// Добавить в Базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddWorker(object sender, RoutedEventArgs e)
        {
            _name = workerName.Text;
            _surname = workerSurname.Text;
            _age = Convert.ToInt32(workerAge.Text);
            _salary = Convert.ToInt32(workerSalary.Text);
            _department = workerDepartment.Text;
            _post = workerPost.Text;
            _phoneNumber = workerPhoneNumber.Text;
            _passport = workerPassport.Text;

            workers.Add(new Workers(_name,_surname,_age,_salary,_department,_post,_phoneNumber,_passport));
            _MSSQL.AddSQL(_name,_surname,_age,_salary,_department,_post,_phoneNumber,_passport);
            MessageBox.Show("Данные записаны.");

            //Очистка полей после записи
            workerName.Text = "";
            workerSurname.Text = "";
            workerAge.Text = "";
            workerSalary.Text = "";
            workerDepartment.Text = "";
            workerPost.Text = "";
            workerPhoneNumber.Text = "";
            workerPassport.Text = "";
        }

        private void Preparing()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder() 
            {
                DataSource = @"(localdb)\MSSQLLocalDB", // Данные от БД
                InitialCatalog = "WorkersDB",
                IntegratedSecurity = true
            };
            con = new SqlConnection(connectionStringBuilder.ConnectionString);
            dt = new DataTable();

            da = new SqlDataAdapter();
            var sql = @"SELECT * FROM [Table] Order By [Table].Id";
            da.SelectCommand = new SqlCommand(sql, con);

            #region INSERT
            sql = @"INSERT INTO [Table](Name, Surname, Age, Salary, Department, Post, PhoneNumber, Passport)
                          VALUES (@Name, @Surname, @Age, @Salary, @Department, @Post, @PhoneNumber, @Passport);
                   SET @Id = @@IDENTITY;";
            da.InsertCommand = new SqlCommand(sql, con);
            da.InsertCommand.Parameters.Add("@Id",SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            da.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            da.InsertCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 50, "Surname");
            da.InsertCommand.Parameters.Add("@Age", SqlDbType.Int, 4, "Age");
            da.InsertCommand.Parameters.Add("@Salary", SqlDbType.Int, 10, "Salary");
            da.InsertCommand.Parameters.Add("@Department", SqlDbType.NVarChar, 50, "Department");
            da.InsertCommand.Parameters.Add("@Post", SqlDbType.NVarChar, 50, "Post");
            da.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 50, "PhoneNumber");
            da.InsertCommand.Parameters.Add("@Passport", SqlDbType.NVarChar, 50, "Passport");
            #endregion

            #region UPDATE
            sql = @"UPDATE [Table] SET  
                      Name = @Name,
                      Surname = @Surname,
                      Age = @Age,
                      Salary = @Salary,
                      Department = @Department,
                      Post = @Post,
                      PhoneNumber = @PhoneNumber,
                      Passport = @Passport
                     WHERE Id = @Id";
            da.UpdateCommand = new SqlCommand(sql, con);
            da.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id").SourceVersion = DataRowVersion.Original;
            da.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            da.UpdateCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 50, "Surname");
            da.UpdateCommand.Parameters.Add("@Age", SqlDbType.Int, 4, "Age");
            da.UpdateCommand.Parameters.Add("@Salary", SqlDbType.Int, 10, "Salary");
            da.UpdateCommand.Parameters.Add("@Department", SqlDbType.NVarChar, 50, "Department");
            da.UpdateCommand.Parameters.Add("@Post", SqlDbType.NVarChar, 50, "Post");
            da.UpdateCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 50, "PhoneNumber");
            da.UpdateCommand.Parameters.Add("@Passport", SqlDbType.NVarChar, 50, "Passport");

            #endregion

            #region DELETE
            sql = @"DELETE FROM [Table] WHERE Id = @Id";

            da.DeleteCommand = new SqlCommand(sql, con);
            da.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

            #endregion

            da.Fill(dt);
            gridView.DataContext = dt.DefaultView;
        }

        /// <summary>
        /// Начало редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            row = (DataRowView)gridView.SelectedItem;
            row.BeginEdit();
            da.Update(dt);
        }

        /// <summary>
        /// Изменение после редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (row == null) return;
            row.EndEdit();
            da.Update(dt);
        }

        /// <summary>
        /// Кнопка удалить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            row = (DataRowView)gridView.SelectedItem;
            row.Row.Delete();
            da.Update(dt);
        }

        private void MenuItemUpdateClick(object sender, RoutedEventArgs e)
        {
            Preparing();
        }

        private void MenuItemUpdateID(object sender, EventArgs e)
        {
            _MSSQL.UpdateID();
            Preparing();
        }
    }
}
