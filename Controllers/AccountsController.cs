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
    public class AccountsController : ControllerBase
    {
       

        private readonly DataContext _context;
        public AccountsController(DataContext context)
        {
            _context = context;
        }

  

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> SearchAccount(Search account)
        {
            if(account.All ?? false) return await _context.Accounts.ToListAsync();
            return SearchValue(account);
        }

   
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {   
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return null;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAccount(int id, Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            try{
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                if(!_context.Accounts.Any(a => a.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Account>>> DeleteAccount(Search acc)
        {
            List<Account> list = null;
            if(acc.All ?? false){
                list = await _context.Accounts.ToListAsync();
            }else{
                list = SearchValue(acc);
            }
            if (list.Capacity == 0) return NotFound();
            _context.Accounts.RemoveRange(list);
            await _context.SaveChangesAsync();
            return list;
        }

        private List<Account> SearchValue(Search account){
           return _context.Accounts.Where(value =>
                value.Name == (account.Name ?? value.Name) &&
                value.Email == (account.Email ?? value.Email) &&
                value.PhoneNumber == (account.PhoneNumber ?? value.PhoneNumber) &&
                value.Id == (account.Id ?? value.Id) &&
                value.Username == (account.Username ?? value.Username)
            ).ToList();
        }
    }
}