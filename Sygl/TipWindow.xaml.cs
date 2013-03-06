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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sygl
{
    /// <summary>
    /// TipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TipWindow : Window
    {
        public TipWindow(MainWindow _mainWindow)
        {
            InitializeComponent();
            this.Left =left;
            this.Top =top;            
            mainWindow = _mainWindow;
        }
        MainWindow mainWindow;
        double left = System.Windows.SystemParameters.PrimaryScreenWidth - 280;
        double top = System.Windows.SystemParameters.PrimaryScreenHeight;

        public void windowUpAnimation(){
            if (!this.IsVisible)
            { 
                this.Show();
                DoubleAnimation doubleAnimation_window = new DoubleAnimation();
                doubleAnimation_window.From = top;
                doubleAnimation_window.To = top - 190;
                doubleAnimation_window.Duration = new Duration(TimeSpan.Parse("00:00:01"));
                this.BeginAnimation(Window.TopProperty, doubleAnimation_window);
            }
        }

        public void WindowFadeOutAnimation() {
            DoubleAnimation doubleAnimation_window = new DoubleAnimation();
            doubleAnimation_window.From = 1;
            doubleAnimation_window.To = 0.4;
            doubleAnimation_window.BeginTime = (TimeSpan.Parse("00:00:03"));
            doubleAnimation_window.Duration = new Duration(TimeSpan.Parse("00:00:03"));
            this.BeginAnimation(Window.OpacityProperty, doubleAnimation_window);
        }
        public void WindowFadeInAnimation()
        {
            DoubleAnimation doubleAnimation_window = new DoubleAnimation();
            doubleAnimation_window.From = 0.4;
            doubleAnimation_window.To = 1; 
            doubleAnimation_window.Duration = new Duration(TimeSpan.Parse("00:00:00.1"));
            this.BeginAnimation(Window.OpacityProperty, doubleAnimation_window);
        }
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_Deactivated_1(object sender, EventArgs e)
        {
            if (this.Opacity == 1)
            {
                WindowFadeOutAnimation();
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            WindowFadeInAnimation();
        }

        private void Window_MouseEnter_1(object sender, MouseEventArgs e)
        {
            this.VisualOpacity = 1;
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
            this.Hide();
        }
    }
}
