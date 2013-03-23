using SyglService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
namespace Sygl
{
    /// <summary>
    /// SetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetWindow : Window
    {
        public SetWindow()
        {
            InitializeComponent();
            this.IsVisibleChanged += delegate {
                if (this.IsVisible)
                {
                    CreatProxy();
                    InitializeLabCombobox();
                    des = new Des();
                    InitPwd();
                    SetStartCounts();
                }
            };
            OpenLogin();
        }
        
        #region 设置登录
        private void OpenLogin()
        {
            IsLogin lg = new IsLogin("登录");
            LoginWindow lw = new LoginWindow(lg);
            lw.ShowDialog();
            if (lg.Login)
            {
                this.Show();
            }
            else
            {
                this.Close();
            }
        }
        #endregion

        //与服务建立连接
        /// <summary>
        /// 服务协定
        /// </summary>
        public ChannelFactory<IJszxService> ChannelFactory { get; set; }
        /// <summary>
        /// 服务代理
        /// </summary>
        public IJszxService Proxy { get; set; }

        const string PwdKey="SatanRabbit";

        /// <summary>
        /// 初始化代理
        /// </summary>
        public void CreatProxy()
        {
            try
            {
                ChannelFactory = new ChannelFactory<IJszxService>("JszxService");
                Proxy = ChannelFactory.CreateChannel();
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 实验室列表
        /// </summary>
        private List<Lab> labList;
        private Des des;
        private int labID;
        private string labName;
        /// <summary>
        /// 初始化LabCombobox
        /// </summary>
        private void InitializeLabCombobox()
        {
            labID = Properties.Settings.Default.LabID;
            labName = Properties.Settings.Default.LabName;
            labList = Proxy.GetLabList();
            this.LabComboBox.ItemsSource = labList;
            //设置选中项
            this.LabComboBox.SelectedValue = labID;
            //在初始化完成后再添加事件
            this.LabComboBox.SelectionChanged += new SelectionChangedEventHandler(LabComboBox_SelectionChanged_1);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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

        /// <summary>
        /// 选择实验室后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            labID = (int)LabComboBox.SelectedValue;
            labName = labList.Where(lbl => lbl.LabID == labID).FirstOrDefault().LabName;
        }
        /// <summary>
        /// 保存按钮出发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSetBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LabID = labID;
            Properties.Settings.Default.LabName = labName;
            Properties.Settings.Default.ClintPWD = des.EncryptDES(this.ClintPwd.Password.Trim(), PwdKey);
            Properties.Settings.Default.StartCounts = Convert.ToInt32(this.StartCounts.Text.Trim());
            Properties.Settings.Default.Save();
            MessageBox.Show("保存成功！");
        }

        #region 密码

        /// <summary>
        /// 初始化密码框
        /// </summary>
        private void InitPwd()
        {
            this.ClintPwd.Password = des.DecryptDES(Properties.Settings.Default.ClintPWD, PwdKey);
        }
         
        private void ViewSetPwd_MouseLeave_1(object sender, MouseEventArgs e)
        {
            this.PWDShow.Text = "密码";
        }

        private void ViewSetPwd_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.PWDShow.Text = this.ClintPwd.Password;
        }

        private void ViewSetPwd_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            this.PWDShow.Text = "密码";
        }
        #endregion

        #region 开机连接延时
        /// <summary>
        /// 开机连接延时
        /// </summary>
        private void SetStartCounts()
        {
            this.StartCounts.Text = Properties.Settings.Default.StartCounts.ToString();
        }
        #endregion
    }
}
