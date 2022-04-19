using System.Collections.Generic;
using System.Linq;
using ZooDemo.Data;
using ZooDemo.Objects;

namespace ZooDemo.Repos
{
    public class AnimalTypeRepo
    {
        private readonly ApplicationDbContext _context;

        public AnimalTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Exists(string name)
        {
            return _context.AnimalTypes.FirstOrDefault(p => p.Name == name) != null;
        }

        public void Add(AnimalType animalType)
        {
            _context.Add(animalType);
            _context.SaveChanges();
        }

        public void Remove(AnimalType animalType)
        {
            _context.Remove(animalType);
            _context.SaveChanges();
        }

        public void Update(AnimalType animalType)
        {
            _context.Update(animalType);
            _context.SaveChanges();
        }

        public List<AnimalType> GetAnimalTypes()
        {
            List<AnimalType> types = _context.AnimalTypes.ToList();
            foreach (AnimalType type in types) {
                type.AnimalsInType = GetAnimalsInType(type);  
            }
            return types;
        }

        private List<Animal> GetAnimalsInType(AnimalType type)
        {
            return _context.Animals.Where(t => t.AnimalType == type).ToList();
        }

        public bool CanBeDeleted(AnimalType type)
        {
            return _context.Animals.Where(a => a.IdAnimalType == type.Id).Count() == 0;
        }

        public AnimalType GetAnimalType(AnimalType type)
        {
            return _context.AnimalTypes.FirstOrDefault(t => t.Id == type.Id);
        }
    }
}
