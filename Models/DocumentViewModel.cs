namespace KAS.CMS.Models;

public class DocumentViewModel
{
    public long Id { get; set; }
    public string DocumentType { get; set; }
    public string MineType { get; set; }
    public string City { get; set; }
    public string StartYear { get; set; }
    public string EndYear { get; set; }
    public string CreatedDate { get; set; }
}