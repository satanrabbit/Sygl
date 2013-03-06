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
        }
        ServiceHost host = new ServiceHost(typeof(SyglService.JszxService));

        private void StartService()
        {
                host.Opened += delegate
                {
                    MessageBox.Show("服务启动");
                };
                host.Open();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartService();
        }
    }
}
