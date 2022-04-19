using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooDemo.Objects
{
    [Table("tbPavilon")]
    public class Pavilon
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<Animal> AnimalsInPavilon { get; set; }

        public Pavilon()
        {

        }

        public Pavilon(int id)
        {
            Id = id;
        }
    }
}
