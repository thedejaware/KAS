namespace KAS.CMS.Models;

public class FilterViewModel
{
    public FilterViewModel()
    {
        DocumentTypeFilters = new List<DocumentTypeFilterViewModel>();
        MineTypeFilters = new List<MineTypeFilterViewModel>();
        CityFilters = new List<CityFilterViewModel>();
    }
    public List<DocumentTypeFilterViewModel> DocumentTypeFilters { get; set; }
    public List<MineTypeFilterViewModel> MineTypeFilters { get; set; }
    public List<CityFilterViewModel> CityFilters { get; set; }
}