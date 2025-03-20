using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer
{
    public class EmployeesDao
    {
        public static List<Employee> GetAllEmployees()
        {
            try
            {
                using var context = new HrmSystemContext();
                return context.Employees.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không lấy được dữ liệu: {ex.Message}");
                return new List<Employee>();
            }
        }

        public static List<Employee> SearchEmployee(string keyword)
        {
            try
            {
                using var context = new HrmSystemContext();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return context.Employees.ToList();
                }

                keyword = keyword.ToLower().Trim();

                return context.Employees
                    .Where(b =>
                        (b.FullName != null && b.FullName.ToLower().Contains(keyword)) ||
                        (b.Address != null && b.Address.ToLower().Contains(keyword)) ||
                        (b.PhoneNumber != null && b.PhoneNumber.Contains(keyword)) || // Không cần ToLower()
                        (b.Email != null && b.Email.ToLower().Contains(keyword)) ||
                        (b.Position != null && b.Position.ToLower().Contains(keyword)) ||
                        (b.Salary.ToString().Contains(keyword)) 
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không tìm được dữ liệu: {ex.Message}");
                return new List<Employee>();
            }
        }



        public static void AddEmployee(Employee p)
        {
            try
            {
                using var context = new HrmSystemContext();
                context.Employees.Add(p);
                MessageBox.Show("Thêm thành công!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                context.SaveChanges();

                ActivityLogDAO.AddActivityLog(new ActivityLog
                {
                    UserId = 1,
                    Action = "Thêm nhận viên",
                    TableName = "Employee", //Tên bảng
                    TablePrimaryKey = "EmployeeId", // Tên khóa chính của bảng
                    RecordId = p.EmployeeId, // khóa chính trong bảng đấy bị thao tác
                    Details = $"Thêm nhận viên mới: {p.FullName}",
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không thêm được dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public static void UpdateEmployee(Employee p)
        {
            try
            {
                using var context = new HrmSystemContext();
                context.Entry(p).State = EntityState.Modified;
                MessageBox.Show("Sửa thành công!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                context.SaveChanges();

                ActivityLogDAO.AddActivityLog(new ActivityLog
                {
                    UserId = 1,
                    Action = "Sửa nhận viên",
                    TableName = "Employee", //Tên bảng
                    TablePrimaryKey = "EmployeeId", // Tên khóa chính của bảng
                    RecordId = p.EmployeeId, // khóa chính trong bảng đấy bị thao tác
                    Details = $"Sửa nhận viên: {p.FullName}",
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không sửa được dữ liệu:: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void DeleteEmployee(Employee p)
        {
            try
            {
                using (var context = new HrmSystemContext())
                {
                    var DeleteContext = context.Employees
                        .FirstOrDefault(c => c.EmployeeId == p.EmployeeId
                                          && c.FullName == p.FullName
                                          && c.DateOfBirth == p.DateOfBirth);

                    if (DeleteContext == null)
                    {
                        MessageBox.Show("Hãy chọn đối tượng cần xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    context.Employees.Remove(DeleteContext);
                    MessageBox.Show("Xóa thành công!");
                    context.SaveChanges();

                    ActivityLogDAO.AddActivityLog(new ActivityLog
                    {
                        UserId = 1,
                        Action = "Xóa nhận viên",
                        TableName = "Employee", //Tên bảng
                        TablePrimaryKey = "EmployeeId", // Tên khóa chính của bảng
                        RecordId = p.EmployeeId, // khóa chính trong bảng đấy bị thao tác
                        Details = $"Xóa nhận viên: {p.FullName}",
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xóa được dữ liệu:: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
