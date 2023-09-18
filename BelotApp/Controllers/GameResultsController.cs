using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelotApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BelotApp.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BelotApp.Controllers
{
    [Authorize]
    public class GameResultsController : Controller
    {
        private readonly NotesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public GameResultsController(NotesDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: GameResults
        public async Task<IActionResult> Index(int? gameId)
        {
            if (gameId == null || _context.GameResults == null)
            {
                return NotFound();
            }

            var userId = _context.Games
                .Where(g => g.Id == gameId)
                .Select(g => g.UserId)
                .FirstOrDefault();

            if (userId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            TempData["GameId"] = gameId;

            var gameResults = _context.GameResults
                .Include(g => g.Game)
                .Where(g  => g.GameId == gameId);

            return View(await gameResults.ToListAsync());
        }

        // GET: GameResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GameResults == null)
            {
                return NotFound();
            }

            var gameResult = await _context.GameResults
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameResult == null)
            {
                return NotFound();
            }

            return View(gameResult);
        }

        // GET: GameResults/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameResultsVM gameResultsVM)
        {
            var gameResult = _mapper.Map<GameResult>(gameResultsVM);

            if (ModelState.IsValid)
            {
                gameResult.GameId = (int)TempData["GameId"]!;
                gameResult.CreatedAt = DateTime.Now;

                _context.Add(gameResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { gameId = gameResult.GameId });
            }
            return View(gameResult);
        }

        // GET: GameResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GameResults == null)
            {
                return NotFound();
            }

            var gameResult = await _context.GameResults.FindAsync(id);
            if (gameResult == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id", gameResult.GameId);
            return View(gameResult);
        }

        // POST: GameResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,TeamOneResult,TeamTwoResult,Combination,CreatedAt")] GameResult gameResult)
        {
            if (id != gameResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameResultExists(gameResult.Id))
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
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id", gameResult.GameId);
            return View(gameResult);
        }

        // GET: GameResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GameResults == null)
            {
                return NotFound();
            }

            var gameResult = await _context.GameResults
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameResult == null)
            {
                return NotFound();
            }

            return View(gameResult);
        }

        // POST: GameResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GameResults == null)
            {
                return Problem("Entity set 'NotesDbContext.GameResults'  is null.");
            }
            var gameResult = await _context.GameResults.FindAsync(id);
            if (gameResult != null)
            {
                _context.GameResults.Remove(gameResult);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameResultExists(int id)
        {
          return (_context.GameResults?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
