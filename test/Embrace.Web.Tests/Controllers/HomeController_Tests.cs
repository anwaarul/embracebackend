using System.Threading.Tasks;
using Embrace.Models.TokenAuth;
using Embrace.Web.Controllers;
using Shouldly;
using Xunit;

namespace Embrace.Web.Tests.Controllers
{
    public class HomeController_Tests: EmbraceWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}