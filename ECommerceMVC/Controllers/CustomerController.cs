using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceMVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;

        // GET: /<controller>/
        public CustomerController(Hshop2023Context context, IMapper mapper)
        {
            this.db = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKey = Utilities.GenerateRandomKey();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;
                    khachHang.VaiTro = 0;

                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "HangHoa");
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                // Log or display the model state errors
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var errors = state.Value.Errors;
                    foreach (var error in errors)
                    {
                        // Log the errors, for example, by writing to the console or adding it to the view's model
                        Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult LogIn(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM model, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(ModelState.IsValid)
            {
                var customer = db.KhachHangs.SingleOrDefault(c => c.MaKh == model.UserName);
                if(customer == null)
                {
                    ModelState.AddModelError("error", "Username is not exist");
                } else
                {
                    if (!customer.HieuLuc)
                    {
                        ModelState.AddModelError("error", "User is not active. Contact Admin!!!");
                    }
                    else
                    {
                        if (customer.MatKhau != model.Password.ToMd5Hash(customer.RandomKey))
                        {
                            ModelState.AddModelError("error", "Password is incorrect");
                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, customer.Email),
                                new Claim(ClaimTypes.Name, customer.HoTen),
                                new Claim("CustomerId", customer.MaKh),
                                new Claim(ClaimTypes.Role, "Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrinciple);

                            if(Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            } else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }

            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}

