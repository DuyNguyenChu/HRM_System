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
    /// Interaction logic for SalaryManageWindow.xaml
    /// </summary>
    public partial class SalaryManageWindow : Window
    {
        private readonly ISalaryService _salaryService;

        private void LoadSalaries()
        {
            var salaries = _salaryService.GetAllSalaries();
            salaryDataGrid.ItemsSource = salaries;
        }

        public SalaryManageWindow()
        {
            InitializeComponent();
            _salaryService = new SalaryService(new SalaryRepository());
            LoadSalaries();
        }
    }
}
