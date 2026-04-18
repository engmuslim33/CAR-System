using CarRentalSystem.Data;
using CarRentalSystem.DTOs;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CustomersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> AddCustomer([FromBody] CreateCustomerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Phone))
        {
            return BadRequest("Customer name and phone are required.");
        }

        var customer = new Customer
        {
            Name = request.Name.Trim(),
            Phone = request.Phone.Trim()
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        return Ok(customer);
    }
}
