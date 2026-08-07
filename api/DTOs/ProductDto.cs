using System.ComponentModel.DataAnnotations;

namespace CrudApp.Api.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public required string Name { get; set; }

    [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be zero or greater.")]
    public decimal Price { get; set; }

    [MaxLength(50, ErrorMessage = "Category must be at most 50 characters.")]
    public string? Category { get; set; }
}

public class UpdateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public required string Name { get; set; }

    [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be zero or greater.")]
    public decimal Price { get; set; }

    [MaxLength(50, ErrorMessage = "Category must be at most 50 characters.")]
    public string? Category { get; set; }
}
