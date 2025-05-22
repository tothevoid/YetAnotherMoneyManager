using AutoMapper;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Infrastructure.Entities.User;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Application.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<UserProfile> _userProfileRepo;
        private readonly IMapper _mapper;
        public UserProfileService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _userProfileRepo = uow.CreateRepository<UserProfile>();
        }

        public async Task<UserProfileDto> Get()
        {
            var users = await _userProfileRepo.GetAll(include: GetFullHierarchyColumns);
            return _mapper.Map<UserProfileDto>(users.FirstOrDefault());
        }

        public async Task Update(UserProfileDto userProfileDto)
        {
            var userProfile = _mapper.Map<UserProfile>(userProfileDto);
            _userProfileRepo.Update(userProfile);
            await _db.Commit();
        }

        private IQueryable<UserProfile> GetFullHierarchyColumns(IQueryable<UserProfile> userProfileQuery)
        {
            return userProfileQuery.Include(profile => profile.Currency);
        }
    }
}