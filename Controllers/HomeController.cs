using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using landingpage.Models;

namespace landingpage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductsdataContext _db;

        public HomeController(ProductsdataContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "user")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Products()
        {
            var data = _db.Items.Include(item => item.Cat);
            return View(data.ToList());
        }

        [Authorize(Roles = "user")]
        public IActionResult Details(int id)
        {
            var item = _db.Items
                           .Include(i => i.Cat)
                           .FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return RedirectToAction("Products");
            }

            ViewBag.Cart = new Cart(); 

            return View(item);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Cart cart)
        {
            cart.Total = cart.Price * cart.Qty;
            _db.Carts.Add(cart);
            _db.SaveChanges();
            return RedirectToAction("Products");
        }

    }
    }

