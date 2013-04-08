using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using SyglService;
using System.Threading;
using Microsoft.Win32;

namespace JszxConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("WCF 服务正在启动......");
            try
            {
                ServiceHost serviceHost = new ServiceHost(typeof(JszxService));
                if (serviceHost.State != CommunicationState.Opened)
                {
                    serviceHost.Open();
                }
                Console.WriteLine("WCF 服务正在运行......");
                Console.WriteLine("输入回车键 <ENTER> 退出WCF服务");
                Console.ReadLine();
                serviceHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
