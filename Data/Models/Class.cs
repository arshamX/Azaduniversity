using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Data.Models
{
    public class Class
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string LessonID { get; set; }
        public string LessonName { get; set; }
        public string GroupName {  get; set; }
        public string Grade { get; set; }
        public string TeacherName {  get; set; }
        public string TeacherID { get; set; }
        public string ClassT { get; set; }
        public string ClassD { get; set; }
        public int ExamT { get; set; }
        public string ExamD { get; set; }
        public int Population { get; set; }
        public string MojtamaID { get; set; }
        public Class(int id ,string code, Lesson lessonid, Teacher teacherid, string classt, string classd, int examt, string examd, int population, string mojtamaid)
        {
            ID = id;
            Code = code;
            LessonID = lessonid.ID;
            LessonName = lessonid.Name;
            TeacherID = teacherid.ID;
            TeacherName = teacherid.FullName;
            GroupName = lessonid.GroupName;
            Grade = lessonid.Grade;
            ClassT = classt;
            ClassD = classd;
            ExamT = examt;
            ExamD = examd;
            Population = population;
            MojtamaID = mojtamaid;

        }
        public Class(string code, Lesson lessonid, Teacher teacherid, string classt, string classd, int examt, string examd, int population)
        {
            Code = code;
            LessonID = lessonid.ID;
            LessonName = lessonid.Name;
            TeacherID = teacherid.ID;
            TeacherName = teacherid.FullName;
            GroupName = lessonid.GroupName;
            Grade = lessonid.Grade;
            ClassT = classt;
            ClassD = classd;
            ExamT = examt;
            ExamD = examd;
            Population = population;
        }

    }
}
