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
    /// Interaction logic for UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        public UserMainWindow()
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



        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            DepartmentControl main = new DepartmentControl();
            main.Show();

        }
    }
}
