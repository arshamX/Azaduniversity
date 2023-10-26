using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Teacher
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Teacher(string id, string firsname, string lastname)
        {
            ID = id;
            FirstName = firsname;
            LastName = lastname;
            FullName = firsname + " " + lastname;
        }
    }
}
