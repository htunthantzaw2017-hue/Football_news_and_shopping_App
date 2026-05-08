using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.PurchaseModel
{
    public class PurchaseListModel
    {
        public Guid Id { get; set; }
        [Range(0, 99999, ErrorMessage = "quantity must greater than 0")]
        public int Qty { get; set; }
        public double Total { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string ItemName { get; set; } = string.Empty;
        [Display(Name = "Status")]
        public string StatusText { get; set; } = string.Empty;  // NEW

        [Display(Name = "Author")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date: ")]
        [DataType(DataType.Date)]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
