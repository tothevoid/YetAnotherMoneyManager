using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MoneyManager.Application.DTO;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Accounts;
using MoneyManager.WebApi.Models.Transactions;
using MoneyManager.WebApi.Models.User;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.User
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly WebApiMapper _mapper;

        public UserProfileController(IUserProfileService userProfileService, WebApiMapper mapper)
        {
            _mapper = mapper;
            _userProfileService = userProfileService;
        }
       
        [HttpGet]
        public async Task<UserProfileModel> Get()
        {
            var userProfile = await _userProfileService.GetAsync();
            return _mapper.Map(userProfile);
        }

        [HttpPatch]
        public async Task Update(UserProfileModel userProfile)
        {
            var userProfileDto = _mapper.Map(userProfile);
            await _userProfileService.UpdateAsync(userProfileDto);
        }

    }
}