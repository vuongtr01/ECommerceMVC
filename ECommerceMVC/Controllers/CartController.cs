using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;

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
    }
}

