using Lab_8.Data;
using Lab_8;
using Lab_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poshak_lab7.Data;

namespace Lab_8.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly BridalProductRepository _repository;
        private readonly BridalProductImageService _images;

        public DeleteModel(
            BridalProductRepository repository,
            BridalProductImageService images)
        {
            _repository = repository;
            _images = images;
        }

        [BindProperty]
        public BridalProduct BridalProduct { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            BridalProduct = product;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            bool deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            if (!string.IsNullOrEmpty(product.Image))
            {
                _images.Delete(product.Image);
            }

            TempData["Success"] = "Bridal product deleted successfully.";

            return RedirectToPage("Index");
        }
    }
}