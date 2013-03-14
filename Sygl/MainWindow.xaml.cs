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
using SyglService.Interface;
using JszxDataModel;

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
            CreatProxy();
        }
        /// <summary>
        /// 窗口查询时间间隔(秒数)
        /// </summary>
        public int SetInterval { get; set; }
        /// <summary>
        /// 距离填写倒计时秒数
        /// </summary>
        public int TimeCounts { get; set; }

        //服务协定
        public ChannelFactory<IJszxService> ChannelFactory{get;set;}
        //服务代理
        public IJszxService Proxy { get; set; } 

        #region 客户端处理
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

       
        //提示窗体
        public TipWindow tipWindow { get; set; }

        private void SubmitSign_Click_1(object sender, RoutedEventArgs e)
        { 
            //this.Hide();
            terms_tb tm = Proxy.GetCurrentTerm();
            if (tm == null)
            {
                MessageBox.Show("未设置当前学期！");
            }
            else
            {
                MessageBox.Show(tm.TermStartDay.ToLongTimeString());
            } 
        }


        /// <summary>
        /// 启动定时程序
        /// </summary>
        private void StartTimer()
        {
           

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
                        if (tipWindow == null)
                        {
                            tipWindow = new TipWindow(this);
                        }
                        tipWindow.Hide();

                    }
                    else
                    {
                        if (TimeCounts <= 60)
                        {
                            if (tipWindow == null)
                            {
                                tipWindow = new TipWindow(this);
                            }
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
       

        #region 数据验证
        private void CourseName_GotFocus_1(object sender, RoutedEventArgs e)
        {

        }

        private void CourseName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (CourseName.Text.Trim() == "")
            {
                CourseNameV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                CourseNameV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ExpName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (CourseName.Text.Trim() == "")
            {
                ExpNameV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ExpNameV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ExpClass_LostFocus_1(object sender, RoutedEventArgs e)
        {

            if (ExpClass.Text.Trim() == "")
            {
                ExpClassV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ExpClassV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Shoulder_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (Shoulder.Text.Trim() == "")
            {
                ShoulderV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ShoulderV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Realize_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (Realize.Text.Trim() == "")
            {
                RealizeV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                RealizeV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ExpGrops_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (ExpGroups.Text.Trim() == "")
            {
                ExpGroupsV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ExpGroupsV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void PerGroup_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (PerGroups.Text.Trim() == "")
            {
                PerGroupsV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                PerGroupsV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void StudentsStatus_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (StudentsStatus.Text.Trim() == "")
            {
                StudentsStatusV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                StudentsStatusV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void InstrumentStatus_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (InstrumentStatus.Text.Trim() == "")
            {
                InstrumentStatusV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                InstrumentStatusV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Problems_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (Problems.Text.Trim() == "")
            {
                ProblemsV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ProblemsV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void TeacherName_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (TeacherName.Text.Trim() == "")
            {
                TeacherNameV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                TeacherNameV.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        private void TeacherNum_LostFocus_1(object sender, RoutedEventArgs e)
        {

            if (TeacherNum.Text.Trim() == "")
            {
                TeacherNumV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                TeacherNumV.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void StudentName_LostFocus_1(object sender, RoutedEventArgs e)
        {

            if (StudentName.Text.Trim() == "")
            {
                StudentNameV.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                StudentNameV.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        #endregion
        #endregion

        #region 服务器交互
        //System.Diagnostics.Process.Start("http://www.baidu.com/");
        /// <summary>
        /// 初始化代理
        /// </summary>
        public void CreatProxy()
        {
            ChannelFactory = new ChannelFactory<IJszxService>("JszxService");
            Proxy = ChannelFactory.CreateChannel();
        }



        #endregion
    }
}
