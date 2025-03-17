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
    public class DepartmentDao
    {
        public static List<Department> GetAllDepartments()
        {
            try
            {
                using var context = new HrmSystemContext();
                return context.Departments.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không lấy được dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);


                return new List<Department>();
            }
        }

        public static List<Department> SearchDepartment(string keyword)
        {
            try
            {
                using var context = new HrmSystemContext();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return context.Departments.ToList();
                }

                keyword = keyword.ToLower().Trim();
                return context.Departments.Where(b =>
                                    (b.DepartmentName != null && b.DepartmentName.ToLower().Contains(keyword))).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không tìm được dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);


                return new List<Department>();
            }
        }

        public static void AddDepartment(Department p)
        {
            try
            {
                using var context = new HrmSystemContext();
                context.Departments.Add(p);
                MessageBox.Show("Thêm thành công");
                context.SaveChanges();

                ActivityLogDao.AddActivityLog(new ActivityLog
                {
                    UserId = 1,
                    Action = "Thêm phòng ban",
                    TableName = "Departments", //Tên bảng
                    TablePrimaryKey = "DepartmentId", // Tên khóa chính của bảng
                    RecordId = p.DepartmentId, // khóa chính trong bảng đấy bị thao tác
                    Details = $"Thêm phòng ban mới: {p.DepartmentName}",
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Lỗi không Thêm được dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        public static void UpdateDepartment(Department p)
        {
            try
            {
                using var context = new HrmSystemContext();
                context.Entry(p).State = EntityState.Modified;
                MessageBox.Show("Sửa thành công");
                context.SaveChanges();

                ActivityLogDao.AddActivityLog(new ActivityLog
                {
                    UserId = 1,
                    Action = "Sửa phòng ban",
                    TableName = "Departments", //Tên bảng
                    TablePrimaryKey = "DepartmentId", // Tên khóa chính của bảng
                    RecordId = p.DepartmentId, // khóa chính trong bảng đấy bị thao tác
                    Details = $"Sửa phòng ban: {p.DepartmentName}",
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không sửa được dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        public static void DeleteDepartment(Department p)
        {
            try
            {
                var context = new HrmSystemContext();
                var DeleteContext = context.Departments.FirstOrDefault(c => c.DepartmentId == p.DepartmentId && c.DepartmentName == p.DepartmentName);
                if (DeleteContext == null)
                {
                    MessageBox.Show("Không tìm thấy nội dung cần xóa");
                }
                else
                {
                    context.Departments.Remove(DeleteContext);
                    MessageBox.Show("Xóa thành công");
                    context.SaveChanges();

                    ActivityLogDao.AddActivityLog(new ActivityLog
                    {
                        UserId = 1,
                        Action = "Xóa phòng ban",
                        TableName = "Departments", //Tên bảng
                        TablePrimaryKey = "DepartmentId", // Tên khóa chính của bảng
                        RecordId = p.DepartmentId, // khóa chính trong bảng đấy bị thao tác
                        Details = $"Xóa phòng ban: {p.DepartmentName}",
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không Xóa được dữ liệu: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


    }
}
