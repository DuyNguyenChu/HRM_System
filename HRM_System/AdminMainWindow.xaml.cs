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

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bạn đã đăng xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogControl main = new ActivityLogControl();
            main.Show();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            EmployeeControl main = new EmployeeControl();
            main.Show();

        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            DepartmentControl main = new DepartmentControl();
            main.Show();

        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            report main = new report();
            main.Show();
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            TimekeepingWindow main = new TimekeepingWindow();
            main.Show();
        }



        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            AttendanceManageWindow main = new AttendanceManageWindow();
            main.Show();
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            LeaveRequestManageWindow main = new LeaveRequestManageWindow();
            main.Show();
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            LRTrackingWindow main = new LRTrackingWindow();
            main.Show();
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            ReportAttendanceWindow  main = new ReportAttendanceWindow();
            main.Show();
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            SalaryManageWindow main = new SalaryManageWindow();
            main.Show();
        }
    }
}
