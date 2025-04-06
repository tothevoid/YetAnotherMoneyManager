namespace MoneyManager.Model
{
    public class DepositFiltrationModel
    {
        public int MonthsFrom { get; set; }

        public int MonthsTo { get; set; }

        public bool OnlyActive { get; set; }
    }
}
