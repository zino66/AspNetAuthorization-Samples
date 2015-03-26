﻿using System;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;

namespace AspNetAuthorization.AuthorizationPolicies
{
    public class MinimumAgeRequirement : AuthorizationHandler<Over18Requirement>, IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }

        public override void Handle(AuthorizationContext context, Over18Requirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return;
            }

            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            if (calculatedAge >= MinimumAge)
            {
                context.Succeed(requirement);
            }
        }
    }

}