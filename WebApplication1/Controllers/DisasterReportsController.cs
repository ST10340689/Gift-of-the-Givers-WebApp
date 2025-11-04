using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

public class DisasterReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DisasterReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /DisasterReports
    public IActionResult Index()
    {
        var reports = _context.DisasterReports
            .OrderByDescending(r => r.ReportDate)
            .ToList();
        return View(reports);
    }

    // GET: /DisasterReports/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /DisasterReports/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string disasterType, string location, string description)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var newReport = new DisasterReport
        {
            DisasterType = disasterType,
            Location = location,
            Description = description,
            ReportDate = DateTime.Now,
            UserProfileId = userId.Value
        };

        _context.DisasterReports.Add(newReport);
        _context.SaveChanges(); // Saves to the Azure DB

        // Redirects back to the reports list
        return RedirectToAction("Index");
    }
}
