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
using DataAccessLayer;
using Repositories;
using Services;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for TimekeepingWindow.xaml
    /// </summary>
    public partial class TimekeepingWindow : Window
    {
        private readonly IAttendanceService _attendanceService;
        private void LoadTimeKeepings()
        {
            var attendances = _attendanceService.GetAllAttendanceByEmployeeId(1);
            timeKeepingDataGrid.ItemsSource = attendances;
        }
        public TimekeepingWindow()
        {
            InitializeComponent();
            _attendanceService = new AttendanceService(new AttendanceRepository());
            LoadTimeKeepings();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new HrmSystemContext())
            {
                int sessionEmployeeId = 1; // Giả định EmployeeId lấy từ session
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
                TimeOnly endOfWork = new TimeOnly(17, 0, 0); // Giờ giới hạn chấm công

                if (now > endOfWork)
                {
                    MessageBox.Show("Đã quá 17:00, không thể chấm công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (context.Attendances.Any(a => a.EmployeeId == sessionEmployeeId && a.WorkDate == today))
                {
                    MessageBox.Show("Bạn đã chấm công rồi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newTimeKeeping = new Attendance
                        {
                            EmployeeId = sessionEmployeeId,
                            WorkDate = today,
                            CheckInTime = now,
                            CheckOutTime = null,
                            OvertimeHours = null,
                            LeaveType = "Đi làm"
                        };

                        context.Attendances.Add(newTimeKeeping);
                        context.SaveChanges(); // Đảm bảo AttendanceId đã có

                        var user = context.Users.FirstOrDefault(u => u.EmployeeId == sessionEmployeeId);
                        if (user == null)
                        {
                            throw new Exception("Không tìm thấy tài khoản người dùng.");
                        }

                        var log = new ActivityLog
                        {
                            UserId = user.UserId,
                            Action = "CREATE",
                            TableName = "Attendance",
                            TablePrimaryKey = "AttendanceId",
                            RecordId = newTimeKeeping.AttendanceId,
                            Details = "Tạo chấm công mới trong ngày",
                            Timestamp = DateTime.Now,
                        };

                        context.ActivityLogs.Add(log);
                        context.SaveChanges();

                        transaction.Commit();
                        MessageBox.Show("Chấm công thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTimeKeepings();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }




        private void Update_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new HrmSystemContext())
            {
                int sessionEmployeeId = 1; // Giả định EmployeeId từ session login
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
                TimeOnly endOfWork = new TimeOnly(17, 0, 0); // Giờ làm việc kết thúc

                var attendance = context.Attendances
                    .FirstOrDefault(a => a.EmployeeId == sessionEmployeeId && a.WorkDate == today);

                if (attendance != null)
                {
                    if (attendance.CheckOutTime != null)
                    {
                        MessageBox.Show("Bạn đã chấm công giờ ra rồi, không thể cập nhật lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Không cho phép cập nhật nếu đã có CheckOutTime
                    }

                    attendance.CheckOutTime = now;

                    // Tính số giờ làm thêm (chỉ tính nếu CheckOutTime sau 17:00)
                    if (now > endOfWork)
                    {
                        attendance.OvertimeHours = Math.Round((decimal)(now.ToTimeSpan().TotalHours - endOfWork.ToTimeSpan().TotalHours), 2);
                    }
                    else
                    {
                        attendance.OvertimeHours = 0; // Nếu checkout trước 17:00 thì không có giờ làm thêm
                    }

                    // Cập nhật attendance vào database
                    _attendanceService.UpdateAttendance(attendance);

                    // 🔥 Thêm transaction để đảm bảo cả 2 hành động đều được thực thi
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Lấy UserId từ EmployeeId
                            var user = context.Users.FirstOrDefault(u => u.EmployeeId == sessionEmployeeId);
                            if (user == null)
                            {
                                MessageBox.Show("Không tìm thấy tài khoản người dùng cho nhân viên này!",
                                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            // Create new activity log
                            var CheckOut = new ActivityLog
                            {
                                UserId = user.UserId, // Gán UserId lấy từ User
                                Action = "UPDATE",
                                TableName = "Attendance",
                                TablePrimaryKey = "AttendanceId",
                                RecordId = attendance.AttendanceId,
                                Details = "Cập nhật thời gian chấm công ra về",
                                Timestamp = DateTime.Now,
                            };

                            // Add activity log vào context
                            context.ActivityLogs.Add(CheckOut);

                            // 🔥 Lưu cả attendance và activity log
                            context.SaveChanges();

                            // Commit transaction
                            transaction.Commit();

                            MessageBox.Show("Cập nhật giờ check-out thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Load lại dữ liệu để cập nhật giao diện ngay lập tức
                            LoadTimeKeepings();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Lỗi khi cập nhật log chấm công: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bản ghi chấm công của bạn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }



        private void AddLeave_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (startdatePicker.SelectedDate == null || enddatePicker.SelectedDate == null || leaveTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ ngày bắt đầu, ngày kết thúc và loại nghỉ phép!",
                    "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Convert selected dates to DateOnly
            DateOnly startDate = DateOnly.FromDateTime(startdatePicker.SelectedDate.Value);
            DateOnly endDate = DateOnly.FromDateTime(enddatePicker.SelectedDate.Value);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            // Validate date range
            if (endDate < startDate)
            {
                MessageBox.Show("Ngày kết thúc phải sau hoặc trùng với ngày bắt đầu!",
                    "Lỗi ngày tháng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (endDate < today || startDate < today)
            {
                MessageBox.Show("Ngày đã qua không thể xin nghỉ phép!",
                    "Lỗi ngày tháng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Get selected leave type
            string leaveType = ((ComboBoxItem)leaveTypeComboBox.SelectedItem).Content.ToString();

            // Session employee ID (assumed to be 1)
            int sessionEmployeeId = 1;

            using (var context = new HrmSystemContext())
            {
                // Check if employee already checked in on any of the requested leave days
                bool alreadyCheckedIn = context.Attendances
                    .Any(a => a.EmployeeId == sessionEmployeeId &&
                              a.WorkDate >= startDate &&
                              a.WorkDate <= endDate &&
                              a.LeaveType == "Đi làm");

                if (alreadyCheckedIn)
                {
                    MessageBox.Show("Bạn đã chấm công trong khoảng thời gian này, không thể xin nghỉ phép!",
                        "Trùng lịch", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Begin transaction
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Create new leave request
                        var leaveRequest = new LeaveRequest
                        {
                            EmployeeId = sessionEmployeeId,
                            LeaveType = leaveType,
                            StartDate = startDate,
                            EndDate = endDate,
                            Status = "Chờ duyệt"
                        };
                        // Add leave request to context
                        context.LeaveRequests.Add(leaveRequest);

// --------------------------------------------------------------------------------------------------------
                        // Lấy UserId từ EmployeeId
                        var user = context.Users.FirstOrDefault(u => u.EmployeeId == sessionEmployeeId);
                        if (user == null)
                        {
                            MessageBox.Show("Không tìm thấy tài khoản người dùng cho nhân viên này!",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // Create new activity log
                        var takeLeave = new ActivityLog
                        {
                            UserId = user.UserId, // Gán UserId lấy từ User
                            Action = "CREATE",
                            TableName = "LeaveRequests",
                            TablePrimaryKey = "LeaveId",
                            RecordId = leaveRequest.LeaveId,
                            Details = "Tạo đơn xin nghỉ phép",
                            Timestamp = DateTime.Now,
                        };

                        // Add activity log vào context
                        context.ActivityLogs.Add(takeLeave);
// --------------------------------------------------------------------------------------------------------

                        // Create attendance records for each day in the leave period
                        DateOnly currentDate = startDate;
                        while (currentDate <= endDate)
                        {
                            // Check if there's an existing attendance record for this date
                            bool existingAttendance = context.Attendances
                                .Any(a => a.EmployeeId == sessionEmployeeId && a.WorkDate == currentDate);

                            if (!existingAttendance)
                            {
                                var attendance = new Attendance
                                {
                                    EmployeeId = sessionEmployeeId,
                                    WorkDate = currentDate,
                                    CheckInTime = null,
                                    CheckOutTime = null,
                                    OvertimeHours = null,
                                    LeaveType = leaveType
                                };

                                context.Attendances.Add(attendance);
                            }

                            currentDate = currentDate.AddDays(1);
                        }

                        // Save changes and commit transaction
                        context.SaveChanges();
                        transaction.Commit();

                        MessageBox.Show("Đơn xin nghỉ phép đã được gửi và đang chờ duyệt!",
                            "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Reset form fields
                        startdatePicker.SelectedDate = null;
                        enddatePicker.SelectedDate = null;
                        leaveTypeComboBox.SelectedItem = null;

                        // Refresh attendance list
                        LoadTimeKeepings();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction in case of error
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi khi tạo đơn nghỉ phép: {ex.Message}",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LeaveTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTimeKeepings();
        }

        //test có thể xóa
        private void test_Click(object sender, RoutedEventArgs e)
        {
            LeaveRequestManageWindow record = new LeaveRequestManageWindow();
            record.Show();
            this.Close();
        }

        private void testActivity_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogManageWindow record = new ActivityLogManageWindow();
            record.Show();
            this.Close();
        }
        //test có thể xóa
    }
}
