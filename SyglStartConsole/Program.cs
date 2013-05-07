using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyglStartConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdNew;
            //系统能够识别有名称的互斥，因此可以使用它禁止应用程序启动两次
            //第二个参数可以设置为产品的名称:Application.ProductName
            //每次启动应用程序，都会验证名称为SingletonWinAppMutex的互斥是否存在
            Mutex mutex = new Mutex(false, "ColorSpy", out createdNew);
            System.Diagnostics.Process.Start( AppDomain.CurrentDomain.BaseDirectory+"/application/ColorSpy.exe"); 
        }
    }
}
