using System;
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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
                Proxy.SaveExpRecord(expSubmit, SelectedClass);
                //提示保存成功和反馈意见窗口 
                FeedbackWindows feedbackWindow = new FeedbackWindows(true);
                feedbackWindow.ShowDialog();

                isSign = true;
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
                if (channelFlag!=1)
                {
                    #region 连接服务
                    if (channelFlag == 0)
                    {
                        this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒连接服务器！";
                    }
                    else
                    {
                        if (channelFlag == 2)
                        {
                            this.ServiceStatus.Text = "服务器连接错误," + TimeCounts.ToString() + "秒后重试！";
                        }
                    }
                    if (TimeCounts == 0)
                    {
                        try
                        {
                            CreatProxy();
                            channelFlag = 1;
                            this.ServiceStatus.Text = "已经连接服务器！";
                            TimeCounts = Proxy.GetPopTimeTallies();
                            if (TimeCounts < 60)
                            {
                                TimeCounts = 70;
                            }
                        }
                        catch (Exception ex)
                        {
                            channelFlag = 2;
                            TimeCounts = 60;
                        }
                    }
                    #endregion

                }
                else
                {
                    if (TimeCounts == 0)
                    {
                        #region 弹出填写
                        //获取下一次弹出时间
                        TimeCounts = Proxy.GetPopTimeTallies();
                        //显示窗口
                        this.Show();
                        if (tipWindow == null)
                        {
                            tipWindow = new TipWindow(this);
                        }
                        tipWindow.Hide();
                        #endregion
                    }
                    else
                    {
                        if (TimeCounts == 60)
                        {
                            #region 查询填写状态
                            expRecordWithFlag = Proxy.GetExpRecordWithFlag(1);
                            //初始化填写窗口
                            switch (expRecordWithFlag.SignFlag)
                            {
                                case 0: //无课，不需填写
                                    isSign = true;
                                    this.ServiceStatus.Text = "当前无课，不需填写！";
                                    break;
                                case 1: //已经填写
                                    isSign = true;
                                    this.ServiceStatus.Text = "已经填写记录！";
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
                            #endregion
                        }
                        else
                        {
                            if (TimeCounts <= 30)
                            {
                                #region 提示填写
                                
                                switch (expRecordWithFlag.SignFlag)
                                {
                                    case 0: //无课，不需填写
                                        this.ServiceStatus.Text = "当前无课，不需填写！等待" + (TimeCounts - 60).ToString() + "秒后查询服务！";
                                        break;
                                    case 1: //已经填写
                                        this.ServiceStatus.Text = "已经填写记录！等待" + (TimeCounts - 60).ToString() + "秒后查询服务！";
                                        break;
                                    case 2: //需要核对
                                        this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后核对记录！";
                                        if (this.Visibility == Visibility.Visible || this.WindowState == WindowState.Minimized)
                                        {
                                            if (tipWindow == null)
                                            {
                                                tipWindow = new TipWindow(this);
                                            }
                                            tipWindow.windowUpAnimation("当前记录信息不一致，距离核对记录还有：", "立刻核对");
                                            tipWindow.timeTip.Text = TimeCounts.ToString();
                                        }
                                        break;
                                    case 3: //新填写
                                        this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后填写记录！";
                                        if (this.Visibility == Visibility.Visible || this.WindowState == WindowState.Minimized)
                                        {
                                            if (tipWindow == null)
                                            {
                                                tipWindow = new TipWindow(this);
                                            }
                                            tipWindow.windowUpAnimation("距离填写本次实验记录还有", "立刻填写");
                                            tipWindow.timeTip.Text = TimeCounts.ToString();
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                #endregion
                            }
                            else
                            {
                                if (30 < TimeCounts && TimeCounts < 60)
                                {
                                    if (!isSign)
                                    {
                                        this.ServiceStatus.Text = "等待" + TimeCounts.ToString() + "秒后填写记录！";
                                    }
                                }
                                else
                                {
                                    this.ServiceStatus.Text = "当前不需填写，等待" + (TimeCounts - 60).ToString() + "秒后查询服务！";
                                }
                            }
                        }
                    }
                }
                TimeCounts--;
            };
            timer.Start();
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

            //初始化textblock
            this.ExpDate.Text = ((DateTime)exp.ExpDate).ToString("yyyy年MM月dd日");
            this.ExpWeek.Text = exp.ExpWeek.ToString();
            this.ExpWeekday.Text = exp.ExpWeekDay.ToString();
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
                if (exp.ExpCls == clt.ClsTmIndex)
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

        private void FeedbackBtn_Click_1(object sender, RoutedEventArgs e)
        {
            FeedbackWindows feedbackWindow = new FeedbackWindows(false);
            feedbackWindow.ShowDialog();
        }

        private void SignBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.SignPanel.Visibility = Visibility.Visible;
            this.SearchPanel.Visibility = Visibility.Collapsed;
        }

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
            this.SearchLab.ItemsSource = Proxy.GetLabList();
            //学期列表
            this.SearchTerm.ItemsSource = Proxy.GetTermList();
            
        }

        private void SearchSubmitByn_Click_1(object sender, RoutedEventArgs e)
        {
            //获取查询学期 
            //获取查询实验室
            //获取查询周次
            //获取查询工作日
            //获取查询节次
            //获取查询页数
            //获取查询显示页数
            //Proxy.GetPageRecords();
        }

        private void DetailBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void SetBtn_Click_1(object sender, RoutedEventArgs e)
        {
            SetWindow setWindow = new SetWindow();
        }

    }

}
