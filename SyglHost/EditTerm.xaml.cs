using JszxDataModel;
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

namespace SyglHost
{
    /// <summary>
    /// EditTerm.xaml 的交互逻辑
    /// </summary>
    public partial class EditTerm : Window
    {
        public EditTerm()
        {
            InitializeComponent();

            //查询当前学期
            jdm = new JszxDataManager();
            tm = jdm.GetCurrentTerm();
            SetTermContent();
        }
        terms_tb tm;
        JszxDataManager jdm;
        //是否新增学期标识，true为新增，false为修改
        bool isAdd=false;

        private void SetTermContent()
        {
            if (tm != null)
            {
                this.CurrentTermContent.Text = "学期编号:" + tm.TermID.ToString() +
                    " - " + tm.TermYear + "学年:" + (tm.TermIndex ? "下" : "上") + "学期-开学日期："
                    + tm.TermStartDay.ToString("yyyy-MM-dd") + "-共" + tm.TermWeeks.ToString() + "周";
            }
            else
            {
                this.CurrentTermContent.Text = "未设置！";
            }
        }

        /// <summary>
        /// 编辑当前学期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCurrentTermBtn_Click_1(object sender, RoutedEventArgs e)
        {
            isAdd = false;
            this.TermFormGrid.Visibility = Visibility.Visible;
            //查询当前学期
            if (tm != null)
            {
                //初始化表单
                this.TermYear.Text = tm.TermYear;
                this.TermWeeks.Text = tm.TermWeeks.ToString();
                this.TermStartDay.SelectedDate = tm.TermStartDay;
                this.TermIndex_True.IsChecked = tm.TermIndex;
                this.TermIndex_False.IsChecked = !tm.TermIndex;
                this.TermIsUse_False.IsChecked = !tm.TermIsUse;
                this.TermIsUse_True.IsChecked = tm.TermIsUse;
            }
            else
            {
                //不存在当前学期，变为新增
                isAdd = true;
                this.TermYear.Text = "";
                this.TermWeeks.Text = "";
                this.TermStartDay.SelectedDate = DateTime.Now;
                this.TermIndex_True.IsChecked = true;
                this.TermIndex_False.IsChecked = false;
                this.TermIsUse_False.IsChecked = false;
                this.TermIsUse_True.IsChecked = true;
            }
            SaveTermForm.Content = "修改";
        }

        /// <summary>
        /// 新增学期事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTermBtn_Click_1(object sender, RoutedEventArgs e)
        {
            isAdd = true;
            this.TermFormGrid.Visibility = Visibility.Visible;
            this.TermYear.Text = "";
            this.TermWeeks.Text = "";
            this.TermStartDay.SelectedDate = DateTime.Now;
            this.TermIndex_True.IsChecked = true;
            this.TermIndex_False.IsChecked = false;
            this.TermIsUse_False.IsChecked = false;
            this.TermIsUse_True.IsChecked = true;
            SaveTermForm.Content = "新增";
        }

        private void SaveTermForm_Click_1(object sender, RoutedEventArgs e)
        {
            terms_tb _tm = new terms_tb();
            //学期开始日期
            if (this.TermStartDay.SelectedDate == null)
            {
                MessageBox.Show("请选择本学期开始日期！");
            }
            else
            {
                _tm.TermStartDay = (DateTime)this.TermStartDay.SelectedDate;
            }
            //学期周数
            if (this.TermWeeks != null)
            {
                _tm.TermWeeks =Convert.ToSByte(this.TermWeeks.Text.Trim());
            }
            else
            {
                MessageBox.Show("请填写本学期周数！");
            }
            //学年
            if (this.TermYear != null)
            {
                _tm.TermYear = this.TermYear.Text.Trim();
            }
            else
            {
                MessageBox.Show("请填写学年！");
            }

            //学期
            if ((bool)this.TermIndex_False.IsChecked)
            {
                _tm.TermIndex = false;
            }
            if ((bool)this.TermIndex_True.IsChecked)
            {
                _tm.TermIndex = true;
            }
            //是否当前学期
            if ((bool)this.TermIsUse_False.IsChecked)
            {
                _tm.TermIsUse = false;

            }
            if ((bool)this.TermIsUse_True.IsChecked)
            {
                _tm.TermIsUse = true;
            }
            if (isAdd)
            {
                //新增学期
                tm = _tm;
            }
            else
            {
                _tm.TermID = tm.TermID;
                tm = _tm; 
            }            
            tm.TermID = jdm.SaveTerm(tm);
            this.TermFormGrid.Visibility = Visibility.Hidden;
            SetTermContent();
        }
    }
}
