﻿using AP.IdentityServer.Services;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AP.IdentityServer.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly IUserService _localUserService;

        public UserProfileService(IUserService localUserService)
        {
            _localUserService = localUserService ??
                throw new ArgumentNullException(nameof(localUserService));
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = (await _localUserService.GetUserClaimsBySubjectAsync(subjectId))
                .ToList();
            context.IssuedClaims.AddRange(claimsForUser);
            //context.AddRequestedClaims(claimsForUser.Select(c => c).ToList());
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = await _localUserService.IsUserActive(subjectId);
        }
    }
}
