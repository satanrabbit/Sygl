﻿using System;
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
using JszxDataModel;

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
 

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            JszxDataManager jszxDataManager = new JszxDataManager();
            PopTimeDG.ItemsSource = jszxDataManager.GetPopTimes(true);
        }

    }
}
