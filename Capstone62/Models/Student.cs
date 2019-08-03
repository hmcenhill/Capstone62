using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone62.Models
{
    public class Student : UserExtend
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Enrollment> Enrollment { get; set; }

        public Student(string userName) : base(userName)
        {
            IsAdmin = false;
            Role = "Student";
        }
    }
}
