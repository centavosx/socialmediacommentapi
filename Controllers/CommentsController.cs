using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TRYWEBAPI.Data;
using TRYWEBAPI.Models;
namespace TRYWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
       

        private readonly DataContext _context;
        public CommentsController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult ShowComment(SearchComments show)
        {
            if(show.All ?? false){
               return Ok(commentTree(_context.Comments.Join(_context.Accounts, c=>c.AccountId, acc=>acc.Id,
                    (c, acc) => new ShowComments{
                        Id = c.Id,
                        Name = acc.Name,
                        Username = acc.Username,
                        Message = c.Message,
                        Date = c.Date,
                        ReplyToId = c.ReplyTo
                    }
                    ).ToList()));
            } 
            return Ok(commentTree(_context.Comments.Join(_context.Accounts, c=>c.AccountId, acc=>acc.Id,
                        (c, acc) => new ShowComments{
                            Id = c.Id,
                            Name = acc.Name,
                            Username = acc.Username,
                            Message = c.Message,
                            Date = c.Date,
                            ReplyToId = c.ReplyTo
                        }
                    ).Where(val=>
                        (val.Id == (show.Id ?? val.Id) ||
                        val.ReplyToId == show.Id) &&
                        val.Name == (show.Name ?? val.Name) &&
                        val.Username == (show.Username ?? val.Username) &&
                        val.Message == (show.Message ?? val.Message) &&
                        val.Date == (show.Date ?? val.Date))
                    .ToList()));
        }

   
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {   
            if(comment.ReplyTo>0) 
                if(_context.Comments.Where((e)=>e.Id == comment.ReplyTo && e.ReplyTo<=0).ToList().Capacity == 0) return NotFound();
                else;
            else comment.ReplyTo = 0; 
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return null;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditComment(int id, Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            try{
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                if(!_context.Comments.Any(a => a.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Comment>>> DeleteComment(SearchComments comment)
        {
            List<Comment> list = null;
            if(comment.All ?? false){
                list = await _context.Comments.ToListAsync();
            }else{
                list = SearchValue(comment);
            }
            if (list.Capacity == 0) return NotFound();
            _context.Comments.RemoveRange(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public List<Comment> SearchValue(SearchComments comments){
            return _context.Comments.Where(value =>
                value.Message== (comments.Message ?? value.Message) &&
                value.Id == (comments.Id ?? value.Id) &&
                value.AccountId == (comments.AccountId ?? value.AccountId) &&
                value.Date == (comments.Date ?? value.Date)
            ).ToList();
        }

        public List<ShowComments> commentTree(List<ShowComments> value){
            List<ShowComments> l = new List<ShowComments>();
            value.ForEach((val)=>{
                if(val.ReplyToId<=0){
                    int id = val.Id;
                    List<ShowComments> x = value.FindAll((e)=>e.ReplyToId == id);
                    val.Replies = x;
                    l.Add(val);
                } 
            });
            return l;
        }
    }
}