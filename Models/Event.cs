using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Event title is required")]
    [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description must be less than 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event date is required")]
    [DataType(DataType.DateTime)]
    public DateTime EventDate { get; set; } = DateTime.Now.AddDays(1);

    [Required(ErrorMessage = "Location is required")]
    [StringLength(200, ErrorMessage = "Location must be less than 200 characters")]
    public string Location { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000")]
    public int MaxCapacity { get; set; } = 50;

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; } = 0;

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation properties
    public List<Registration> Registrations { get; set; } = new();
    public List<AttendanceRecord> AttendanceRecords { get; set; } = new();

    // Computed properties
    public int RegisteredCount => Registrations.Count(r => r.IsConfirmed);
    public int AvailableSpots => MaxCapacity - RegisteredCount;
    public bool IsFull => AvailableSpots <= 0;
    public bool IsUpcoming => EventDate > DateTime.Now;
    public string StatusText => IsUpcoming ? "Upcoming" : "Past";
}