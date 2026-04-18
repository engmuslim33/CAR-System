using CarRentalSystem.Data;
using CarRentalSystem.DTOs;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CarsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Car>> AddCar([FromBody] CreateCarRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.DailyRate <= 0)
        {
            return BadRequest("Car name is required and daily rate must be greater than zero.");
        }

        var car = new Car
        {
            Name = request.Name.Trim(),
            DailyRate = request.DailyRate,
            IsAvailable = true
        };

        _db.Cars.Add(car);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllCars), new { id = car.Id }, car);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
    {
        var cars = await _db.Cars.AsNoTracking().ToListAsync();
        return Ok(cars);
    }
}
