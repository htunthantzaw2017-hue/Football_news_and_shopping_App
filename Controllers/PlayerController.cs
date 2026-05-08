using ManchesterUnitedApp.Models.Player;
using ManchesterUnitedApp.Services;
using ManchesterUnitedApp.Services.PlayerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ManchesterUnitedApp.Controllers
{
    public class PlayerController : Controller
    {
        private readonly PlayerService _service;
        private readonly PlayerPositionService _playerPositionService;
        private readonly PlayerStatusService _playerStatusService;
        private readonly GenderTypeService _genderTypeService;
        private readonly TeamService _teamService;

        public PlayerController(
            PlayerService service,
            PlayerPositionService playerPositionService,
            PlayerStatusService playerStatusService,
            GenderTypeService genderTypeService,
            TeamService teamService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _playerPositionService = playerPositionService ?? throw new ArgumentNullException(nameof(playerPositionService));
            _playerStatusService = playerStatusService ?? throw new ArgumentNullException(nameof(playerStatusService));
            _genderTypeService = genderTypeService ?? throw new ArgumentNullException(nameof(genderTypeService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        }

        // ✅ Index with filters + search
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? gender, string? team, string? search)
        {
            var players = await _service.GetFilteredAsync(gender, team, search);

            ViewBag.SelectedGender = gender ?? string.Empty;
            ViewBag.SelectedTeam = team ?? string.Empty;
            ViewBag.SearchTerm = search ?? string.Empty;

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
                return View("AdminIndex", players);

            return View(players);
        }

        // ✅ Position-based compact grids
        [AllowAnonymous]
        public async Task<IActionResult> Goalkeepers()
        {
            var players = await _service.GetAllAsync();
            var goalkeepers = players.Where(p => p.PositionName.Equals("Goalkeeper", StringComparison.OrdinalIgnoreCase));
            return View(goalkeepers);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Defenders()
        {
            var players = await _service.GetAllAsync();
            var defenders = players.Where(p => p.PositionName.Equals("Defender", StringComparison.OrdinalIgnoreCase));
            return View(defenders);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Midfielders()
        {
            var players = await _service.GetAllAsync();
            var mids = players.Where(p => p.PositionName.Equals("Midfielder", StringComparison.OrdinalIgnoreCase));
            return View(mids);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Forwards()
        {
            var players = await _service.GetAllAsync();
            var forwards = players.Where(p => p.PositionName.Equals("Forward", StringComparison.OrdinalIgnoreCase));
            return View(forwards);
        }

        // ✅ Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            var model = new PlayerEntryModel();
            LoadDropdowns();
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerEntryModel model)
        {
            ProcessImageUpload(model);

            if (ModelState.IsValid)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.CreatedBy = userId ?? string.Empty;

                await _service.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(model);
        }

        // ✅ Edit
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _service.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            LoadDropdowns();
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerEntryModel model)
        {
            ProcessImageUpload(model);

            if (ModelState.IsValid)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _service.UpdateAsync(model, userId ?? string.Empty);
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(model);
        }

        // ✅ Delete
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _service.GetDetailByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, string? confirm = null)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ✅ Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await _service.GetDetailByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // ✅ Image retrieval
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var player = await _service.GetDetailByIdAsync(id);
            if (player?.ImageData == null || player.ImageData.Length == 0)
                return NotFound();

            var contentType = string.IsNullOrWhiteSpace(player.ImageType)
                ? "image/jpeg"
                : player.ImageType;

            return File(player.ImageData, contentType);
        }

        // 🔧 Helper Methods
        private void ProcessImageUpload(PlayerEntryModel model)
        {
            if (model.Photo != null && model.Photo.Length > 0)
            {
                using var ms = new MemoryStream();
                model.Photo.CopyTo(ms);
                model.ImageData = ms.ToArray();
                model.ImageType = model.Photo.ContentType;
            }
        }

        private void LoadDropdowns()
        {
            ViewBag.availablePositions = GetSelectList(_playerPositionService.GetAll());
            ViewBag.availableStatuses = GetSelectList(_playerStatusService.GetAll());
            ViewBag.availableGenderTypes = GetSelectList(_genderTypeService.GetAll());
            ViewBag.availableTeams = GetSelectList(_teamService.GetAll());
        }

        private SelectList GetSelectList(IEnumerable<dynamic> items)
        {
            var list = items
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
            return new SelectList(list, "Value", "Text");
        }
    }
}