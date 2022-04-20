using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ZooDemo.Models;
using ZooDemo.Objects;
using ZooDemo.Repos;

namespace ZooDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ImageRepo _imageRepo;

        public HomeController(ILogger<HomeController> logger, ImageRepo repo)
        {
            _logger = logger;
            _imageRepo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Pricing()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OpeningHours()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Gallery()
        {
            GalleryViewModel model = new GalleryViewModel();
            model.Images = _imageRepo.GetGalleryImages();
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteImage(string name)
        {
            _imageRepo.RemoveImageGallery(name);
            return Redirect("/Home/Gallery");
        }

        [HttpPost]
        public IActionResult AddImageGallery([FromForm] Image image)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            var file = Request.Form.Files["ImagePath"];

            image.Added = DateTime.Now;
            image.Name = Guid.NewGuid().ToString();
            _imageRepo.AddImageGallery(file.OpenReadStream(), image);
            return Redirect("/Home/Gallery");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
