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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using SyglService;
using SyglService.Interface;
using JszxDataModel;
using System.Threading;
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
            //StartService();
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
       
    }
}
