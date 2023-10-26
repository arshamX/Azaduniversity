using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Data;
using Data.Models;

namespace WPFUnivercity
{
    public partial class AddEditPrimaryLesson : Window
    {
        private Database DB;
        private Teacher? currentreacher;
        private Class? primaryClass;
        private PrimaryLesson? primary;
        private int Primaryid = 0;
        public AddEditPrimaryLesson(Database db)
        {
            InitializeComponent();
            DB = db;
            dgLessons.ItemsSource = DB.Lessons;
            rbtnAll.IsChecked = true;
            FillTeachers();
            nPopulation.Minimum = 1;
        }
        public AddEditPrimaryLesson(Database db , Lesson les , Class room)
        {
            InitializeComponent();
            DB = db;
            dgLessons .ItemsSource = DB.ReadLessons(Data.Type.ID , les.ID);
            FillTeachers();
            txtNameD.Text = les.Name;
            txtCodeD.Text = les.ID;
            txtVahed.Text = les.Vahed.ToString();
            txtGroup.Text = les.GroupName;
            dumy1.Visibility = Visibility.Collapsed;
            dumy2.Visibility = Visibility.Visible;
            topbar.Visibility = Visibility.Collapsed;
            nPopulation.Text = room.Population.ToString();
            FindTeacher(room);
            txtClassCode.Text = room.Code;
            string[] t = room.ClassT.Split('-');
            tpFirst.Text = t[0];
            tpSecond.Text = t[1];
            cbClassD.Text = room.ClassD;
            nPopulation.Text = room.Population.ToString();
            cbSans.SelectedIndex = Convert.ToInt32(room.ExamT) - 1;
            tpExamDate.Text = room.ExamD;
            Primaryid = room.ID;
            nPopulation.Minimum = 1;
        }
        private void FillTeachers()
        {
            foreach (Teacher teacher in DB.Teachers)
            {
                cbTeacherName.Items.Add(teacher.FullName);
            }
        }
        private void FindTeacher(Class room)
        {
            foreach (Teacher teacher in DB.Teachers)
            {
                if(teacher.ID == room.TeacherID)
                {
                    currentreacher = teacher;
                    cbTeacherName.Text = teacher.FullName;
                }
                    
            }
        }
        private void GenaratePrimaryClassLesson()
        {
            string lessonID = txtCodeD.Text;
            int population = Convert.ToInt32(nPopulation.Text);
            string classcode = txtClassCode.Text;
            string teacherid = currentreacher!.ID;
            string classtime = tpFirst.Text + "-" + tpSecond.Text;
            string classday = cbClassD.Text;
            int examtime = cbSans.SelectedIndex + 1;
            string examdate = tpExamDate.Text;
            primary = new PrimaryLesson(lessonID, population);
            Lesson tl = DB.GetLesson(lessonID);
            Teacher teach = DB.GetTeacher(teacherid);
            primaryClass = new Class(classcode, tl, teach, classtime, classday, examtime, examdate, population);
        }
        private void rbtnAll_Checked(object sender, RoutedEventArgs e)
        {
            rbtnCode.IsChecked = false;
            rbtnName.IsChecked = false;
            rbtnGCode.IsChecked = false;
            txtGCode.Text = "";
            txtName.Text = "";
            txtCode.Text = "";
            txtName.IsReadOnly = true;
            txtCode.IsReadOnly = true;
            txtGCode.IsReadOnly = true;
        }

        private void rbtnName_Checked(object sender, RoutedEventArgs e)
        {
            rbtnAll.IsChecked = false;
            rbtnCode.IsChecked = false;
            rbtnGCode.IsChecked = false;
            txtGCode.Text = "";
            txtName.Text = "";
            txtName.IsReadOnly = false;
            txtCode.Text = "";
            txtCode.IsReadOnly = true;
            txtGCode.IsReadOnly = true;
        }

        private void rbtnCode_Checked(object sender, RoutedEventArgs e)
        {
            rbtnAll.IsChecked = false;
            rbtnName.IsChecked= false;
            rbtnGCode.IsChecked = false;
            txtGCode.Text = "";
            txtName.Text = "";
            txtGCode.IsReadOnly = true;
            txtName.IsReadOnly = true;
            txtCode.IsReadOnly = false;
            txtCode.Text = "";
        }
        private void rbtnGCode_Checked(object sender, RoutedEventArgs e)
        {
            rbtnAll.IsChecked = false;
            rbtnName.IsChecked = false;
            rbtnCode.IsChecked = false;
            txtName.Text = "";
            txtCode.Text = "";
            txtGCode.Text = "";
            txtGCode.IsReadOnly = false;
            txtName.IsReadOnly = true;
            txtCode.IsReadOnly = true;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnAll.IsChecked == true)
            {
                dgLessons.ItemsSource = DB.Lessons;
            }
            else if(rbtnName.IsChecked == true)
            {
                dgLessons.ItemsSource= DB.ReadLessons(Data.Type.Name,txtName.Text);
            }
            else if (rbtnCode.IsChecked == true)
            {
                dgLessons.ItemsSource = DB.ReadLessons(Data.Type.ID, txtCode.Text);
            }
            else if(rbtnGCode.IsChecked == true)
            {
                dgLessons.ItemsSource = DB.ReadLessons(Data.Type.Group,txtGCode.Text);
            }
        }

        private void dgLessons_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                Lesson temp = (dgLessons.SelectedItem as Lesson)!;
                txtNameD.Text = temp.Name;
                txtCodeD.Text = temp.ID;
                txtVahed.Text = temp.Vahed.ToString();
                txtGroup.Text = temp.GroupName;
            }
            catch
            {

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GenaratePrimaryClassLesson();
            try
            {
                DB.AddPrimaryLesson(primary!);
                DB.AddPrimaryClass(primaryClass!);
                MessageBox.Show("با موفقیت اضافه شد", "موفق");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا");
            }
        }

        private void cbTeacherName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(Teacher teacher in DB.Teachers)
            {
                if(teacher.FullName == cbTeacherName.SelectedItem.ToString())
                {
                    currentreacher = teacher;
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            GenaratePrimaryClassLesson();
            try
            {
                DB.UpdatePrimaryLesson(primary!);
                DB.UpdatePrimaryClass(primaryClass!, Primaryid);
                MessageBox.Show("تغییرات اعمال شد", "موفق");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا");
            }
        }

    }
}
