using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using MySql.Data.Entity; 

namespace JszxDataModel
{
    public class JszxDataManager : IDisposable
    {

        public JszxDataManager()
        {
            jszxEntity = new jszxEntities();
        }

        jszxEntities jszxEntity;

        #region  获取当前学期
        /// <summary>
        /// 获取当前学期
        /// </summary>
        /// <returns>当前学期</returns>
        public terms_tb GetCurrentTerm()
        {
            return jszxEntity.terms_tb.Where(t => t.TermIsUse == true).FirstOrDefault();
        }
        #endregion


        #region 保存学期
        /// <summary>
        /// 保存学期
        /// </summary>
        /// <param name="tm"></param>
        public int SaveTerm(terms_tb tm){
            //重置当前学期
            if (tm.TermIsUse)
            {
                foreach (terms_tb vtm in jszxEntity.terms_tb.Where(t => t.TermIsUse == true & t.TermID!=tm.TermID))
                {
                    vtm.TermIsUse = false;
                }

            }
            if (tm.TermID == 0)
            {
                //保存新学期
                jszxEntity.terms_tb.Add(tm);
            }
            else
            {
                terms_tb _tm = jszxEntity.terms_tb.Where(t => t.TermID == tm.TermID).FirstOrDefault();
                _tm.TermIndex = tm.TermIndex;
                _tm.TermIsUse = tm.TermIsUse;
                _tm.TermStartDay = tm.TermStartDay;
                _tm.TermWeeks = tm.TermWeeks;
                _tm.TermYear = tm.TermYear;
                
            }
            jszxEntity.SaveChanges();
            return tm.TermID;
        }
        #endregion

        #region 根据指定时间点判断当前课节，返回当前课节信息
        /// <summary>
        /// 根据指定时间点判断当前课节，返回当前课节信息
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>当前课节信息</returns>
        public classtimes_tb GetClsTimeByTimeSpan(TimeSpan ts){
            return jszxEntity.classtimes_tb.Where(ct => ct.ClsTmStart < ts && ct.ClsTmEnd > ts).FirstOrDefault();
        }
        #endregion

        #region 获取指定的实验室、学期、周次、工作日、节次的课表信息
        /// <summary>
        /// 获取指定的实验室、学期、周次、工作日、节次的课表信息
        /// </summary>
        /// <param name="labID">指定的实验室</param>
        /// <param name="term">学期</param>
        /// <param name="week">周次</param>
        /// <param name="weekDay">工作日</param>
        /// <param name="cls">节次</param>
        /// <returns>课表信息</returns>
        public schedule_tb GetScheduleByCls(int labID,int term, int week,int weekDay,int cls)
        {
            return jszxEntity.schedule_tb.Where(sch=>sch.ScdLab==labID && sch.ScdTerm==term && sch.ScdWeek==week&& sch.ScdWeekDay==weekDay && sch.ScdClass==cls ).FirstOrDefault();
        }
        #endregion

        #region 获取指定的实验室、学期、周次、工作日、节次的实验记录
        /// <summary>
        /// 获取指定的实验室、学期、周次、工作日、节次的实验记录
        /// </summary>
        /// <param name="labID">指定的实验室</param>
        /// <param name="term">学期</param>
        /// <param name="week">周次</param>
        /// <param name="weekDay">工作日</param>
        /// <param name="cls">节次</param>
        /// <returns>实验记录</returns>
        public exprecords_tb GetExprecordByCls(int labID, int term, int week, int weekDay, int cls)
        {
            return jszxEntity.exprecords_tb.Where(exp => exp.ExpLabID == labID && exp.ExpTerm == term && exp.ExpWeek == week && exp.ExpWeekDay == weekDay && exp.ExpCls == cls).FirstOrDefault();
        }
        #endregion

        #region 获取弹出时间列表
        /// <summary>
        /// 获取弹出时间列表
        /// </summary>
        /// <param name="orderFlag">是否升序排列，true 为升序，否则为降序</param>
        /// <returns>排序后的弹出时间列表</returns>
        public List<poptimes_tb> GetPopTimes(bool orderFlag)
        {
            if (orderFlag)
            {
                return jszxEntity.poptimes_tb.OrderBy(p => p.PopTime).ToList();
            }
            else
            {
                return jszxEntity.poptimes_tb.OrderByDescending(p=>p.PopTime).ToList();
            }
        }
        #endregion

        #region 获取课节时间
        /// <summary>
        /// 获取课节时间
        /// </summary>
        /// <param name="orderFlag">是否升序排列，true 为升序，否则为降序</param>
        /// <returns>排序后的课节时间列表</returns>
        public List<classtimes_tb> GetClassTimes(bool orderFlag)
        {
            if (orderFlag)
            {
                return jszxEntity.classtimes_tb.OrderBy(ct => ct.ClsTmIndex).ToList();
            }
            else
            {
                return jszxEntity.classtimes_tb.OrderByDescending(ct => ct.ClsTmIndex).ToList();
            }
        }
        #endregion

