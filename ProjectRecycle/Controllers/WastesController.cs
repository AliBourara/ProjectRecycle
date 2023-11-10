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
    public class WastesController : Controller
    {
        private readonly MyContext _context;

        public WastesController(MyContext context)
        {
            _context = context;
        }

        // GET: Wastes
        public async Task<IActionResult> Index()
        {

              return _context.Wastes != null ? 
                          View(await _context.Wastes.ToListAsync()) :
                          Problem("Entity set 'MyContext.Wastes'  is null.");
        }

        // GET: Wastes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wastes == null)
            {
                return NotFound();
            }

            var waste = await _context.Wastes
                .FirstOrDefaultAsync(m => m.WasteId == id);
            //Waste wasteTest = _context.Wastes.FirstOrDefault(mission => mission.Type == "Metal");

            if (waste == null)
            {
                return NotFound();
            }

            return View(waste);
        }

        // GET: Wastes/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Waste waste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(waste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(waste);
        }

        // GET: Wastes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wastes == null)
            {
                return NotFound();
            }

            var waste = await _context.Wastes.FindAsync(id);
            if (waste == null)
            {
                return NotFound();
            }
            return View(waste);
        }

        // POST: Wastes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id,Waste waste)
        {
            if (id != waste.WasteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WasteExists(waste.WasteId))
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
            return View(waste);
        }

        // GET: Wastes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wastes == null)
            {
                return NotFound();
            }

            var waste = await _context.Wastes
                .FirstOrDefaultAsync(m => m.WasteId == id);
            if (waste == null)
            {
                return NotFound();
            }

            return View(waste);
        }

        // POST: Wastes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wastes == null)
            {
                return Problem("Entity set 'MyContext.Wastes'  is null.");
            }
            var waste = await _context.Wastes.FindAsync(id);
            if (waste != null)
            {
                _context.Wastes.Remove(waste);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WasteExists(int id)
        {
          return (_context.Wastes?.Any(e => e.WasteId == id)).GetValueOrDefault();
        }
    }
}
