using System;
using System.Windows;
using Services;

namespace HRM_System
{
    public partial class Login : Window
    {
        private readonly IUserServices _userServices;

        public Login(IUserServices userServices)
        {
            InitializeComponent();
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
        }

        public Login() : this(new UserServices()) { }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = txtUser.Text.Trim();
            string password = txtPass.Password.Trim();

            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại và mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _userServices.GetUserAcc(phoneNumber, password);
            if (user != null)
            {
                Application.Current.Properties["CurrentUser"] = user;

                if (user.Role == "Admin")
                {
                    MessageBox.Show($"Đăng nhập thành công! Chào Admin {user.PhoneNumber}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow main = new MainWindow();
                    main.Show();
                }
                else if (user.Role == "Employee")
                {
                    MessageBox.Show($"Đăng nhập thành công! Chào {user.PhoneNumber} (Quyền Reader)", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    EmployeeControl main = new EmployeeControl();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền truy cập.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Số điện thoại hoặc mật khẩu không chính xác.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
