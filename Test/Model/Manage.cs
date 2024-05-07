using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageInformation.Model
{
    public class Manage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }
}