using System.Collections;
using Test_Tree_Log.Models;

namespace Test_Tree_Log.Service
{
    public interface IJournalService
    {
        /// <summary>
        /// Adding an event to the log
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task<ExceptionJournal> SetNewJournaLine(Exception exception, HttpContext httpContext);
        /// <summary>
        /// Getting an event by its Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<ExceptionJournal> GetJournaById(int Id);
        /// <summary>
        /// Getting an List event
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<( List<ExceptionJournal> list, int allCount)> GetJournaList(int skip, int take, JournalFilter filter);
    }
}
