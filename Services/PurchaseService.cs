using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Enum;
using ManchesterUnitedApp.Models.PurchaseModel;
using ManchesterUnitedApp.Models.Shop;
using ManchesterUnitedApp.Repositories;

namespace ManchesterUnitedApp.Services
{
    public class PurchaseService
    {
        private readonly ItemRepository _itemRepo;
        private readonly PurchaseRepository _purchaseRepo;
        private readonly UserRepository _userRepo;
        //private readonly CartRepository _cartRepo;

        public PurchaseService(ItemRepository itemRepo, PurchaseRepository purchaseRepo, UserRepository userRepo)
        {
            _itemRepo = itemRepo;
            _purchaseRepo = purchaseRepo;
            _userRepo = userRepo;
            // _cartRepo = cartRepo;
        }

        public List<PurchaseListModel> GetAll()
        {
            return _purchaseRepo.GetAll()
                .Select(p => new PurchaseListModel
                {
                    Id = p.Id,
                    Qty = p.Qty,
                    Total = p.Total,
                    PurchaseDate = p.PurchaseDate,
                    ItemName = p.Item != null ? p.Item.Name : string.Empty,
                    CreatedBy = _userRepo.GetUserName(p.CreatedBy) ?? string.Empty,
                    CreatedOn = p.CreatedOn
                })
                .OrderBy(p => p.CreatedOn)
                .ToList();
        }
        public string CreatePurchase(PurchaseEntryModel model)
        {
            var item = _itemRepo.GetById(model.ItemId);

            if (item == null)
                return "Item not found";
            if (model.Qty <= 0)
            {
                return "You enter wrong quantity number check again";
            }
            // stock check
            if (model.Qty > item.Qty)
                return "Not enough item";

            //byte[]? imageBytes = null;

            //if (model.PaymentBillImage != null)
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        model.PaymentBillImage.CopyTo(ms);
            //        imageBytes = ms.ToArray();
            //    }
            //}

            // Save uploaded image
            byte[]? imageBytes = null;
            string? imageType = null;

            if (model.PaymentBillImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.PaymentBillImage.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }

                imageType = model.PaymentBillImage.ContentType; // <-- REQUIRED
            }

            // ⬇️⬇️ DECREASE STOCK HERE
            item.Qty -= model.Qty;

            // save stock change
            _itemRepo.Edit(item);

            var purchase = new Purchase
            {
                Id = Guid.NewGuid(),
                Qty = model.Qty,
                Total = item.Price * model.Qty,
                PaymentImage = imageBytes,
                //PaymentType = model.PaymentType,
                PaymentType = imageType,
                ItemId = item.Id,
                PurchaseDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
                Status = PurchaseStatus.New
            };

            _purchaseRepo.Add(purchase);

            return "SUCCESS";
        }

        public PurchaseDetailModel? GetDetailById(Guid id)
        {
            Purchase? entity = _purchaseRepo.GetDetailById(id);
            if (entity == null)
                return null;
            return new PurchaseDetailModel
            {
                Id = entity.Id,
                Qty = entity.Qty,
                Total = entity.Total,
                PaymentImage = entity.PaymentImage,
                PaymentType = entity.PaymentType,
                PurchaseDate = entity.PurchaseDate,
                ItemId = entity.ItemId,
                ItemName = entity.Item != null ? entity.Item.Name : string.Empty,
                CreatedBy = _userRepo.GetUserName(entity.CreatedBy) ?? string.Empty,
                CreatedOn = entity.CreatedOn
            };
        }

        public void Add(PurchaseEntryModel model)
        {
            var now = DateTimeOffset.UtcNow;

            Purchase entity = new Purchase
            {
                //Id = model.Id,
                Qty = model.Qty,
                Total = model.Total,
                PaymentImage = model.PaymentImage,
                PaymentType = model.PaymentType,
                ItemId = model.ItemId,
                PurchaseDate = DateTime.Now,
                CreatedBy = model.CreatedBy
            };
        }

        public string CompleteOrder(Guid id)
        {
            var p = _purchaseRepo.GetById(id);
            if (p == null) return "Order not found";

            p.Status = PurchaseStatus.Completed;
            _purchaseRepo.Update(p);
            return "SUCCESS";
        }

        public string CancelOrder(Guid id)
        {
            var p = _purchaseRepo.GetById(id);
            if (p == null) return "Order not found";

            p.Status = PurchaseStatus.Canceled;
            _purchaseRepo.Update(p);
            return "SUCCESS";
        }

        public List<PurchaseListModel> GetByStatus(PurchaseStatus status)
        {
            return _purchaseRepo.GetByStatus(status)
                .Select(p => new PurchaseListModel
                {
                    Id = p.Id,
                    Qty = p.Qty,
                    Total = p.Total,
                    PurchaseDate = p.PurchaseDate,
                    ItemName = p.Item.Name,
                    CreatedBy = _userRepo.GetUserName(p.CreatedBy),
                    CreatedOn = p.CreatedOn
                })
                .ToList();
        }


        public List<PurchaseListModel> GetAllForUser(string userId)
        {
            return _purchaseRepo.GetAll()
                .Where(p => p.CreatedBy == userId)
                .Select(p => new PurchaseListModel
                {
                    Id = p.Id,
                    Qty = p.Qty,
                    Total = p.Total,
                    PurchaseDate = p.PurchaseDate,
                    ItemName = p.Item?.Name ?? string.Empty,
                    CreatedBy = _userRepo.GetUserName(p.CreatedBy) ?? string.Empty,
                    CreatedOn = p.CreatedOn,
                    StatusText = p.Status switch
                    {
                        PurchaseStatus.New => "Action in Progress...",
                        PurchaseStatus.Completed => "Done",
                        PurchaseStatus.Canceled => "Canceled",
                        _ => "Unknown"
                    }
                })
                .OrderByDescending(p => p.CreatedOn)
                .ToList();
        }

    }
}