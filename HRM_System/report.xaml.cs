using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Services;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for report.xaml
    /// </summary>
    public partial class report : Window
    {
        private readonly IEmployeeServices _employeeService;
        public report()
        {
            InitializeComponent();
           
            _employeeService = new EmployeeServices(); LoadData();
        }
        private void LoadData()

        {
            var Employees = _employeeService.GetAllEmployees();
            var departmentStats = Employees
                .GroupBy(e => e.Department)
                .Select(g => new { Department = g.Key, Count = g.Count() })
                .ToList();
            DepartmentStatsGrid.ItemsSource = departmentStats;

            
            var positionStats = Employees
                .GroupBy(e => e.Position)
                .Select(g => new { Position = g.Key, Count = g.Count() })
                .ToList();
            PositionStatsGrid.ItemsSource = positionStats;

            
            var genderStats = Employees
                .GroupBy(e => e.Gender)
                .Select(g => new { Gender = g.Key, Count = g.Count() })
                .ToList();
            GenderStatsGrid.ItemsSource = genderStats;
        }
        public class ExcelExporter
        {
            public static void ExportStatisticsToExcel(List<Employee> employees, string filePath)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required for EPPlus

                    using (var package = new ExcelPackage())
                    {
                        var workbook = package.Workbook;

                        // 1️⃣ Thống kê theo Phòng Ban
                        var departmentStats = employees
                   .Where(e => e.Department != null)
                   .GroupBy(e => e.Department.DepartmentName) // Chuyển Object thành string
                   .Select(g => new Tuple<string, int>(g.Key, g.Count()))
                   .ToList();

                        // 2️⃣ Thống kê theo Chức Vụ
                        var positionStats = employees
                            .Where(e => !string.IsNullOrEmpty(e.Position))
                            .GroupBy(e => e.Position)
                            .Select(g => new Tuple<string, int>(g.Key, g.Count()))
                            .ToList();

                        // 3️⃣ Thống kê theo Giới Tính
                        var genderStats = employees
                            .Where(e => !string.IsNullOrEmpty(e.Gender))
                            .GroupBy(e => e.Gender)
                            .Select(g => new Tuple<string, int>(g.Key, g.Count()))
                            .ToList();

                        // 📊 Tạo các sheet trong file Excel
                        AddWorksheet(workbook, "Department Stats", "Department", departmentStats);
                        AddWorksheet(workbook, "Position Stats", "Position", positionStats);
                        AddWorksheet(workbook, "Gender Stats", "Gender", genderStats);

                        // ✅ Lưu file Excel
                        package.SaveAs(new FileInfo(filePath));
                        MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // 🔹 Hàm hỗ trợ tạo sheet trong Excel
            private static void AddWorksheet(ExcelWorkbook workbook, string sheetName, string column1, List<Tuple<string, int>> data)
            {
                var worksheet = workbook.Worksheets.Add(sheetName);
                worksheet.Cells[1, 1].Value = column1;
                worksheet.Cells[1, 2].Value = "Count";

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.Item1;
                    worksheet.Cells[row, 2].Value = item.Item2;
                    row++;
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            var employees = _employeeService.GetAllEmployees();
            string filePath = "D:\\EmployeeData.xlsx";
            ExcelExporter.ExportStatisticsToExcel(employees, filePath);


        }
}
}
