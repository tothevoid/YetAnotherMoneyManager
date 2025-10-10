using AutoMapper;
using MoneyManager.Infrastructure.Interfaces.Database;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.User;
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
        private readonly IMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public UserProfileService(IUnitOfWork uow, IMapper mapper, ICurrencyService currencyService)
        {
            _db = uow;
            _mapper = mapper;
            _userProfileRepo = uow.CreateRepository<UserProfile>();
            _currencyService = currencyService;
        }

        public async Task<UserProfileDto> Get()
        {
            var users = await _userProfileRepo.GetAll(include: GetFullHierarchyColumns);
            return _mapper.Map<UserProfileDto>(users.FirstOrDefault());
        }

        public async Task Update(UserProfileDto userProfileDto)
        {
            var currentUserState = await Get();
            var userProfile = _mapper.Map<UserProfile>(userProfileDto);

            var currencyChanged = currentUserState.CurrencyId != userProfile.CurrencyId;

            _userProfileRepo.Update(userProfile);
            await _db.Commit();

            if (currencyChanged)
            {
                var currency = await _currencyService.GetById(userProfileDto.CurrencyId);
                await _currencyService.SyncRates(currency);
            }
        }

        private IQueryable<UserProfile> GetFullHierarchyColumns(IQueryable<UserProfile> userProfileQuery)
        {
            return userProfileQuery.Include(profile => profile.Currency);
        }
    }
}