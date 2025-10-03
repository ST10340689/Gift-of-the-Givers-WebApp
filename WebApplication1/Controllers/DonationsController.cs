using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

public class DonationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DonationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Donations
    public IActionResult Index()
    {
        var donations = _context.Donations
            .OrderByDescending(d => d.DateDonated)
            .ToList();
        return View(donations);
    }

    // GET: /Donations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Donations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string item, int quantity)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var donation = new Donation
        {
            Item = item,
            Quantity = quantity,
            DateDonated = DateTime.Now,
            UserProfileId = userId.Value
        };

        _context.Donations.Add(donation);
        _context.SaveChanges(); // Saves directly to the Azure DB

        return RedirectToAction("Index");
    }
}
