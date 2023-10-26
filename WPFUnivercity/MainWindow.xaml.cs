using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClosedXML.Excel;
using Data;
using Data.Models;
using Microsoft.Win32;

namespace WPFUnivercity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database db = new Database();
        private Lesson? currentlesson;
        private Class? currentclass;
        public MainWindow()
        {
            InitializeComponent();
            //crating database instance
            try
            {
                db = new Database();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            FillDataGrids();
        }

        private void FillDataGrids()
        {
            dgOLessons.ItemsSource = db.PrimaryLessonsFinder();
            dgALessons.ItemsSource = db.Classes;
        }
        private void btnOrginal_Click(object sender, RoutedEventArgs e)
        {
            brdOrginal.Visibility = Visibility.Visible;
            brdMojtama.Visibility = Visibility.Collapsed;
            FillDataGrids();
            this.Width = 988;
        }

        private void btnMojtama_Click(object sender, RoutedEventArgs e)
        {
            brdMojtama.Visibility = Visibility.Visible;
            brdOrginal.Visibility = Visibility.Collapsed;
            this.Width = 1400;
        }
        private void btnAddOrg_Click(object sender, RoutedEventArgs e)
        {
            new AddEditPrimaryLesson(db!).ShowDialog();
            FillDataGrids();
        }

        private void btnEditOrg_Click(object sender, RoutedEventArgs e)
        {
            currentlesson = dgOLessons.SelectedItem as Lesson;
            if (currentlesson != null)
            {
                currentclass = db.GetClass(currentlesson.ID);
                new AddEditPrimaryLesson(db, currentlesson, currentclass).ShowDialog();
                currentlesson = null;
                currentclass = null;
                FillDataGrids();
            }
            else
            {
                MessageBox.Show("لطفا برای ایجاد تغییر از دروس بالا یکی را انتخاب کنید");
            }
        }

        private void dgALessons_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            currentclass = dgALessons.SelectedItem as Class;
            if(currentclass != null)
            {
                currentlesson = db.GetLesson(currentclass!.LessonID);
                bool flag = db.isPrimaryLesson(currentlesson.ID);
                if (flag == true)
                {
                    bt1.Visibility = Visibility.Visible;
                    bt2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    bt1.Visibility = Visibility.Collapsed;
                    bt2.Visibility = Visibility.Visible;
                }
                currentlesson = null;
                currentclass = null;
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Class temp = (dgALessons.SelectedItem as Class)!;
            int max =temp.Population - MaxPop(temp);
            if(max > 0)
            {
                new AddEditMojtama(db, temp, max,false).ShowDialog();
            }
            else
            {
                MessageBox.Show("ظرفیت اصلی این کلاس تکمیل میباشد");
            }

        }
        private int MaxPop(Class room)
        {
            int pop = 0;
            foreach (Class croom in db.Classes)
            {
                if (croom.MojtamaID == room.LessonID)
                    pop += croom.Population;
            }
            return pop;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Class temp = (dgALessons.SelectedItem as Class)!;
            Class Ptemp = db.GetClass(temp.MojtamaID);
            int dummy = MaxPop(temp);
            int max = Ptemp.Population - MaxPop(Ptemp) + temp.Population;
            if (max > 0)
            {
                new AddEditMojtama(db, temp, max, true).ShowDialog();
            }
            else
            {
                MessageBox.Show("ظرفیت اصلی این کلاس تکمیل میباشد");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            string command = "";
            if (dgALessons.SelectedItems.Count <= 0)
            {
                command = "SELECT c_code AS 'شماره کلاس', (l_name ||CHAR(13)||CHAR(10)|| lesson_id) AS 'درس اصلی' ,(name||' '||last_name||CHAR(13)||CHAR(10)||teacher_id) AS 'استاد' , class_time AS 'ساعت کلاس' , class_day as 'روز کلاس' , exam_time as 'سانس امتحان' ,exam_date as 'تاریخ امتحان' , population as 'ظرفیت' , mojtama_id as 'کد درس مجتمع' , (g_name||CHAR(13)||CHAR(10)||grade) as 'گروه' , l_vahed as 'تعداد واحد' FROM class join teacher ON (class.teacher_id =  teacher.id) join lesson ON class.lesson_id = lesson.l_id join groh ON lesson.group_id = groh.g_id;";
            }
            else
            {
                string temp = $"class.c_id = {(dgALessons.SelectedItems[0] as Class).ID} ";
                for (int i = 1;i<dgALessons.SelectedItems.Count;i++)
                {
                    temp += $"OR class.c_id = {(dgALessons.SelectedItems[i] as Class).ID} ";
                }
                command = $"SELECT c_code AS 'شماره کلاس', (l_name || CHAR(13)||CHAR(10)|| lesson_id ) AS 'درس اصلی' ,(name||' '||last_name||CHAR(13)||CHAR(10)||teacher_id) AS 'استاد' , class_time AS 'ساعت کلاس' , class_day as 'روز کلاس' , exam_time as 'سانس امتحان' ,exam_date as 'تاریخ امتحان' , population as 'ظرفیت' , mojtama_id as 'کد درس مجتمع' , (g_name||CHAR(13)||CHAR(10)||grade) as 'گروه' , l_vahed as 'تعداد واحد' FROM class join teacher ON (class.teacher_id =  teacher.id)AND({temp})join lesson ON class.lesson_id = lesson.l_id join groh ON lesson.group_id = groh.g_id;";
            }
            dt = db.Getreport(command);
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Exel File|*.xlsx" };
            if(sfd.ShowDialog()==true)
            {
                using(XLWorkbook work = new XLWorkbook())
                {
                    try
                    {
                        var sheet = work.Worksheets.Add(dt, "دروس");
                        sheet.Columns().AdjustToContents();
                        sheet.RangeUsed().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        sheet.RangeUsed().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        sheet.RangeUsed().Style.Alignment.WrapText = true;
                        work.SaveAs(sfd.FileName);
                        MessageBox.Show($"فایل با موفقیت ساخته شد", "خروجی");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ارور در زمان ساخت فایل");
                    }
                }
            }
        }
    }
}
