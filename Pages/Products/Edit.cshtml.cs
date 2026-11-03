using Lab_8.Data;
using Lab_8;
using Lab_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poshak_lab7.Data;

namespace Lab_8.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly BridalProductRepository _repository;
        private readonly BridalProductImageService _images;

        public EditModel(
            BridalProductRepository repository,
            BridalProductImageService images)
        {
            _repository = repository;
            _images = images;
        }

        [BindProperty]
        public BridalProduct BridalProduct { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

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
            if (id != BridalProduct.Id)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string? error = _images.Validate(ImageFile);

                if (error != null)
                    ModelState.AddModelError(nameof(ImageFile), error);
            }

            if (!ModelState.IsValid)
            {
                BridalProduct.Image = existing.Image;
                return Page();
            }

            string oldImage = existing.Image;
            string newImage = oldImage;

            if (ImageFile != null && ImageFile.Length > 0)
                newImage = await _images.SaveAsync(ImageFile);

            BridalProduct.Image = newImage;

            try
            {
                bool updated = await _repository.UpdateAsync(BridalProduct);

                if (!updated)
                {
                    if (newImage != oldImage)
                        _images.Delete(newImage);

                    return NotFound();
                }
            }
            catch
            {
                if (newImage != oldImage)
                    _images.Delete(newImage);

                throw;
            }

            if (newImage != oldImage)
                _images.Delete(oldImage);

            TempData["Success"] = "Bridal product updated successfully.";

            return RedirectToPage("Index");
        }
    }
}