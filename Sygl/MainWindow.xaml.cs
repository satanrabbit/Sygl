﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ServiceModel;
using System.Windows.Threading;
using SyglService.Interface;
using System.Windows.Forms;
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
            InitialTray();
            TimeCounts=TIMECOUNTS = Properties.Settings.Default.StartCounts;
            StartTimer();
            this.Topmost = true;
        }

        /// <summary>
        /// 启动后开始连接服务器的时间（秒数）
        /// </summary>
        int TIMECOUNTS;
        /// <summary>
        /// 链接服务是否成功
        /// </summary>
        sbyte channelFlag = 0;
        /// <summary>
        /// 窗口查询时间间隔(秒数)
        /// </summary>
        public int SetInterval { get; set; }
        /// <summary>
        /// 距离填写倒计时秒数
        /// </summary>
        public int TimeCounts { get; set; }

        /// <summary>
        /// 服务协定
        /// </summary>
        public ChannelFactory<IJszxService> ChannelFactory{get;set;}
        /// <summary>
        /// 服务代理
        /// </summary>
        public IJszxService Proxy { get; set; }

        /// <summary>
        /// 提示窗体
        /// </summary>
        public TipWindow tipWindow { get; set; }
         
        ExpRecordWithFlag expRecordWithFlag;

        /// <summary>
        /// 课节时间列表
        /// </summary>
        List<ClassTime> clsTimeList;

        /// <summary>
        /// 当前选中的课节
        /// </summary>
        List<int> SelectedClass;
        /// <summary>
        /// 当前课是否填写
        /// </summary>
        bool isSign = true;

        private SearchRecordParam searchRecordParam;

        #region 客户端处理
        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Button_Click_1(object sender, RoutedEventArgs e)
        {
            //检查是否填写
            if (isSign)
            {
                this.Hide();
            }
            else
            {
                System.Windows.MessageBox.Show("请提交实验记录后关闭！");
            }
        }
        /// <summary>
        /// 窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DragMove();
            //}
        }

        #region 提交事件
        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
  
        private void SubmitSign_Click_1(object sender, RoutedEventArgs e)
        {
            //获取数据
            Exprecord expSubmit = new Exprecord();

            expSubmit.CourseName = this.CourseName.Text.Trim();
            if (expSubmit.CourseName == "")
            {
               System.Windows.MessageBox.Show("请填写课程名！");
               return;
            }
            expSubmit.ExpClasses = this.ExpClass.Text.Trim();
            if (expSubmit.ExpClasses == "")
            {
                System.Windows.MessageBox.Show("请填写实验班级！");
                return;
            }
            expSubmit.ExpDate = DateTime.Now.Date;
            
            expSubmit.ExpLab = Properties.Settings.Default.LabName;

            expSubmit.ExpLabID = Properties.Settings.Default.LabID;
            expSubmit.ExpName = this.ExpName.Text.Trim();
            if (expSubmit.ExpName == "")
            {
                System.Windows.MessageBox.Show("请填写实验名称！");
                return;
            }
            //实验学期在服务器端完成
            //expSubmit.ExpTerm=
            
            expSubmit.ExpWeek =Convert.ToSByte(this.ExpWeek.Text.Trim());

            expSubmit.ExpWeekDay = Convert.ToSByte(this.ExpWeekday.Text.Trim());
            if (this.Shoulder.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写学生应到人数！");
                return;
            }

            expSubmit.Shoulder = Convert.ToInt32(this.Shoulder.Text.Trim());
            if (this.Realize.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写学生实到人数！");
                return;
            }
            expSubmit.Realizer = Convert.ToInt32(this.Realize.Text.Trim());

            if (this.ExpGroups.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写实验组数！");
                return;
            }
            expSubmit.Groups =Convert.ToInt32( this.ExpGroups.Text.Trim());
            if (this.PerGroups.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写实验每组人数！");
                return;
            }
            expSubmit.PerGroup = Convert.ToInt32(this.PerGroups.Text.Trim());
            
            expSubmit.PostTime = DateTime.Now;
            if (this.StudentsStatus.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写学生出勤情况！");
                return;
            }
            expSubmit.StudentStatus = this.StudentsStatus.Text.Trim();
            if (this.InstrumentStatus.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写仪器使用情况！");
                return;
            }
            expSubmit.InstrumentStatus = this.InstrumentStatus.Text.Trim();
            if (this.Problems.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写实验出现问题！");
                return;
            }
            expSubmit.Problems = this.Problems.Text.Trim();
            if (this.TeacherName.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写教师姓名！");
                return;
            }
            expSubmit.TeacherName = this.TeacherName.Text.Trim();
            if (this.TeacherNum.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写教工号！");
                return;
            }
            expSubmit.TeacherNumber = this.TeacherNum.Text.Trim();
            if (this.StudentName.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("请填写学生组长姓名！");
                return;
            }
            expSubmit.StudentName = this.StudentName.Text.Trim();
            if (SelectedClass.Count <= 0)
            {
                System.Windows.MessageBox.Show("请选择实验课节！");
            }
            try
            {
                Proxy = ChannelFactory.CreateChannel();
                Proxy.SaveExpRecord(expSubmit, SelectedClass);
                //提示保存成功和反馈意见窗口 
                FeedbackWindows feedbackWindow = new FeedbackWindows(true);
                feedbackWindow.ShowDialog();
                isSign = true;
                TimeCounts = Proxy.GetPopTimeTallies();
               this.Hide();
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("出现错误："+ex.Message);
            }


        }
        #endregion

        #region 启动定时程序
        /// <summary>
        /// 启动定时程序
        /// </summary>
        private void StartTimer()
        {
           
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += delegate
            {
                if (TimeCounts < 0)
                {
                    TimeCounts = TIMECOUNTS;
                }

                switch (channelFlag)
                {
                    case 0:
                        if (TimeCounts == 0)
                        {
                            TryLink();
                        }
                        else
                        {
                            this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后连接服务器";
                        }
                        break;
                    case 1:
                        TralAction();
                        break;
                    case 2:
                        if (TimeCounts == 0)
                        {
                            TryLink();
                        }
                        else
                        {
                            this.ServiceStatus.Text = "网络错误，等待" + TimeCounts.ToString() + "秒后重试";
                        }
                        break;
                    default:
                        break;
                }

                TimeCounts--; 
            };
            timer.Start();
        }
        #endregion

        #region 尝试连接服务器
        private void TryLink()
        {
            try {
                CreatProxy();
                channelFlag = 1;
                TimeCounts = Proxy.GetPopTimeTallies();
                if (TimeCounts < 60)
                {
                    TimeCounts = 70;
                }
            }
            catch (Exception ex){
                channelFlag = 2; 
                TimeCounts = 70;
            }
        }
        #endregion

        #region 设置当面前时间点的填写情况
        private void TralAction()
        {
            if (TimeCounts > 60)
            {
                int _timeCount = TimeCounts - 60;
                //不需要提示和弹窗，并且未查询填写情况
                int h, m, s;
                h = 0;
                m = 0;
                s = _timeCount % 60;
                if (_timeCount > 60)
                {
                    m = _timeCount / 60;
                    if (m > 60)
                    {
                        h = m / 60;
                        m = m % 60;
                    }
                }
                string timeTipStr = "";
                if (h > 0)
                {
                    timeTipStr = h.ToString() + "小时"+ m.ToString() + "分钟" + s + "秒";
                }
                else
                {
                    if (m > 0)
                    {
                        timeTipStr = timeTipStr + m.ToString() + "分钟" + s + "秒";
                    }
                    else
                    {
                        timeTipStr = timeTipStr + s + "秒";
                    }
                }
                this.ServiceStatus.Text = "当前不需要填写，" + timeTipStr + "后查询服务器";
            }
            else
            {
                if (TimeCounts == 60)
                {
                    this.ServiceStatus.Text = "正在查询服务器信息，当前不可操作.....";
                    Proxy = ChannelFactory.CreateChannel();
                    expRecordWithFlag = Proxy.GetExpRecordWithFlag(Properties.Settings.Default.LabID);
                    //初始化填写窗口
                    switch (expRecordWithFlag.SignFlag)
                    {
                        case 0: //无课，不需填写
                            isSign = true;
                            this.ServiceStatus.Text = "当前无课，不需填写！";
                            break;
                        case 1: //已经填写
                            isSign = true;
                            this.ServiceStatus.Text = "已经填写本次实验记录！";
                            break;
                        case 2: //需要核对
                            isSign = false;
                            this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后核对记录！";
                            SetForm(expRecordWithFlag.ExpRecord);
                            break;
                        case 3: //新填写
                            isSign = false;
                            this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后填写记录！";
                            SetForm(expRecordWithFlag.ExpRecord);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!isSign)
                    {
                        //需要填写
                        if (TimeCounts > 30)
                        {
                            //初始化填写窗口
                            switch (expRecordWithFlag.SignFlag)
                            {
                                case 2: //需要核对 
                                    this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后核对记录！"; 
                                    break;
                                case 3: //新填写 
                                    this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后填写记录！"; 
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            //0-30秒内 
                            switch (expRecordWithFlag.SignFlag)
                            {
                                case 2: //需要核对 
                                    this.ServiceStatus.Text = "等待核对记录！";
                                    break;
                                case 3: //新填写 
                                    this.ServiceStatus.Text = "等待填写记录！";
                                    break;
                                default:
                                    break;
                            }

                            if (this.Visibility == Visibility.Hidden || this.WindowState == WindowState.Minimized)
                            {
                                if (tipWindow == null)
                                {
                                    tipWindow = new TipWindow(this);
                                }
                                tipWindow.timeTip.Text = TimeCounts.ToString();
                                if (TimeCounts > 0)
                                {
                                    //初始化填写窗口
                                    switch (expRecordWithFlag.SignFlag)
                                    {
                                        case 2: //需要核对 
                                                tipWindow.windowUpAnimation("当前记录信息不一致，距离核对记录还有：", "立刻核对");
                                            break;
                                        case 3: //新填写 
                                                tipWindow.windowUpAnimation("距离填写本次实验记录还有", "立刻填写");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    //TimeCounts == 0
                                    this.Visibility = Visibility.Visible;
                                    this.Show();
                                    tipWindow.Hide();
                                }
                            }

                            if (TimeCounts == 0)
                            {
                                TimeCounts++;
                            }

                        }
                    }
                }
            }
        }
        #endregion

        #region 最小化按钮事件
        private void MiniBtn_Click_1(object sender, RoutedEventArgs e)
        {
            //检查是否填写
            if (isSign)
            {
                this.Hide();
            }
            else
            {
                System.Windows.MessageBox.Show("请提交实验记录后关闭！");
            }
        }
        #endregion

        #region 关于按钮单击事件
        private void AboutBtn_Click_1(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
        #endregion

        #region 初始化窗口表单
        /// <summary>
        /// 初始化窗口表单
        /// </summary>
        /// <param name="exp">待初始化的实验记录</param>
        private void SetForm(Exprecord exp){
            //初始化文本框
            this.CourseName.Text = exp.CourseName;
            this.ExpName.Text = exp.ExpName;
            this.ExpClass.Text = exp.ExpClasses;
            this.Shoulder.Text = exp.Shoulder.ToString();
            this.Realize.Text = exp.Realizer.ToString();
            this.ExpGroups.Text = exp.Groups.ToString();
            this.PerGroups.Text = exp.PerGroup.ToString();
            this.StudentsStatus.Text = exp.StudentStatus;
            this.InstrumentStatus.Text = exp.InstrumentStatus;
            this.Problems.Text = exp.Problems;
            this.TeacherName.Text = exp.TeacherName;
            this.TeacherNum.Text = exp.TeacherNumber;
            this.StudentName.Text = exp.StudentName;
            
            SetCls();
        }
        #endregion
        #region 初始化课节
        private void SetCls( )
        {
            Proxy = ChannelFactory.CreateChannel();
            this.ExpDate.Text = (DateTime.Now).ToString("yyyy年MM月dd日");
            this.ExpWeek.Text = Proxy.GetCurrentWeeks().ToString();//exp.ExpWeek.ToString();
            this.ExpWeekday.Text = Proxy.GetCurrentWeekDay().ToString();//exp.ExpWeekDay.ToString();
            ClassTime expCls=  Proxy.GetCurrentClassTime();
            int nowCls=0;
            if (expCls != null)
            {
                nowCls = expCls.ClsTmIndex;
            }
            this.ClsTimePanel.Children.Clear();
            //初始化课节 
            SelectedClass = new List<int>();
            clsTimeList = Proxy.GetClassTimeList();
            foreach (ClassTime clt in clsTimeList)
            {
                System.Windows.Controls.CheckBox chb = new System.Windows.Controls.CheckBox();
                chb.Content = clt.ClsTmName;

                chb.Style = Resources["CheckBoxSty"] as Style;
                chb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                chb.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                chb.Checked += new RoutedEventHandler(ExpClass_Checked);
                chb.Unchecked += new RoutedEventHandler(ExpClass_UnChecked);

                //是否当前课节
                if (nowCls == clt.ClsTmIndex)
                {
                    chb.IsChecked = true;
                }               
                this.ClsTimePanel.Children.Add(chb);
            } 
        }
        #endregion
        #region 添加课节选择时间
        private void ExpClass_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox chb = (System.Windows.Controls.CheckBox)sender;
            //当前选择的课节的信息
            SelectedClass.Add(clsTimeList.Where(ct => ct.ClsTmName == chb.Content.ToString()).FirstOrDefault().ClsTmIndex);
           
        }
        #endregion

        #region 去除课节选择时间
        private void ExpClass_UnChecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox chb = (System.Windows.Controls.CheckBox)sender;
            //判断当前选择的课节的信息，删除当前课节
            SelectedClass.Remove(clsTimeList.Where(ct => ct.ClsTmName == chb.Content.ToString()).FirstOrDefault().ClsTmIndex);
        }
        #endregion

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
            SetCls();
        }

        /// <summary>
        /// 在子线程内更改ServiceStatus文字
        /// </summary>
        /// <param name="msg">待显示的信息</param>
        private void SetServiceStatusText(string msg)
        {
            this.ServiceStatus.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        this.ServiceStatus.Text=msg;
                    }
                ), null);
        }
        #endregion


        #region 托盘图标
        private NotifyIcon notifyIcon; 
        /// <summary>
        /// 初始化托盘图标
        /// </summary>
        private void InitialTray()
        {
            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "实验记录系统开始运行";
            notifyIcon.Text = "实验记录";
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath + @"\logo.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            System.Windows.Forms.MenuItem SetMenu = new System.Windows.Forms.MenuItem("设置");
            SetMenu.Click += new EventHandler(SetMenu_Click);

            
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("exit");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { SetMenu, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }

       /// <summary>
        /// 窗体状态改变时候触发
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 退出选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            IsLogin lg = new IsLogin("退出");
            LoginWindow lgw = new LoginWindow(lg);
            lgw.ShowDialog();
            if(lg.Login){
            System.Windows.Application.Current.Shutdown();
            }
        }

        private void SetMenu_Click(object sender,EventArgs e)
        {
            SetWindow setWindow = new SetWindow();
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }
        #endregion

        #region 确定反馈窗口
        private void FeedbackBtn_Click_1(object sender, RoutedEventArgs e)
        {
            FeedbackWindows feedbackWindow = new FeedbackWindows(false);
            feedbackWindow.ShowDialog();
        }
        #endregion

        private void SignBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.SignPanel.Visibility = Visibility.Visible;
            this.SearchPanel.Visibility = Visibility.Collapsed;
        }

        #region 查询
        private void SearchBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.SignPanel.Visibility = Visibility.Collapsed;
            this.SearchPanel.Visibility = Visibility.Visible;
            if (ChannelFactory == null)
            {
                try
                {
                    CreatProxy();
                    channelFlag = 1;
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show("服务器链接错误，请稍后再试：\n"+ex.Message);
                    return ;
                }
            }
            //初始化查询信息
            searchRecordParam = new SearchRecordParam();
            //实验室列表
            Proxy = ChannelFactory.CreateChannel();
            this.SearchLab.ItemsSource = Proxy.GetLabList();
            //学期列表 
            this.SearchTerm.ItemsSource = Proxy.GetTermList();
            Term ct = Proxy.GetCurrentTerm();
            this.SearchTerm.SelectedItem = ct;
            //获取周数列表
            SetSearchWeek(ct.TermID);
            //设置工作日列表
            SetSearchWeekday();
            //设置课节列表
            SetSearchCls();
        }
        /// <summary>
        /// 设置指定的学期的周选择列表
        /// </summary>
        /// <param name="term"></param>
        private void SetSearchWeek(int term)
        {
            Proxy = ChannelFactory.CreateChannel();
            Term tm = Proxy.GetPurposeTerm(term);
            this.SearchWeek.Items.Clear();
            this.SearchWeek.Items.Add("");
            for (int i = 1; i <= tm.TermWeeks; i++)
            {
               
                this.SearchWeek.Items.Add(i);
            }

        }
        /// <summary>
        /// 设置查询工作日列表
        /// </summary>
        private void SetSearchWeekday()
        {
            this.SearchWeekday.Items.Clear();
            this.SearchWeekday.Items.Add("");
            for (int i = 1; i < 8; i++)
            {
                this.SearchWeekday.Items.Add(i);
            }
        }
        private void SetSearchCls()
        {
            Proxy = ChannelFactory.CreateChannel();
            List<ClassTime>  clsTimes = Proxy.GetClassTimeList();
            clsTimes.Insert(0, new ClassTime());
            this.SearchCls.ItemsSource = clsTimes;
        }
        /// <summary>
        /// 查询的页数
        /// </summary>
        int page = 0;
        int pageSize = 20;
        /// <summary>
        /// 提交查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchSubmitByn_Click_1(object sender, RoutedEventArgs e)
        {

            if (ChannelFactory == null)
            {
                CreatProxy();
            }
            //获取查询页数
            page = 0;
            SearchRecordParam pm = new SearchRecordParam();
            if (this.SearchTerm.SelectedValue==null)
            {
                pm.term = null;
            }
            else
            {
                pm.term = Convert.ToInt32(this.SearchTerm.SelectedValue);
            }
            if (this.SearchLab.SelectedValue == null)
            {
                pm.lab = null;
            }
            else
            {
                pm.lab = Convert.ToInt32(this.SearchLab.SelectedValue);
            }
            if (this.SearchWeek.Text == null || this.SearchWeek.Text == "")
            {
                pm.week = null;
            }
            else
            {
                pm.week = Convert.ToInt32(this.SearchWeek.Text);
            }
            if (this.SearchWeekday.Text == null || this.SearchWeekday.Text == "")
            {
                pm.weekday = null;
            }
            else
            {
                pm.weekday = Convert.ToInt32(this.SearchWeekday.Text);
            }
            if (Convert.ToInt32(this.SearchCls.SelectedValue) == 0 || (this.SearchCls.SelectedValue) == ""||(this.SearchCls.SelectedValue) == null)
            {
                pm.cls = null;
            }
            else
            {
                pm.cls = Convert.ToInt32(this.SearchCls.SelectedValue);
            }
            pm.page = page;
            pm.pageSize = pageSize;

            Proxy = ChannelFactory.CreateChannel();
            PageRecord pgRecord= Proxy.GetPageRecords(pm);
            this.SearchResultDG.ItemsSource = pgRecord.ExpRecordList;
        }
        #endregion
        private void DetailBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SetBtn_Click_1(object sender, RoutedEventArgs e)
        {
            SetWindow setWindow = new SetWindow();
        }

        private void MainSetBtn_Click_1(object sender, RoutedEventArgs e)
        {
            SetWindow setWindow = new SetWindow();
        }

        private void MainAboutBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }

    }

}
