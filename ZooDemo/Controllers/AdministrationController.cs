using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZooDemo.Models;
using ZooDemo.Objects;
using ZooDemo.Repos;

namespace ZooDemo.Controllers
{
    public class AdministrationController : Controller
    {
        private AnimalRepo _animalRepo;
        private PavilonRepo _pavilonRepo;
        private AnimalTypeRepo _animalTypeRepo;
        private ImageRepo _imageRepo;

        public AdministrationController(AnimalRepo animalRepo, PavilonRepo pavilonRepo, AnimalTypeRepo animalTypeRepo, ImageRepo imageRepo)
        {
            _animalRepo = animalRepo;
            _pavilonRepo = pavilonRepo;
            _animalTypeRepo = animalTypeRepo;
            _imageRepo = imageRepo;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            List<Animal> animals = _animalRepo.GetAnimals();
            List<Pavilon> pavilons = _pavilonRepo.GetPavilons();
            List<AnimalType> animalTypes = _animalTypeRepo.GetAnimalTypes();
            AdministrationViewModel model = new AdministrationViewModel(animals, pavilons, animalTypes);
            ViewBag.Error = TempData["Error"];
            return View(model);
        }
        #region Delete
        public IActionResult DeleteAnimal(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            _imageRepo.DeleteImage(_animalRepo.GetImagePathForAnimal(id));
            _animalRepo.Remove(new Animal(id));
            return Redirect("/Administration");
        }

        public IActionResult DeletePavilon(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            Pavilon temp = new Pavilon(id);
            if (_pavilonRepo.CanBeDeleted(temp)){
                _pavilonRepo.Remove(temp);
                return Redirect("/Administration");
            }
            TempData["Error"] = "Nelze smazat pavilon ve kterém jsou zvířata";
            return Redirect("/Administration");
        }

        public IActionResult DeleteAnimalType(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            AnimalType temp = new AnimalType(id);
            if (_animalTypeRepo.CanBeDeleted(temp)) {
                _animalTypeRepo.Remove(temp);
                return Redirect("/Administration");
            }
            TempData["Error"] = "Nelze smazat druh ve kterém jsou zvířata";
            return Redirect("/Administration");
        }
        #endregion
        #region Add
        [HttpGet]
        public IActionResult AddAnimalType()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            return View();
        }

        [HttpPost]
        public IActionResult AddAnimalType([FromForm] AnimalType animal)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            if (!_animalTypeRepo.Exists(animal.Name)) {
                _animalTypeRepo.Add(animal);
                return Redirect("/Administration");
            }
            else {
                ViewBag.Error = "Zadaný druh již existuje";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddPavilon()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            return View();
        }

        [HttpPost]
        public IActionResult AddPavilon([FromForm] Pavilon pavilon)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            if (!_pavilonRepo.Exists(pavilon.Name)) {
                _pavilonRepo.Add(pavilon);
                return Redirect("/Administration");
            }
            else {
                ViewBag.Error = "Zadaný pavilon již existuje";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddAnimal()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            AddUpdateAnimalViewModel model = new AddUpdateAnimalViewModel();
            model.Pavilons = _pavilonRepo.GetPavilons();
            model.AnimalTypes = _animalTypeRepo.GetAnimalTypes();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddAnimal([FromForm]Animal animal)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

                if (_animalRepo.Exists(animal)) {
                    ViewBag.Error = "Zadané zvíře již existuje";
                    AddUpdateAnimalViewModel model = new AddUpdateAnimalViewModel();
                    model.Pavilons = _pavilonRepo.GetPavilons();
                    model.AnimalTypes = _animalTypeRepo.GetAnimalTypes();
                    return View(model);
                }

                var file = Request.Form.Files["Animal.ImagePath"];
                Stream stream = file.OpenReadStream();

                var form = Request.Form;
                Animal animalFromFile = new Animal();
                animal.Name = form["Animal.Name"];
                animal.AnimalName = form["Animal.AnimalName"];
                animal.DateOfBirth = Convert.ToDateTime(form["Animal.DateOfBirth"]);
                animal.IdPavilon = Convert.ToInt32(form["Animal.IdPavilon"]);
                animal.IdAnimalType = Convert.ToInt32(form["Animal.IdAnimalType"]);
                animal.ImagePath = animal.AnimalName + animal.Name + ".png";
                animal.ImagePath = animal.ImagePath.Replace(' ', '_');
                _animalRepo.Add(animal);
                _imageRepo.AddImage(stream, animal.ImagePath);
                return Redirect("/Administration");
        }
        #endregion
        #region Update

        [HttpGet]
        public IActionResult UpdateAnimal(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            AddUpdateAnimalViewModel model = new AddUpdateAnimalViewModel();
            model.Pavilons = _pavilonRepo.GetPavilons();
            model.AnimalTypes = _animalTypeRepo.GetAnimalTypes();
            model.Animal = _animalRepo.GetAnimal(new Animal(id));
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateAnimal(int id, [FromForm]Animal a)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            //update
            //update without date and image
            a.Id = id;
            a.ImagePath = Request.Form.Files.Count > 0 ? Request.Form.Files["ImagePath"].FileName : "";

            if (Request.Form["Animal.DateOfBirth"] == "" && Request.Form["ImagePath"] == "") { //update without image and dateofbirth
                _animalRepo.UpdateWithoutDateAndPhoto(a);
            }
            else if (Request.Form["ImagePath"] == "") { //update without image
                _animalRepo.UpdateWithoutPhoto(a);
            }
            else if (Request.Form["Animal.DateOfBirth"] == "") { //update without dateofbirth

            }
            else { //full update
                a.ImagePath = a.AnimalName + a.Name + ".png";
                string oldImageName = _animalRepo.GetImagePathForAnimal(a.Id);
                var file = Request.Form.Files["ImagePath"];

                _imageRepo.UpdateImage(oldImageName, a.ImagePath, file.OpenReadStream());
                _animalRepo.Upadte(a);
            }
            return Redirect("/Administration");
        }

        [HttpGet]
        public IActionResult UpdatePavilon(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            Pavilon model = new Pavilon();
            model = _pavilonRepo.GetPavilon(new Pavilon(id));
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePavilon(int id, [FromForm] Pavilon p)
        {
            p.Id = id;
            _pavilonRepo.Upadte(p);
            return Redirect("/Administration");
        }

        [HttpGet]
        public IActionResult UpdateAnimalType(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            AnimalType model = new AnimalType(id);
            model = _animalTypeRepo.GetAnimalType(new AnimalType(id));
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateAnimalType(int id, [FromForm] AnimalType t)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            t.Id = id;
            _animalTypeRepo.Update(t);
            return Redirect("/Administration");
        }
        #endregion
    }
}
