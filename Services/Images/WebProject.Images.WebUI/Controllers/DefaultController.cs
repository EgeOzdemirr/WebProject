using Microsoft.AspNetCore.Mvc;
using WebProject.Images.WebUI.DAL.Entities;
using WebProject.Images.WebUI.Services;

namespace WebProject.Images.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private readonly ICloudStorageService _cloudStorageService;
        public DefaultController(ICloudStorageService cloudStorageService)
        {
            _cloudStorageService = cloudStorageService;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ImageDrive imageDrive)
        {
            if (imageDrive.Photo != null)
            {
                var extension = Path.GetExtension(imageDrive.Photo.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension) || !imageDrive.Photo.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                    return View(imageDrive);
                }
                if (imageDrive.Photo.Length > MaxFileSizeBytes)
                {
                    ModelState.AddModelError(string.Empty, "File size must not exceed 5 MB.");
                    return View(imageDrive);
                }

                imageDrive.SavedFileName = GenerateFileNameToSave(imageDrive.Photo.FileName);
                imageDrive.SavedUrl = await _cloudStorageService.UploadFileAsync(imageDrive.Photo, imageDrive.SavedFileName);
            }
            var url = GenerateSignedUrl(imageDrive);
            return RedirectToAction("Index", "Default");
        }
        private string? GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss")}{extension}";
        }
        private async Task GenerateSignedUrl(ImageDrive imageDrive)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(imageDrive.SavedFileName))
            {
                imageDrive.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(imageDrive.SavedFileName);
            }
        }
    }
}