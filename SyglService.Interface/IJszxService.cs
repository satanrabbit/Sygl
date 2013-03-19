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
        Term GetCurrentTerm();
        /// <summary>
        /// 获取当前服务器时间
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DateTime GetCurrentDateTime();

        /// <summary>
        /// 查询当前时间是否需要填写，需要则返回待填写的实验记录 Schedule
        /// </summary>
        /// <returns>待填写的实验记录 Schedule</returns>
        [OperationContract]
        Exprecord GetCurrentExprecord(int labID);

        /// <summary>
        /// 获取当前周数
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetCurrentWeeks();
        /// <summary>
        /// 获取服务器时间当前的weekday（0、1、2、3、4、5、6）
        /// </summary>
        /// <returns>weekDay(0、1、2、3、4、5、6)</returns>
        [OperationContract]
        int GetCurrentWeekDay();

       
        /// <summary>
        /// 获取当前的课节信息
        /// </summary>
        /// <returns>当前的课节信息</returns>
        [OperationContract]
        ClassTime GetCurrentClassTime();
      
        /// <summary>
        /// 获取当前时间实验是否需要填写  
        /// </summary>
        /// <param name="labID">实验室ID</param>
        /// <returns>带填写标识的记录</returns>
         [OperationContract]
        ExpRecordWithFlag GetExpRecordWithFlag(int labID);

        /// <summary>
        /// 获取弹窗倒计时时间
        /// </summary>
        /// <returns>弹窗倒计时时间</returns>
        [OperationContract]
         int GetPopTimeTallies();

        /// <summary>
        /// 获取课节时间列表
        /// </summary>
        /// <returns>课节时间列表</returns>
        [OperationContract]
        List<ClassTime> GetClassTimeList();
        #endregion

    }

    #region 数据契约
     
    [DataContract]
    public class Admin
    {
        /// <summary>
        /// 管理员编号
        /// </summary>
        [DataMember]
        public int AdminID { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>
        [DataMember]
        public string AdminName { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        [DataMember]
        public string AdminAccount { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [DataMember]
        public string AdminPWD { get; set; }
        /// <summary>
        /// 管理员权限，1超级管理员，默认值2，普通管理员
        /// </summary>
        [DataMember]
        public sbyte AdminAuthority { get; set; }
        /// <summary>
        /// 管理员状态，默认1正常状态，2挂起状态①
        /// </summary>
        [DataMember]
        public sbyte AdminStatus { get; set; }
        /// <summary>
        /// 管理员备注信息
        /// </summary>
        [DataMember]
        public string AdminBackup { get; set; } 
        
    }
    [DataContract]
    public class Lab
    {
        /// <summary>
        /// 主键，自增，实验室编号
        /// </summary>
        [DataMember]
        public int LabID { get; set; }
        /// <summary>
        /// 实验室名
        /// </summary>
        [DataMember]
        public string LabName { get; set; }
        /// <summary>
        /// 实验室管理员，外键admins_tb-adminID
        /// </summary>
        [DataMember]
        public Nullable<int> LabAdmin { get; set; }
        /// <summary>
        /// 实验室位置
        /// </summary>
        [DataMember]
        public string LabAddr { get; set; }
        /// <summary>
        /// 实验室IP
        /// </summary>
        [DataMember]
        public string LabIP { get; set; }
        /// <summary>
        /// 实验室关键字，用于课表导入
        /// </summary>
        [DataMember]
        public string LabKeyWord { get; set; }
    }
    [DataContract]
    public class ClassTime
    {
       /// <summary>
        /// 主键，课节号
       /// </summary>
        [DataMember]
        public int ClsTmIndex { get; set; }
        /// <summary>
        /// 课节名称
        /// </summary>
        [DataMember]
        public string ClsTmName { get; set; }
        /// <summary>
        /// 课节开始时间
        /// </summary>
        [DataMember]
        public System.TimeSpan ClsTmStart { get; set; }
        /// <summary>
        /// 课节结束时间
        /// </summary>
        [DataMember]
        public System.TimeSpan ClsTmEnd { get; set; }
    }
    [DataContract]
    public class Term
    {
        /// <summary>
        /// 主键，自增，学期编号
        /// </summary>
        [DataMember]
        public int TermID { get; set; }
        /// <summary>
        /// 学期年
        /// </summary>
        [DataMember]
        public string TermYear { get; set; }
        /// <summary>
        /// 默认值0上学期，1下学期
        /// </summary>
        [DataMember]
        public bool TermIndex { get; set; }
        /// <summary>
        /// 本学期开学日期，必须为周一
        /// </summary>
        [DataMember]
        public DateTime TermStartDay { get; set; }
        /// <summary>
        /// 本学期周数
        /// </summary>
        [DataMember]
        public sbyte TermWeeks { get; set; }
        /// <summary>
        /// 默认值0不可用，1当前可用学期
        /// </summary>
        [DataMember]
        public bool TermIsUse { get; set; }
    }
     [DataContract]
    public class Course
    {
         /// <summary>
        /// 主键，自增，课程编号
         /// </summary>
        [DataMember]
        public int CrsID { get; set; }
         /// <summary>
        /// 课程名
         /// </summary>
        [DataMember]
        public string CrsName { get; set; }
         /// <summary>
        /// 教师姓名
         /// </summary>
        [DataMember]
        public string CrsTeacher { get; set; }
         /// <summary>
        /// 上课班级
         /// </summary>
        [DataMember]
        public string CrsClasses { get; set; }
         /// <summary>
        /// 上课学时
         /// </summary>
        [DataMember]
        public Nullable<float> CrsHour { get; set; }
         /// <summary>
        /// 上课次数
         /// </summary>
        [DataMember]
        public Nullable<int> CrsTimes { get; set; }
         /// <summary>
        /// 上课人数
         /// </summary>
        [DataMember]
        public Nullable<int> CrsNum { get; set; }
         /// <summary>
        /// 配置要求
         /// </summary>
        [DataMember]
        public string CrsConf { get; set; }
         /// <summary>
        /// 备注
         /// </summary>
        [DataMember]
        public string CrsRemark { get; set; }
    }
    [DataContract]
     public class Schedule
     {
        /// <summary>
         /// 主键，自增，课程安排编号
        /// </summary>
        [DataMember]
        public int ScdID { get; set; }
        /// <summary>
        /// 上课课程，外键courses_tb-CrsID
        /// </summary>
        [DataMember]
        public int ScdCrs { get; set; }
        /// <summary>
        /// 上课周次
        /// </summary>
        [DataMember]
        public sbyte ScdWeek { get; set; }
        /// <summary>
        /// 上课工作日，周日值为0
        /// </summary>
        [DataMember]
        public sbyte ScdWeekDay { get; set; }
        /// <summary>
        /// 上课课节，外键classTimes_tb-ClsTmIndex
        /// </summary>
        [DataMember]
        public int ScdClass { get; set; }
        /// <summary>
        /// 上课实验室，外键labs_tb-LabID
        /// </summary>
        [DataMember]
        public int ScdLab { get; set; }
        /// <summary>
        /// 上机学期，外键terms_tb-TermID
        /// </summary>
        [DataMember]
        public int ScdTerm { get; set; } 
     }

    #region 实验记录
    [DataContract]
    public class Exprecord
    {
        /// <summary>
        /// 主键，自增，实验记录编号
        /// </summary>
        [DataMember]
        public long RecordID { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        [DataMember]
        public string CourseName { get; set; }
        /// <summary>
        /// 实验名称
        /// </summary>
        [DataMember]
        public string ExpName { get; set; }
        /// <summary>
        /// 实验班级
        /// </summary>
        [DataMember]
        public string ExpClasses { get; set; }
        /// <summary>
        /// 应到人数
        /// </summary>
        [DataMember]
        public Nullable<int> Shoulder { get; set; }
        /// <summary>
        /// 实到人数
        /// </summary>
        [DataMember]
        public Nullable<int> Realizer { get; set; }
        /// <summary>
        /// 分组数
        /// </summary>
        [DataMember]
        public Nullable<int> Groups { get; set; }
        /// <summary>
        /// 每组人数
        /// </summary>
        [DataMember]
        public Nullable<int> PerGroup { get; set; }
        /// <summary>
        /// 学生状态
        /// </summary>
        [DataMember]
        public string StudentStatus { get; set; }
        /// <summary>
        /// 仪器状态
        /// </summary>
        [DataMember]
        public string InstrumentStatus { get; set; }
        /// <summary>
        /// 出现问题
        /// </summary>
        [DataMember]
        public string Problems { get; set; }
        /// <summary>
        /// 教师姓名
        /// </summary>
        [DataMember]
        public string TeacherName { get; set; }
        /// <summary>
        /// 教师教工号
        /// </summary>
        [DataMember]
        public string TeacherNumber { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        [DataMember]
        public string StudentName { get; set; }
        /// <summary>
        /// 上课节次
        /// </summary>
        [DataMember]
        public Nullable<sbyte> ExpCls { get; set; }
        /// <summary>
        /// 提交时间，默认当前时间
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> PostTime { get; set; }
        /// <summary>
        /// 实验日期
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> ExpDate { get; set; }
        /// <summary>
        /// 实验机房
        /// </summary>
        [DataMember]
        public string ExpLab { get; set; }
        /// <summary>
        /// 实验学期，外键Terms_tb-TermID
        /// </summary>
        [DataMember]
        public Nullable<int> ExpTerm { get; set; }
        /// <summary>
        /// 实验周次
        /// </summary>
        [DataMember]
        public Nullable<sbyte> ExpWeek { get; set; }
        /// <summary>
        /// 实验工作日
        /// </summary>
        [DataMember]
        public Nullable<sbyte> ExpWeekDay { get; set; }
        /// <summary>
        /// 实验室机房ID，外键Labs_tb-LabID
        /// </summary>
        [DataMember]
        public Nullable<int> ExpLabID { get; set; }
    }
    #endregion

    #region 带是否填写标识的实验记录
    /// <summary>
    /// 带是否填写标识的实验记录
    /// </summary>
    [DataContract]
    public class ExpRecordWithFlag
    {
        /// <summary>
        /// 填写标识，0当前无课，1已经填写，2需要核对，3新填写
        /// </summary>
        [DataMember]
        public int SignFlag { get; set; }
        /// <summary>
        /// 待填写的实验记录，
        /// </summary>
        [DataMember]
        public Exprecord ExpRecord { get; set; }
    }
    #endregion
    #endregion

    
}
