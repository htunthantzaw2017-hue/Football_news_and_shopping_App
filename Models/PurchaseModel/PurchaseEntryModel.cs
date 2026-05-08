using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManchesterUnitedApp.Models.PurchaseModel
{
    public class PurchaseEntryModel
    {
        [Range(0, 99999, ErrorMessage = "quantity must greater than 0")]
        public int Qty { get; set; }
        public double Total { get; set; }
        public byte[]? PaymentImage { get; set; }
        public string? PaymentType { get; set; }
        public int AvailableStock { get; set; }
        [NotMapped]
        public IFormFile? PaymentBillImage { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = String.Empty;
    }
}
