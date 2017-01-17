namespace TheWorld.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using TheWorld.Models;
    using TheWorld.ViewModels;

    public class AuthController : Controller
    {
        private SignInManager<WorldUser> signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Trips", "App");
                
            }

            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel login, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var signInResult = await this.signInManager.PasswordSignInAsync(
                                       login.Username,
                                       login.Password,
                                       true,
                                       false);

                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return this.RedirectToAction("Trips", "App");
                    }

                    return this.Redirect(returnUrl);
                }

                this.ModelState.AddModelError(string.Empty, "Username or password incorrect");
            }

            return this.View();
        }

        public async Task<ActionResult> Logout()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                await this.signInManager.SignOutAsync();
            }

            return this.RedirectToAction("Index", "App");
        }
    }
}