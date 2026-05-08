using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.PurchaseModel
{
    public class PurchaseDetailModel
    {
        public Guid Id { get; set; }
        [Range(0, 99999, ErrorMessage = "quantity must greater than 0")]
        public int Qty { get; set; }
        public double Total { get; set; }
        public byte[]? PaymentImage { get; set; }
        public string? PaymentType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        [Display(Name = "Customers")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date: ")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
