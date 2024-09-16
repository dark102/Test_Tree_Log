using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test_Tree_Log.Models;
using Test_Tree_Log.Service;

namespace Test_Tree_Log.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly  IJournalService journalService;
        private readonly ApplicationDbContext _context;
        public JournalController(IJournalService journalService)
        {
            this.journalService = journalService;
        }
        /// <summary>
        /// getting the entire log according to filters
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ViewsModelExceptionJournal>> GetAllexceptionJournalList([FromQuery][Required] int skip, [FromQuery][Required] int take, [FromBody] JournalFilter filter)
        {
            try
            {
                var list = (await journalService.GetJournaList(skip, take, filter));
                
                return new ViewsModelExceptionJournal
                {
                    count = list.allCount,
                    skip = skip,
                    items = list.list.Select(x => new item { eventID = x.eventID, createdDate = x.createdDate.ToString(), id = x.id }).ToList()
                };
            }
            catch (Exception ex)
            {
                await journalService.SetNewJournaLine(ex, this.HttpContext);
                return StatusCode(500, ex.Message);
            }

        }
        /// <summary>
        /// getting detailed information about the event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ViewsModelExceptionJournalDetails>> GetExceptionJournalbyId(int id)
        {
            try
            {
                var item = await journalService.GetJournaById(id);
                return new ViewsModelExceptionJournalDetails
                {
                    id = item.id,
                    createdDate = item.createdDate.ToString(),
                    eventID = item.eventID,
                    text = JsonConvert.SerializeObject(item)
                };
            }
            catch (Exception ex)
            {
                await journalService.SetNewJournaLine(ex, this.HttpContext);
                return StatusCode(500, ex.Message);
            }
            
        }

    }
}
