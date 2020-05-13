﻿using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MatchBox.Internal;
using System.Linq;
using System.Threading.Tasks;
using MatchBox.Data.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection;

namespace MatchBox.Controllers
{
    // https://code-maze.com/user-lockout-aspnet-core-identity/

    public class UsersController : RESTControllerBase<User, DbUser>
    {        
        public UsersController(
            MatchBoxDbContext dbContext, 
            IMapper mapper, 
            UserManager<DbUser> userManager,
            IEmailSender emailSender,
            IDataProtectionProvider dataProtectionProvider)
            : base(dbContext, mapper, dataProtectionProvider)
        {
            UserManager = userManager;
            EmailSender = emailSender;
        }

        public UserManager<DbUser> UserManager { get; }
        public IEmailSender EmailSender { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;

        async Task<ActionResult<EmptyResult>> LockUnlockUser(UsernameOrEmailModel model, bool isLocked)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!EnsureIsAdmin<EmptyResult>(out var mandatoryResponse))
            {
                return mandatoryResponse;
            }

            var userResp = await UserManager.FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (!userResp.Found)
                return NotFound();

            var resp = await UserManager.SetLockoutEnabledAsync(userResp.Value, isLocked);
            if (resp.Succeeded)
            {
                if (isLocked)
                    await UserManager.SetLockoutEndDateAsync(userResp.Value, DateTimeOffset.Now.AddYears(100));
                else
                    await UserManager.SetLockoutEndDateAsync(userResp.Value, null);
            }

            if (resp.Succeeded)
                return Ok();            

            foreach (var error in resp.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(UnlockUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ActionResult<EmptyResult>> UnlockUser([FromBody] UsernameOrEmailModel model)
        {
            return LockUnlockUser(model, false);
        }

        [HttpPost(nameof(LockUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<ActionResult<EmptyResult>> LockUser([FromBody] UsernameOrEmailModel model)
        {
            return LockUnlockUser(model, true);
        }

        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmptyResult>> Delete([FromBody] UsernameOrEmailModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userResp = await UserManager.FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (!userResp.Found)
                return NotFound();

            var resp = await UserManager.DeleteAsync(userResp.Value);
            if (resp.Succeeded)
                return Ok();

            foreach (var error in resp.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);            
        }

        [HttpPatch()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmptyResult>> Update([FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userResp = await UserManager.FindUserByUsernameOrEmail(model.Username);
            if (!userResp.Found)
                return NotFound();

            Mapper.Map(model, userResp.Value);

            var resp = await UserManager.UpdateAsync(userResp.Value);
            if (resp.Succeeded)
                return Ok();

            foreach (var error in resp.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost(nameof(AddClaimToUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmptyResult>> AddClaimToUser([FromBody] AddClaimToUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userResp = await UserManager.FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (!userResp.Found)
                return NotFound();

            var identityClaim = new System.Security.Claims.Claim(model.Claim.ClaimType, model.Claim.ClaimValue);
            var resp = await UserManager.AddClaimAsync(userResp.Value, identityClaim);

            if (resp.Succeeded)
                return Ok();

            foreach (var error in resp.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }


        [HttpPost(nameof(RemoveClaimFromUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmptyResult>> RemoveClaimFromUser([FromBody] RemoveClaimFromUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userResp = await UserManager.FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (!userResp.Found)
                return NotFound();

            var storedClaims = await UserManager.GetClaimsAsync(userResp.Value);
            foreach (var claimToRemove in storedClaims.Where(c => c.Type.Equals(model.ClaimType, StringComparison.OrdinalIgnoreCase)))
            {
                var removeResp = await UserManager.RemoveClaimAsync(userResp.Value, claimToRemove);
                if (!removeResp.Succeeded)
                {
                    foreach (var error in removeResp.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                }
            }
            if (ModelState.ErrorCount>0)
                return BadRequest(ModelState);
            else
                return Ok();
        }

        [HttpGet(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ActionResult<IEnumerable<User>>> Get(QueryModel model)
        {            
            return base.GetAll(model);
        }
    }
}