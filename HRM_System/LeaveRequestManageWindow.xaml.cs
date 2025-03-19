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
using Repositories;
using Services;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for LeaveRequestManageWindow.xaml
    /// </summary>
    public partial class LeaveRequestManageWindow : Window
    {
        private readonly ILeaveRequestService _leaveRequestService;

        private void LoadLeaveRequests()
        {
            var leaveRequests = _leaveRequestService.GetAllLeaveRequests();
            leaveRequestDataGrid.ItemsSource = leaveRequests;
        }
        public LeaveRequestManageWindow()
        {
            InitializeComponent();
            _leaveRequestService = new LeaveRequestService(new LeaveRequestRepository());
            LoadLeaveRequests();
        }

        private void LRTracking_Click(object sender, RoutedEventArgs e)
        {
            LRTrackingWindow record = new LRTrackingWindow();
            record.Show();
            this.Close();
        }
       
        
        //test có thể xóa
        private void test_Click(object sender, RoutedEventArgs e)
        {
            TimekeepingWindow record = new TimekeepingWindow();
            record.Show();
            this.Close();
        }
        //test có thể xóa

        private LeaveRequest? selectedLeaveRequest;

        private void leaveRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedLeaveRequest = (LeaveRequest)leaveRequestDataGrid.SelectedItem;
            if (selectedLeaveRequest != null)
            {
                txtLeaveDetails.Text = $"Mã nghỉ: {selectedLeaveRequest.LeaveId}\n" +
                                        $"Mã nhân viên: {selectedLeaveRequest.EmployeeId}\n" +
                                        $"Tên nhân viên: {selectedLeaveRequest.Employee.FullName}\n" +
                                        $"Loại nghỉ: {selectedLeaveRequest.LeaveType}\n" +
                                        $"Ngày bắt đầu: {selectedLeaveRequest.StartDate}\n" +
                                        $"Ngày kết thúc: {selectedLeaveRequest.EndDate}\n" +
                                        $"Trạng thái: {selectedLeaveRequest.Status}";
            }
        }

        private void UpdateRefuse_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLeaveRequest != null)
            {
                _leaveRequestService.UpdateLeaveRequestStatus(selectedLeaveRequest.LeaveId, "Từ chối");
                LoadLeaveRequests();
            }
        }

        private void UpdateApprove_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLeaveRequest != null)
            {
                _leaveRequestService.UpdateLeaveRequestStatus(selectedLeaveRequest.LeaveId, "Đã duyệt");
                LoadLeaveRequests();
            }
        }
    }
}
