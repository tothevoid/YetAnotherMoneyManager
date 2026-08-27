#nullable enable
using MoneyManager.Application.DTO.User;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.User
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetAsync();

        Task<UserProfileDto?> GetByUserNameAsync(string userName);

        Task<UserProfileDto?> GetByAuthAsync(string userName, string password);

        Task UpdateAsync(UserProfileDto newUserStateDto);
    }
}