        #region 获取实验室列表
        public List<labs_tb> GetLabs_tbList()
        {
            return jszxEntity.labs_tb.OrderBy(lb=>lb.LabID).ToList();
        }
        #endregion

        #region 查询指定学期周次工作日节次的记录
        /// <summary>
        /// 获取实验机录
        /// </summary>
        /// <returns></returns>
        public List<exprecords_tb> GetExpRecords()
        {
            return jszxEntity.exprecords_tb.ToList();
        }
        /// <summary>
        /// 获取实验记录，指定学期
        /// </summary>
        /// <param name="term">指定学期ID</param>
        /// <returns></returns>
        public List<exprecords_tb> GetExpRecords(int term)
        {
            return jszxEntity.exprecords_tb.Where(ep=>ep.ExpTerm==term).OrderByDescending(ep=>ep.PostTime).ToList();
        }
        /// <summary>
        /// 获取实验记录
        /// </summary>
        /// <param name="term">指定学期ID</param>
        /// <param name="lab">实验室ID</param>
        /// <returns></returns>
        public List<exprecords_tb> GetExpRecords(int term, int lab)
        {
            return jszxEntity.exprecords_tb.Where(ep => ep.ExpTerm == term&&ep.ExpLabID==lab).OrderByDescending(ep => ep.PostTime).ToList();
        }
        /// <summary>
        /// 获取实验记录
        /// </summary>
        /// <param name="term">指定学期ID</param>
        /// <param name="lab">指定实验室ID</param>
        /// <param name="week">指定周次</param>
        /// <returns></returns>
        public List<exprecords_tb> GetExpRecords(int term, int lab,int week)
        {
            return jszxEntity.exprecords_tb.Where(ep => ep.ExpTerm == term && ep.ExpLabID == lab && ep.ExpWeek==week).OrderByDescending(ep => ep.PostTime).ToList();
        }
        /// <summary>
        /// 获取实验记录
        /// </summary>
        /// <param name="term">指定学期ID</param>
        /// <param name="lab">指定实验室ID</param>
        /// <param name="week">指定周次</param>
        /// <param name="weekDay">指定工作日</param>
        /// <returns></returns>
        public List<exprecords_tb> GetExpRecords(int term, int lab, int week,int weekDay)
        {
            return jszxEntity.exprecords_tb.Where(ep => ep.ExpTerm == term && ep.ExpLabID == lab && ep.ExpWeek == week && ep.ExpWeekDay==weekDay).OrderByDescending(ep => ep.PostTime).ToList();
        }
        /// <summary>
        /// 获取实验记录
        /// </summary>
        /// <param name="term">指定学期ID</param>
        /// <param name="lab">指定实验室ID</param>
        /// <param name="week">指定周次</param>
        /// <param name="weekDay">指定的工作日</param>
        /// <param name="cls">指定的节次</param>
        /// <returns></returns>
        public List<exprecords_tb> GetExpRecords(int term, int lab, int week, int weekDay,int cls)
        {
            return jszxEntity.exprecords_tb.Where(ep => ep.ExpTerm == term && ep.ExpLabID == lab && ep.ExpWeek == week && ep.ExpWeekDay == weekDay && ep.ExpCls == cls).OrderByDescending(ep => ep.PostTime).ToList();
        }
        #endregion

        #region 保存实验记录
        public long SaveExpRecord(exprecords_tb exp)
        {
            if (exp.RecordID == null || exp.RecordID == 0)
            {
                //新添加
                jszxEntity.exprecords_tb.Add(exp);
            }
            else
            {
                //x更新
                exprecords_tb _exp = jszxEntity.exprecords_tb.Where(er => er.RecordID == exp.RecordID).FirstOrDefault();
                _exp.CourseName = exp.CourseName;
                _exp.ExpClasses = exp.ExpClasses;
                _exp.ExpCls = exp.ExpCls;
                _exp.ExpDate = exp.ExpDate;
                _exp.ExpLab = exp.ExpLab;
                _exp.ExpLabID = exp.ExpLabID;
                _exp.ExpName = exp.ExpName;
                _exp.ExpTerm = exp.ExpTerm;
                _exp.ExpWeek = exp.ExpWeek;
                _exp.ExpWeekDay = exp.ExpWeekDay;
                _exp.Groups = exp.Groups;
                _exp.InstrumentStatus = exp.InstrumentStatus;
                _exp.PerGroup = exp.PerGroup;
                _exp.PostTime = exp.PostTime;
                _exp.Problems = exp.Problems;
                _exp.Realizer = exp.Realizer;
                _exp.Shoulder = exp.Shoulder;
                _exp.StudentName = exp.StudentName;
                _exp.StudentStatus = exp.StudentStatus;
                _exp.TeacherName = exp.TeacherName;
                _exp.TeacherNumber = exp.TeacherNumber;

            }

            jszxEntity.SaveChanges();
            return exp.RecordID;

        }
        #endregion

        #region IDisposable 成员
        public void Dispose()
        {
            jszxEntity.Dispose();
        }
        #endregion
    }
}
