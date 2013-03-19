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
            jszxEntity = new JszxEntities();
        }

        JszxEntities jszxEntity;

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

        #region IDisposable 成员
        public void Dispose()
        {
            jszxEntity.Dispose();
        }
        #endregion
    }
}
