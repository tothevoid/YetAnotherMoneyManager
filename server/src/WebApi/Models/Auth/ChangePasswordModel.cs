namespace MoneyManager.WebApi.Models.Auth
{
    public class ChangePasswordModel
    {
        public string UserName { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
