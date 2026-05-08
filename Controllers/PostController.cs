using ManchesterUnitedApp.Data.Entities.Post;
using ManchesterUnitedApp.Models.Post;
using ManchesterUnitedApp.Models.Shop;
using ManchesterUnitedApp.Services.PostServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ManchesterUnitedApp.Controllers
{
    public class PostController : Controller
    {
        private readonly PostService _service;
        private readonly PostStatusService _poststatusService;
        private readonly CategoryService _categoryService;
        public PostController(PostService service, CategoryService categoryService, PostStatusService poststatusService)
        {
            _service = service;
            _categoryService = categoryService;
            _poststatusService = poststatusService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            
            bool isAdmin = User.IsInRole("SuperAdmin") || User.IsInRole("Admin");

            
            IList<PostListModel> posts = _service.GetAll(isAdmin);

            if (isAdmin)
            {
               
                return View("AdminIndex", posts);
            }
            
            return View(posts.ToList());
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            PostEntryModel model = new PostEntryModel();
            ViewBag.AvaliableCategories = GetCategorySelectList();
            ViewBag.AvaliablePostStatuses = GetPostStatusSelectList();
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public IActionResult Create(PostEntryModel model)
        {

            if (model.UploadImage != null && model.UploadImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.UploadImage.CopyTo(ms);
                    model.ItemImage = ms.ToArray();
                }

                model.ImageType = model.UploadImage.ContentType;
            }

            if (ModelState.IsValid)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.CreatedBy = userId ?? string.Empty;
                _service.Add(model);
                return RedirectToAction("Index");
            }

            ViewBag.AvaliableCategories = GetCategorySelectList();
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Edit(Guid id)
        {
            PostEntryModel? model = _service.GetById(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.AvaliablePostStatuses = GetPostStatusSelectList();
            ViewBag.AvaliableCategories = GetCategorySelectList();
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public IActionResult Edit(PostEntryModel model)
        {

            if (model.UploadImage != null && model.UploadImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.UploadImage.CopyTo(ms);
                    model.ItemImage = ms.ToArray();
                }

                model.ImageType = model.UploadImage.ContentType;
            }

            if (!ModelState.IsValid)
            {
                // **FIX:** Re-populate ALL SelectList ViewBags before returning the View
                ViewBag.AvaliablePostStatuses = GetPostStatusSelectList(); // <-- ADD THIS LINE
                ViewBag.AvaliableCategories = GetCategorySelectList();

                return View(model);
            }

            // ... (rest of the successful update logic)
            _service.Update(model);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            PostDetailModel? model = _service.GetDetailById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Delete(Guid id)
        {
            PostDetailModel? model = _service.GetDetailById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]

        public IActionResult DeleteConfrim(Guid id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }

        private SelectList GetCategorySelectList()
        {
            IList<SelectListItem> availableCategories = _categoryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return new SelectList(availableCategories, "Value", "Text");
        }

        private SelectList GetPostStatusSelectList()
        {
            IList<SelectListItem> availablePostStatuses = _poststatusService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return new SelectList(availablePostStatuses, "Value", "Text");
        }

        public IActionResult GetImage(Guid id)
        {
            var item = _service.GetDetailById(id);

            if (item == null || item.ItemImage == null)
                return NotFound();

            return File(item.ItemImage, item.ImageType);
        }
    }
}
