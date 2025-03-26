using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WPF
{
    internal class MSSQL
    {
        /// <summary>
        /// Подключение к БД и запись данных в БД
        /// </summary>
        /// <param name="Name">Имя работника</param>
        /// <param name="Surname">Фамилия работника</param>
        /// <param name="Age">Возраст</param>
        /// <param name="Salary">Зарплата</param>
        /// <param name="Department">Департамент</param>
        /// <param name="Post">Должность</param>
        /// <param name="PhoneNumber">Номер телефона</param>
        /// <param name="Passport">Паспортные данные</param>
        public void AddSQL(string Name, string Surname, int Age, int Salary, string Department,
            string Post, string PhoneNumber, string Passport)
        {
            SqlConnectionStringBuilder strCon = new SqlConnectionStringBuilder() 
            {
                DataSource = @"(localdb)\MSSQLLocalDB", // Данные от БД
                InitialCatalog = "WorkersDB",
                IntegratedSecurity = true
            };
            SqlConnection sqlConnection = new SqlConnection()
            {
                ConnectionString = strCon.ConnectionString,
            };
            sqlConnection.StateChange += (s, e) =>
            {
                Console.WriteLine($@"{nameof(sqlConnection)} " +
                $"в состоянии: {(s as SqlConnection).State}");
            };

            try // Попытка открыть и запись
            {
                sqlConnection.Open();
                string sql = $@"INSERT INTO [Table]([Name],[Surname],[Age],[Salary],[Department],[Post],[PhoneNumber],[Passport]) 
                     values (N'{Name}',N'{Surname}',{Age},{Salary},N'{Department}',N'{Post}','{PhoneNumber}','{Passport}')";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(sqlConnection.State);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void UpdateID()
        {
            SqlConnectionStringBuilder strCon = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB", // Данные от БД
                InitialCatalog = "WorkersDB",
                IntegratedSecurity = true
            };
            SqlConnection sqlConnection = new SqlConnection()
            {
                ConnectionString = strCon.ConnectionString,
            };
            sqlConnection.StateChange += (s, e) =>
            {
                Console.WriteLine($@"{nameof(sqlConnection)} " +
                $"в состоянии: {(s as SqlConnection).State}");
            };

            try // Попытка открыть и запись
            {
                sqlConnection.Open();
                var sql = @"INSERT INTO reservTable (Name, Surname, Age, Salary, Department, Post, PhoneNumber, Passport) 
                    SELECT name, Surname, Age, Salary, Department, Post, PhoneNumber, Passport 
                    FROM [Table] ORDER BY Id ASC;;";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
                sql = @"DELETE FROM [Table];";
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
                sql = @"DBCC CHECKIDENT ('[Table]', RESEED, 0);";
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
                sql = @"INSERT INTO [Table] (Name, Surname, Age, Salary, Department, Post, PhoneNumber, Passport) 
                    SELECT name, Surname, Age, Salary, Department, Post, PhoneNumber, Passport 
                    FROM [reservTable] ORDER BY Id ASC;";
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
                sql = @"DELETE FROM reservTable";
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
                sql = @"DBCC CHECKIDENT ('reservTable', RESEED, 0);";
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(sqlConnection.State);
            }
            finally
            {
                sqlConnection.Close();
            }

        }
    }
}
