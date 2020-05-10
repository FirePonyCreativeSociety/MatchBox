using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MatchBox.Data;
using System.Linq;
using MatchBox.Controllers;
using System.Threading.Tasks;
using MatchBox.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace MatchBox.Tests
{
    public class UsersControllerTests : IntegrationTestBase<AuthenticationController>
    {
        public UsersControllerTests(IocFixture dbFixture)
            : base(dbFixture)
        {

        }                

        [Fact]
        public async Task LoginOkForDefaultUsers()
        {
            var ctrl = CreateController();

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            foreach (var name in MatchBoxDbInitializer.GetDefaultUserNames())
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            {
                var result = await ctrl.Login(new LoginModel
                {
                    UsernameOrEmail = MatchBoxDbContext.AdminUserName,
                    Password = MatchBoxDbInitializer.DefaultPassword
                });
                
                var okResult = result.Result as OkObjectResult;
                Assert.NotNull(okResult);
            }
        }

        [Fact]
        public async Task ResetPasswordWorks()
        {
            var ctrl = CreateController();
            
            var ac = new ActionContext() { 
                
            };
            ctrl.Url = new UrlHelper(ac);
            _ = await ctrl.ForgotPassword(new UsernameOrEmailModel
            {
                UsernameOrEmail = "alef",
            });

            //await ctrl.ResetPassword(new ResetPasswordModel
            //{ 
            //    Email = "afederici75@gmail.com",
            //    Token = forgotResp.
            //});
        }
    }
}
