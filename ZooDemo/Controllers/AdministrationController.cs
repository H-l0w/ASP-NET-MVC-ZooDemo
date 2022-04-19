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
        public IActionResult AddAnimalType(AnimalType animal)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            if (animal.Info == null || animal.Name == null)
                return View();
            else {
                if (!_animalTypeRepo.Exists(animal.Name)) {
                    _animalTypeRepo.Add(animal);
                    return Redirect("/Administration");
                }
                else {
                    ViewBag.Error = "Zadaný druh již existuje";
                    return View();
                }
            }
        }
        
        public IActionResult AddPavilon(Pavilon pavilon)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            if (pavilon.Name == null)
                return View();
            else {
                if (!_pavilonRepo.Exists(pavilon.Name)) {
                    _pavilonRepo.Add(pavilon);
                    return Redirect("/Administration");
                }
                else {
                    ViewBag.Error = "Zadaný pavilon již existuje";
                    return View();
                }
            }
        }

        public IActionResult AddAnimal(Animal animal)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");
            if (animal.Name == null) //|| animal.AnimalName == null || animal.AnimalType == null || animal.Pavilon == null
            {
                AddUpdateAnimalViewModel model = new AddUpdateAnimalViewModel();
                model.Pavilons = _pavilonRepo.GetPavilons();
                model.AnimalTypes = _animalTypeRepo.GetAnimalTypes();
                return View(model);
            }
            else {
                if (_animalRepo.Exists(animal)) {
                    ViewBag.Error = "Zadané zvíře již existuje";
                    AddUpdateAnimalViewModel model = new AddUpdateAnimalViewModel();
                    model.Pavilons = _pavilonRepo.GetPavilons();
                    model.AnimalTypes = _animalTypeRepo.GetAnimalTypes();
                    return View(model);
                }

                var file = Request.Form.Files["ImagePath"];
                Stream stream = file.OpenReadStream();

                var form = Request.Form;
                Animal animalFromFile = new Animal();
                animal.Name = form["Animal.Name"];
                animal.AnimalName = form["Animal.AnimalName"];
                animal.DateOfBirth = Convert.ToDateTime(form["DateOfBirth"]);
                animal.IdPavilon = Convert.ToInt32(form["IdPavilon"]);
                animal.IdAnimalType = Convert.ToInt32(form["IdAnimalType"]);
                animal.ImagePath = animal.AnimalName + animal.Name + ".png";
                _animalRepo.Add(animal);
                _imageRepo.AddImage(stream, animal.ImagePath);
                return Redirect("/Administration");
            }
        }
        #endregion
        #region Update
        public IActionResult UpdateAnimal(int id)
        {
            //needs to be done
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            if (!Request.HasFormContentType) {
                AddUpdateAnimalViewModel model = new AddUpdateAnimalViewModel();
                model.Pavilons = _pavilonRepo.GetPavilons();
                model.AnimalTypes = _animalTypeRepo.GetAnimalTypes();
                model.Animal = _animalRepo.GetAnimal(new Animal(id));
                return View(model);
            }
            //update
            else {
                //update without date and image
                Animal animal = new Animal();
                animal.Id = id;
                animal.Name = Request.Form["Name"];
                animal.AnimalName = Request.Form["AnimalName"];
                animal.DateOfBirth = Request.Form["DateOfBirth"] == "" ? DateTime.Now : Convert.ToDateTime(Request.Form["DateOfBirth"]); 
                animal.ImagePath = Request.Form.Files.Count > 0 ? Request.Form.Files["ImagePath"].FileName : "";
                animal.IdAnimalType = Convert.ToInt32(Request.Form["IdAnimalType"]);
                animal.IdPavilon = Convert.ToInt32(Request.Form["IdPavilon"]);

                if (Request.Form["DateOfBirth"] == "" && Request.Form["ImagePath"] == "") { //update without image and dateofbirth
                    _animalRepo.UpdateWithoutDateAndPhoto(animal);
                }
                else if (Request.Form["ImagePath"] == "") { //update without image
                    _animalRepo.UpdateWithoutPhoto(animal);
                }
                else if (Request.Form["DateOfBirth"] == "") { //update without dateofbirth

                }
                else { //full update
                    animal.ImagePath = animal.AnimalName + animal.Name + ".png";
                    string oldImageName = _animalRepo.GetImagePathForAnimal(animal.Id);
                    var file = Request.Form.Files["ImagePath"];

                    _imageRepo.UpdateImage(oldImageName, animal.ImagePath, file.OpenReadStream());
                    _animalRepo.Upadte(animal);
                }
                return Redirect("/Administration");
            }
        }
        public IActionResult UpdatePavilon(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            if (!Request.HasFormContentType) {
                Pavilon model = new Pavilon();
                model = _pavilonRepo.GetPavilon(new Pavilon(id));
                return View(model);
            }
            else {
                Pavilon pavilon = new Pavilon(id);
                pavilon.Name = Request.Form["Name"];
                _pavilonRepo.Upadte(pavilon);
                return Redirect("/Administration");
            }
        }
        public IActionResult UpdateAnimalType(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            if (!Request.HasFormContentType) {
               AnimalType model = new AnimalType(id);
               model = _animalTypeRepo.GetAnimalType(new AnimalType(id));
                return View(model);
            }
            else {
                AnimalType type = new AnimalType();
                type.Name = Request.Form["Name"];
                type.Info = Request.Form["Info"];
                _animalTypeRepo.Update(type);
                return Redirect("/Administration");
            }
        }
        #endregion
    }
}
