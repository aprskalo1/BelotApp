using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelotApp.Models;
using BelotApp.Data;
using Microsoft.AspNetCore.Authorization;
using BelotApp.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PagedList;

namespace BelotApp.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly NotesDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public GamesController(NotesDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Games
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            ViewData["CurrentFilter"] = searchString;

            var userId = _userManager.GetUserId(User);
            var games = await _context.Games
                .Where(g => g.UserId == userId)
                .ToListAsync();

            if (searchString != null)
            {
                page = 1;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                games = games
                    .Where(g => g.TeamOneName != null && g.TeamOneName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                             || g.TeamTwoName != null && g.TeamTwoName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            var gamesVM = _mapper.Map<List<GameVM>>(games);

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            ViewBag.CurrentFilter = searchString;

            return View(gamesVM.ToPagedList(pageNumber, pageSize));
        }

        //GET: Games/GamesByUserId
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GamesByUserId(string userId)
        {
            var games = await _context.Games
                .Where(g => g.UserId == userId)
                .ToListAsync();

            var gamesVM = _mapper.Map<List<GameVM>>(games);
            return View(gamesVM);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameVM gameVM)
        {
            var game = _mapper.Map<Game>(gameVM);
            if (ModelState.IsValid)
            {
                game.UserId = _userManager.GetUserId(User);
                game.PlayedAt = DateTime.Now;

                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "GameResults", new { gameId = game.Id });

            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var gameVM = _mapper.Map<GameVM>(game);
            return View(gameVM);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameVM gameVM)
        {
            var game = _mapper.Map<Game>(gameVM);

            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    game.UserId = await _context.Games.Where(g => g.Id == id).Select(g => g.UserId).FirstOrDefaultAsync();

                }
                else
                {
                    game.UserId = _userManager.GetUserId(User);
                }
                game.PlayedAt = DateTime.Now;

                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gameVM);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'NotesDbContext.Games'  is null.");
            }
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                var gameResults = _context.GameResults.Where(gr => gr.GameId == id);
                _context.GameResults.RemoveRange(gameResults);

                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SetWinner(int gameId, string winningTeam)
        {
            var game = _context.Games.Find(gameId);
            if (game == null)
            {
                return NotFound();
            }
            game.Winner = winningTeam;
            _context.SaveChanges();
            return Ok();
        }

        private bool GameExists(int id)
        {
            return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
