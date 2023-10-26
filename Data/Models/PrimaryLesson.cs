using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PrimaryLesson
    {
        public int ID { get; set; }
        public string LessonID { get; set; }
        public int MaxPopulation { get; set; }
        public PrimaryLesson(string lessonID, int maxPopulation)
        {
            LessonID = lessonID;
            MaxPopulation = maxPopulation;
        }
        public PrimaryLesson(int id,string lessonID, int maxPopulation)
        {
            ID = id;
            LessonID = lessonID;
            MaxPopulation = maxPopulation;
        }
    }
}
