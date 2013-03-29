using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JszxDataModel;
using System.Transactions;

namespace SyglHost
{
    /// <summary>
    /// ExcelWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelWindow : Window
    {
        public ExcelWindow()
        {
            InitializeComponent();
        }
         
        private void SelectExcelFile_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "选择课表文件";
            openFile.Filter = "Excel文件(*.xls;*xlsx)|*.xls;*.xlsx";
            if (openFile.ShowDialog().GetValueOrDefault())
            {
                string filePath = openFile.FileName;
                this.ExcleFile.Text = filePath;
                string connStr = SqlExcel.GetConnectionString(filePath.Trim());
                DataTable dt = SqlExcel.GetExcelTableName(filePath);
                this.ExcelTable.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][2].ToString().EndsWith("Print_Area"))
                    {
                    }
                    else
                    {
                        this.ExcelTable.Items.Add(dt.Rows[i]["Table_Name"].ToString());
                    }
                }

                this.TipTextBlock.Text = connStr;
            }
        }

        #region 将DataTable中指定行的数据补充完整，其空值与上一行相同;
        /// <summary>
        /// 将DataTable中指定行的数据补充完整，其空值与上一行相同;
        /// </summary>
        /// <param name="dt">数据表</param> 
        /// <param name="clm">待格式化的行</param>
        /// <returns></returns>
        private DataRow FormatDataRow(DataTable dt, int clm)
        {
            if (clm == 0)
            {
            }
            else
            {
                for (int i = 0; i < dt.Rows[clm].ItemArray.Length; i++)
                {
                    if (dt.Rows[clm][i].ToString() == "")
                    {
                        dt.Rows[clm][i] = FormatDataRow(dt, clm - 1)[i];
                    }

                }
            } 
            return dt.Rows[clm];
        }
        #endregion

        #region 处理字符串
        /// <summary>
        /// 处理字符串，获取字符串表示的数字集合
        /// </summary>
        /// <param name="weekStr">待处理字符串</param>
        /// <param name="isd">是否取奇数，1为奇数，2为偶数，0为全部</param>
        /// <returns>所得结果</returns>
        private List<int> GetListOfStr(string weekStr, int isd)
        {
            //'、'分割
            Regex rgx = new Regex(@"[^\d、-]");
            weekStr = rgx.Replace(weekStr, "");
            string[] weeksArray = weekStr.Split('、');
            List<int> weeks = new List<int>();
            foreach (string wa in weeksArray)
            {
                //判断是否存在“-”
                if (wa != "" && wa != null)
                {
                    if (wa.Contains("-"))
                    {
                        //存在，获取区间
                        string[] waa = wa.Split('-');
                        int wU = Convert.ToInt32(waa[1]);
                        int wL = Convert.ToInt32(waa[0]);
                        for (int iw = wL; iw <= wU; iw++)
                        {
                            //单双周区分
                            switch (isd)
                            {
                                case 0:
                                    weeks.Add(iw);
                                    break;
                                case 1:
                                    if (Convert.ToBoolean(iw & 1))
                                    {
                                        weeks.Add(iw);
                                    }
                                    break;
                                case 2:
                                    if (!Convert.ToBoolean(iw & 1))
                                    {
                                        weeks.Add(iw);
                                    }
                                    break;
                                default:
                                    weeks.Add(iw);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //不存在，不可再分
                        weeks.Add(Convert.ToInt32(wa));
                    }
                }

            }
            return weeks;
        } 
        #endregion

        private void StartInportBtn_Click_1(object sender, RoutedEventArgs e)
        {
            string table = this.ExcelTable.Text.Trim();
            string sql = string.Format("select * from [{0}]", table);
            SwitchTip.Text = "查询依赖信息";
            
            //try
            //{
                DataTable dt = SqlExcel.ExcuteSelect(this.ExcleFile.Text.Trim(), sql);
                int rowsCount = dt.Rows.Count;
                SwitchTip.Text = "共"+rowsCount+"条记录";
                SwitchProgressBar.Value = 10;
                JszxDataManager jszxDataManager = new JszxDataManager();
                //获取当前学期id
                SwitchTip.Text = "获取当前学期";
                SwitchProgressBar.Value = 40;
                int termID = jszxDataManager.GetCurrentTerm().TermID;
                //获取实验室列表
                SwitchTip.Text = "获取实验室列表";
                SwitchProgressBar.Value = 70;
                List<labs_tb> labs = jszxDataManager.GetLabs_tbList();
                #region 开始导入 
                SwitchProgressBar.Value = 100;
                SwitchTip.Text = "获取实验室列表";
                SwitchProgressBar.Maximum = rowsCount;
                SwitchProgressBar.Value = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    for (int i = 0; i < rowsCount; i++)
                    {
                        SwitchTip.Text = "导入第" + (i + 1).ToString() + "条记录";
                        #region 保存课程信息
                        courses_tb course = new courses_tb();
                        //格式化第i行
                        DataRow dr = FormatDataRow(dt, i);
                        course.CrsName = dr["课程名称"].ToString();
                        //crsTeacher 教师姓名
                        course.CrsTeacher = dr["任课教师"].ToString();
                        //crsClass 上课班级
                        course.CrsClasses = dr["班级"].ToString();
                        //crsHour上机学时                   
                        course.CrsHour = Convert.ToInt32(dr["上机学时"]);
                        //crsTimes 上机次数
                        course.CrsTimes = Convert.ToInt32(dr["上机次数"]);
                        //crsNum 上机人数
                        course.CrsNum = Convert.ToInt32(dr["人数"]);
                        //crsConf  实验环境配置要求
                        course.CrsConf = dr["对软硬件环境的要求"].ToString();
                        //备注
                        course.CrsRemark = dr["备注"].ToString();

                        jszxDataManager.jszxEntity.courses_tb.Add(course);
                        jszxDataManager.jszxEntity.SaveChanges();
                        int courseID = course.CrsID;
                        #endregion
                        #region 保存课程安排
                        #region 处理实验室
                        //获取该条记录对应的实验室，根据每条记录的上机地点是否包含该实验室关键字确定是否包含实验室
                        string lab = dr["上机地点"].ToString();
                        List<int> courseLabs = new List<int>();
                        foreach (var drlab in labs)
                        {
                            if (lab.Contains(drlab.LabKeyWord))
                            {
                                courseLabs.Add(drlab.LabID);
                            }
                        }

                        if (courseLabs.Count == 0)
                        {
                            //此条记录没有对应的实验室，继续循环下一条
                            continue;
                        }

                        #endregion
                        #region 处理周次
                        string weekStr = dr["周"].ToString();

                        int isDouble = 0;
                        if (weekStr.Contains("双"))
                        {
                            isDouble = 2;
                        }
                        else if (weekStr.Contains("单"))
                        {
                            isDouble = 1;
                        }
                        List<int> weeks = GetListOfStr(weekStr, isDouble);
                        #endregion

                        #region 处理工作日
                        string weekdayStr = dr["星期"].ToString();
                        List<int> weekdays = GetListOfStr(weekdayStr, 0);
                        #endregion

                        #region 处理节次
                        string classStr = dr["节"].ToString();
                        List<int> classes = GetListOfStr(classStr, 0);
                        #endregion

                        #region 循环导入

                        //schtb.
                        foreach (int wk in weeks)
                        {
                            //周

                            foreach (int wkd in weekdays)
                            {
                                //工作日
                                foreach (int lb in courseLabs)
                                {
                                    //实验室
                                    foreach (int cls in classes)
                                    {
                                        schedule_tb schtb = new schedule_tb();
                                        schtb.ScdCrs = courseID;
                                        schtb.ScdTerm = termID;
                                        schtb.ScdWeek = (sbyte)wk;
                                        schtb.ScdWeekDay = (sbyte)wkd;
                                        schtb.ScdLab = lb;
                                        //节次
                                        schtb.ScdClass = cls;
                                        //保存至数据库
                                        jszxDataManager.jszxEntity.schedule_tb.Add(schtb);
                                        jszxDataManager.jszxEntity.SaveChanges();
                                    }
                                }
                            }
                        }
                        #endregion

                        #endregion
                        SwitchProgressBar.Value = i + 1;
                    }
                    scope.Complete();
                }
                #endregion
                SwitchTip.Text = "导入完成,正在保存数据";
                SwitchProgressBar.IsIndeterminate = true;
                
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("出现错误：\n"+ex.Message);
            //}

        }


        //#region 转换按钮触发事件
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    string table = this.comboBox1.Text.Trim();
        //    //log
        //    string logMsg = table + "\t" + srCom.GetCurSourceFileName() + "----" + srCom.GetLineNum();

        //    string sql = string.Format("select * from [{0}]", table);

        //    //进度提示
        //    label4.Visible = true;
        //    label5.Visible = true;
        //    label5.Text = "查询依赖信息";
        //    progressBar.Visible = true;
        //    progressBar.Maximum = 100;
        //    progressBar.Value = 0;

        //    try
        //    {
        //        #region 查询
        //        DataTable dt = OledbExcel.ExcuteSelect(this.textBox1.Text.Trim(), sql);
        //        int rowsCount = dt.Rows.Count;

        //        //进度提示
        //        progressBar.Value = 10;

        //        MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.connStr);
        //        MySqlCommand cmd = conn.CreateCommand();
        //        conn.Open();
        //        MySqlTransaction trans = conn.BeginTransaction();
        //        cmd.Transaction = trans;

        //        //查询实验室ID和keyword
        //        cmd.CommandText = @"select labID,labKeyWord from labtb ";

        //        MySqlDataAdapter da = new MySqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        DataSet ds = new DataSet();

        //        da.Fill(ds, "labs_tb");

        //        //进度提示
        //        progressBar.Value = 40;

        //        //当前学期的ID
        //        cmd.CommandText = "select * from term_tb where termUse = 1";
        //        cmd.Parameters.Clear();
        //        da.Fill(ds, "term_tb");

        //        //进度提示
        //        progressBar.Value = 80;

        //        int termID = Convert.ToInt32(ds.Tables["term_tb"].Rows[0]["termID"].ToString());

        //        //进度提示
        //        progressBar.Value = 100;
        //        progressBar.Maximum = rowsCount;
        //        progressBar.Value = 0;
        //        label5.Text = "开始导入";

        //        #endregion

        //        #region 导入
        //        for (int i = 0; i < rowsCount; i++)
        //        {
        //            progressBar.Value = i;
        //            label5.Text = "导入第" + (i + 1) + "条，共" + rowsCount + "条记录";
        //            //格式化第i行
        //            DataRow dr = FormatDataRow(dt, i);
        //            //保存每行的数据
        //            #region 保存课程
        //            //crsName  课程名
        //            string crsName = "";
        //            crsName = dr["课程名称"].ToString();
        //            //crsTeacher 教师姓名
        //            string crsTeacher = "";
        //            crsTeacher = dr["任课教师"].ToString();
        //            //crsClass 上课班级
        //            string crsClass = "";
        //            crsClass = dr["班级"].ToString();
        //            //crsHour上机学时                   
        //            int crsHour = 0;
        //            crsHour = Convert.ToInt32(dr["上机学时"]);
        //            //crsTimes 上机次数
        //            int crsTimes = 0;
        //            crsTimes = Convert.ToInt32(dr["上机次数"]);
        //            //crsNum 上机人数
        //            int crsNum = 0;
        //            crsNum = Convert.ToInt32(dr["人数"]);
        //            //crsConf  实验环境配置要求
        //            string crsConf = "";
        //            crsConf = dr["对软硬件环境的要求"].ToString();

        //            cmd.CommandText = "insert into course_tb (crsName,crsTeacher,crsClass,crsHour,crsTimes,crsNum,crsConf)values(@crsName,@crsTeacher,@crsClass,@crsHour,@crsTimes,@crsNum,@crsConf)";
        //            cmd.Parameters.Clear();
        //            cmd.Parameters.AddWithValue("@crsName", crsName);
        //            cmd.Parameters.AddWithValue("@crsTeacher", crsTeacher);
        //            cmd.Parameters.AddWithValue("@crsClass", crsClass);
        //            cmd.Parameters.AddWithValue("@crsHour", crsHour);
        //            cmd.Parameters.AddWithValue("@crsTimes", crsTimes);
        //            cmd.Parameters.AddWithValue("@crsNum", crsNum);
        //            cmd.Parameters.AddWithValue("@crsConf", crsConf);

        //            cmd.ExecuteNonQuery();

        //            cmd.CommandText = "SELECT @@Identity ";
        //            #endregion

        //            //课程ID
        //            int crsID = Convert.ToInt32(cmd.ExecuteScalar());

        //            #region 处理实验室
        //            //获取该条记录对应的实验室，根据每条记录的上机地点是否包含该实验室关键字确定是否包含实验室
        //            string lab = dr["上机地点"].ToString();
        //            List<int> labs = new List<int>();
        //            foreach (DataRow drlab in ds.Tables["labs_tb"].Rows)
        //            {
        //                if (lab.Contains(drlab["labKeyWord"].ToString()))
        //                {
        //                    labs.Add(Convert.ToInt32(drlab["labID"].ToString()));
        //                }
        //            }
        //            if (labs.Count == 0)
        //            {
        //                continue;
        //            }
        //            #endregion

        //            #region 处理周次
        //            string weekStr = dr["周"].ToString();

        //            int isDouble = 0;
        //            if (weekStr.Contains("双"))
        //            {
        //                isDouble = 2;
        //            }
        //            else if (weekStr.Contains("单"))
        //            {
        //                isDouble = 1;
        //            }
        //            List<int> weeks = GetListOfStr(weekStr, isDouble);
        //            #endregion

        //            #region 处理工作日
        //            string weekdayStr = dr["星期"].ToString();
        //            List<int> weekdays = GetListOfStr(weekdayStr, 0);
        //            #endregion

        //            #region 处理节次
        //            string classStr = dr["节"].ToString();
        //            List<int> classes = GetListOfStr(classStr, 0);
        //            #endregion

        //            #region 保存课节至数据库
        //            //总记录数


        //            int counts = weeks.Count * weekdays.Count * classes.Count;
        //            cmd.CommandText = "insert into schedule_tb (scdCrs,scdWeek,scdWeekDay,scdClass,scdLab,scdTerm) values(@scdCrs,@scdWeek,@scdWeekDay,@scdClass,@scdLab,@scdTerm)";
        //            foreach (int ws in weeks)
        //            {
        //                foreach (int wds in weekdays)
        //                {
        //                    foreach (int clsS in classes)
        //                    {
        //                        foreach (int lbs in labs)
        //                        {
        //                            //判断是否已经存在
        //                            cmd.Parameters.Clear();
        //                            cmd.Parameters.AddWithValue("@scdCrs", crsID);
        //                            cmd.Parameters.AddWithValue("@scdWeek", ws);
        //                            cmd.Parameters.AddWithValue("@scdWeekDay", wds);
        //                            cmd.Parameters.AddWithValue("@scdClass", clsS);
        //                            cmd.Parameters.AddWithValue("@scdLab", lbs);
        //                            cmd.Parameters.AddWithValue("@scdTerm", termID);
        //                            cmd.ExecuteNonQuery();
        //                        }
        //                    }
        //                }
        //            }

        //            #endregion
        //        }
        //        #endregion

        //        //提交事务
        //        trans.Commit();
        //        srCom.SrLog(Application.ExecutablePath, logMsg);
        //        MessageBox.Show("导出完成！");
        //        this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        logMsg += ex.ToString();
        //        //log
        //        srCom.SrLog(Application.ExecutablePath, logMsg);
        //    }
        //}
        //#endregion
    }
}
