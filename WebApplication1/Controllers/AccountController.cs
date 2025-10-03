using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password)
    {
        var user = _context.UserProfiles
            .FirstOrDefault(u => u.Email == email); // Add password check

        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);

            return Redirect("https://webapplication120251003180125-f9c5a0ckbkdtezgw.southafricanorth-01.azurewebsites.net/Home/Index");
        }

        ViewBag.Error = "Invalid email or password";
        return View();
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(string fullName, string email, string role, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Password is required";
            return View();
        }

        // Hash the password before storing it
        var hasher = new PasswordHasher<UserProfile>();
        var newUser = new UserProfile
        {
            FullName = fullName,
            Email = email,
            Role = role,
            PasswordHash = hasher.HashPassword(null, password)
        };

        _context.UserProfiles.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.Id);
        HttpContext.Session.SetString("UserName", newUser.FullName);

        return Redirect("https://webapplication120251003180125-f9c5a0ckbkdtezgw.southafricanorth-01.azurewebsites.net/Home/Index");
    }

}
