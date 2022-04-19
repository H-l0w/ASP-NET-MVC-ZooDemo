using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooDemo.Objects
{
    [Table("tbAnimalType")]
    public class AnimalType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        [NotMapped]
        public List<Animal> AnimalsInType { get; set; }

        public AnimalType()
        {

        }

        public AnimalType(int id)
        {
            Id = id;
        }
    }
}
