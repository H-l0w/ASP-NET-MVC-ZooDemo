using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZooDemo.Models;
using ZooDemo.Objects;
using ZooDemo.Repos;

namespace ZooDemo.Controllers
{
    public class AnimalController : Controller
    {
        private AnimalRepo _repo;

        public AnimalController(AnimalRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            List<Animal> animals = _repo.GetAnimals();
            AnimalsViewModel model = new AnimalsViewModel(animals);
            return View(model);
        }

        public IActionResult Search(string search)
        {
            List<Animal> animals = _repo.FindAnimals(search);
            AnimalsViewModel model = new AnimalsViewModel(animals);
            return View("/Views/Animal/Index.cshtml",model);
        }
    }
}
