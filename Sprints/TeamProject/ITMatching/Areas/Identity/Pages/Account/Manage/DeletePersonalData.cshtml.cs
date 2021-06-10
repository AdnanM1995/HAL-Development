using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ITMatching.Models;
using ITMatching.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ITMatching.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IItmuserRepository _itmuserRepo;
        private readonly IExpertRepository _expertRepo;

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IItmuserRepository itmuserRepo,
            IExpertRepository expertRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _itmuserRepo = itmuserRepo;
            _expertRepo = expertRepo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Security Phrase")]
            public string SecurityPhrase { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            RequirePassword = await _userManager.HasPasswordAsync(user);

            if (user.Email.ToLower() != Input.SecurityPhrase.ToLower())
            {
                ModelState.AddModelError(string.Empty, "Incorrect security phrase.");
                return Page();
            }

            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }
            user.UserName = $"deleted@user-{user.Id}.com";
            user.Email = user.UserName;
            user.PhoneNumber = "0000000000";
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{user.Id}'.");
            }

            Itmuser itmUser = await _itmuserRepo.GetByAspNetUserIdAsync(user.Id);
            if (itmUser != null)
            {
                itmUser.UserName = user.UserName;
                itmUser.Email = user.Email;
                itmUser.PhoneNumber = user.PhoneNumber;
                itmUser.FirstName = "Deleted";
                itmUser.LastName = "User";
                await _itmuserRepo.AddOrUpdateAsync(itmUser);

                Expert expert = await _expertRepo.GetByItmUserIdAsync(itmUser.Id);
                if (expert != null)
                { await _expertRepo.DeleteServicesAsync(expert.Id); }
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            return Redirect("~/");
        }
    }
}
