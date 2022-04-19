using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZooDemo.Data;
using ZooDemo.Models;
using ZooDemo.Objects;
using ZooDemo.Repos;

namespace ZooDemo.Controllers
{
    public class TypeController : Controller
    {
        private AnimalTypeRepo _repo;

        public TypeController(AnimalTypeRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            List<AnimalType> types = _repo.GetAnimalTypes();
            TypeViewModel model = new TypeViewModel();
            model.AnimalTypes = types;
            return View(model);
        }
    }
}
