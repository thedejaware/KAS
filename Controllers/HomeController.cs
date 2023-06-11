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
        var filterViewModel = GetFilters();
        return View(filterViewModel);
    }
    
    [HttpGet("Filter")]
    public IActionResult Index(int? docTypeId= null, int? mineTypeId = null, int? cityId = null)
    {
        ViewBag.Documents = FilterDocuments(docTypeId, mineTypeId, cityId);
        var filterViewModel = GetFilters();
        return View(filterViewModel);
    }
    
    public IActionResult Login()
    {
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

    public IActionResult DownloadFile(string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

        if (System.IO.File.Exists(filePath))
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        return NotFound();
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
                Filename = doc.Filename,
                DocumentType = docType.Title,
                MineType = mineType.Title,
                City = city.Title,
                StartYear = sYear.Title,
                EndYear = eYear.Title,
                CreatedDate = doc.CreatedDate.ToString("dd.MM.yyyy HH:mm:ss")
            }).OrderBy(p=>p.Id).ToList();
    }
    
    private List<DocumentViewModel> FilterDocuments(int? docTypeId, int? mineTypeId, int? cityId)
    {
        return (from doc in _dbContext.Documents
            join docType in _dbContext.DocumentTypes on doc.DocumentType equals docType.Id
            join mineType in _dbContext.MineTypes on doc.MineType equals mineType.Id
            join city in _dbContext.Cities on doc.City equals city.Id
            join sYear in _dbContext.StartYears on doc.StartYear equals sYear.Id
            join eYear in _dbContext.EndYears on doc.EndYear equals eYear.Id
            where (!docTypeId.HasValue || docType.Id == docTypeId.Value)
                  && (!mineTypeId.HasValue || mineType.Id == mineTypeId.Value)
                  && (!cityId.HasValue || city.Id == cityId.Value)
            select new DocumentViewModel
            {
                Id = doc.Id,
                Filename = doc.Filename,
                DocumentType = docType.Title,
                MineType = mineType.Title,
                City = city.Title,
                StartYear = sYear.Title,
                EndYear = eYear.Title,
                CreatedDate = doc.CreatedDate.ToString("dd.MM.yyyy HH:mm:ss")
            }).OrderBy(p=>p.Id).ToList();
    }

    private FilterViewModel GetFilters()
    {
        var filterViewModel = new FilterViewModel();
        filterViewModel.DocumentTypeFilters = GetDocumentTypeFilters();
        filterViewModel.MineTypeFilters = GetMineTypeTypeFilters();
        filterViewModel.CityFilters = GetCityFilters();
        
        return filterViewModel;
    }

    private List<DocumentTypeFilterViewModel> GetDocumentTypeFilters()
    {
        var result = _dbContext.Documents
            .Join(
                _dbContext.DocumentTypes,
                doc => doc.DocumentType,
                dt => dt.Id,
                (doc, dt) => new { Document = doc, DocumentType = dt }
            )
            .GroupBy(
                x => new { x.DocumentType.Id, x.DocumentType.Title },
                x => x.Document
            )
            .Select(g => new DocumentTypeFilterViewModel
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                Count = g.Count()
            })
            .ToList();
        
        return result;
    }
    
    private List<MineTypeFilterViewModel> GetMineTypeTypeFilters()
    {
        var result = _dbContext.Documents
            .Join(
                _dbContext.MineTypes,
                doc => doc.MineType,
                dt => dt.Id,
                (doc, mt) => new { Document = doc, MineType = mt }
            )
            .GroupBy(
                x => new { x.MineType.Id, x.MineType.Title },
                x => x.Document
            )
            .Select(g => new MineTypeFilterViewModel
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                Count = g.Count()
            })
            .ToList();
        
        return result;
    }
    
    private List<CityFilterViewModel> GetCityFilters()
    {
        var result = _dbContext.Documents
            .Join(
                _dbContext.Cities,
                doc => doc.City,
                dt => dt.Id,
                (doc, city) => new { Document = doc, City = city }
            )
            .GroupBy(
                x => new { x.City.Id, x.City.Title },
                x => x.Document
            )
            .Select(g => new CityFilterViewModel
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                Count = g.Count()
            })
            .ToList();
        
        return result;
    }
    
    
}