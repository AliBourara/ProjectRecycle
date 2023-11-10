using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectRecycle.Models;

namespace ProjectRecycle.Controllers
{
    public class MissionsController : Controller
    {
        private readonly MyContext _context;

        public MissionsController(MyContext context)
        {
            _context = context;
        }

        // GET: Missions
        public async Task<IActionResult> Index()
        {
            var myContext = _context.Missions.Include(m => m.Waste);
            return View(await myContext.ToListAsync());
        }

        // GET: Missions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions
                .Include(m => m.Waste)
                .FirstOrDefaultAsync(m => m.MissionId == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // GET: Missions/Create
        public IActionResult Create()
        {
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId");
            return View();
        }

        // POST: Missions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MissionId,WasteId,UserId,Status,Message,CreatedAt,UpdatedAt")] Mission mission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId", mission.WasteId);
            return View(mission);
        }

        // GET: Missions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId", mission.WasteId);
            return View(mission);
        }

        // POST: Missions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MissionId,WasteId,UserId,Status,Message,CreatedAt,UpdatedAt")] Mission mission)
        {
            if (id != mission.MissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MissionExists(mission.MissionId))
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
            ViewData["WasteId"] = new SelectList(_context.Wastes, "WasteId", "WasteId", mission.WasteId);
            return View(mission);
        }

        // GET: Missions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Missions == null)
            {
                return NotFound();
            }

            var mission = await _context.Missions
                .Include(m => m.Waste)
                .FirstOrDefaultAsync(m => m.MissionId == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // POST: Missions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Missions == null)
            {
                return Problem("Entity set 'MyContext.Missions'  is null.");
            }
            var mission = await _context.Missions.FindAsync(id);
            if (mission != null)
            {
                _context.Missions.Remove(mission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MissionExists(int id)
        {
          return (_context.Missions?.Any(e => e.MissionId == id)).GetValueOrDefault();
        }
    }
}
