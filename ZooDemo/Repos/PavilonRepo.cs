using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ZooDemo.Data;
using Microsoft.Extensions.Options;
using ZooDemo.Objects;

namespace ZooDemo.Repos
{
    public class PavilonRepo
    {
        private readonly ApplicationDbContext _context;

        public PavilonRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Exists(string name)
        {
            return _context.Pavilons.FirstOrDefault(p => p.Name == name) != null;
        }

        public bool ExistsById(int id)
        {
            return _context.Pavilons.FirstOrDefault(p => p.Id == id) != null;
        }

        public void Add(Pavilon pavilon)
        {
            _context.Add(pavilon);
            _context.SaveChanges();
        }

        public void Remove(Pavilon pavilon)
        {
            _context.Remove(pavilon);
            _context.SaveChanges();
        }

        public void Upadte(Pavilon pavilon)
        {
            _context.Update(pavilon);
            _context.SaveChanges();
        }

        public List<Pavilon> GetPavilons()
        {
            List<Pavilon> pavilons = _context.Pavilons.ToList();
            foreach (Pavilon p in pavilons) {
                p.AnimalsInPavilon = GetAnimalsForPavilon(p);
            }
            return pavilons;
        }

        private List<Animal> GetAnimalsForPavilon(Pavilon pavilon)
        {
            return _context.Animals.Where(a => a.IdPavilon == pavilon.Id).ToList();
        }

        public bool CanBeDeleted(Pavilon pavilon)
        {
            return _context.Animals.Where(a => a.IdPavilon == pavilon.Id).Count() == 0;
        }

        public Pavilon GetPavilon(Pavilon pavilon)
        {
            return _context.Pavilons.FirstOrDefault(p => p.Id == pavilon.Id);
        }
    }
}
