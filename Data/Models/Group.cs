using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Group
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public Group(string iD, string name, string grade)
        {
            ID = iD;
            Name = name;
            Grade = grade;
        }
    }
}
