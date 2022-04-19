using System.Collections.Generic;
using ZooDemo.Objects;

namespace ZooDemo.Models
{
    public class AdministrationViewModel
    {
        public List<Animal> Animals { get; set; }
        public List<Pavilon> Pavilon { get; set; }
        public List<AnimalType> AnimalTypes { get; set; }

        public AdministrationViewModel(List<Animal> animals, List<Pavilon> pavilons, List<AnimalType> types)
        {
            Animals = animals;
            Pavilon = pavilons;
            AnimalTypes = types;
        }
    }
}
