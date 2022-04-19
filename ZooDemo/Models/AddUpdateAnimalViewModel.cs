using System.Collections.Generic;
using ZooDemo.Objects;

namespace ZooDemo.Models
{
    public class AddUpdateAnimalViewModel
    {
        public List<AnimalType> AnimalTypes { get; set; }
        public List<Pavilon> Pavilons { get; set; }
        public Animal Animal { get; set; }
    }
}
