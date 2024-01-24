namespace SweetWeeding.Areas.Admin.ViewModels
{
    public class CreateFamilyVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public IFormFile Photo { get; set; }

    }
}
