namespace CarRentalSystem.DTOs;

public class CreateBookingRequest
{
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
