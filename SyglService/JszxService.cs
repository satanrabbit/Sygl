using SyglService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JszxDataModel;

namespace SyglService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class JszxService : IJszxService
    {
        #region 注释掉
        //    public string GetData(int value)
    //    {
    //        DateTime date = DateTime.Now;
    //        return string.Format("You entered: {0}", value+date.Date.ToShortDateString());
    //    }

    //    public CompositeType GetDataUsingDataContract(CompositeType composite)
    //    {
    //        if (composite == null)
    //        {
    //            throw new ArgumentNullException("composite");
    //        }
    //        if (composite.BoolValue)
    //        {
    //            composite.StringValue += "Suffix";
    //        }
    //        return composite;
        //    }
        #endregion
        /// <summary>
        /// 获取当前学期
        /// </summary>
        /// <returns>当前学期</returns>
        public terms_tb GetCurrentTerm()
        {
            using(JszxDataManager jszxDateManage=new JszxDataManager()){
                return jszxDateManage.GetCurrentTerm();
            } 
        }
        /// <summary>
        /// 获取当前服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetNowDateTime()
        {
            return DateTime.Now;
        }
    }
}
