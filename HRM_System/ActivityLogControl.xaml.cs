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
using BusinessObjects;
using Services;
using static System.Net.Mime.MediaTypeNames;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for ActivityLogControl.xaml
    /// </summary>
    public partial class ActivityLogControl : Window
    {
        private readonly IActivityLogServices _ActivityLogServices;
        private ActivityLog? selectedItem;
        public ActivityLogControl()
        {
            InitializeComponent();
            _ActivityLogServices = new ActivityLogServices();
            LoadData();
        }
        private void LoadData()
        {
            dgShowData.ItemsSource = _ActivityLogServices.GetActivityLog();
        }
        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            dgShowData.ItemsSource = _ActivityLogServices.SearchActivityLog(keyword);
        }
        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgShowData.SelectedItem is ActivityLog activityLog)
            {
                selectedItem = activityLog;

                txtActivityLogId.Text = activityLog.ActivityLogId.ToString();
                txtUserId.Text = activityLog?.UserId.ToString();
                txtAction.Text = activityLog?.Action.ToString();
                txtTableName.Text = activityLog?.TableName.ToString();
                txtTablePrimaryKey.Text = activityLog?.TablePrimaryKey.ToString();
                txtRecordId.Text = activityLog?.RecordId.ToString();
                txtDetails.Text = activityLog?.Details?.ToString();
                txtTimestamp.Text = activityLog?.Timestamp.ToString();
            }
            else
            {
                btnReset_Click(sender, e);
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = null;
            txtActivityLogId.Clear();
            txtUserId.Clear();
            txtAction.Clear();
            txtTableName.Clear();
            txtTablePrimaryKey.Clear();
            txtRecordId.Clear();
            txtDetails.Clear();
            txtTimestamp.Clear();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedItem == null)
                {
                    MessageBox.Show("Hãy chọn nội dung cần xóa!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = MessageBox.Show($"Bạn muốn xóa dữ liệu này không? {txtAction} {txtTimestamp}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _ActivityLogServices.DeleteActivityLog(selectedItem);
                    LoadData();
                    btnReset_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi  khi xóa: {ex.Message}");
            }
        }
    }
}
