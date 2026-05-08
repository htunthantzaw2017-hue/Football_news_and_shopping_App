using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Models.Shop;
using ManchesterUnitedApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ManchesterUnitedApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ItemService _service;
        private readonly ItemTypeService _itemtypeService;
        public ShopController(ItemService service, ItemTypeService itemtypeService)
        {
            _service = service;
            _itemtypeService = itemtypeService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            IList<ItemListModel> items = _service.GetAll();
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
    {
                return View("AdminIndex", items);
            }
            // User is logged in and is an admin
            return View(items.ToList());
        }
        private bool IsValidImage(IFormFile? file)
        {
            if (file == null) return true; // No file uploaded
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            ItemEntryModel model = new ItemEntryModel();
            ViewBag.AvaliableItemTypes = GetItemTypeSelectList();
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public IActionResult Create(ItemEntryModel model)
        {
            if (!IsValidImage(model.UploadImage))
                ModelState.AddModelError("UploadImage", "UploadImage must be an image (.jpg, .jpeg, .png, .gif).");

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
            ViewBag.AvaliableItemTypes = GetItemTypeSelectList();
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Details(Guid id)
        {
            ItemDetailHistory? model = _service.GetDetailById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Edit(Guid id)
        {
            ItemEntryModel? model = _service.GetById(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.AvaliableItemTypes = GetItemTypeSelectList();
            return View(model);
        }

        [HttpPost]

        public IActionResult Edit(ItemEntryModel item)
        {

            if (item.UploadImage != null && item.UploadImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    item.UploadImage.CopyTo(ms);
                    item.ItemImage = ms.ToArray();
                }

                item.ImageType = item.UploadImage.ContentType;
            }

            if (ModelState.IsValid)
            {
                _service.Edit(item);
                return RedirectToAction("Index");
            }
            ViewBag.AvaliableItemTypes = GetItemTypeSelectList();
            return View(item);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Delete(Guid id)
        {
            ItemEntryModel? model = _service.GetById(id);
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

        private SelectList GetItemTypeSelectList()
        {
            IList<SelectListItem> availableItemTypes = _itemtypeService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return new SelectList(availableItemTypes, "Value", "Text");
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
