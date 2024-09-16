using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test_Tree_Log.Models;
using Test_Tree_Log.Service;

namespace Test_Tree_Log.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJournalService _journalService;

        public ChildrenController(ApplicationDbContext context, IJournalService journalService)
        {
            _context = context;
            _journalService = journalService;
        }

        #region Since your swager contains only the methods of adding, deleting and renaming, I comment on the rest

        //// GET: api/GetAllChildren
        ///// <summary>
        ///// List of all Children
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Child>>> GetAllChildren()
        //{
        //    if (_context.child == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _context.child.ToListAsync();
        //}
        //// GET: api/Children/GetChildrenByName?name={name}
        ///// <summary>
        ///// Search for a children by name
        ///// </summary>
        ///// <param name="name">the name of the children </param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult<Child>> GetChildrenByName(string name, int? parentId, int treeId)
        //{
        //    if (_context.child == null)
        //    {
        //        throw new Exception("Entity set 'ApplicationDbContext.child' is null.");
        //    }
        //    var child = await _context.child.FirstOrDefaultAsync(x =>x.parentChildid == parentId && x.parentTreeid == treeId && x.name == name);

        //    if (child == null)
        //    {
        //        throw new Exception($"The entity with the name ={name}, and the parentId={parentId} and the treeId={treeId} was not found");
        //    }

        //    return child;
        //}


        #endregion

        // POST: api/Children/UpdateChildren?childrenId={childrenId}&childrenNewName={childrenNewName}
        /// <summary>
        /// Updating the children
        /// </summary>
        /// <param name="treeId">Children ID</param>
        /// <param name="treeNewName">new children name</param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> RenameChildren(string treeName, int childrenId, string childrenNewName)
        {
            try
            {
                if (_context.child == null)
                {
                    throw new Exception("Entity set 'ApplicationDbContext.child' is null.");
                }
                Child? children = await _context.child.FindAsync(childrenId);
                if (children == null)
                {
                    throw new Exception($"A child childrenId={childrenId} was not found");
                }
                children.name = childrenNewName;

                _context.Entry(children).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _journalService.SetNewJournaLine(ex, this.HttpContext);
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Children/CreateChild?treeName={treeName}
        [HttpPost]
        public async Task<ActionResult<Child>> CreateChild(string treeName, int? parentChildrenId, string childrenName)
        {
            try
            {
                if (_context.child == null || _context.tree == null)
                {
                    throw new Exception("Entity set 'ApplicationDbContext.child'/'ApplicationDbContext.tree'  is null.");
                }
                var tree = await _context.tree.FirstOrDefaultAsync(x => x.name == treeName);
                if (tree == null)
                {
                    throw new Exception($"A tree treeName={treeName} was not found");
                }
                Child child = new Child
                {
                    name = childrenName,
                    treeid = tree.id,
                    children = new List<Child>()
                };
                if(parentChildrenId != null)
                {
                    var parentChildren = await _context.child.FirstOrDefaultAsync(x => x.id == parentChildrenId);
                    if (parentChildren == null)
                    {
                        throw new Exception($"A parentChildren parentChildrenId={parentChildrenId} was not found");
                    }
                    parentChildren.children.Add(child);
                    _context.Entry(parentChildren).State = EntityState.Modified;
                }
                else
                {
                    tree.children.Add(child);
                    _context.Entry(tree).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                return child;
            }
            catch (Exception ex)
            {
               var result =  await _journalService.SetNewJournaLine(ex, this.HttpContext);
                return StatusCode(500, result);
            }
        }

        // POST: api/Children/5
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteChild(string treeName, int? childrenId)
        {
            var child = await _context.child.FindAsync(childrenId);
            if (child == null)
            {
                return NotFound();
            }

            _context.child.Remove(child);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
