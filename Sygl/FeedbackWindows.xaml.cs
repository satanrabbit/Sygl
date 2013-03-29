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
    /// FeedbackWindows.xaml 的交互逻辑
    /// </summary>
    public partial class FeedbackWindows : Window
    {
        public FeedbackWindows(bool isSign)
        {
            InitializeComponent();
            if (isSign)
            {
                this.FromSign.Visibility=Visibility.Visible;
            }
        }

        /// <summary>
        /// 工作评价分数
        /// </summary>
        int workStarFlag = 0;
        /// <summary>
        /// 评价软件分数
        /// </summary>
        int softstarFlag = 0;

        private void WorkStar1_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(1);
        }

        private void WorkStar1_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(workStarFlag);
        }

        private void WorkStar2_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(2);
        }

        private void WorkStar2_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(workStarFlag);
        }

        private void WorkStar3_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(3);
        }

        private void WorkStar3_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(workStarFlag);
        }

        private void WorkStar4_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(4);
        }

        private void WorkStar4_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(workStarFlag);
        }

        private void WorkStar5_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(5);
        }

        private void WorkStar5_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetWorkStar(workStarFlag);
        }

        private void WorkStar1_Click_1(object sender, RoutedEventArgs e)
        {
            workStarFlag = 1;
            resetWorkStar(workStarFlag);
        }

        private void WorkStar2_Click_1(object sender, RoutedEventArgs e)
        {
            workStarFlag = 2;
            resetWorkStar(workStarFlag);
        }

        private void WorkStar3_Click_1(object sender, RoutedEventArgs e)
        {
            workStarFlag = 3;
            resetWorkStar(workStarFlag);
        }

        private void WorkStar4_Click_1(object sender, RoutedEventArgs e)
        {
            workStarFlag = 4;
            resetWorkStar(workStarFlag);
        }

        private void WorkStar5_Click_1(object sender, RoutedEventArgs e)
        {
            workStarFlag = 5;
            resetWorkStar(workStarFlag);
        }

        private void SoftStar1_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(1);
        }

        private void SoftStar2_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(2);
        }

        private void SoftStar1_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(softstarFlag);
        }

        private void SoftStar2_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(softstarFlag);
        }

        private void SoftStar3_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(3);
        }

        private void SoftStar3_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(softstarFlag);
        }

        private void SoftStar4_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(4);
        }

        private void SoftStar4_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(softstarFlag);
        }

        private void SoftStar5_MouseEnter_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(5);
        }

        private void SoftStar5_MouseLeave_1(object sender, MouseEventArgs e)
        {
            resetSoftStar(softstarFlag);
        }

        private void SoftStar5_Click_1(object sender, RoutedEventArgs e)
        {
            softstarFlag = 5;
            resetSoftStar(softstarFlag);
        }

        private void SoftStar4_Click_1(object sender, RoutedEventArgs e)
        {
            softstarFlag = 4;
            resetSoftStar(softstarFlag);
        }

        private void SoftStar3_Click_1(object sender, RoutedEventArgs e)
        {
            softstarFlag = 3;
            resetSoftStar(softstarFlag);
        }

        private void SoftStar2_Click_1(object sender, RoutedEventArgs e)
        {
            softstarFlag = 2;
            resetSoftStar(softstarFlag);
        }

        private void SoftStar1_Click_1(object sender, RoutedEventArgs e)
        {
            softstarFlag = 1;
            resetSoftStar(softstarFlag);
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void resetWorkStar(int star){
            switch (star)
            {
                case 0:
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStarTip.Text = "";
                    break;
                case 1:
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStarTip.Text = "1星\t不到位 ";
                    break;
                case 2:
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStarTip.Text = "2星\t一般般";
                    break;
                case 3:
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStarTip.Text = "3星\t刚及格 ";
                    break;
                case 4:
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStarTip.Text = "4星\t继续努力";
                    break;
                case 5:
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle1");
                    this.WorkStarTip.Text = "5星\t鼓励一下";
                    break;
                default :
                    this.WorkStar1.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar2.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    this.WorkStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    break;
            }
        }
        private void resetSoftStar(int star)
        {
            switch (star)
            {
                case 0:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle0"); 
                     this.SoftStarTip.Text = "";
                    break;
                case 1:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStarTip.Text = "1星\t很差";
                    break;
                case 2:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStarTip.Text = "2星\t一般";
                    break;
                case 3:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStarTip.Text = "3星\t还好";
                    break;
                case 4:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStarTip.Text = "4星\t不错";
                    break;
                case 5:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle1");
                     this.SoftStarTip.Text = "5星\t很好";
                    break;
                default:
                     this.SoftStar1.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar2.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar3.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar4.SetResourceReference(Button.StyleProperty, "StarStyle0");
                     this.SoftStar5.SetResourceReference(Button.StyleProperty, "StarStyle0");
                    break;
            }
        }

        private void SubmitFeedback_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
