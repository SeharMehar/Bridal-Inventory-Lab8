using Lab_8.Data;
using Lab_8;
using Lab_8.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab_8.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly BridalProductRepository _repository;

        public IndexModel(BridalProductRepository repository)
        {
            _repository = repository;
        }

        public List<BridalProduct> BridalProducts { get; set; } = new();

        public async Task OnGetAsync()
        {
            BridalProducts = await _repository.GetAllAsync();
        }
    }
}
