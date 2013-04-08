using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using JszxDataModel;
using System.Collections.ObjectModel;

namespace SyglHost
{
    /// <summary>
    /// SetPopTime.xaml 的交互逻辑
    /// </summary>
    public partial class SetPopTime : Window
    {
        public SetPopTime()
        {
            InitializeComponent();
        }

        private List<poptimes_tb> popList1;
        public ObservableCollection<poptimes_tb> popList ;
        JszxDataManager jszxDataManager;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            jszxDataManager = new JszxDataManager();
            popList1 = jszxDataManager.GetPopTimes(true);
            popList = new ObservableCollection<poptimes_tb>(popList1);
            PopTimeDG.ItemsSource = popList;
        }

        private void AddPopBtn_Click_1(object sender, RoutedEventArgs e)
        {
            EidtPopTime editPopW = new EidtPopTime(this, new poptimes_tb() , "添加弹出时间");
            editPopW.ShowDialog();
        } 
        private void EditBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            poptimes_tb pop = btn.Tag as poptimes_tb;
            EidtPopTime editPopW = new EidtPopTime(this,pop,"修改弹出时间");
            editPopW.ShowDialog();
        }

        private void DeleteBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定删除该条记录吗?", "请确认", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Button btn = sender as Button;
                poptimes_tb pop = btn.Tag as poptimes_tb;
                //更新数据库
                jszxDataManager.DeletePopTime(pop.PopTimeID);
                popList.Remove(pop);                

            }       
        }

    }
}
