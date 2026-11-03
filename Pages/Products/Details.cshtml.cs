using Lab_8.Data;
using Lab_8;
using Lab_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab_8.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly BridalProductRepository _repository;

        public DetailsModel(BridalProductRepository repository)
        {
            _repository = repository;
        }

        public BridalProduct BridalProduct { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            BridalProduct = product;

            return Page();
        }
    }
}