using System.Diagnostics;
using KAS.CMS.Data;
using Microsoft.AspNetCore.Mvc;
using KAS.CMS.Models;

namespace KAS.CMS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        ViewBag.Documents = GetDocuments();
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(long id)
    {
        // Retrieve the form from the database
        var form = _dbContext.Documents.FirstOrDefault(p=>p.Id == id);

        if (form == null)
        {
            return NotFound();
        }

        // Delete the form
        _dbContext.Documents.Remove(form);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Update(long id)
    {
        return RedirectToAction("Update","Documents", new { id = id });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private List<DocumentViewModel> GetDocuments()
    {
        return (from doc in _dbContext.Documents
            join docType in _dbContext.DocumentTypes on doc.DocumentType equals docType.Id
            join mineType in _dbContext.MineTypes on doc.MineType equals mineType.Id
            join city in _dbContext.Cities on doc.City equals city.Id
            join sYear in _dbContext.StartYears on doc.StartYear equals sYear.Id
            join eYear in _dbContext.EndYears on doc.EndYear equals eYear.Id
            select new DocumentViewModel
            {
                Id = doc.Id,
                DocumentType = docType.Title,
                MineType = mineType.Title,
                City = city.Title,
                StartYear = sYear.Title,
                EndYear = eYear.Title,
                CreatedDate = doc.CreatedDate.ToString("dd.MM.yyyy HH:mm:ss")
            }).OrderBy(p=>p.Id).ToList();
    }
}