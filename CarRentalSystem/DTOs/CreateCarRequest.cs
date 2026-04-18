namespace CarRentalSystem.DTOs;

public class CreateCarRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
}
