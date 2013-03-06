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

namespace Sygl
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitAbout_Click_1(object sender, RoutedEventArgs e)
        {
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

        private void CloseAboutAuthor_Click_1(object sender, RoutedEventArgs e)
        {
            this.AboutAuthorGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OpenAboutAuthorBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.AboutAuthorGrid.Visibility = System.Windows.Visibility.Visible; 
            this.AboutCCGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CloseAboutCC_Click_1(object sender, RoutedEventArgs e)
        {
            this.AboutCCGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OpenAboutCCBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.AboutAuthorGrid.Visibility = System.Windows.Visibility.Hidden;
            this.AboutCCGrid.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
