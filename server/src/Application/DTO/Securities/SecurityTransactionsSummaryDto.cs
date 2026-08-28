namespace Audex.Application.DTO.Securities
{
    public class SecurityTransactionsSummaryDto
    {
        public int ActualQuantity { get; set; }

        public decimal PurchasePriceSum { get; set; }

        public decimal SellPriceSum { get; set; }
    }
}
