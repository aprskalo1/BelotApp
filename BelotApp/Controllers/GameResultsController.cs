using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BelotApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BelotApp.Controllers
{
    [Authorize]
    public class GameResultsController : Controller
    {
        private readonly NotesDbContext _context;

        public GameResultsController(NotesDbContext context)
        {
            _context = context;
        }

        // GET: GameResults
        public async Task<IActionResult> Index()
        {
            var notesDbContext = _context.GameResults.Include(g => g.Game);
            return View(await notesDbContext.ToListAsync());
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
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id");
            return View();
        }

        // POST: GameResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameId,TeamOneResult,TeamTwoResult,CreatedAt")] GameResult gameResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Id", gameResult.GameId);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,TeamOneResult,TeamTwoResult,CreatedAt")] GameResult gameResult)
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
