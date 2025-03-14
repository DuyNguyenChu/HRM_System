using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Repositories;
using Services;

namespace HRM_System
{
    public partial class LRTrackingWindow : Window
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private List<LeaveRequest> _leaveRequests;
        private List<Employee> _employees;

        public LRTrackingWindow()
        {
            InitializeComponent();
            _leaveRequestService = new LeaveRequestService(new LeaveRequestRepository());
            InitializeYearComboBox();
            LoadEmployees();
            LoadLeaveRequests();
        }

        private void InitializeYearComboBox()
        {
            for (int year = 2000; year <= 2025; year++)
                yearComboBox.Items.Add(year);
            yearComboBox.SelectedItem = DateTime.Now.Year;
        }

        private void LoadEmployees()
        {
            _employees = _leaveRequestService.GetAllLeaveRequests()
                .Select(lr => lr.Employee)
                .Distinct()
                .ToList();
            employeeComboBox.ItemsSource = _employees;
            employeeComboBox.DisplayMemberPath = "FullName";
            employeeComboBox.SelectedValuePath = "EmployeeId";
        }

        private void LoadLeaveRequests()
        {
            FilterLeaveRequests();
        }

        private void FilterLeaveRequests()
        {
            int? selectedEmployeeId = employeeComboBox.SelectedValue as int?;
            int? selectedYear = yearComboBox.SelectedItem as int?;

            var leaveRequests = _leaveRequestService.GetLeaveRequestByYear(selectedYear ?? DateTime.Now.Year);
            if (selectedEmployeeId.HasValue)
                leaveRequests = leaveRequests.Where(lr => lr.EmployeeId == selectedEmployeeId).ToList();

            var groupedByEmployee = leaveRequests.GroupBy(lr => new { lr.EmployeeId, lr.Employee.FullName })
                .Select(group => new
                {
                    group.Key.EmployeeId,
                    group.Key.FullName,
                    OnLeaveLeft = 12 - group.Where(lr => lr.LeaveType == "Nghỉ phép")
                                            .Sum(lr => (lr.EndDate.ToDateTime(TimeOnly.MinValue) -
                                                        lr.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1),
                    SickLeaveLeft = 36 - group.Where(lr => lr.LeaveType == "Nghỉ bệnh")
                                              .Sum(lr => (lr.EndDate.ToDateTime(TimeOnly.MinValue) -
                                                          lr.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1),
                    LeaveWithoutPayLeft = 168 - group.Where(lr => lr.LeaveType == "Nghỉ không lương")
                                                     .Sum(lr => (lr.EndDate.ToDateTime(TimeOnly.MinValue) -
                                                                 lr.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1)
                }).ToList();

            reportDataGrid.ItemsSource = groupedByEmployee;
        }




        private void EmployeeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterLeaveRequests();
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterLeaveRequests();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LeaveRequestManageWindow record = new LeaveRequestManageWindow();
            record.Show();
            this.Close();
        }
    }
}
