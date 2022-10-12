using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneTwoMany_Fiorello.Data;
using OneTwoMany_Fiorello.Models;
using OneTwoMany_Fiorello.Models.Accordion;
using OneTwoMany_Fiorello.Models.Home;
using OneTwoMany_Fiorello.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OneTwoMany_Fiorello.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("name", "Sada");

            Response.Cookies.Append("surname", "Mursaliyeva", new CookieOptions { MaxAge = TimeSpan.FromDays(10) });

            SliderDetail sliderDetail = await _context.SliderDetail.FirstOrDefaultAsync();
            IEnumerable<Slider> sliders = await _context.Sliders.ToListAsync();
            IEnumerable<Category> categories = await _context.Categories.Where(m => m.IsDeleted == false).ToListAsync();
            IEnumerable<Product> products = await _context.Products
                .Where(m => m.IsDeleted == false)
                .Include(m => m.Category)
                .Include(m => m.ProductImages).ToListAsync();
            About about = await _context.Abouts.FirstOrDefaultAsync();
            ExpertTitle expertTitle = await _context.ExpertTitle.FirstOrDefaultAsync();
            IEnumerable<Expert> experts = await _context.Experts.ToListAsync();
            Subscribe subscribe = await _context.Subscribe.FirstOrDefaultAsync();
            BlogTitle blogTitle = await _context.BlogTitle.FirstOrDefaultAsync();
            IEnumerable<Blog> blogs = await _context.Blogs.ToListAsync();
            IEnumerable<Say> says = await _context.Says.ToListAsync();
            IEnumerable<Instagram> instagrams = await _context.Instagrams.ToListAsync();

            HomeVM homeVM = new HomeVM
            {
                SliderDetail = sliderDetail,
                Sliders = sliders,
                Categories = categories,
                Products = products,
                About = about,
                ExpertTitle = expertTitle,
                Experts = experts,
                Subscribe = subscribe,
                BlogTitle = blogTitle,
                Blogs = blogs,
                Says = says,
                Instagrams = instagrams
            };

            return View(homeVM);
        }

        public async Task<IActionResult> Addbasket(int? id)
        {
            //var dbProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            var dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();

            if (id is null) return BadRequest();

            List<BasketVM> basket;

            if (Request.Cookies["surname"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

            basket.Add(new BasketVM
            {
                Id = dbProduct.Id,
                Count = 1
            });

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction("Index");
        }

        //public IActionResult Test()
        //{
        //    var sessionData = HttpContext.Session.GetString("name");
        //    var cookieData = Request.Cookies["surname"];

        //    return Json(sessionData + "-" + cookieData);
        //}
    }
}
