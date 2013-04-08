using SyglService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JszxDataModel;
using System.Net;

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

        #region 获取当前学期
        /// <summary>
        /// 获取当前学期
        /// </summary>
        /// <returns>当前学期</returns>
        public Term GetCurrentTerm()
        {
            using(JszxDataManager jszxDateManage=new JszxDataManager()){
                terms_tb tmtb = jszxDateManage.GetCurrentTerm();
                Term tm = new Term();
                if (tmtb != null)
                {
                    tm.TermID = tmtb.TermID;
                    tm.TermIndex = tmtb.TermIndex;
                    tm.TermIsUse = tmtb.TermIsUse;
                    tm.TermStartDay = tmtb.TermStartDay;
                    tm.TermWeeks = tmtb.TermWeeks;
                    tm.TermYear = tmtb.TermYear;
                }
                return tm;
            } 
        }
        #endregion

        #region 获取当前服务器时间
        /// <summary>
        /// 获取当前服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        #endregion

        #region 获取当前教学周
        /// <summary>
        /// 获取当前教学周
        /// </summary>
        /// <returns></returns> 
        public int GetCurrentWeeks()
        {
            int week = 0;
            Term tm = GetCurrentTerm();
            week = (DateTime.Now - tm.TermStartDay).Days / 7 + 1;
            return week;
        }
        #endregion

        #region 获取服务器时间当前的weekday（0、1、2、3、4、5、6）
        /// <summary>
        /// 获取服务器时间当前的weekday（0、1、2、3、4、5、6）
        /// </summary>
        /// <returns>weekDay(0、1、2、3、4、5、6)</returns> 
        public int GetCurrentWeekDay()
        {
            int wd = 0;
            wd = (int)DateTime.Now.DayOfWeek;
            return wd;
        }

        #endregion

        #region 获取当前的课节信息
        /// <summary>
        /// 获取当前的课节信息
        /// </summary>
        /// <returns>当前的课节信息</returns>
        public ClassTime GetCurrentClassTime()
        {
            ClassTime clt = new ClassTime();
            using (JszxDataManager jszxDateManage = new JszxDataManager())
            {
                classtimes_tb clt_tb = jszxDateManage.GetClsTimeByTimeSpan(TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")));
                if (clt_tb == null)
                {
                    return null;
                }
                else
                {
                    clt.ClsTmEnd = clt_tb.ClsTmEnd;
                    clt.ClsTmIndex = clt_tb.ClsTmIndex;
                    clt.ClsTmName = clt_tb.ClsTmName;
                    clt.ClsTmStart = clt_tb.ClsTmStart;                    
                    return clt;
                }
            }

        }
        #endregion

        #region 获取当前待填写的实验记录
        /// <summary>
        /// 获取当前时间待填写的实验记录
        /// </summary>
        /// <returns>待填写的实验记录 Exprecord</returns>
        public Exprecord GetCurrentExprecord(int labID)
        {
            Exprecord expRecord = new Exprecord();            
            using(JszxDataManager jszxDateManage=new JszxDataManager()){
                //当前课节
                ClassTime ct = GetCurrentClassTime();
                
                 if (ct == null)
                 {
                     //当前不在上课时间，返回空的待填实验记录
                     return null;
                 }
                 else
                 {
                     //在上课时间，判断当前是否有课
                     //当前学期
                     Term tm = GetCurrentTerm();
                     //当前周次
                     int wk = GetCurrentWeeks();
                     //当前工作日
                     int weekDay = GetCurrentWeekDay();

                     schedule_tb scd_tb = jszxDateManage.GetScheduleByCls(labID,tm.TermID,wk,weekDay,ct.ClsTmIndex);

                     if (scd_tb == null)
                     {
                         //没课
                         return null;
                     }
                     else
                     {
                         //有课
                         //构建实验记录信息
                         expRecord.CourseName = scd_tb.courses_tb.CrsName;
                         expRecord.ExpClasses = scd_tb.courses_tb.CrsClasses;
                         expRecord.ExpCls = (sbyte?)scd_tb.ScdClass;
                         expRecord.ExpDate = DateTime.Now;
                         expRecord.ExpLab = scd_tb.labs_tb.LabName;
                         expRecord.ExpLabID = scd_tb.ScdLab;
                         expRecord.ExpName = "";
                         expRecord.ExpTerm = scd_tb.ScdTerm;
                         expRecord.ExpWeek = scd_tb.ScdWeek;
                         expRecord.ExpWeekDay = scd_tb.ScdWeekDay;
                         expRecord.Groups = scd_tb.courses_tb.CrsNum;
                         expRecord.InstrumentStatus = "正常";
                         expRecord.PerGroup = 1;
                         expRecord.Problems = "无";
                         expRecord.Realizer = scd_tb.courses_tb.CrsNum;
                         expRecord.Shoulder = scd_tb.courses_tb.CrsNum;
                         expRecord.StudentStatus = "良好";
                         expRecord.TeacherName = scd_tb.courses_tb.CrsTeacher;                         
                     }


                 }
             }
            
            return expRecord;
        }
        #endregion

        #region 获取当前时间实验是否需要填写
        /// <summary>
        /// 获取当前时间实验是否需要填写
        /// </summary>
        /// <param name="labID">实验室ID</param>
        /// <returns>带填写标识的记录</returns>
        public ExpRecordWithFlag GetExpRecordWithFlag(int labID)
        {
            ExpRecordWithFlag expFlag = new ExpRecordWithFlag();
            int signFlag = 0;
            Exprecord expRecord = new Exprecord();
            using (JszxDataManager jszxDateManage = new JszxDataManager())
            {
                //当前课节
                ClassTime ct = GetCurrentClassTime();

                if (ct == null)
                {
                    //当前不在上课时间，返回空的待填实验记录
                    signFlag = 0;
                }
                else
                {
                    //在上课时间，判断当前是否有课
                    //当前学期
                    Term tm = GetCurrentTerm();
                    //当前周次
                    int wk = GetCurrentWeeks();
                    //当前工作日
                    int weekDay = GetCurrentWeekDay();

                    schedule_tb scd_tb = jszxDateManage.GetScheduleByCls(labID, tm.TermID, wk, weekDay, ct.ClsTmIndex);

                    if (scd_tb == null)
                    {
                        //没课,不需要填写
                        signFlag = 0;
                        expRecord = null;
                    }
                    else
                    {
                        //有课
                        //查询是否填写

                        exprecords_tb exp_tb = jszxDateManage.GetExprecordByCls(labID, tm.TermID, wk, weekDay, ct.ClsTmIndex);
                        if (exp_tb == null)
                        {
                            //未填写
                            signFlag = 3;
                            //未填写，构建实验记录，返回填写 
                            expRecord.CourseName = scd_tb.courses_tb.CrsName;
                            expRecord.ExpClasses = scd_tb.courses_tb.CrsClasses;
                            expRecord.ExpCls = (sbyte?)scd_tb.ScdClass;
                            expRecord.ExpDate = DateTime.Now;
                            expRecord.ExpLab = scd_tb.labs_tb.LabName;
                            expRecord.ExpLabID = scd_tb.ScdLab;
                            expRecord.ExpName = "";
                            expRecord.ExpTerm = scd_tb.ScdTerm;
                            expRecord.ExpWeek = scd_tb.ScdWeek;
                            expRecord.ExpWeekDay = scd_tb.ScdWeekDay;
                            expRecord.Groups = scd_tb.courses_tb.CrsNum;
                            expRecord.InstrumentStatus = "正常";
                            expRecord.PerGroup = 1;
                            expRecord.Problems = "无";
                            expRecord.Realizer = scd_tb.courses_tb.CrsNum;
                            expRecord.Shoulder = scd_tb.courses_tb.CrsNum;
                            expRecord.StudentStatus = "良好";
                            expRecord.TeacherName = scd_tb.courses_tb.CrsTeacher;
                            expRecord.StudentName = "";
                            expRecord.TeacherNumber = "";
                            //expRecord.PostTime = 

                        }
                        else
                        {
                            //已经填写
                            if (exp_tb.CourseName == scd_tb.courses_tb.CrsName)
                            {
                                //课程名称相同，判断为正确的记录
                                expRecord = null;
                                signFlag = 1;
                            }
                            else
                            {
                                //课程名称不相同，错误的填写记录
                                signFlag = 2;

                                //返回待确认记录
                                //未填写，构建实验记录，返回填写 
                                expRecord.CourseName = exp_tb.CourseName;
                                expRecord.ExpClasses = exp_tb.ExpClasses;
                                expRecord.ExpCls = exp_tb.ExpCls;
                                expRecord.ExpDate = exp_tb.ExpDate;
                                expRecord.ExpLab = exp_tb.ExpLab;
                                expRecord.ExpLabID = exp_tb.ExpLabID;
                                expRecord.ExpName = exp_tb.ExpName;
                                expRecord.ExpTerm = exp_tb.ExpTerm;
                                expRecord.ExpWeek = exp_tb.ExpWeek;
                                expRecord.ExpWeekDay = exp_tb.ExpWeekDay;
                                expRecord.Groups = exp_tb.Groups;
                                expRecord.InstrumentStatus = exp_tb.InstrumentStatus;
                                expRecord.PerGroup = exp_tb.PerGroup;
                                expRecord.Problems = exp_tb.Problems;
                                expRecord.Realizer = exp_tb.Realizer;
                                expRecord.Shoulder = exp_tb.Shoulder;
                                expRecord.StudentStatus = exp_tb.StudentStatus;
                                expRecord.TeacherName = exp_tb.TeacherName;
                                expRecord.StudentName = exp_tb.StudentName;
                                expRecord.TeacherNumber = exp_tb.TeacherNumber;
                                expRecord.PostTime = exp_tb.PostTime;
                            }
                        }
                        
                    }
                }
            }
            expFlag.SignFlag = signFlag;
            expFlag.ExpRecord = expRecord;
            return expFlag;
        }
        #endregion

        #region 获取弹窗倒计时时间
        /// <summary>
        /// 获取弹窗倒计时时间
        /// </summary>
        /// <returns>弹窗倒计时时间</returns> 
        public int GetPopTimeTallies()
        {
            int tallies = 100;
            using (JszxDataManager jszxDateManage = new JszxDataManager())
            {
                List<poptimes_tb> popTimeList= jszxDateManage.GetPopTimes(true);
                //遍历获取当距离当前最近的弹出时间 
                TimeSpan latestTime ; 
                TimeSpan nowTime = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));

               latestTime= popTimeList.OrderBy(p => p.PopTime).Where(p => p.PopTime > nowTime).FirstOrDefault().PopTime; 
               tallies =(int)( latestTime.TotalSeconds - nowTime.TotalSeconds);
            }
            return tallies;
        }
        #endregion

        #region 获取课节时间列表
        /// <summary>
        /// 获取课节时间列表
        /// </summary>
        /// <returns>课节时间列表</returns>
       public List<ClassTime> GetClassTimeList()
        {
            List<ClassTime> ClsTimeList = new List<ClassTime>();
            using (JszxDataManager jszxDateManage = new JszxDataManager())
            {
                List<classtimes_tb> ClassTime_tbList = jszxDateManage.GetClassTimes(true);
                foreach(classtimes_tb cltdb in ClassTime_tbList){
                    ClassTime clt = new ClassTime();
                    clt.ClsTmEnd = cltdb.ClsTmEnd;
                    clt.ClsTmIndex = cltdb.ClsTmIndex;
                    clt.ClsTmName = cltdb.ClsTmName;
                    clt.ClsTmStart = cltdb.ClsTmStart;
                    ClsTimeList.Add(clt);
                }
            }
            return ClsTimeList;
        }
        #endregion

        #region 获取实验室列表
       /// <summary>
       /// 获取实验室列表
       /// </summary>
       /// <returns>实验室列表</returns>
       public List<Lab> GetLabList()
       {
           List<Lab> labs = new List<Lab>();
           using (JszxDataManager jszxDateManage = new JszxDataManager())
           {
               List<labs_tb> lab_tbs = jszxDateManage.GetLabs_tbList();
               
               foreach (labs_tb lb_tb in lab_tbs)
               {
                   Lab lb = new Lab();
                   lb.LabAddr = lb_tb.LabAddr;
                   lb.LabAdmin = lb_tb.LabAdmin;
                   lb.LabID = lb_tb.LabID;
                   lb.LabIP = lb_tb.LabIP;
                   lb.LabKeyWord = lb_tb.LabKeyWord;
                   lb.LabName = lb_tb.LabName;

                   labs.Add(lb);
               }
           }
           return labs;
       }
        #endregion

        #region  保存实验记录
        /// <summary>
        /// 保存实验记录
        /// </summary>
        /// <param name="exp">实验记录信息，不包含实验学期，和实验节次</param>
        /// <param name="selectedClass">实验记录节次的列表</param>
        public void SaveExpRecord(Exprecord exp, List<int> selectedClass)
        {
            using(JszxDataManager jszxDataManager=new JszxDataManager()){
                int tm = jszxDataManager.GetCurrentTerm().TermID;
                exp.ExpTerm = tm;
                foreach(int cls in selectedClass){
                    exp.ExpCls =(sbyte)cls;
                    exprecords_tb exptb = null;
                    //判断当前课是否填写过
                    exptb = jszxDataManager.GetExpRecords(tm, (int)exp.ExpLabID, (int)exp.ExpWeek, (int)exp.ExpWeekDay, (int)exp.ExpCls).FirstOrDefault();
                    if (exptb == null)
                    {
                        exptb = new exprecords_tb();
                    }
                    exptb.CourseName = exp.CourseName;
                    exptb.ExpClasses = exp.ExpClasses;
                    exptb.ExpCls = exp.ExpCls;
                    exptb.ExpDate = exp.ExpDate;
                    exptb.ExpLab = exp.ExpLab;
                    exptb.ExpLabID = exp.ExpLabID;
                    exptb.ExpName = exp.ExpName;
                    exptb.ExpTerm = exp.ExpTerm;
                    exptb.ExpWeek = exp.ExpWeek;
                    exptb.ExpWeekDay = exp.ExpWeekDay;
                    exptb.Groups = exp.Groups;
                    exptb.InstrumentStatus = exp.InstrumentStatus;
                    exptb.PerGroup = exp.PerGroup;
                    exptb.PostTime = exp.PostTime;
                    exptb.Problems = exp.Problems;
                    exptb.Realizer = exp.Realizer;
                    exptb.Shoulder = exp.Shoulder;
                    exptb.StudentName = exp.StudentName;
                    exptb.StudentStatus = exp.StudentStatus;
                    exptb.TeacherName = exp.TeacherName;
                    exptb.TeacherNumber = exp.TeacherNumber;

                    jszxDataManager.SaveExpRecord(exptb);

                }
            }
        }
        #endregion

        #region 获取查询的记录信息 
        public PageRecord GetPageRecords(SearchRecordParam pm)
        {
            PageRecord pageRecord = new PageRecord();
            using (JszxDataManager jszxDataManager = new JszxDataManager())
            {
               PageExpRecord _pageRecord = jszxDataManager.GetPageExpRecords(pm.term,pm.lab,pm.week,pm.weekday,pm.cls,pm.teacherNum,pm.page,pm.pageSize);
               pageRecord.ExpRecordList = _pageRecord.ExpRecordList;
               pageRecord.Page = _pageRecord.Page;
               pageRecord.Pages = _pageRecord.Pages;
               pageRecord.PageSize = _pageRecord.PageSize;
            }
            return pageRecord;
        }
        #endregion

        #region 获取学期列表
        public List<Term> GetTermList(){
            List<Term> terms = new List<Term>();
            using (JszxDataManager jszxDataManager = new JszxDataManager())
            {
                List<terms_tb> termtbs = jszxDataManager.GetTerms();
                foreach(var tmtb in termtbs){
                    Term tm = new Term();
                    tm.TermID = tmtb.TermID;
                    tm.TermIndex = tmtb.TermIndex;
                    tm.TermIsUse = tmtb.TermIsUse;
                    tm.TermStartDay = tmtb.TermStartDay;
                    tm.TermWeeks = tmtb.TermWeeks;
                    tm.TermYear = tmtb.TermYear;
                    terms.Add(tm);
                }
            }
            return terms;
        }
        #endregion
    }
}
