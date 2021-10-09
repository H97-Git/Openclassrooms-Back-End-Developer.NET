using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using Xunit;
using LoginModel = P3AddNewFunctionalityDotNetCore.Models.ViewModels.LoginModel;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{

    // https://github.com/aspnet/Identity/issues/640#issuecomment-435599771

    public class AccountControllerTests
    {
        private readonly AccountController _controller;
        private readonly Mock<UserManager<IdentityUser>> _mockU;
        private readonly Mock<SignInManager<IdentityUser>> _mockS;

        public AccountControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            _mockU = new Mock<UserManager<IdentityUser>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            _mockS = new Mock<SignInManager<IdentityUser>>(_mockU.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            _controller = new AccountController(_mockU.Object, _mockS.Object);
        }

        [Fact]
        public void Login_ViewResult_GoodPath()
        {
            var viewResult = _controller.Login("/");
            var model = viewResult.Model as LoginModel;
            Assert.NotNull(model);
            Assert.Equal("/",model.ReturnUrl);
        }

        [Fact]
        public async void Login_IActionResult_GoodPath()
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = "Admin",
            };

            _mockU.Setup(u => u.FindByNameAsync("Admin")).ReturnsAsync(identityUser);

            var loginModel = new LoginModel
            {
                Name = "Admin",
                Password = "P@ssword123",
                ReturnUrl = "/Admin/Index"
            };
            _mockS.Setup(s => s.PasswordSignInAsync(identityUser, loginModel.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);


            var viewResult = await _controller.Login(loginModel) as RedirectResult;

             _mockU.Verify(s => s.FindByNameAsync("Admin"), Times.Once);
            Assert.NotNull(viewResult);
            Assert.Equal("/Admin/Index", viewResult.Url);

        }

        [Fact]
        public async void Logout_GoodPath()
        {
            var viewResult = await _controller.Logout("/");
            var returnUrl = viewResult.Url;

            Assert.Equal("/", returnUrl);
        }
    }
}
