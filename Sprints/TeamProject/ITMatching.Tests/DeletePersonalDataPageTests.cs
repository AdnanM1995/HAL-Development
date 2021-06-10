using NUnit.Framework;
using ITMatching.Controllers;
using ITMatching.Areas.Identity.Pages.Account.Manage;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using ITMatching.Models.Abstract;
using ITMatching.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
using MockQueryable.Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITMatching.Tests
{
    public class DeletePersonalDataPageTests
    {
        private DeletePersonalDataModel _pageModel;
        private Mock<IItmuserRepository> _mockItmuserRepo;
        private Mock<IExpertRepository> _mockExpertRepo;

        private List<Itmuser> itmUsers;
        private List<Expert> experts;

        private const string ASPNET_USER_ID = "2a44c5fa-13a4-4477-82db-512da5e6f32a";
        private const int ITMUSER_ID = 1;
        private const int EXPERT_ID = 2;

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<DeletePersonalDataModel>>();
            itmUsers = new List<Itmuser>
            {
                new Itmuser { Id = ITMUSER_ID, FirstName = "Test", LastName = "BBB", PhoneNumber = "9998524256", Email = "jame@im.com" , UserName = "jame@im.com", AspNetUserId = ASPNET_USER_ID },
                new Itmuser { Id = 2, FirstName = "Test", LastName = "ZZZ", PhoneNumber = "9985242568", Email = "zzz@im.com" , UserName = "zzz@im.com", AspNetUserId = Guid.NewGuid().ToString()  },
                new Itmuser { Id = 3, FirstName = "Test", LastName = "AAA", PhoneNumber = "9985242526", Email = "aaa@im.com" , UserName = "aaa@im.com", AspNetUserId = Guid.NewGuid().ToString() },
            };

            experts = new List<Expert>
            {
                new Expert { Id = EXPERT_ID, ItmuserId = ITMUSER_ID, WorkSchedule = "Work Schedule", IsAvailable = false },
                new Expert { Id = 45, ItmuserId = 22, WorkSchedule = "Work Schedule", IsAvailable = true },
                new Expert { Id = 55, ItmuserId = 33, WorkSchedule = "Work Schedule", IsAvailable = false },
            };

            // Mock the Itmuser repository
            _mockItmuserRepo = new Mock<IItmuserRepository>();
            _mockItmuserRepo.Setup(m => m.GetByAspNetUserIdAsync(ASPNET_USER_ID)).ReturnsAsync(itmUsers.Where(u => u.AspNetUserId == ASPNET_USER_ID).FirstOrDefault()).Verifiable();

            // Mock the Expert repository
            _mockExpertRepo = new Mock<IExpertRepository>();
            _mockExpertRepo.Setup(m => m.GetByItmUserIdAsync(ITMUSER_ID)).ReturnsAsync(experts.Where(e => e.ItmuserId == ITMUSER_ID).FirstOrDefault()).Verifiable();
            _mockExpertRepo.Setup(m => m.DeleteServicesAsync(EXPERT_ID)).Verifiable();

            _pageModel = new DeletePersonalDataModel(new FakeUserManager(), new FakeSignInManager(), mockLogger.Object, _mockItmuserRepo.Object, _mockExpertRepo.Object);
        }

        [Test]
        public async Task DeletePersonalDataPage_DeleteAccount_Success()
        {
            //Arrange
            var userId = "2a44c5fa-13a4-4477-82db-512da5e6f32a";
            var userName = "jame@im.com";
            var updatedUserName = $"deleted@user-{userId}.com";
            _pageModel.Input = new DeletePersonalDataModel.InputModel { Password = "password", SecurityPhrase = userName };

            //Act
            IActionResult result = await _pageModel.OnPostAsync();

            //Assert
            _mockItmuserRepo.Verify(mock => mock.GetByAspNetUserIdAsync(ASPNET_USER_ID), Times.Once());
            _mockExpertRepo.Verify(mock => mock.GetByItmUserIdAsync(ITMUSER_ID), Times.Once());
            _mockExpertRepo.Verify(mock => mock.DeleteServicesAsync(EXPERT_ID), Times.Once());
            Assert.That((result as PageResult), Is.Null);
            Assert.That((result as RedirectResult), Is.Not.Null);
            Assert.That((result as RedirectResult).Url, Is.EqualTo("~/"));
        }

        [Test]
        public async Task DeletePersonalDataPage_DeleteAccount_InvalidSecurityPhrase()
        {
            //Arrange
            var userId = "2a44c5fa-13a4-4477-82db-512da5e6f32a";
            var userName = "jame@im.com";
            var updatedUserName = $"deleted@user-{userId}.com";
            _pageModel.Input = new DeletePersonalDataModel.InputModel { Password = "password", SecurityPhrase = "wrong phrase" };

            //Act
            IActionResult result = await _pageModel.OnPostAsync();

            //Assert
            _mockItmuserRepo.Verify(mock => mock.GetByAspNetUserIdAsync(ASPNET_USER_ID), Times.Never());
            _mockExpertRepo.Verify(mock => mock.GetByItmUserIdAsync(ITMUSER_ID), Times.Never());
            _mockExpertRepo.Verify(mock => mock.DeleteServicesAsync(EXPERT_ID), Times.Never());
            Assert.That(_pageModel.ModelState.IsValid, Is.False);
            Assert.That(_pageModel.ModelState.Count, Is.EqualTo(1));
            Assert.That(_pageModel.ModelState.ErrorCount, Is.EqualTo(1));
            Assert.That(_pageModel.ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage, Is.EqualTo("Incorrect security phrase."));
            Assert.That((result as RedirectResult), Is.Null);
            Assert.That((result as PageResult), Is.Not.Null);
        }
    }
}