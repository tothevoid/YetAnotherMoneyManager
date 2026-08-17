using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);

        Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword);
    }
}