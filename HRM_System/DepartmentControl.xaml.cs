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

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for DepartmentControl.xaml
    /// </summary>
    public partial class DepartmentControl : Window
    {
        private readonly IDepartmentServices _DepartmentServices;
        private Department? selectedDepartment;
        public DepartmentControl()
        {
            InitializeComponent();
            _DepartmentServices = new DepartmentServices();
            LoadData();
        }
        private void LoadData()
        {
            dgShowData.ItemsSource = _DepartmentServices.GetAllDepartments();
        }
        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            dgShowData.ItemsSource = _DepartmentServices.SearchDepartment(keyword);
        }
        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgShowData.SelectedItem is Department ContentSelect)
            {
                selectedDepartment = ContentSelect;

                txtDepartmentID.Text = ContentSelect.DepartmentId.ToString();
                txtDepartmentName.Text = ContentSelect.DepartmentName;
            }
            else
            {
                btnReset_Click(sender, e);
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            selectedDepartment = null;
            txtDepartmentID.Clear();
            txtDepartmentName.Clear();
        }

        private bool ValidateInput(out string DepartmentName)
        {
            List<string> errors = new List<string>();

            DepartmentName = txtDepartmentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(DepartmentName))
                errors.Add("Hãy điền tên phòng ban.");

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
                if (!ValidateInput(out string DepartmentName)) return;
                var NewContent = new Department
                {
                    DepartmentName = DepartmentName,
                };


                _DepartmentServices.AddDepartment(NewContent);
                btnReset_Click(sender, e);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedDepartment == null)
                {
                    MessageBox.Show("Hãy chọn nội dung cần sửa!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedDepartment.DepartmentName = txtDepartmentName.Text;

                _DepartmentServices.UpdateDepartment(selectedDepartment);
                btnReset_Click(sender, e);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa: {ex.Message}");

            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedDepartment == null)
                {
                    MessageBox.Show("Hãy chọn nội dung cần xóa!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = MessageBox.Show($"Bạn có chắc muốn xóa {txtDepartmentName}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _DepartmentServices.DeleteDepartment(selectedDepartment);
                    LoadData();
                    btnReset_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"lỗi khi xóa: {ex.Message}");
            }
        }
    }
}
