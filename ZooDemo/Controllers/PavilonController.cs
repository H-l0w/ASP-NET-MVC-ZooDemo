using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZooDemo.Models;
using ZooDemo.Objects;
using ZooDemo.Repos;

namespace ZooDemo.Controllers
{
    public class PavilonController : Controller
    {
        private PavilonRepo _repo;

        public PavilonController(PavilonRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            List<Pavilon> pavilons = new List<Pavilon>();
            pavilons = _repo.GetPavilons();
            PavilonViewModel model = new PavilonViewModel();
            model.Pavilons = pavilons;
            return View(model);
        }
    }
}
