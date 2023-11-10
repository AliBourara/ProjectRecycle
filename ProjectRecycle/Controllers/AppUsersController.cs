using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectRecycle.Models;

namespace ProjectRecycle.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly MyContext _context;

        public AppUsersController(MyContext context)
        {
            _context = context;
        }

        // GET: AppUsers
        public async Task<IActionResult> Index()
        {
              return _context.AppUsers != null ? 
                          View(await _context.AppUsers.ToListAsync()) :
                          Problem("Entity set 'MyContext.AppUsers'  is null.");
        }

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }
        //----------------------------------------------------------------Start Consultant Pages--------------------------------------------->

        // GET: AppUsers/Consultant/5
        [HttpGet("AppUsers/Consultant/{id}")]
        public async Task<IActionResult> Consultant(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }


        // GET: AppUsers/ConsultWastes/5
        [HttpGet("AppUsers/ConsultWastes/{id}")]
        public async Task<IActionResult> ConsultWaste(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .Include(u=>u.Missions).ThenInclude(u=>u.Waste).ThenInclude(u=>u.Offers)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }
        // GET: AppUsers/ConsultOffre/5
        [HttpGet("AppUsers/ConsultOffre/{id}")]
        public async Task<IActionResult> ConsultOffre(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        //----------------------------------------------------------------Ending Consultant Pages--------------------------------------------->

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Register(AppUser newUser)
        {
            if (ModelState.IsValid)
            {
                // Email Exist ?
                if (_context.Users.Any(u => u.Email == newUser.Email))
                {
                    // True
                    ModelState.AddModelError("Email", "Email already in use .");
                    return View("LoginPage");
                }
                else
                {
                    // False
                    // 1 - Hash Password
                    PasswordHasher<AppUser> Hasher = new PasswordHasher<AppUser>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    // 2 - Add 
                    _context.Add(newUser);
                    // 3 - Save
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userId", newUser.UserId);
                    HttpContext.Session.SetString("username", newUser.FirstName);
                    // HttpContext.Session.
                    return RedirectToAction("Index");
                }
            }

            return View("LoginPage");
        }
       
        //-------------------- Logout ---------------------

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }

        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Email,Password,Role,Diploma,Expertise,Description,CreatedAt,UpdatedAt")] AppUser appUser)
        {
            if (id != appUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.UserId))
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
            return View(appUser);
        }


        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AppUsers == null)
            {
                return Problem("Entity set 'MyContext.AppUsers'  is null.");
            }
            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser != null)
            {
                _context.AppUsers.Remove(appUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
          return (_context.AppUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
