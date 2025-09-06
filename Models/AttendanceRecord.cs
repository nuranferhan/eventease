namespace EventEase.Models;

public class AttendanceRecord
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int RegistrationId { get; set; }

    public DateTime CheckInTime { get; set; } = DateTime.Now;

    public DateTime? CheckOutTime { get; set; }

    public bool IsPresent { get; set; } = true;

    public string? Notes { get; set; }

    // Navigation properties
    public Event? Event { get; set; }
    public Registration? Registration { get; set; }

    // Computed properties
    public TimeSpan? Duration => CheckOutTime?.Subtract(CheckInTime);
    public string DurationText => Duration?.ToString(@"hh\:mm") ?? "Still present";
    public string StatusText => CheckOutTime.HasValue ? "Checked Out" : "Present";
}