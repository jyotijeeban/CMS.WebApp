using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.DataAccess
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string CountryName { get; set; }
    }
}
