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
using Repositories;
using Services;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for AttendanceManageWindow.xaml
    /// </summary>
    public partial class AttendanceManageWindow : Window
    {
        private readonly IAttendanceService _attendanceService;

        private void LoadAttendances()
        {
            var attendances = _attendanceService.GetAllAttendances();
            attendanceDataGrid.ItemsSource = attendances;
        }
        public AttendanceManageWindow()
        {
            InitializeComponent();
            _attendanceService = new AttendanceService(new AttendanceRepository());
            LoadAttendances();
        }

        private void ReportAttendance_Click(object sender, RoutedEventArgs e)
        {

            ReportAttendanceWindow record = new ReportAttendanceWindow();
            record.Show();
            this.Close();
        }


    }
}
