using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sygl
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(IsLogin l)
        {
            InitializeComponent();
            lg = l;
            this.LoginTitle.Content = lg.Title;
        }

        IsLogin lg;
        private bool isLogin = false;
        /// <summary>
        /// 窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            lg.Login = isLogin;
        }

        private void LoginBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Des des = new Des();
            string pwd = des.EncryptDES(this.LoginPwd.Password.Trim(),"SatanRabbit");
            if (pwd == Properties.Settings.Default.ClintPWD)
            {
                isLogin = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("密码错误！");
            }
        }
    }

}
