using EventEase.Models;

namespace EventEase.Services;

public class RegistrationService
{
    private readonly List<Registration> _registrations = new();
    private readonly List<AttendanceRecord> _attendanceRecords = new();
    private int _nextRegistrationId = 1;
    private int _nextAttendanceId = 1;

    public Task<List<Registration>> GetAllRegistrationsAsync()
    {
        return Task.FromResult(_registrations.OrderByDescending(r => r.RegistrationDate).ToList());
    }

    public Task<List<Registration>> GetRegistrationsByEventIdAsync(int eventId)
    {
        var eventRegistrations = _registrations
            .Where(r => r.EventId == eventId)
            .OrderByDescending(r => r.RegistrationDate)
            .ToList();

        return Task.FromResult(eventRegistrations);
    }

    public Task<Registration?> GetRegistrationByIdAsync(int id)
    {
        var registration = _registrations.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(registration);
    }

    public Task<Registration?> GetRegistrationByConfirmationCodeAsync(string confirmationCode)
    {
        var registration = _registrations.FirstOrDefault(r => r.ConfirmationCode == confirmationCode);
        return Task.FromResult(registration);
    }

    public Task<Registration> CreateRegistrationAsync(Registration registration)
    {
        registration.Id = _nextRegistrationId++;
        registration.RegistrationDate = DateTime.Now;
        registration.ConfirmationCode = GenerateConfirmationCode();
        _registrations.Add(registration);
        return Task.FromResult(registration);
    }

    public Task<bool> ConfirmRegistrationAsync(string confirmationCode)
    {
        var registration = _registrations.FirstOrDefault(r => r.ConfirmationCode == confirmationCode);
        if (registration != null)
        {
            registration.IsConfirmed = true;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> CancelRegistrationAsync(int id)
    {
        var registration = _registrations.FirstOrDefault(r => r.Id == id);
        if (registration != null)
        {
            _registrations.Remove(registration);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> IsEmailRegisteredForEventAsync(string email, int eventId)
    {
        var exists = _registrations.Any(r => r.Email.Equals(email, StringComparison.OrdinalIgnoreCase) 
                                           && r.EventId == eventId);
        return Task.FromResult(exists);
    }

    public Task<int> GetRegistrationCountForEventAsync(int eventId)
    {
        var count = _registrations.Count(r => r.EventId == eventId && r.IsConfirmed);
        return Task.FromResult(count);
    }

    // Attendance Management
    public Task<List<AttendanceRecord>> GetAttendanceByEventIdAsync(int eventId)
    {
        var attendance = _attendanceRecords
            .Where(a => a.EventId == eventId)
            .OrderByDescending(a => a.CheckInTime)
            .ToList();

        return Task.FromResult(attendance);
    }

    public Task<AttendanceRecord> CheckInAsync(int eventId, int registrationId)
    {
        // Check if already checked in
        var existingRecord = _attendanceRecords
            .FirstOrDefault(a => a.EventId == eventId && a.RegistrationId == registrationId && !a.CheckOutTime.HasValue);

        if (existingRecord != null)
        {
            return Task.FromResult(existingRecord);
        }

        var attendance = new AttendanceRecord
        {
            Id = _nextAttendanceId++,
            EventId = eventId,
            RegistrationId = registrationId,
            CheckInTime = DateTime.Now,
            IsPresent = true
        };

        _attendanceRecords.Add(attendance);
        return Task.FromResult(attendance);
    }

    public Task<bool> CheckOutAsync(int attendanceId)
    {
        var attendance = _attendanceRecords.FirstOrDefault(a => a.Id == attendanceId);
        if (attendance != null && !attendance.CheckOutTime.HasValue)
        {
            attendance.CheckOutTime = DateTime.Now;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<AttendanceRecord?> GetActiveAttendanceAsync(int eventId, int registrationId)
    {
        var attendance = _attendanceRecords
            .FirstOrDefault(a => a.EventId == eventId && a.RegistrationId == registrationId && !a.CheckOutTime.HasValue);
        
        return Task.FromResult(attendance);
    }

    public Task<int> GetAttendanceCountAsync(int eventId)
    {
        var count = _attendanceRecords.Count(a => a.EventId == eventId && a.IsPresent);
        return Task.FromResult(count);
    }

    public Task<List<Registration>> SearchRegistrationsAsync(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return GetAllRegistrationsAsync();

        var filteredRegistrations = _registrations
            .Where(r => r.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       r.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       r.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       r.ConfirmationCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.RegistrationDate)
            .ToList();

        return Task.FromResult(filteredRegistrations);
    }

    private string GenerateConfirmationCode()
    {
        return Guid.NewGuid().ToString("N")[..8].ToUpper();
    }
}