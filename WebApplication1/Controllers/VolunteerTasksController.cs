using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

public class VolunteerTasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public VolunteerTasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /VolunteerTasks
    public IActionResult Index()
    {
        var tasks = _context.VolunteerTasks
            .OrderBy(t => t.ScheduledDate)
            .ToList();
        return View(tasks);
    }

    // GET: /VolunteerTasks/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /VolunteerTasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string taskName, DateTime scheduledDate)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var task = new VolunteerTask
        {
            TaskName = taskName,
            ScheduledDate = scheduledDate,
            UserProfileId = userId.Value
        };

        _context.VolunteerTasks.Add(task);
        _context.SaveChanges(); // Saves directly to the Azure DB

        return RedirectToAction("Index");
    }
}
