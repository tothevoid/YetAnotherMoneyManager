namespace MoneyManager.WebApi.Models.Deposit
{
    public class DepositFiltrationModel
    {
        public int MonthsFrom { get; set; }

        public int MonthsTo { get; set; }

        public bool OnlyActive { get; set; }
    }
}
