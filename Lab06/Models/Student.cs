using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab06.Models
{
    public class Student
    {
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; set; }

        [Required]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Column("LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Column("Program")]
        [Display(Name = "Program")]
        public string Program { get; set; }


    }
}
