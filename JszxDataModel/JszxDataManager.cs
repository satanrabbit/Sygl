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

        #region IDisposable 成员
        public void Dispose()
        {
            jszxEntity.Dispose();
        }
        #endregion
    }
}
