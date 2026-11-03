using Lab_8.Data;
using Lab_8;
using Lab_8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poshak_lab7.Data;

namespace Lab_8.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly BridalProductRepository _repository;
        private readonly BridalProductImageService _images;

        public CreateModel(
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

        public void OnGet()
        {
            BridalProduct.Status = "Available";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(ImageFile), "Please select a product image.");
            }
            else
            {
                string? error = _images.Validate(ImageFile);

                if (error != null)
                    ModelState.AddModelError(nameof(ImageFile), error);
            }

            if (!ModelState.IsValid)
                return Page();

            string imagePath = await _images.SaveAsync(ImageFile!);

            try
            {
                BridalProduct.Image = imagePath;
                await _repository.CreateAsync(BridalProduct);

                TempData["Success"] = "Bridal product created successfully.";

                return RedirectToPage("Index");
            }
            catch
            {
                _images.Delete(imagePath);
                throw;
            }
        }
    }
}