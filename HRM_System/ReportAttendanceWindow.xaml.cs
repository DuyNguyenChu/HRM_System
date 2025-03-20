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
using Repositories;
using Services;

namespace HRM_System
{
    /// <summary>
    /// Interaction logic for ReportAttendanceWindow.xaml
    /// </summary>
    public partial class ReportAttendanceWindow : Window
{
    private readonly IAttendanceService _attendanceService;

    public ReportAttendanceWindow()
    {
        InitializeComponent();
        _attendanceService = new AttendanceService(new AttendanceRepository());
        InitializeYearComboBox();
        LoadAttendances();
    }

    private void InitializeYearComboBox()
    {
        for (int year = 2000; year <= 2025; year++)
        {
            yearComboBox.Items.Add(year);
        }
        yearComboBox.SelectedItem = DateTime.Now.Year;
        monthComboBox.SelectedIndex = DateTime.Now.Month - 1;
    }

    private void LoadAttendances()
    {
        if (monthComboBox.SelectedItem is ComboBoxItem selectedMonthItem &&
            yearComboBox.SelectedItem is int selectedYear)
        {
            int selectedMonth = int.Parse(selectedMonthItem.Content.ToString());
            var attendances = _attendanceService.GetAttendanceByMonthYear(selectedMonth, selectedYear);
            reportDataGrid.ItemsSource = attendances;
        }
    }

    private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadAttendances();
    }

    private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadAttendances();
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        AttendanceManageWindow record = new AttendanceManageWindow();
        record.Show();
        this.Close();
    }
}

}
