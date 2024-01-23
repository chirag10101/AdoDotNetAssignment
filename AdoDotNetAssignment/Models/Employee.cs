using System.ComponentModel.DataAnnotations;

namespace AdoDotNetAssignment.Models
{
    public class Employee
    {
        
            public int Id { get; set; }
            [Required]
            [MinLength(15)]
            [RegularExpression("[a-z A-z]+")]
            public string Name { get; set; }
            [Required]
            public DateOnly Doj { get; set; }
            [Required]
            [Range(20000, 50000)]
            public int Salary { get; set; }

    }
}
