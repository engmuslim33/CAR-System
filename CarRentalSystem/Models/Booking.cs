namespace CarRentalSystem.Models;

public class Booking
{
    public int Id { get; set; }

    public int CarId { get; set; }
    public Car? Car { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Optional: basic total price calculation output
    public decimal TotalPrice { get; set; }
}
