using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.WebApi.Models.Accounts;
using MoneyManager.WebApi.Models.Transactions;
using MoneyManager.WebApi.Models.User;

namespace MoneyManager.WebApi.Controllers.User
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;

        public UserProfileController(IUserProfileService userProfileService, IMapper mapper)
        {
            _mapper = mapper;
            _userProfileService = userProfileService;
        }
       
        [HttpGet]
        public async Task<UserProfileModel> Get()
        {
            var userProfile = await _userProfileService.Get();
            return _mapper.Map<UserProfileModel>(userProfile);
        }

        [HttpPatch]
        public async Task Update(UserProfileModel userProfile)
        {
            var userProfileDto = _mapper.Map<UserProfileDto>(userProfile);
            await _userProfileService.Update(userProfileDto);
        }

    }
}