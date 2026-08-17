using MoneyManager.Application.DTO;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.User
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetAsync();

        Task<UserProfileDto> GetByAuthAsync(string userName, string password);

        Task UpdateAsync(UserProfileDto newUserStateDto);
    }
}
