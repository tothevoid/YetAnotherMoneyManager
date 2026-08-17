using MoneyManager.Infrastructure.Interfaces.Database;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.User;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Services.Currencies;

namespace MoneyManager.Application.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<UserProfile> _userProfileRepo;
        private readonly ApplicationMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public UserProfileService(IUnitOfWork uow, ApplicationMapper mapper, ICurrencyService currencyService)
        {
            _db = uow;
            _mapper = mapper;
            _userProfileRepo = uow.CreateRepository<UserProfile>();
            _currencyService = currencyService;
        }

        public async Task<UserProfileDto> GetAsync()
        {
            var users = await _userProfileRepo.GetAllAsync(include: GetFullHierarchyColumns);
            return _mapper.Map(users.FirstOrDefault());
        }

        public async Task<UserProfileDto> GetByAuthAsync(string userName, string password)
        {
            var users = await _userProfileRepo.GetAllAsync((user) =>
                string.Equals(user.UserName, userName) && 
                (string.Equals(user.Password, password) || 
                  (string.IsNullOrEmpty(user.Password) && string.IsNullOrEmpty(password))));
            return _mapper.Map(users.FirstOrDefault());
        }

        public async Task UpdateAsync(UserProfileDto newUserStateDto)
        {
            var currentUserState = await GetAsync();
            var userProfile = _mapper.Map(newUserStateDto);

            var currencyChanged = currentUserState.CurrencyId != userProfile.CurrencyId;

            userProfile.UserName = !string.IsNullOrEmpty(newUserStateDto.UserName) ? 
                newUserStateDto.UserName : 
                currentUserState.UserName;
            userProfile.Password = !string.IsNullOrEmpty(newUserStateDto.Password) ?
                newUserStateDto.Password :
                currentUserState.Password;

            _userProfileRepo.Update(userProfile);
            await _db.CommitAsync();

            if (currencyChanged)
            {
                var currency = await _currencyService.GetByIdAsync(newUserStateDto.CurrencyId);
                await _currencyService.SyncRatesAsync(currency);
            }
        }

        private IQueryable<UserProfile> GetFullHierarchyColumns(IQueryable<UserProfile> userProfileQuery)
        {
            return userProfileQuery.Include(profile => profile.Currency);
        }
    }
}