using BnsBazarApp.Models.DTOs;
using BnsBazarApp.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BnsBazarApp.Controllers
{
    // Controller responsible for handling login, logout and account creation
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _repository;

        public AuthenticationController(IAuthenticationRepository repository)
        {
            _repository = repository;
        }

        // -----------------------------
        // LOGIN (GET)
        // -----------------------------
        public IActionResult Login()
        {
            return View();
        }

        // -----------------------------
        // LOGIN (POST) - CLAIMS BASED
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = _repository.Authenticate(loginDTO);

            if (user == null)
            {
                ViewBag.LoginMessage = "Email or Password Incorrect";
                return View(loginDTO);
            }

            // 🔐 CREATE CLAIMS
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            // 🔑 SIGN IN (COOKIE)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Home");
        }

        // -----------------------------
        // LOGOUT
        // -----------------------------
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Index", "Home");
        }

        // -----------------------------
        // CREATE ACCOUNT (GET)
        // -----------------------------
        public IActionResult Create()
        {
            return View();
        }

        // -----------------------------
        // CREATE ACCOUNT (POST)
        // -----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserDTO userDTO)
        {
            if (!userDTO.Password.Equals(userDTO.PasswordConfirmation))
            {
                ViewBag.CreateUserError = "Password and confirmation do not match";
                return View(userDTO);
            }

            if (!ModelState.IsValid)
            {
                return View(userDTO);
            }

            var user = _repository.CreateUser(userDTO);

            if (user == null)
            {
                ViewBag.CreateUserError = "Username already exists.";
                return View(userDTO);
            }

            return RedirectToAction("Login");
        }

        // -----------------------------
        // ACCESS DENIED (OPTIONAL)
        // -----------------------------
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}