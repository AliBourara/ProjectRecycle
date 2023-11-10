using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectRecycle.Models;
using System.Diagnostics;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectRecycle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context; // 5 -

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //-------------------- Login ---------------------

        public IActionResult Login(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                // User Registered ?
                Company? companyFromDb = _context.Companies.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
                AppUser? userFromDb = _context.AppUsers.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
                if (companyFromDb is null )
                {
                    if(userFromDb is null)
                    {
                        ModelState.AddModelError("LoginEmail", "Email dose not exist !");
                        return View("LoginPage");
                    }
                    else
                    {
                        // Initialize hasher object
                        var hasher = new PasswordHasher<LoginUser>();

                        // verify provided password against hash stored in db
                        var result = hasher.VerifyHashedPassword(loginUser, userFromDb.Password, loginUser.LoginPassword);

                        // result can be compared to 0 for failure
                        if (result == 0)
                        {
                            // handle failure (this should be similar to how "existing email" is handled)
                            ModelState.AddModelError("LoginPassword", "Wrong Password !");
                            return View("LoginPage");
                        }
                        else
                        {
                            switch (userFromDb.Role.ToString())
                            {
                                case "Admin":
                                    HttpContext.Session.SetInt32("adminId", userFromDb.UserId);

                                    return RedirectToAction("Details", "AppUsers", new { id = userFromDb.UserId });
                                case "Consultant":
                                    HttpContext.Session.SetInt32("consultantId", userFromDb.UserId);

                                    return RedirectToAction("Consultant", "AppUsers", new { id = userFromDb.UserId });
                            }

                            
                        }
                    }
                }
            
                else
                {
                    // Initialize hasher object
                    var hasher = new PasswordHasher<LoginUser>();

                    // verify provided password against hash stored in db
                    var result = hasher.VerifyHashedPassword(loginUser, companyFromDb.Password, loginUser.LoginPassword);

                    // result can be compared to 0 for failure
                    if (result == 0)
                    {
                        // handle failure (this should be similar to how "existing email" is handled)
                        ModelState.AddModelError("LoginPassword", "Wrong Password !");
                        return View("LoginPage");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("companyId", companyFromDb.CompanyId);

                        return RedirectToAction("Details","Companies", new { id = companyFromDb.CompanyId });
                    }
                }
            }
            return View("LoginPage");

        }
        public IActionResult LoginPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}