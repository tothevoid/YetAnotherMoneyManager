using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Audex.Application.DTO;
using Audex.Application.DTO.Transactions;
using Audex.Application.Interfaces.Transactions;
using Audex.Application.Interfaces.User;
using Audex.WebApi.Mappings;
using Audex.WebApi.Models.Accounts;
using Audex.WebApi.Models.Transactions;
using Audex.WebApi.Models.User;
using Microsoft.AspNetCore.Authorization;

namespace Audex.WebApi.Controllers.User
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