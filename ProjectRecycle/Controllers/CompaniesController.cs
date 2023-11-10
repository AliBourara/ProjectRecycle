using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectRecycle.Models;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectRecycle.Controllers
{
    //[Area("Company")]
    public class CompaniesController : Controller
    {
        private readonly MyContext _context;
        private int companyId;

        public CompaniesController(MyContext context)
        {
            _context = context;
        }

        // GET: Companies
        [HttpGet("companies")]
        public async Task<IActionResult> Index()
        {
              return _context.Companies != null ? 
                          View(await _context.Companies.ToListAsync()) :
                          Problem("Entity set 'MyContext.Companies'  is null.");
        }

        // GET: Companies/Details/5
        [HttpGet("Companies/Details/{companyId}")]
        public async Task<IActionResult> Details(int? companyId)
        {
            if (companyId == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == companyId);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
        // GET: Companies/Dashboard/5
        [HttpGet("Companies/Dashboard/{id}")]

        public async Task<IActionResult> Dashboard(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
        .Include(c => c.CompanyWastes) 
        .Include(c => c.Bids) 
        .FirstOrDefaultAsync(m => m.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }


            int numberOfWastes = company.CompanyWastes.Count;
            int numberOfBids = company.Bids.Count;
            int numberOfOffers = company.CompanyWastes.SelectMany(waste => waste.Offers).Count();

            ViewBag.NumberOfWastes = numberOfWastes;
            ViewBag.NumberOfBids = numberOfBids;
            ViewBag.NumberOfOffers = numberOfOffers;

            return View(company);
        }
        // GET: Companies/WasteCreate/5
        [HttpGet("Companies/WasteCreate/{companyId}")]
        public async Task<IActionResult> WasteCreate(int? companyId)
        {
            if (companyId == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
        .Include(c => c.CompanyWastes)
        .FirstOrDefaultAsync(m => m.CompanyId == companyId);

            if (company == null)
            {
                return NotFound();
            }


           

            return View(company);
        }
        // GET: Companies/Wastes/5
        [HttpGet("Companies/Wastes/{companyId}")]

        public async Task<IActionResult> Wastes(int? companyId)
        {
            if (companyId == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
        .Include(c => c.CompanyWastes) 
        .Include(c => c.Bids) 
        .FirstOrDefaultAsync(m => m.CompanyId == companyId);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }// GET: Companies/Bids/5
        [HttpGet("Companies/Bids/{companyId}")]

        public async Task<IActionResult> Bids(int? companyId)
        {
            if (companyId == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.Bids)
                .FirstOrDefaultAsync(m => m.CompanyId == companyId);

            if (company == null)
            {
                return NotFound();
            }



            var orderedBids = company.Bids.OrderByDescending(b => b.CreatedAt).DistinctBy(bid => bid.OfferId).ToList();
            foreach (var item in orderedBids)
            {
                var offer = _context.Offers.Include(o=>o.Waste).FirstOrDefaultAsync(o => o.OfferId == item.OfferId).Result;
                if (offer!=null) item.Offer = offer ;
            }

            company.Bids = orderedBids;
            //company.Bids = company.Bids.DistinctBy(bid => bid.OfferId).ToList();

            return View(company);
        }

        // GET: Companies/Create
        [HttpGet("Companies/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Register(Company newCompany)
        {
            if (ModelState.IsValid)
            {
                // Email Exist ?
                if (_context.Companies.Any(u => u.Email == newCompany.Email))
                {
                    // True
                    ModelState.AddModelError("Email", "Email already in use .");
                    return View("Index");
                }
                else
                {
                    // False
                    // 1 - Hash Password
                    PasswordHasher<Company> Hasher = new PasswordHasher<Company>();
                    newCompany.Password = Hasher.HashPassword(newCompany, newCompany.Password);
                    // 2 - Add 
                    _context.Add(newCompany);
                    // 3 - Save
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("companyId", newCompany.CompanyId);
                    HttpContext.Session.SetString("companyName", newCompany.Name);
                    // HttpContext.Session.
                    return RedirectToAction("Details", new { id = newCompany.CompanyId });
                }
            }
            return View("Create");
        }
        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetInt32("companyId") == null)
            {
                return RedirectToAction("Index");
            }
            int? CompanyId = (int)HttpContext.Session.GetInt32("companyId");
            Company? company = _context.Companies.FirstOrDefault(u => u.CompanyId == companyId);
            return View(company);
        }
        

       

        [HttpPost]
        public async Task<IActionResult> CreateWaste(Waste waste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(waste);
                await _context.SaveChangesAsync();
                return RedirectToAction("Wastes", new { id = waste.CompanyId });

            }
            return RedirectToAction("Details", new { id = waste.CompanyId });
        }

        // GET: Companies/Edit/5
        [HttpGet("companies/{companyId}/edit")]
        public async Task<IActionResult> Edit(int? companyId)
        {
            if (companyId == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = company.CompanyId }); ;
            }
            return RedirectToAction("Details", new { id = company.CompanyId }); ;
        }
        

        // GET: Companies/Delete/5
        [HttpGet("companies/{id}/delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'MyContext.Companies'  is null.");
            }
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return (_context.Companies?.Any(e => e.CompanyId == id)).GetValueOrDefault();
        }
    }
}
