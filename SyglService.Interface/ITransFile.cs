using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace SyglService.Interface
{
    [ServiceContract]
    interface ITransFile
    {
        [OperationContract]
        ResultMessage UpLoadFile(TransferFileMessage tMsg);
        TransferFileMessage DownLoadFile(string fileName);

    }
    #region 消息协定
    [MessageContract]
    public class TransferFileMessage  
    {  
         /// <summary>
        /// 文件名
        /// </summary>
        [MessageHeader]  
        public string File_Name; //文件名  
        /// <summary>
        /// 文件流
        /// </summary>
        [MessageBodyMember]  
        public Stream File_Stream; //文件流  
    }  
        [MessageContract]  
    public class ResultMessage  
    {  
        /// <summary>
        /// 错误消息内容
        /// </summary>
        [MessageHeader]  
        public string ErrorMessage;
        /// <summary>
        /// 成功与否标识
        /// </summary>
        [MessageBodyMember]  
        public bool IsSuccessed;  //成功与否标识
    }  
    #endregion
}
