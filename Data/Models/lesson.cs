using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Lesson
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public int Vahed { get; set; }
        public string Grade {  get; set; }
        public Lesson(string id, string name, string groupid, string groupname,int vahed,string grade)
        {
            ID = id;
            Name = name;
            GroupID = groupid;
            GroupName = groupname;
            Vahed = vahed;
            Grade = grade;
        }
    }
}
