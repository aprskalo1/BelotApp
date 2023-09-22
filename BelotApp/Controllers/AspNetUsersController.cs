using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelotApp.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BelotApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;

namespace BelotApp.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class AspNetUsersController : Controller
    {
        private readonly NotesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _Mapper;

        public AspNetUsersController(NotesDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _Mapper = mapper;
            _userManager = userManager;
        }

        // GET: AspNetUsers
        public async Task<IActionResult> Index()
        {
            var aspNetUsers = await _context.AspNetUser.ToListAsync();
            var aspNetUsersVM = _Mapper.Map<List<UsersVM>>(aspNetUsers);
            return View(aspNetUsersVM);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AspNetUser == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            var aspNetUserVM = _Mapper.Map<UsersVM>(aspNetUser);
            return View(aspNetUserVM);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var games = _context.Games.Where(g => g.UserId == id).ToList();
                foreach (var game in games)
                {
                    var gameResults = _context.GameResults.Where(gr => gr.GameId == game.Id).ToList();
                    _context.GameResults.RemoveRange(gameResults);
                }
                _context.Games.RemoveRange(games);
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); 
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            return View();
        }
    }
}
