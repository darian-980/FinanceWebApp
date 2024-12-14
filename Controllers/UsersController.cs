using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;

namespace GroupProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            //return View();
            return RedirectToAction("Register"); // use register instead
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Gender,City,UserStatus")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Gender,City,UserStatus")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        // GET: Users/Search
        public IActionResult Search()
        {
            //return View();
            return RedirectToAction("SearchAjaxView"); // use search ajax instead
        }

        // POST: Users/Search
        [HttpPost]
        public async Task<IActionResult> Search(int? userId, string name)
        {
            var users = _context.Users.AsQueryable();

            if (userId.HasValue)
            {
                users = users.Where(u => u.UserId == userId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            return View("Index", await users.ToListAsync());
        }

        // Action to render the SearchAjax.cshtml view
        public IActionResult SearchAjaxView()
        {
            return View("SearchAjax"); // 
        }

        [HttpGet]
        public JsonResult SearchAjax(string searchBy, string search)
        {
            IEnumerable<User> users;

            if (string.IsNullOrEmpty(search))
            {
                users = _context.Users.ToList();
            }
            else
            {
                switch (searchBy)
                {
                    case "Name":
                        users = _context.Users.Where(x => x.Name.StartsWith(search)).ToList();
                        break;
                    case "UserStatus":
                        users = _context.Users.Where(x => x.UserStatus.StartsWith(search)).ToList();
                        break;
                    case "Gender":
                        users = _context.Users.Where(x => x.Gender.StartsWith(search)).ToList();
                        break;
                    case "City":
                        users = _context.Users.Where(x => x.City.StartsWith(search)).ToList();
                        break;
                    default:
                        users = _context.Users.ToList();
                        break;
                }
            }

            return Json(users);
        }


        /*        public ActionResult OrderByName()
                {
                    var users = from p in _context.Users
                                orderby p.Name ascending
                                select p;
                    return View(users);
                    //return View(db.Products.OrderBy(x=>x.Name).ToList());
                }

                public ActionResult OrderByStatus()
                {
                    var users = from p in _context.Users
                                orderby p.UserStatus ascending
                                select p;
                    return View(users);

                }*/

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,Name,Gender,City,UserStatus,UserName,Email,Password,ConfirmPassword,IsAdmin")] User user, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    // Generate a unique file name for the uploaded photo
                    var fileName = Path.GetFileName(photo.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    // Ensure the 'uploads' directory exists
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadDir);

                    // Save the file to the uploads folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    // Save the relative file path to the Photo property in the User model
                    user.Photo = "/uploads/" + fileName; // Relative path
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }


        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password)
        {
            // Check if user exists with matching username and password
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);


            if (user != null)
            {
                bool isAdmin = user.IsAdmin; //check if user is admin
                // Set session values
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserIsAdmin", user.IsAdmin.ToString()); // change admin boolean to true or false string
                HttpContext.Session.SetString("Name", user.Name.ToString()); // change admin boolean to true or false string


                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
            }

            return View();
        }

        public IActionResult LoggedIn()
        {
            // Check if the user session is set
            var userId = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userId))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult Logout()
        {
   
            HttpContext.Session.Clear();

            //return RedirectToAction("Login");
            return View();
        }





    }
}
