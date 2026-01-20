using BnsBazarApp.Models;
using BnsBazarApp.Models.Data;
using BnsBazarApp.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BnsBazarApp.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly BnsBazarDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly EmailService _emailService;

        public AdvertisementController(
            BnsBazarDbContext db,
            IWebHostEnvironment env,
            EmailService emailService)
        {
            _db = db;
            _env = env;
            _emailService = emailService;
        }

        // =====================================================
        // BROWSE (UNIVERSAL LISTING + SEARCH)
        // =====================================================
        [HttpGet]
        public IActionResult Browse(
            string? module,
            string? type,
            string? q,
            string? location)
        {
            var ads = _db.Advertisements.AsQueryable();

            if (!string.IsNullOrWhiteSpace(module))
                ads = ads.Where(a => a.Module == module);

            if (!string.IsNullOrWhiteSpace(type))
                ads = ads.Where(a => a.Category == type);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                ads = ads.Where(a =>
                    (a.Title != null && a.Title.Contains(q)) ||
                    (a.Description != null && a.Description.Contains(q)));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                location = location.Trim();
                ads = ads.Where(a =>
                    a.Location != null && a.Location.Contains(location));
            }

            var result = ads
                .OrderByDescending(a => a.PostedDate)
                .ToList();

            ViewBag.Module = module;
            ViewBag.Type = type;
            ViewBag.Q = q;
            ViewBag.Location = location;

            return View(result);
        }

        // =====================================================
        // CREATE (GET)
        // =====================================================
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Advertisement());
        }

        // =====================================================
        // CREATE (POST)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Advertisement ad,
            List<IFormFile> images,
            IFormFile? video)
        {
            if (!ModelState.IsValid)
                return View(ad);

            // =============================
            // UPLOAD FOLDER
            // =============================
            string uploadRoot = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            // =============================
            // IMAGE UPLOAD (MAX 5)
            // =============================
            string[] imageUrls = new string[5];
            int index = 0;

            if (images != null)
            {
                foreach (var img in images.Take(5))
                {
                    if (img.Length > 0)
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                        string path = Path.Combine(uploadRoot, fileName);

                        using var stream = new FileStream(path, FileMode.Create);
                        await img.CopyToAsync(stream);

                        imageUrls[index++] = "/uploads/" + fileName;
                    }
                }
            }

            ad.Image1Url = imageUrls[0];
            ad.Image2Url = imageUrls[1];
            ad.Image3Url = imageUrls[2];
            ad.Image4Url = imageUrls[3];
            ad.Image5Url = imageUrls[4];

            // =============================
            // VIDEO UPLOAD (SAFE: 0–1 MIN)
            // =============================
            if (video != null && video.Length > 0)
            {
                // Size check (50MB)
                if (video.Length > 50 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "भिडियो ५०MB भन्दा ठूलो छ।");
                    return View(ad);
                }

                // Allowed formats (iPhone + Android)
                var allowedTypes = new[] { "video/mp4", "video/quicktime" };
                if (!allowedTypes.Contains(video.ContentType))
                {
                    ModelState.AddModelError("", "अवैध भिडियो format। MP4 वा MOV मात्र।");
                    return View(ad);
                }

                string videoName = Guid.NewGuid() + Path.GetExtension(video.FileName);
                string videoPath = Path.Combine(uploadRoot, videoName);

                using var stream = new FileStream(videoPath, FileMode.Create);
                await video.CopyToAsync(stream);

                ad.VideoUrl = "/uploads/" + videoName;
            }
            // =============================
            // DEFAULT VALUES
            // =============================
            ad.PostedDate = DateTime.Now;

            // AUTO ASSIGN MODULE
            ad.Module = ad.Category switch
            {
                "House" or "Land" => "RealEstate",
                "Car" or "Bike" => "Automobile",
                "Electrical" or "Electronics" => "Electronics",
                "Computer" or "Laptop" => "Computer",
                "Mobile" => "Mobile",
                _ => "Other"
            };

            // =============================
            // SAVE TO DATABASE
            // =============================
            _db.Advertisements.Add(ad);
            await _db.SaveChangesAsync();

            // =============================
            // EMAIL CONFIRMATION (SAFE)
            // =============================
            try
            {
                if (!string.IsNullOrWhiteSpace(ad.Email))
                {
                    string subject = "Your advertisement is live on BnsBazar";

                    string body = $@"
                        <h3>Thank you for posting on BnsBazar</h3>
                        <p><strong>Title:</strong> {ad.Title}</p>
                        <p><strong>Category:</strong> {ad.Category}</p>
                        <p>Your advertisement has been published successfully.</p>
                        <br/>
                        <p>धन्यवाद! तपाईंको विज्ञापन सफलतापूर्वक प्रकाशित गरिएको छ।</p>
                        <hr/>
                        <small>BnsBazar Team</small>
                    ";

                    await _emailService.SendAsync(ad.Email, subject, body);
                }
            }
            catch
            {
                // optional logging
            }

            // =============================
            // REDIRECT TO FILTERED LIST
            // =============================
            return RedirectToAction(
                nameof(Browse),
                new { module = ad.Module, type = ad.Category }
            );
        }

        // =====================================================
        // DELETE (ADMIN ONLY)
        // =====================================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var ad = _db.Advertisements.FirstOrDefault(a => a.Id == id);
            if (ad == null)
                return NotFound();

            _db.Advertisements.Remove(ad);
            _db.SaveChanges();

            return RedirectToAction(nameof(Browse));
        }
    }
}