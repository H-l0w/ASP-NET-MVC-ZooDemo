using System.Collections.Generic;
using ZooDemo.Objects;

namespace ZooDemo.Models
{
    public class AnimalsViewModel
    {
        public List<Animal> Animals { get; set; }

        public string Search { get; set; }

        public AnimalsViewModel(List<Animal> animals)
        {
            Animals = animals;
            Search = string.Empty;
        }
    }
}
