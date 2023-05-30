using DAL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using PublicModule.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PublicModule.Controllers
{
    public class AuthorizationController : Controller
    {
        private IUserRepository _userRepo;
        private RwaMoviesContext _db;

        public AuthorizationController(IUserRepository userRepo, RwaMoviesContext db)
        {
            _userRepo = userRepo;
            _db = db;
        }

        // GET: AuthorizationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuthorizationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthorizationController/Create
        public ActionResult Register()
        {
            ViewData["Countries"] = _db.Countries.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Register(VMRegister input)
        {
            try
            {
                _userRepo.CreateUser(
                    input.Username,
                    input.FirstName,
                    input.LastName,
                    input.Email,
                    input.Password,
                    input.CountryOfResidence,
                    input.PhoneNumber
                    );
            }
            catch
            {
                return NotFound();
            }

            return Redirect("LogIn");
        }

        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(VMLogIn credentials)
        {
            if (credentials.Username.Equals("admin") && credentials.Password.Equals("11111111"))
                return RedirectToAction("Index", "Admin");

            try
            {
                var user = _userRepo.GetConfirmedUser(credentials.Username, credentials.Password);
                if (user is null)
                {
                    return BadRequest();
                }
                HttpContext.Response.Cookies.Append(
            "SessionIDCookie",
       GetJWTToken(),
       new CookieOptions
       {
           Expires = DateTimeOffset.Now.AddHours(2), 
           Domain = Request.Host.Host,
           Path = "/",
       });
                ViewData["userLogged"] = user.Username;
                return RedirectToAction(nameof(Index),"Home");
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction(nameof(LogIn));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: AuthorizationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorizationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorizationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorizationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorizationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private static string GetJWTToken()
        {
            // Get secret key bytes
            var tokenKey = Encoding.UTF8.GetBytes("12345678901234567890123456789012");

            // Create a token descriptor (represents a token, kind of a "template" for token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create token using that descriptor, serialize it and return it
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return serializedToken;
        }
    }
}
