using CrudApp.Api.Data;
using CrudApp.Api.DTOs;
using CrudApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger  = logger;
    }

    // GET api/products?search=laptop
    [HttpGet]
    [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(s) ||
                (p.Category    != null && p.Category.ToLower().Contains(s)) ||
                (p.Description != null && p.Description.ToLower().Contains(s)));
        }

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(products);
    }

    // GET api/products/5
    [HttpGet("{id:int}")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with ID {id} not found." });

        return Ok(product);
    }

    // POST api/products
    [HttpPost]
    [ProducesResponseType<Product>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var product = new Product
        {
            Name        = dto.Name,
            Description = dto.Description,
            Price       = dto.Price,
            Category    = dto.Category,
            CreatedAt   = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product created — ID: {Id}, Name: {Name}", product.Id, product.Name);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT api/products/5
    [HttpPut("{id:int}")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with ID {id} not found." });

        product.Name        = dto.Name;
        product.Description = dto.Description;
        product.Price       = dto.Price;
        product.Category    = dto.Category;
        product.UpdatedAt   = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Product updated — ID: {Id}, Name: {Name}", product.Id, product.Name);
        return Ok(product);
    }

    // DELETE api/products/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
            return NotFound(new { message = $"Product with ID {id} not found." });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product deleted — ID: {Id}, Name: {Name}", id, product.Name);
        return NoContent();
    }
}
