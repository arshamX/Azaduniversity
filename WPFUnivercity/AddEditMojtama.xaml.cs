using System;
using System.Collections.Generic;
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
using System.Xml.Linq;
using Data;
using Data.Models;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WPFUnivercity
{
    /// <summary>
    /// Interaction logic for AddEditMojtama.xaml
    /// </summary>
    public partial class AddEditMojtama : Window
    {
        private Database DB;
        private Class PRoom;
        private Lesson PLesson;
        public AddEditMojtama(Database db , Class room , int max , bool isedditing)
        {
            InitializeComponent();
            DB = db;
            rbtnAll.IsChecked = true;
            dgvLessons.ItemsSource = DB.Lessons;
            PRoom = room;
            PLesson = DB.GetLesson(PRoom.LessonID);
            txtOrginalName.Text = PLesson.Name;
            txtTeacherName.Text = DB.GetTeacherName(PRoom.TeacherID);
            nudPopulation.Maximum = max;
            nudPopulation.Text = "1";
            nudPopulation.Minimum = 1;
            if(isedditing)
            {
                spDummy1.Visibility = Visibility.Collapsed;
                spDummy2.Visibility = Visibility.Visible;
            }
        }
        private void rbtnAll_Checked(object sender, RoutedEventArgs e)
        {
            rbtnLCode.IsChecked = false;
            rbtnLName.IsChecked = false;
            rbtnGCode.IsChecked = false;
            txtGCode.IsReadOnly = true;
            txtLName.IsReadOnly = true;
            txtLCode.IsReadOnly = true;
            txtGCode.Text = "";
            txtLCode.Text = "";
            txtLName.Text = "";
        }

        private void rbtnLName_Checked(object sender, RoutedEventArgs e)
        {
            rbtnAll.IsChecked = false;
            rbtnLCode.IsChecked = false;
            rbtnGCode.IsChecked = false;
            txtLName.IsReadOnly = false;
            txtGCode.IsReadOnly = true;
            txtLCode.IsReadOnly = true;
            txtGCode.Text = "";
            txtLCode.Text = "";
            txtLName.Text = "";
        }

        private void rbtnLCode_Checked(object sender, RoutedEventArgs e)
        {
            rbtnAll.IsChecked= false;
            rbtnLName.IsChecked= false;
            rbtnGCode.IsChecked = false;
            txtLName.IsReadOnly= true;
            txtGCode.IsReadOnly = true;
            txtLCode.IsReadOnly = false;
            txtGCode.Text = "";
            txtLCode.Text = "";
            txtLName.Text = "";
        }
        private void rbtnGCode_Checked(object sender, RoutedEventArgs e)
        {
            rbtnAll.IsChecked = false;
            rbtnLCode.IsChecked= false;
            rbtnLName.IsChecked = false;
            txtLName.IsReadOnly= true;
            txtLCode.IsReadOnly = true;
            txtGCode.IsReadOnly = false;
            txtGCode.Text = "";
            txtLCode.Text = "";
            txtLName.Text = "";
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnAll.IsChecked == true)
            {
                dgvLessons.ItemsSource = DB.Lessons;
            }
            else if (rbtnLName.IsChecked == true)
            {
                dgvLessons.ItemsSource = DB.ReadLessons(Data.Type.Name, txtLName.Text);
            }
            else if (rbtnLCode.IsChecked == true)
            {
                dgvLessons.ItemsSource = DB.ReadLessons(Data.Type.ID, txtLCode.Text);
            }
            else if(rbtnGCode.IsChecked == true)
            {
                dgvLessons.ItemsSource = DB.ReadLessons(Data.Type.Group, txtGCode.Text);
            }
        }

        private void dgvLessons_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if ((dgvLessons.SelectedItem as Lesson) != null)
                {
                    Lesson temp = (dgvLessons.SelectedItem as Lesson);
                    txtMojtamaName.Text = temp.Name;
                }
            }
            catch { }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Lesson temp = dgvLessons.SelectedItem as Lesson;
            Teacher teach = DB.GetTeacher(PRoom.TeacherID);
            if (temp != null) 
            {
                Class temp1 = new Class(0,PRoom.Code, temp, teach, PRoom.ClassT, PRoom.ClassD, PRoom.ExamT, PRoom.ExamD, Int32.Parse(nudPopulation.Text), PRoom.LessonID);
                try
                {
                    DB.AddMojtamaClass(temp1);
                    MessageBox.Show("کلاس با موفقیت اضافه شد");
                    this.Close();
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Lesson temp = dgvLessons.SelectedItem as Lesson;
            Teacher teach = DB.GetTeacher(PRoom.TeacherID);
            if (temp != null)
            {
                Class temp1 = new Class(PRoom.ID, PRoom.Code, temp, teach, PRoom.ClassT, PRoom.ClassD, PRoom.ExamT, PRoom.ExamD, Int32.Parse(nudPopulation.Text), PRoom.MojtamaID);
                try
                {
                    DB.UpdateMojtamaClass(temp1,temp1.ID);
                    MessageBox.Show("کلاس با موفقیت تغییر کرد");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


    }
}
