using System;
using System.Windows;
using System.ServiceModel;
using SyglService;
using System.Threading;
using Microsoft.Win32; 

namespace SyglHost
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Thread StartServiceThread = new Thread(new ThreadStart(delegate
            {
                StartService();
            }));
            //设置为后台线程
            StartServiceThread.IsBackground = true;
            StartServiceThread.Start();

            if (IsRegKeIsExt())
            {
                this.autoStartButton.Content = "取消开机启动";
            }

        }
        ServiceHost host = new ServiceHost(typeof(JszxService));
        /// <summary>
        /// 启动服务
        /// </summary>
        private void StartService()
        {
                SetControlServiceBtnText("服务启动中。。。");
                SetControlServiceBtnAbility(false);
                
                host.Opened += delegate
                {
                    SetControlServiceBtnText("服务已经启动！"); 
                }; 
                if (host.State == CommunicationState.Created)
                {
                    host.Open();
                }
        } 
        private void ControlServiceBtn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread StartServiceThread = new Thread(new ThreadStart(delegate
                {
                    StartService();
                }));
                //设置为后台线程
                StartServiceThread.IsBackground = true;
                StartServiceThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetTermBtn_Click_1(object sender, RoutedEventArgs e)
        {
            EditTerm editTerm = new EditTerm();
            editTerm.ShowDialog();
        }
        /// <summary>
        /// 在子线程内更改btn文字
        /// </summary>
        /// <param name="msg"></param>
        private void SetControlServiceBtnText(string msg)
        { 
            ControlServiceBtn.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        this.ControlServiceBtn.Content = msg;
                    }
                ), null);
            
        }
        /// <summary>
        /// 在子线程内更改btn文字
        /// </summary>
        /// <param name="msg"></param>
        private void SetControlServiceBtnAbility(bool b)
        {
            ControlServiceBtn.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        this.ControlServiceBtn.IsEnabled = b;
                    }
                ), null);

        }
         
        private void SetPopTimeBtn_Click_1(object sender, RoutedEventArgs e)
        {
            SetPopTime setPopTime = new SetPopTime();
            setPopTime.ShowDialog();
        }

        private void SqlExcel_Click_1(object sender, RoutedEventArgs e)
        {
            ExcelWindow excelWindow = new ExcelWindow();
            excelWindow.ShowDialog();
        }

        #region 设置开机启动
        /// <summary>
        /// 判断开机启动项是否已存在 
        /// </summary>
        /// <returns></returns>
        private bool IsRegKeIsExt()
        {
            bool IsExt = false;
            try
            {
                RegistryKey R_local = Registry.LocalMachine;
                //查找到要添加到的注册表项
                RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                //获得注册表项的值
                object _obj = R_run.GetValue("StartSign");
                R_run.Close();
                R_local.Close();

                if (_obj != null)
                {
                    IsExt = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("运行错误：\n" + ex.Message + "\n\n 请以管理员身份运行本程序后设置开机启动项！");
            }
            return IsExt;
        }

        private void autoStartButton_Click(object sender, EventArgs e)
        {
            string R_startPath = System.AppDomain.CurrentDomain.BaseDirectory + "SyglHost.exe";
            try
            {
                if (!IsRegKeIsExt())
                {
                    #region 添加开机启动
                    RegistryKey R_local = Registry.LocalMachine;

                    //查找到要添加到的注册表项
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    //添加注册表项
                    R_run.SetValue("StartSign", R_startPath);

                    R_run.Close();
                    R_local.Close();
                    #endregion
                    this.autoStartButton.Content = "取消开机启动";
                }
                else
                {
                    RegistryKey R_local = Registry.LocalMachine; RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    //删除相应的注册表项
                    R_run.DeleteValue("StartSign", false);
                    R_run.Close();
                    R_local.Close();
                    this.autoStartButton.Content = "开机启动";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
            }
        }
        #endregion
       
    }
}
