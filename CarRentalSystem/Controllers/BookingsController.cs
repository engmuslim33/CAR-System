using CarRentalSystem.Data;
using CarRentalSystem.DTOs;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public BookingsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (request.StartDate.Date > request.EndDate.Date)
        {
            return BadRequest("StartDate cannot be after EndDate.");
        }

        var car = await _db.Cars.FindAsync(request.CarId);
        if (car is null)
        {
            return NotFound($"Car with ID {request.CarId} not found.");
        }

        var customer = await _db.Customers.FindAsync(request.CustomerId);
        if (customer is null)
        {
            return NotFound($"Customer with ID {request.CustomerId} not found.");
        }

        var isAvailable = await IsCarAvailable(request.CarId, request.StartDate.Date, request.EndDate.Date);
        if (!isAvailable)
        {
            return BadRequest("This car is already booked for the selected date range.");
        }

        var totalDays = (request.EndDate.Date - request.StartDate.Date).Days + 1;

        var booking = new Booking
        {
            CarId = request.CarId,
            CustomerId = request.CustomerId,
            StartDate = request.StartDate.Date,
            EndDate = request.EndDate.Date,
            TotalPrice = car.DailyRate * totalDays
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return Ok(booking);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
    {
        var bookings = await _db.Bookings
            .AsNoTracking()
            .Include(b => b.Car)
            .Include(b => b.Customer)
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpGet("availability")]
    public async Task<ActionResult<object>> CheckAvailability([FromQuery] int carId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate.Date > endDate.Date)
        {
            return BadRequest("startDate cannot be after endDate.");
        }

        var carExists = await _db.Cars.AnyAsync(c => c.Id == carId);
        if (!carExists)
        {
            return NotFound($"Car with ID {carId} not found.");
        }

        var available = await IsCarAvailable(carId, startDate.Date, endDate.Date);
        return Ok(new { carId, startDate = startDate.Date, endDate = endDate.Date, available });
    }

    private async Task<bool> IsCarAvailable(int carId, DateTime startDate, DateTime endDate)
    {
        return !await _db.Bookings.AnyAsync(b =>
            b.CarId == carId &&
            startDate <= b.EndDate &&
            endDate >= b.StartDate);
    }
}
