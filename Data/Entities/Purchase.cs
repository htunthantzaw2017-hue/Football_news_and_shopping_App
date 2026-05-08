using ManchesterUnitedApp.Enum;
using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities
{
    public class Purchase : Base
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Qty { get; set; }
        public double Total { get; set; }
        public byte[]? PaymentImage { get; set; }
        public string? PaymentType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public PurchaseStatus Status { get; set; } = PurchaseStatus.New;
        public Guid ItemId { get; set; }
        //Navigation properties
        public Item Item { get; set; }
    }
}
