
using System.ComponentModel.DataAnnotations;

public class Student {

    [Required]
    public int Id { get; set; }

    [Required]
    public required string Identifier { get; set; }
    [Required]
    [StringLength(30)]
    public required string FirstName {get; set; }
    [StringLength(30)]
    [Required]
    public required string LastName {get; set; }
    [Required]
    [StringLength(100)]
    public required string Degree {get; set; }
    [Required]
    public required int Age {get; set; }
}