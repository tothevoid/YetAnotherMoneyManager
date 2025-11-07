using System;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Login(string username, string password);

        Task<bool> ChangePassword(string userName, string currentPassword, string newPassword);
    }
}