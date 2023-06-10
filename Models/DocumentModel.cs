namespace KAS.CMS.Models;

public class DocumentModel: IEntity
{
    public long DocumentType { get; set; }
    public long MineType { get; set; }
    public long City { get; set; }
    public long StartYear { get; set; }
    public long EndYear { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Filename { get; set; }
}