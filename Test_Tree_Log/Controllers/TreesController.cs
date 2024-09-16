using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Test_Tree_Log.Models;
using Test_Tree_Log.Service;
using static System.Net.Mime.MediaTypeNames;

namespace Test_Tree_Log.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TreesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJournalService _journalService;

        public TreesController(ApplicationDbContext context, IJournalService journalService)
        {
            try
            {
                _context = context;
                _journalService = journalService;
            }
            catch (Exception ex)
            {
                journalService.SetNewJournaLine(ex, this.HttpContext);
            }
        }
        
        //// GET: api/Trees/GetTreeByName?name={name}
        ///// <summary>
        ///// Search for a tree by name
        ///// </summary>
        ///// <param name="name">the name of the tree</param>
        ///// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Tree>> GetTreeByName(string treeName)
        {
            try
            {
                Tree tree = await _context.tree.FirstOrDefaultAsync(x => x.name == treeName);
                if (tree == null)
                {
                    return NotFound($"Tree with name {treeName} not found");
                }
                return tree;
            }
            catch (Exception ex)
            {
                await _journalService.SetNewJournaLine(ex, this.HttpContext);
                return StatusCode(500, ex.Message);
            }
        }

        #region Since your swager contains only the method of obtaining a tree, I comment on the rest
        
        // GET: api/Trees/GetAllTree
        /// <summary>
        /// List of all trees
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Tree>>> GetAllTree()
        //{
        //    if (_context.tree == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _context.tree.ToListAsync();
        //}

        //// POST: api/Trees/UpdateTree?treeId={treeId}&treeNewName={treeNewName}
        ///// <summary>
        ///// Updating the tree
        ///// </summary>
        ///// <param name="treeId">Tree ID</param>
        ///// <param name="treeNewName">new tree name</param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> UpdateTree(int treeId, string treeNewName)
        //{
        //    try
        //    {
        //        Tree? tree = await _context.tree.FindAsync(treeId);
        //        if (tree == null)
        //        {
        //            return BadRequest();
        //        }
        //        tree.name = treeNewName;

        //        _context.Entry(tree).State = EntityState.Modified;

        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _journalService.SetNewJournaLine(ex, this.HttpContext);
        //        return StatusCode(500, ex.Message);
        //    }

        //}

        //// POST: api/Trees/AddTree?treeNewName={treeName}
        ///// <summary>
        ///// Adding a tree
        ///// </summary>
        ///// <param name="treeName">Name of the new tree</param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<ActionResult<Tree>> AddTree(string treeName)
        //{
        //    try
        //    {
        //        Tree tree = new Tree { name = treeName };
        //        _context.tree.Add(tree);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _journalService.SetNewJournaLine(ex, this.HttpContext);
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //// POST: api/Trees/DeleteTree?id={id}
        ///// <summary>
        ///// Deleting a tree
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> DeleteTree(int id)
        //{
        //    try
        //    {
        //        var tree = await _context.tree.FindAsync(id);
        //        if (tree == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.tree.Remove(tree);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _journalService.SetNewJournaLine(ex, this.HttpContext);
        //        return StatusCode(500, ex.Message);
        //    }
        //}
        #endregion
    }
}