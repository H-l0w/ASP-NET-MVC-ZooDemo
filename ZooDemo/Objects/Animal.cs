using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ZooDemo.Objects
{
    [Table("tbAnimal")]
    public class Animal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string AnimalName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey("Pavilon")]
        public int IdPavilon { get; set; }

        public virtual Pavilon Pavilon { get; set; }

        [ForeignKey("AnimalType")]
        public int IdAnimalType { get; set; }
        
        public virtual AnimalType AnimalType { get; set; }

        public Animal()
        {

        }

        public Animal(int id)
        {
            Id = id;
        }
    }
}
