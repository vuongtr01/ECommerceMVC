using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Data;
using ECommerceMVC.Helpers;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;

        public CartController(Hshop2023Context context)
        {
            this.db = context;
        }
        public List<CardItem> Cart => HttpContext.Session.Get<List<CardItem>>(Constants.CART_KEY) ?? new List<CardItem>();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCard(int id, int quantity=1)
        {
            var currentCart = Cart;
            var item = currentCart.SingleOrDefault(p => p.MaHH == id);
            if(item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    return Redirect("/404");
                }
                item = new CardItem
                {
                    MaHH = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? "",
                    SoLuong = quantity
                };
                currentCart.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(Constants.CART_KEY, currentCart);
            return RedirectToAction("index");
        }

        public IActionResult RemoveCart(int id)
        {
            var currentCart = Cart;
            var item = currentCart.SingleOrDefault(p => p.MaHH == id);

            if (item != null)
            {
                currentCart.Remove(item);
                HttpContext.Session.Set(Constants.CART_KEY, currentCart);
            }
            return RedirectToAction("index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var myCart = Cart;
            if(myCart.Count == 0)
            {
                return Redirect("/");
            }

            return View(myCart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            var myCart = Cart;
            if (ModelState.IsValid)
            {
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "CustomerId").Value;
               
                var hoaDon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "Prime",
                    MaTrangThai = 0,
                    GhiChu = model.GhiChu,
                };
                db.Database.BeginTransaction();
                try
                {
                    db.Database.CommitTransaction();
                    db.Add(hoaDon);
                    db.SaveChanges();

                    var cthd = new List<ChiTietHd>();
                    foreach(var it in Cart)
                    {
                        cthd.Add(new ChiTietHd
                        {
                            MaHd = hoaDon.MaHd,
                            SoLuong = it.SoLuong,
                            DonGia = it.DonGia,
                            MaHh = it.MaHH,
                            GiamGia = 0,
                        });
                    }
                    db.AddRange(cthd);
                    db.SaveChanges();
                    HttpContext.Session.Set<List<CardItem>>(Constants.CART_KEY, new List<CardItem>());

                    return View("Success");
                } catch
                {
                    db.Database.RollbackTransaction();
                }
            }
            else
            {
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
            return View(myCart);
        }
    }
}

