using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone62.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public ICollection<Enrollment> Enrollment { get; set; }

    }
}
