using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class Registration
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Event ID is required")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name must be less than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be less than 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    public string? PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Special requirements must be less than 500 characters")]
    public string? SpecialRequirements { get; set; }

    public bool IsConfirmed { get; set; } = false;

    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public string ConfirmationCode { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

    // Navigation property
    public Event? Event { get; set; }

    // Computed properties
    public string FullName => $"{FirstName} {LastName}";
    public string StatusText => IsConfirmed ? "Confirmed" : "Pending";
}