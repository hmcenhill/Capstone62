using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone62.Models
{
    public class DataVM
    {
        public Student CurrentStudent { get; set; }
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<Enrollment> Enrollments { get; set; }

    }
}
