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

        public MainWindow()
        {
            InitializeComponent();
        }

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
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
