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
    /// Interaction logic for ActivityLogManageWindow.xaml
    /// </summary>
    public partial class ActivityLogManageWindow : Window
    {
        private readonly IActivityLogService _activityLogService;
        private void LoadActivityLogs()
        {
            var activityLogs = _activityLogService.GetAllActivityLogs();
            activityLogDataGrid.ItemsSource = activityLogs;
        }
        public ActivityLogManageWindow()
        {
            InitializeComponent();
            _activityLogService = new ActivityLogService(new ActivityLogRepository());
            LoadActivityLogs();
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            TimekeepingWindow record = new TimekeepingWindow();
            record.Show();
            this.Close();
        }
    }
}
