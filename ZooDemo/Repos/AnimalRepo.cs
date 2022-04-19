using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZooDemo.Data;
using ZooDemo.Objects;

namespace ZooDemo.Repos
{
    public class AnimalRepo
    {
        private ApplicationDbContext _context;

        public AnimalRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Animal animal)
        {
            _context.Add(animal);
            _context.SaveChanges();
        }

        public void Upadte(Animal animal)
        {
            _context.Update(animal);
            _context.SaveChanges();
        }

        public void UpdateWithoutPhoto(Animal animal)
        {
            _context.Animals.Attach(animal);
            _context.Entry(animal).Property(p => p.Name).IsModified = true;
            _context.Entry(animal).Property(p => p.AnimalName).IsModified = true;
            _context.Entry(animal).Property(p => p.DateOfBirth).IsModified = true;
            _context.Entry(animal).Property(p => p.IdPavilon).IsModified = true;
            _context.Entry(animal).Property(p => p.IdAnimalType).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateWithoudDate(Animal animal)
        {
            _context.Animals.Attach(animal);
            _context.Entry(animal).Property(p => p.Name).IsModified = true;
            _context.Entry(animal).Property(p => p.AnimalName).IsModified = true;
            _context.Entry(animal).Property(p => p.ImagePath).IsModified = true;
            _context.Entry(animal).Property(p => p.IdPavilon).IsModified = true;
            _context.Entry(animal).Property(p => p.IdAnimalType).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateWithoutDateAndPhoto(Animal animal)
        {
            _context.Animals.Attach(animal);
            _context.Entry(animal).Property(p => p.Name).IsModified = true;
            _context.Entry(animal).Property(p => p.AnimalName).IsModified = true;
            _context.Entry(animal).Property(p => p.IdPavilon).IsModified = true;
            _context.Entry(animal).Property(p => p.IdAnimalType).IsModified = true;
            _context.SaveChanges();
        }

        public void Remove(Animal animal)
        {
            _context.Remove(animal);
            _context.SaveChanges();
        }

        public Animal GetAnimal(Animal animal)
        {
            return _context.Animals.FirstOrDefault(a => a.Id == animal.Id);
        }

        public List<Animal> GetAnimals()
        {
            return _context.Animals.Include(a => a.Pavilon).Include(a => a.AnimalType).ToList();
        }

        public List<Animal> FindAnimals(string search)
        {
            if (string.IsNullOrEmpty(search))
                return GetAnimals();
            return _context.Animals.Include(a => a.Pavilon).Include(a => a.AnimalType)
                .Where(a => a.Name.ToLower().Contains(search.ToLower()) 
            || a.AnimalName.ToLower().Contains(search.ToLower())).ToList();
        }

        public string GetImagePathForAnimal(int id)
        {
            return _context.Animals.AsNoTracking().FirstOrDefault(a => a.Id == id).ImagePath;
        }

        public bool Exists(Animal animal)
        {
            Animal name, animalName;
            name = _context.Animals.FirstOrDefault(a => a.Name == animal.Name);
            animalName = _context.Animals.FirstOrDefault(a => a.AnimalName == animal.AnimalName);
            return name != null && animalName != null;
        }
    }
}