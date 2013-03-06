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
using SyglService;
using System.ServiceModel;
using System.Windows.Threading;

namespace Sygl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartTimer();
            TimeCounts = 70;
        }
        /// <summary>
        /// 窗口查询时间间隔(秒数)
        /// </summary>
        public int SetInterval { get; set; }
        /// <summary>
        /// 距离填写倒计时秒数
        /// </summary>
        public int TimeCounts { get; set; }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Button_Click_1(object sender, RoutedEventArgs e)
        {
            //检查是否填写
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

       //System.Diagnostics.Process.Start("http://www.baidu.com/");
        //IJszxService proxy = channelFactory.CreateChannel();
            //MessageBox.Show(proxy.GetData(5));

       // ChannelFactory<IJszxService> channelFactory = new ChannelFactory<IJszxService>("JszxService");

        //提示窗体
        public TipWindow tipWindow { get; set; }

        private void SubmitSign_Click_1(object sender, RoutedEventArgs e)
        {
            if (tipWindow == null)
            {
                tipWindow = new TipWindow(this);
            }
           
            tipWindow.Show();
            this.Hide();
        }


        /// <summary>
        /// 启动定时程序
        /// </summary>
        private void StartTimer()
        {
            if (tipWindow == null)
            {
                tipWindow = new TipWindow(this);
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += delegate
            {
                if (!this.IsVisible)
                {
                    if (TimeCounts == 0)
                    {
                        TimeCounts = 70;
                        this.Show();
                        tipWindow.Hide();

                    }
                    else
                    {
                        if (TimeCounts <= 60)
                        {
                            tipWindow.windowUpAnimation();
                            tipWindow.timeTip.Text = TimeCounts.ToString();
                        }
                    }

                    TimeCounts--;
                }
                else
                {
                    
                }
            };
            timer.Start();
        }

        private void MiniBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void AboutBtn_Click_1(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}
