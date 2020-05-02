using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace MatchBox.Tests
{
    public abstract class IntegrationTestBase<TCONTROLLER> : IClassFixture<IocFixture>
        where TCONTROLLER : ControllerBase
    {
        protected readonly IServiceProvider ServiceProvider;

        public IntegrationTestBase(IocFixture dbFixture)
        {
            ServiceProvider = dbFixture.ServiceProvider ?? throw new ArgumentNullException(nameof(dbFixture));
        }

        protected TCONTROLLER CreateController()
        {            
            var svc = ServiceProvider.GetRequiredService<TCONTROLLER>();
            return svc;
        }
    }
}