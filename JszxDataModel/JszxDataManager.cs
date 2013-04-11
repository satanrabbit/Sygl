using System;
using System.Collections.Generic;
using System.Linq;

namespace JszxDataModel
{
    public class JszxDataManager : IDisposable
    {

        public JszxDataManager()
        {
            jszxEntity = new jszxEntities();
        }

        public jszxEntities jszxEntity;

        #region  获取当前学期
        /// <summary>
        /// 获取当前学期
        /// </summary>
        /// <returns>当前学期</returns>
        public terms_tb GetCurrentTerm()
        {
            return jszxEntity.terms_tb.Where(t => t.TermIsUse == true).FirstOrDefault();
        }
        /// <summary>
        /// 获取指定的学期
        /// </summary>
        /// <param name="termID"></param>
        /// <returns></returns>
        public terms_tb GetCurrentTerm(int termID)
        {
            return jszxEntity.terms_tb.Where(t => t.TermID == termID).FirstOrDefault();
        }
        #endregion
        #region 获取学期列表
        public List<terms_tb> GetTerms()
        {
            return jszxEntity.terms_tb.OrderByDescending(tm=>tm.TermID).ToList();
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
        /// <summary>
        /// 分页获取实验记录
        /// </summary>
        /// <param name="term"></param>
        /// <param name="lab"></param>
        /// <param name="week"></param>
        /// <param name="weekDay"></param>
        /// <param name="cls"></param>
        /// <param name="teacherNum"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageExpRecord GetPageExpRecords(int? term, int? lab, int? week, int? weekDay, int? cls, string teacherNum, int? page, int? pageSize)
        {
            var exps = jszxEntity.exprecords_tb.Where(ep => ep.RecordID >0);
            if (term != null)
            {
                exps = exps.Where(ep => ep.ExpTerm == term);
            }
            if (lab != null)
            {
                exps = exps.Where(ep => ep.ExpLabID == lab);
            }
            if (week != null)
            {
                exps = exps.Where(ep => ep.ExpWeek == week);
            }
            if (weekDay != null)
            {
                exps = exps.Where(ep => ep.ExpWeekDay == weekDay);
            }
            if (cls != null)
            {
                exps = exps.Where(ep => ep.ExpCls == cls);
            }
            if (teacherNum != null)
            {
                exps = exps.Where(ep => ep.TeacherNumber == teacherNum);
            }
            int pageCount = exps.Count();
            if (page == null)
            {
                page = 0;
            }
            if (pageSize == null)
            {
                pageSize = 20;
            }
            PageExpRecord pageRecord = new PageExpRecord();
            pageRecord.Page = (int)page;
            pageRecord.Pages = pageCount;
            pageRecord.PageSize = (int)pageSize;
            pageRecord.ExpRecordList = exps.OrderByDescending(p => p.PostTime).Skip((int)pageSize * (int)page).Take((int)pageSize).ToList();
            return pageRecord;
        }
        #endregion

        #region 保存实验记录
        public long SaveExpRecord(exprecords_tb exp)
        {
            if (exp.RecordID == 0)
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

        #region 删除指定的弹出时间
        /// <summary>
        /// 删除指定的弹出时间
        /// </summary>
        /// <param name="popID">指定弹出时间的ID</param>
        public void DeletePopTime( int popID)
        {
            jszxEntity.poptimes_tb.Remove(jszxEntity.poptimes_tb.Where(p=>p.PopTimeID==popID).FirstOrDefault());
            jszxEntity.SaveChanges();
        }
        #endregion
        #region 保存弹出时间
        /// <summary>
        /// 保存弹出时间设置
        /// </summary>
        /// <param name="pop">待保存的弹出时间</param>
        /// <returns>弹出时间的ID</returns>
        public int SavePopTime(poptimes_tb pop)
        {
            if (pop.PopTimeID == 0)
            {
                //新添加的PopTime
                jszxEntity.poptimes_tb.Add(pop);
            }
            else
            {
                //修改
                poptimes_tb _pop = jszxEntity.poptimes_tb.Where(pt => pt.PopTimeID == pop.PopTimeID).FirstOrDefault();
                _pop.PopTime = pop.PopTime;
            }
            jszxEntity.SaveChanges();
            return pop.PopTimeID;
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
