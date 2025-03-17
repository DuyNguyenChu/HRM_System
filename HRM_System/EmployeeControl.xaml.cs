using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repositories;
using Services;
using static System.Net.Mime.MediaTypeNames;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for EmployeeControl.xaml
    /// </summary>
    public partial class EmployeeControl : Window
    {
        private readonly IEmployeeServices _employeeServices;
        private Employee? selectedEmployee;
        public EmployeeControl()
        {
            InitializeComponent();
            _employeeServices = new EmployeeServices();
            LoadData();
        }
        private void LoadData()
        {
            dgShowData.ItemsSource = _employeeServices.GetAllEmployees();
        }
        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            dgShowData.ItemsSource = _employeeServices.SearchEmployee(keyword);
        }
        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgShowData.SelectedItem is Employee employee)
            {
                selectedEmployee = employee;

                txtEmployeeID.Text = employee.EmployeeId.ToString();
                txtFullName.Text = employee.FullName;
                txtDateOfBirth.SelectedDate = employee.DateOfBirth.ToDateTime(TimeOnly.MinValue);

                txtGender.SelectedValue = employee.Gender;

                txtAddress.Text = employee.Address;
                txtPhoneNumber.Text = employee.PhoneNumber;
                txtEmail.Text = employee.Email;
                txtDepartmentID.Text = employee.DepartmentId.ToString();
                txtPosition.Text = employee.Position;
                txtSalary.Text = employee.Salary.ToString();
                txtStartDate.SelectedDate = employee.StartDate.ToDateTime(TimeOnly.MinValue);
                txtProfileImage.Text = employee.ProfileImage;
                LoadProfileImage(employee.ProfileImage);
            }
            else
            {
                btnReset_Click(sender, e);
            }
        }

        private void LoadProfileImage(string imageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                {
                    imgProfile.Source = null;
                    return;
                }

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgProfile.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}");
            }
        }


        private bool ValidateInput(out string FullName, out DateOnly DateOfBirth, out string Gender, out string? Address, out string PhoneNumber, out string? Email, out int? DepartmentId, out string? Position, out decimal Salary, out DateOnly StartDate, out string? ProfileImage)
        {
            List<string> errors = new List<string>();

            FullName = txtFullName.Text.Trim();
            Address = txtAddress.Text.Trim();
            PhoneNumber = txtPhoneNumber.Text.Trim();
            Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            Gender = txtGender.Text.Trim();
            Position = txtPosition.Text.Trim();
            ProfileImage = txtProfileImage.Text.Trim();
            var isExist = _employeeServices.GetAllEmployees()
                            .Any(e => e.PhoneNumber == txtPhoneNumber.Text || e.Email == txtEmail.Text);

            if (isExist)
            {
                errors.Add("Số điện thoại hoặc email đã tồn tại!");
            }

            if (string.IsNullOrWhiteSpace(FullName))
                errors.Add("Hãy điền tên nhân viên.");

            if (!DateOnly.TryParse(txtDateOfBirth.Text.Trim(), out DateOfBirth))
                errors.Add("hãy điền ngày sinh hợp lệ.");

            if (string.IsNullOrWhiteSpace(Gender) || !(Gender == "Nam" || Gender == "Nữ" || Gender == "Khác"))
                errors.Add("Hãy lựa chọn giới tính.");

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                errors.Add("Hãy điền số điện thoại.");
            else if (!PhoneNumber.All(char.IsDigit) || PhoneNumber.Length != 10)
                errors.Add("Hãy điền số điện thoại hợp lệ.");

            if (!string.IsNullOrWhiteSpace(Email))
            {
                string emailToCheck = Email;
                string[] validDomains = { "@gmail.com", "@yahoo.com", "@outlook.com", "@hotmail.com" };

                if (!emailToCheck.Contains('@') || !validDomains.Any(domain => emailToCheck.EndsWith(domain)))
                    errors.Add("Hãy điền email hợp lệ.");
            }


            if (string.IsNullOrWhiteSpace(txtDepartmentID.Text))
                DepartmentId = null;
            else if (!int.TryParse(txtDepartmentID.Text.Trim(), out int deptId))
            {
                errors.Add("Hãy điền mã phòng ban hợp lệ.");
                DepartmentId = null;
            }
            else
            {
                DepartmentId = deptId;
            }

            if (!decimal.TryParse(txtSalary.Text.Trim(), out Salary) || Salary < 0)
                errors.Add("Lương không thể nhỏ hơn 0.");

            if (!DateOnly.TryParse(txtStartDate.Text.Trim(), out StartDate))
                errors.Add("Ngày bắt đầu phải hợp lệ.");
            else if (DateOfBirth != default && StartDate < DateOfBirth)
                errors.Add("Ngày bắt đầu không thể trước ngày sinh.");
            else if (StartDate.AddYears(-18) < DateOfBirth)
                errors.Add("Nhân viên phải ít nhất 18 tuổi.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Lỗi đầu vào", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput(out string FullName, out DateOnly DateOfBirth, out string Gender, out string? Address, out string PhoneNumber, out string? Email, out int? DepartmentId, out string? Position, out decimal Salary, out DateOnly StartDate, out string? ProfileImage)) return;
                var NewContent = new Employee
                {
                    FullName = FullName,
                    DateOfBirth = DateOfBirth,
                    Gender = Gender,
                    Address = Address,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    DepartmentId = DepartmentId,
                    Position = Position,
                    Salary = Salary,
                    StartDate = StartDate,
                    ProfileImage = ProfileImage,
                };

                //var NewContent = new Employee
                //{
                //    FullName = txtFullName.Text,
                //    DateOfBirth = string.IsNullOrWhiteSpace(txtDateOfBirth.Text)
                //                  ? default
                //                  : DateOnly.FromDateTime(DateTime.Parse(txtDateOfBirth.Text)),

                //    Gender = txtGender.SelectedItem is ComboBoxItem selectedGender
                //             ? selectedGender.Content.ToString()
                //             : "Other",

                //    Address = txtAddress.Text,
                //    PhoneNumber = txtPhoneNumber.Text,
                //    Email = txtEmail.Text,

                //    DepartmentId = int.TryParse(txtDepartmentID.Text, out int deptId) ? deptId : null,
                //    Salary = decimal.TryParse(txtSalary.Text, out decimal salary) ? salary : 0,

                //    StartDate = DateOnly.FromDateTime(DateTime.Now)
                //};

                _employeeServices.AddEmployee(NewContent);
                btnReset_Click(sender, e);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thêm: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!ValidateInput(out string FullName, out DateOnly DateOfBirth, out string Gender, out string? Address, out string PhoneNumber, out string? Email, out int? DepartmentId, out decimal Salary, out DateOnly StartDate)) return;
                if (selectedEmployee == null)
                {
                    MessageBox.Show("Hãy chọn nội dung cần sửa!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //selectedEmployee.FullName = FullName;
                //selectedEmployee.DateOfBirth = DateOfBirth;
                //selectedEmployee.Gender = Gender;
                //selectedEmployee.Address = Address;
                //selectedEmployee.PhoneNumber = PhoneNumber;
                //selectedEmployee.Email = Email;
                //selectedEmployee.DepartmentId = DepartmentId;
                //selectedEmployee.Salary = Salary;
                //selectedEmployee.StartDate = StartDate;

                selectedEmployee.FullName = txtFullName.Text;
                selectedEmployee.DateOfBirth = string.IsNullOrWhiteSpace(txtDateOfBirth.Text)
                                  ? default
                                  : DateOnly.FromDateTime(DateTime.Parse(txtDateOfBirth.Text));
                selectedEmployee.Gender = (txtGender.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "khác";

                selectedEmployee.Address = txtAddress.Text;
                selectedEmployee.PhoneNumber = txtPhoneNumber.Text;
                selectedEmployee.Email = txtEmail.Text;
                selectedEmployee.DepartmentId = int.TryParse(txtDepartmentID.Text, out int deptId) ? deptId : null;
                selectedEmployee.Position = txtPosition.Text;
                selectedEmployee.Salary = decimal.TryParse(txtSalary.Text, out decimal salary) ? salary : 0;
                selectedEmployee.StartDate = string.IsNullOrWhiteSpace(txtDateOfBirth.Text)
                                  ? default
                                  : DateOnly.FromDateTime(DateTime.Parse(txtStartDate.Text));
                selectedEmployee.ProfileImage = txtProfileImage.Text;
                _employeeServices.UpdateEmployee(selectedEmployee);
                btnReset_Click(sender, e);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lõi khi sửa: {ex.Message}");

            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!ValidateInput(out string FullName, out DateOnly DateOfBirth, out string Gender, out string? Address, out string PhoneNumber, out string? Email, out int? DepartmentId, out string? Position, out decimal Salary, out DateOnly StartDate, out string? ProfileImage)) return;
                if (selectedEmployee == null)
                {
                    MessageBox.Show("Hãy chọn nội dung cần xóa!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = MessageBox.Show($"Bạn muốn xóa dữ liệu này không? {txtFullName}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _employeeServices.DeleteEmployee(selectedEmployee);
                    LoadData();
                    btnReset_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}");
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            selectedEmployee = null;
            txtEmployeeID.Clear();
            txtDateOfBirth.SelectedDate = null;
            txtFullName.Clear();
            txtGender.SelectedIndex = -1;
            txtAddress.Clear();
            txtPhoneNumber.Clear();
            txtEmail.Clear();
            txtDepartmentID.Clear();
            txtPosition.Clear();
            txtSalary.Clear();
            txtStartDate.SelectedDate = null;
            txtProfileImage.Clear();
        }

    }
}
