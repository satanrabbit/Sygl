using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
