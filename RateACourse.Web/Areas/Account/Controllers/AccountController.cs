using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateACourse.Core.Entities;
using RateACourse.Web.Areas.Account.ViewModels;

namespace RateACourse.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<Student> _userManager;
        private readonly SignInManager<Student> _signInManager;

        public AccountController(UserManager<Student> userManager, SignInManager<Student> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            AccountLoginViewModel accountLoginViewModel = new AccountLoginViewModel();
            accountLoginViewModel.PageTitle = "Login";
            return View(accountLoginViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountLoginViewModel);
            }
            //login user = check credentials
            var result = await _signInManager.PasswordSignInAsync(accountLoginViewModel.Username,
                accountLoginViewModel.Password,false,false);//lockoutonfailure on true in production code
            if(result.Succeeded == false)
            {
                ModelState.AddModelError("", "Wrong credentials!");
                return View(accountLoginViewModel);
            }
            return RedirectToAction("Index","Home",new {Area=""});
        }
        [HttpGet]
        public IActionResult Register()
        {
            AccountRegisterViewModel accountRegisterViewModel = new();
            accountRegisterViewModel.PageTitle = "Register";
            return View(accountRegisterViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterViewModel accountRegisterViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(accountRegisterViewModel);
            }
            //register user 
            if(await _userManager.FindByNameAsync(accountRegisterViewModel.Username) == null)
            {
                //create user
                Student newStudent = new Student
                {
                    UserName = accountRegisterViewModel.Username,
                    FirstName = accountRegisterViewModel.Firstname,
                    LastName = accountRegisterViewModel.Lastname,
                    Email = accountRegisterViewModel.Username,
                    EmailConfirmed = true, //just for testing purposes,in production this is done by emailconfirmation
                };
                var result = await _userManager.CreateAsync(newStudent,accountRegisterViewModel.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new {Area = "" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(accountRegisterViewModel);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home", new {Area = "" });
        }
        [HttpGet]
        public IActionResult AccesDenied() 
        {
            return View();        
        }
    }
}
