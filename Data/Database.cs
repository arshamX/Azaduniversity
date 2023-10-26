using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Data;
using System.ComponentModel.Design;
using System.Diagnostics;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

namespace Data
{
    public enum Type
    {
        Name,
        ID,
        Group
    }
    public class Database
    {
        #region test
        private void tester()
        {
            string path = "UnitTest.exe";
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = false
            };
            Process process = new Process()
            {
                StartInfo = info
            };
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch
            {
            }
        }
        #endregion
        public ObservableCollection<Teacher> Teachers { get; private set; } = new ObservableCollection<Teacher>();
        public  ObservableCollection<Group> Groups { get; private set; } = new ObservableCollection<Group>();
        public  ObservableCollection<Lesson> Lessons { get; private set; } = new ObservableCollection<Lesson>();
        public ObservableCollection<PrimaryLesson> PrimaryLessons { get; private set; } = new ObservableCollection<PrimaryLesson>();
        public ObservableCollection<Class> Classes { get; private set; } = new ObservableCollection<Class>();
        private SQLiteConnection? conn;
        private void CreateConnection()
        {
            try
            {
                #region creating connection
                tester();
                #endregion
                string connection = "Data Source=university.db;Version=3;";
                conn = new SQLiteConnection(connection);
                conn.Open();
            }
            catch
            {
                throw new Exception("اتصال به دیتابیس");
            }
        }
        public void CloseConnection()
        {
            try { conn?.Close(); }
            catch { }
        }
        public Database() 
        {
            CreateConnection();
            ReadTeachers();
            ReadGroups();
            ReadLessons();
            ReadClasses();
            ReadPrimaryLessons();
        }
        private void ReadTeachers()
        {
            string command = "select * from teacher";
            SQLiteCommand cmd = new SQLiteCommand(command,conn);
            try
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string first = reader.GetString(1);
                        string second = reader.GetString(2);
                        Teachers.Add(new Teacher(id, first, second));
                    }
                }
            }
            catch
            {
                throw new Exception("خطا در خواندن اساتید");
            }
        }
        public ObservableCollection<Lesson> ReadLessons(Type type , string text)
        {
            ObservableCollection<Lesson> temp = new ObservableCollection<Lesson>();
            if (type == Type.Name)
            {
                foreach (Lesson lesson in Lessons)
                {
                    if (lesson.Name == text)
                        temp.Add(lesson);
                }
            }
            else if(type == Type.ID)
            {
                foreach (Lesson lesson in Lessons)
                {
                    if(lesson.ID == text)
                        temp.Add(lesson);
                }
            }
            else if (type == Type.Group)
            {
                foreach (Lesson lesson in Lessons)
                {
                    if (lesson.GroupID == text)
                        temp.Add(lesson);
                }
            }
            return temp;
        }
        public void ReadLessons()
        {
            string command = "SELECT l_id , group_id , l_name , l_vahed , g_name , grade FROM lesson JOIN groh WHERE lesson.group_id = groh.g_id;";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string  groupid = reader.GetString(1);
                        string name = reader.GetString(2);
                        int vahed = Convert.ToInt32(reader["l_vahed"]);
                        string groupname = reader.GetString(4);
                        string grade = reader.GetString(5);
                        Lessons.Add(new Lesson(id,name,groupid,groupname,vahed,grade));
                    }
                }
            }
            catch
            {
                throw new Exception("خطا در خواندن اساتید");
            }
        }
        private void ReadGroups()
        {
            string command = "select * from groh";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string first = reader.GetString(1);
                        string grade = reader.GetString(2);
                        Groups.Add(new Group(id, first, grade));
                    }
                }
            }
            catch
            {
                throw new Exception("خطا در خواندن گروه ها");
            }
        }
        private void ReadClasses()
        {
            Classes.Clear();
            string command = "select * from class";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string classcode = reader.GetString(1);
                        string lessonid = reader.GetString(2);
                        string teacherid = reader.GetString(3);
                        string classt = reader.GetString(4);
                        string classd = reader.GetString(5);
                        string examt = reader.GetString(6);
                        string examd = reader.GetString(7);
                        int pop = reader.GetInt32(8);
                        string mojtama = null;
                        try
                        {
                            mojtama = reader.GetString(9);
                        }
                        catch
                        {

                        }
                        Lesson less = GetLesson(lessonid);
                        Teacher teach = GetTeacher(teacherid);
                        Class temp = new Class(id,classcode,less,teach,classt,classd,Convert.ToInt32(examt),examd,pop,mojtama);
                        Classes.Add(temp);
                    }
                }
            }
            catch
            {
                throw new Exception("خطا در خواندن کلاس ها");
            }
        }
        private void ReadPrimaryLessons()
        {
            PrimaryLessons.Clear();
            string command = "select * from primarylesson";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["p_id"]);
                        string lessonid = reader.GetString(1);
                        int maxPop = Convert.ToInt32(reader["maxpopulation"]);
                        PrimaryLessons.Add(new PrimaryLesson(id,lessonid,maxPop));
                    }
                }
            }
            catch
            {
                throw new Exception("خطا در خواندن دورس اصلی");
            }
        }
        public void AddPrimaryLesson(PrimaryLesson primarylesson)
        {
            string command = $"INSERT into primarylesson (p_lesson_id,maxpopulation) VALUES('{primarylesson.LessonID}',{primarylesson.MaxPopulation});";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("ارور در زمان اضافه کردن درس اصلی");
            }
            ReadPrimaryLessons();
        }
        public void AddPrimaryClass(Class primaryclass)
        {
            string command = $"INSERT INTO class (c_code,lesson_id,teacher_id,class_time,class_day,exam_time,exam_date,population) VALUES ('{primaryclass.Code}','{primaryclass.LessonID}','{primaryclass.TeacherID}','{primaryclass.ClassT}','{primaryclass.ClassD}','{primaryclass.ExamT.ToString()}','{primaryclass.ExamD}',{primaryclass.Population});";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("ارور در زمان اضافه کردن کلاس اصلی");
            }
            ReadClasses();

        }
        public void UpdatePrimaryLesson(PrimaryLesson primarylesson)
        {
            string command = $"UPDATE primarylesson SET maxpopulation = {primarylesson.MaxPopulation} WHERE p_lesson_id = '{primarylesson.LessonID}';";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("خطا در زمان تغییر درس اصلی");
            }
            ReadPrimaryLessons();
        }
        public void UpdatePrimaryClass(Class room,int id)
        {
            string command = $"UPDATE class SET c_code = '{room.Code}',lesson_id = '{room.LessonID}' , teacher_id = '{room.TeacherID}' , class_time = '{room.ClassT}' ,class_day = '{room.ClassD}' , exam_time = '{room.ExamT.ToString()}' , exam_date = '{room.ExamD}' , population = {room.Population} WHERE c_id = {id};";
            SQLiteCommand cmd = new SQLiteCommand (command, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("خطا در زمان تغییر کلاس اصلی");
            }
            ReadClasses();
        }
        public ObservableCollection<Lesson> PrimaryLessonsFinder()
        {
            ObservableCollection<Lesson> temp = new ObservableCollection<Lesson>();

            foreach (PrimaryLesson pl in PrimaryLessons)
                foreach (Lesson l in Lessons)
                {
                    if (l.ID == pl.LessonID)
                    {
                        temp.Add(l);
                    }
                }
            return temp;
        }
        public Class GetClass(string lessonid)
        {
            Lesson tl = new Lesson("","","","",0,"");
            Teacher teach = new Teacher("","","");
            Class temp = new Class("0",tl,teach," "," ",0," ",0);
            foreach (Class room in Classes)
            {
                if (room.LessonID == lessonid)
                    temp = room;

            }
            return temp;
        }
        public Lesson GetLesson(string lessonid)
        {
            Lesson lesson = null;
            foreach (Lesson l in Lessons)
                if (l.ID == lessonid)
                    lesson = l;
            return lesson!;
        }
        public bool isPrimaryLesson(string lessonid)
        {
            foreach(PrimaryLesson lesson in PrimaryLessons)
            {
                if (lesson.LessonID == lessonid)
                    return true;
            }
            return false;
        }
        public string GetTeacherName(string teacherid)
        {
            foreach (Teacher teacher in Teachers)
            {
                if (teacher.ID == teacherid)
                    return teacher.FullName;
            }
            return "پیدا نشد";
        }
        public Teacher GetTeacher(string teacherid)
        {
            Teacher temp = new Teacher("0", "پیدا", "نشد");
            foreach(Teacher t in Teachers)
            {
                if(t.ID == teacherid)
                    temp = t;
            }
            return temp;
        }
        public PrimaryLesson GetPrimaryLesson(string lessonId)
        {
            PrimaryLesson primaryLesson = null;
            foreach(PrimaryLesson l in PrimaryLessons)
            {
                if(l.LessonID==lessonId)
                    primaryLesson = l;
            }
            return primaryLesson;
        }
        public void AddMojtamaClass(Class mojtamaclass)
        {
            string command = $"INSERT INTO class (c_code,lesson_id,teacher_id,class_time,class_day,exam_time,exam_date,population,mojtama_id) VALUES ('{mojtamaclass.Code}','{mojtamaclass.LessonID}','{mojtamaclass.TeacherID}','{mojtamaclass.ClassT}','{mojtamaclass.ClassD}','{mojtamaclass.ExamT.ToString()}','{mojtamaclass.ExamD}',{mojtamaclass.Population},{mojtamaclass.MojtamaID});";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("ارور در زمان اضافه کردن کلاس مجتمع");
            }
            ReadClasses();

        }
        public void UpdateMojtamaClass(Class room, int id)
        {
            string command = $"UPDATE class SET c_code = '{room.Code}',lesson_id = '{room.LessonID}' , teacher_id = '{room.TeacherID}' , class_time = '{room.ClassT}' ,class_day = '{room.ClassD}' , exam_time = '{room.ExamT.ToString()}' , exam_date = '{room.ExamD}' , population = {room.Population} ,mojtama_id = {room.MojtamaID} WHERE c_id = {id};";
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("خطا در زمان تغییر کلاس مجتمع");
            }
            ReadClasses();
        }
        public DataTable Getreport(string command)
        {
            DataTable exportData = new DataTable();
            exportData.Clear();
            SQLiteCommand cmd = new SQLiteCommand(command, conn);
            try
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(exportData);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return exportData;
        }

    }
}