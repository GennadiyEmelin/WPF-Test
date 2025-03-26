using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    /// <summary>
    /// Структура данных на случайный случай из случайности)
    /// </summary>
    struct Workers
    {
        public string Name {  get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string Department { get; set; }
        public string Post {  get; set; }
        public string PhoneNumber{  get; set; }
        public string Passport { get; set; }

        public Workers(string Name, string Surname, int Age, int Salary, string Department, string Post, string PhoneNumber, string Passport)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Age = Age;
            this.Salary = Salary;
            this.Department = Department;
            this.Post = Post;
            this.PhoneNumber = PhoneNumber;
            this.Passport = Passport;
        }
    }
}
