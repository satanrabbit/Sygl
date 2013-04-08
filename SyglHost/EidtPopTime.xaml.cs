using System;
using System.Linq;
using System.Windows;
using JszxDataModel;
namespace SyglHost
{
    /// <summary>
    /// EidtPopTime.xaml 的交互逻辑
    /// </summary>
    public partial class EidtPopTime : Window
    {
        public EidtPopTime(SetPopTime _setPopW,poptimes_tb _pop,string windowTitle)
        {
            InitializeComponent();
            setPopW = _setPopW;
            pop = _pop;
            this.Title = windowTitle;
            this.PopTimeTextBox.Text = pop.PopTime.ToString();
        }

        SetPopTime setPopW;
        poptimes_tb pop;
        private void CancelEditBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitEditBtn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                pop.PopTime = TimeSpan.Parse(this.PopTimeTextBox.Text.Trim());
            }
            catch(Exception ex)
            {
                MessageBox.Show("时间格式不正确!");
                return;
            }
            using(JszxDataManager jszxM=new JszxDataManager()){
                pop.PopTimeID=jszxM.SavePopTime(pop);
                poptimes_tb _pop= setPopW.popList.Where(p => p.PopTimeID == pop.PopTimeID).FirstOrDefault();
                if (_pop == null)
                {
                    //新增
                    setPopW.popList.Add(pop);
                }
                else
                {
                    //修改
                    setPopW.popList.Where(p => p.PopTimeID == pop.PopTimeID).FirstOrDefault().PopTime = pop.PopTime;
                }

                setPopW.PopTimeDG.Items.Refresh();
                this.Close();
            }
        }
    }
}
