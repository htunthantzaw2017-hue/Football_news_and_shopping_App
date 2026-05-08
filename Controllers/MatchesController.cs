using ManchesterUnitedApp.Models.ViewModel;
using ManchesterUnitedApp.Services.IMatchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManchesterUnitedApp.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchService _service;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MatchesController(IMatchService service, IWebHostEnvironment hostEnvironment)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var upcoming = await _service.GetUpcomingMatchesAsync();
            var results = await _service.GetMatchResultsAsync();

            var viewModel = new MatchesIndexViewModel
            {
                UpcomingMatches = upcoming,
                MatchResults = results
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var match = await _service.GetMatchByIdAsync(id);
            if (match == null) return NotFound();
            return View(match);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View(new MatchCreateViewModel { MatchDate = DateTime.Now });
        }

        // Helper method to validate image files
        private bool IsValidImage(IFormFile? file)
        {
            if (file == null) return true; // No file uploaded
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }

        // POST: Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create(MatchCreateViewModel viewModel)
        {
            if (!IsValidImage(viewModel.HomeLogoFile))
                ModelState.AddModelError("HomeLogoFile", "Home logo must be an image (.jpg, .jpeg, .png, .gif).");

            if (!IsValidImage(viewModel.AwayLogoFile))
                ModelState.AddModelError("AwayLogoFile", "Away logo must be an image (.jpg, .jpeg, .png, .gif).");

            if (ModelState.IsValid)
            {
                await _service.CreateMatchAsync(viewModel, _hostEnvironment.WebRootPath);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _service.GetMatchForEditAsync(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        // POST: Matches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, MatchCreateViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!IsValidImage(viewModel.HomeLogoFile))
                ModelState.AddModelError("HomeLogoFile", "Home logo must be an image (.jpg, .jpeg, .png, .gif).");

            if (!IsValidImage(viewModel.AwayLogoFile))
                ModelState.AddModelError("AwayLogoFile", "Away logo must be an image (.jpg, .jpeg, .png, .gif).");

            if (ModelState.IsValid)
            {
                await _service.UpdateMatchAsync(viewModel, _hostEnvironment.WebRootPath);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _service.GetMatchByIdAsync(id);
            if (match == null) return NotFound();
            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteMatchAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

    public class MatchesIndexViewModel
    {
        public IEnumerable<MatchViewModel> UpcomingMatches { get; set; }
        public IEnumerable<MatchViewModel> MatchResults { get; set; }
    }
}
