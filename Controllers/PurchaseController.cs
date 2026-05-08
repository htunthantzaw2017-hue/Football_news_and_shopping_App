using ManchesterUnitedApp.Enum;
using ManchesterUnitedApp.Models.PurchaseModel;
using ManchesterUnitedApp.Models.Shop;
using ManchesterUnitedApp.Repositories;
using ManchesterUnitedApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManchesterUnitedApp.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly PurchaseService _service;
        private readonly ItemRepository _itemRepo;
        public PurchaseController(PurchaseService service, ItemRepository itemRepo)
        {
            _service = service;
            _itemRepo = itemRepo;
        }

        private bool IsValidImage(IFormFile? file)
        {
            if (file == null) return true; // No file uploaded
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Buy(Guid id)
        {
            var item = _itemRepo.GetById(id);
            if (item == null)
                return NotFound();

            PurchaseEntryModel model = new PurchaseEntryModel
            {
                ItemId = id,
                ItemName = item.Name,
                Qty = 1,
                Total = item.Price,
                // Add available stock to model
                AvailableStock = item.Qty
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Buy(PurchaseEntryModel model)
        {
            if (!IsValidImage(model.PaymentBillImage))
            {
                ModelState.AddModelError("UploadImage", "UploadImage must be an image (.jpg, .jpeg, .png, .gif).");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.CreatedBy = userId ?? string.Empty;

            var result = _service.CreatePurchase(model);

            if (result == "SUCCESS")
            {
                return RedirectToAction("SUCCESS");
            }

            ModelState.AddModelError("", result);

            return View(model);
        }
        public IActionResult Success()
        {
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult PurchaseHistory()
        {
            var newOrders = _service.GetByStatus(PurchaseStatus.New);
            return View(newOrders);
        }
        public IActionResult CompletedOrders()
        {
            var completedOrders = _service.GetByStatus(PurchaseStatus.Completed);
            return View(completedOrders);
        }
        public IActionResult CanceledOrders()
        {
            var canceledOrders = _service.GetByStatus(PurchaseStatus.Canceled);
            return View(canceledOrders);
        }
        public IActionResult Complete(Guid id)
        {
            _service.CompleteOrder(id);
            return RedirectToAction("PurchaseHistory");
        }
        public IActionResult Cancel(Guid id)
        {
            _service.CancelOrder(id); // updates status to Canceled
            return RedirectToAction("PurchaseHistory"); // back to New Orders page
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Details(Guid id)
        {
            PurchaseDetailModel? model = _service.GetDetailById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(new List<PurchaseDetailModel> { model });
        }

        [Authorize]
        public IActionResult MyPurchases()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var purchases = _service.GetAllForUser(userId);
            return View(purchases);
        }
    }
}
