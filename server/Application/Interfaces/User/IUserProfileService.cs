using MoneyManager.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.User
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> Get();

        Task<UserProfileDto> GetByAuth(string userName, string password);

        Task Update(UserProfileDto userProfile);
    }
}
