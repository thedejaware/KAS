using System.Diagnostics;
using KAS.CMS.Data;
using Microsoft.AspNetCore.Mvc;
using KAS.CMS.Models;

namespace KAS.CMS.Controllers;

public class DocumentsController: Controller
{
    private readonly ILogger<DocumentsController> _logger;
    private readonly ApplicationDbContext _dbContext;
    
    public DocumentsController(ILogger<DocumentsController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> Create()
    {
        ViewBag.DocumentTypes = _dbContext.DocumentTypes.OrderBy(p => p.Title).ToList();
        ViewBag.Cities = _dbContext.Cities.OrderBy(p=>p.Title).ToList();
        ViewBag.MineTypes = _dbContext.MineTypes.OrderBy(p=>p.Title).ToList();
        ViewBag.StartYears = _dbContext.StartYears.OrderBy(p => p.Title).ToList();
        ViewBag.EndYears = _dbContext.EndYears.OrderBy(p => p.Title).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(IFormFile file, DocumentModel request)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.DocumentTypes = _dbContext.DocumentTypes.OrderBy(p => p.Title).ToList();
            ViewBag.Cities = _dbContext.Cities.OrderBy(p=>p.Title).ToList();
            ViewBag.MineTypes = _dbContext.MineTypes.OrderBy(p=>p.Title).ToList();
            ViewBag.StartYears = _dbContext.StartYears.OrderBy(p => p.Title).ToList();
            ViewBag.EndYears = _dbContext.EndYears.OrderBy(p => p.Title).ToList();
            return View();
        }
        
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create)) 
        {
            file.CopyTo(stream); 
        }

        request.CreatedDate= DateTime.UtcNow;
        request.Filename = file.FileName;
        _dbContext.Documents.Add(request); 
        _dbContext.SaveChanges();
        return RedirectToAction("Index","Home");
    }
    
    [HttpGet("documents/update/{id}")]
    public async Task<IActionResult> Update(long id)
    {
        ViewBag.Id = id;
        ViewBag.DocumentTypes = _dbContext.DocumentTypes.OrderBy(p => p.Title).ToList();
        ViewBag.Cities = _dbContext.Cities.OrderBy(p=>p.Title).ToList();
        ViewBag.MineTypes = _dbContext.MineTypes.OrderBy(p=>p.Title).ToList();
        ViewBag.StartYears = _dbContext.StartYears.OrderBy(p => p.Title).ToList();
        ViewBag.EndYears = _dbContext.EndYears.OrderBy(p => p.Title).ToList();
        var document = _dbContext.Documents.FirstOrDefault(p => p.Id == id);
        
        return View(document);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Update(DocumentModel request)
    {
        var documentInDb = _dbContext.Documents.FirstOrDefault(p=>p.Id == request.Id);
        documentInDb.DocumentType = request.DocumentType;
        documentInDb.MineType = request.MineType;
        documentInDb.City = request.City;
        documentInDb.StartYear = request.StartYear;
        documentInDb.EndYear = request.EndYear;
        _dbContext.Documents.Update(documentInDb);
        _dbContext.SaveChanges();
        return RedirectToAction("Index","Home");
    }
    
}