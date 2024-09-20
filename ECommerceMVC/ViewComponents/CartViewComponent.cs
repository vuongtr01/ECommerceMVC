using System;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;

namespace ECommerceMVC.ViewComponents
{
	public class CartViewComponent : ViewComponent
	{
		public CartViewComponent()
		{
		}

		public IViewComponentResult Invoke()
		{
			var cart = HttpContext.Session.Get<List<CardItem>>(Constants.CART_KEY) ?? new List<CardItem>();
		
			return View("CartPanel", new CartModel
			{
				Quantity = cart.Sum(p => p.SoLuong),
				Total = cart.Sum(p => p.ThanhTien)
			});
		}
	}
}

