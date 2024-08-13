using landingpage.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using landingpage.Models;

namespace TempEmbeddin2302C2.Controllers
{
    public class ItemController : Controller
    {
        //_2302c2ecommerceContext db = new _2302c2ecommerceContext();

        private readonly ProductsdataContext db;

        public ItemController(ProductsdataContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var data = db.Items.Include(item => item.Cat);
            return View(data.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Item item, IFormFile file)
        {
            string imagename = DateTime.Now.ToString("yymmddhhmmss");//2410152541245412
            imagename += "-" + Path.GetFileName(file.FileName);//2410152541245412-sonata.jpg

            var imagepath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Uploads");
            var imageValue = Path.Combine(imagepath, imagename);

            using (var stream = new FileStream(imageValue, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var dbimage = Path.Combine("/Uploads", imagename);//Uploads/2410152541245412-sonata.jpg

            item.Pimage = dbimage;

            db.Items.Add(item);
            db.SaveChanges();

            ViewBag.CatId = new SelectList(db.Categories, "CatId", "CatName");
            return RedirectToAction("Index");
        }



    }
}