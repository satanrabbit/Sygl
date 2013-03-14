using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using JszxDataModel;
using System.Runtime.Serialization;

namespace SyglService.Interface
{

    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IJszxService
    {
        #region 操作契约

        /// <summary>
        ///  获取当前学期
        /// </summary>
        /// <returns>当前学期</returns>
        [OperationContract]
        terms_tb GetCurrentTerm();
        /// <summary>
        /// 获取当前服务器时间
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DateTime GetNowDateTime();



        #endregion


        #region 注释掉Operation实例
        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: 在此添加您的服务操作
        #endregion

    }

    #region 注释掉用例
    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    // 可以将 XSD 文件添加到项目中。在生成项目后，可以通过命名空间“SyglService.ContractType”直接使用其中定义的数据类型。
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
    #endregion
}
