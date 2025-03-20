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
    public class ActivityLogDAO
    {
        private readonly HrmSystemContext _context;

        public ActivityLogDAO()
        {
            _context = new HrmSystemContext();
        }

        public List<ActivityLog> GetAllActivityLogs()
        {
            return _context.ActivityLogs.Include(role => role.User).ToList();
        }

        public static List<ActivityLog> GetActivityLog()
        {
            try
            {
                using var context = new HrmSystemContext();
                return context.ActivityLogs.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<ActivityLog>();
            }
        }

        public static List<ActivityLog> SearchActivityLog(string keyword)
        {
            try
            {
                using var context = new HrmSystemContext();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return context.ActivityLogs.ToList();
                }

                keyword = keyword.ToLower().Trim();
                return context.ActivityLogs.Where(b =>
                                    (b.Action != null && b.Action.ToLower().Contains(keyword)) ||
                                    (b.TableName != null && b.TableName.ToLower().Contains(keyword)) ||
                                    (b.TablePrimaryKey != null && b.TablePrimaryKey.ToLower().Contains(keyword)) ||
                                    (b.Details != null && b.Details.ToLower().Contains(keyword)) ||
                                    (b.RecordId.ToString().Contains(keyword)) ||
                                    (b.Timestamp != null &&
                                        (
                                        b.Timestamp.Value.ToString("dd/MM/yyyy").Contains(keyword) ||
                                        b.Timestamp.Value.Day.ToString().Contains(keyword) ||
                                        b.Timestamp.Value.Month.ToString().Contains(keyword) ||
                                        b.Timestamp.Value.Year.ToString().Contains(keyword)
                                        )
                                    )).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<ActivityLog>();
            }
        }
        public static void AddActivityLog(ActivityLog p)
        {
            try
            {
                using var context = new HrmSystemContext();
                context.ActivityLogs.Add(p);
                MessageBox.Show("Hành động thêm đã được lưu");
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }
        public static void DeleteActivityLog(ActivityLog p)
        {
            try
            {
                var context = new HrmSystemContext();
                var DeleteContext = context.ActivityLogs.FirstOrDefault(c => c.ActivityLogId == p.ActivityLogId && c.UserId == p.UserId);
                if (DeleteContext == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cần xóa");
                }
                else
                {
                    context.ActivityLogs.Remove(DeleteContext);
                    MessageBox.Show("Hành động xóa đã được lưu");


                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.InnerException?.Message ?? ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
