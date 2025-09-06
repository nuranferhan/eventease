using EventEase.Models;

namespace EventEase.Services;

public class EventService
{
    private readonly List<Event> _events = new();
    private int _nextId = 1;

    public EventService()
    {
        // Seed with sample data
        SeedData();
    }

    public Task<List<Event>> GetAllEventsAsync()
    {
        return Task.FromResult(_events.OrderBy(e => e.EventDate).ToList());
    }

    public Task<List<Event>> GetUpcomingEventsAsync()
    {
        var upcomingEvents = _events
            .Where(e => e.EventDate > DateTime.Now && e.IsActive)
            .OrderBy(e => e.EventDate)
            .ToList();

        return Task.FromResult(upcomingEvents);
    }

    public Task<Event?> GetEventByIdAsync(int id)
    {
        var eventItem = _events.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(eventItem);
    }

    public Task<List<Event>> SearchEventsAsync(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return GetAllEventsAsync();

        var filteredEvents = _events
            .Where(e => e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       e.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.EventDate)
            .ToList();

        return Task.FromResult(filteredEvents);
    }

    public Task<Event> CreateEventAsync(Event eventItem)
    {
        eventItem.Id = _nextId++;
        eventItem.CreatedDate = DateTime.Now;
        _events.Add(eventItem);
        return Task.FromResult(eventItem);
    }

    public Task<Event?> UpdateEventAsync(Event eventItem)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == eventItem.Id);
        if (existingEvent != null)
        {
            existingEvent.Title = eventItem.Title;
            existingEvent.Description = eventItem.Description;
            existingEvent.EventDate = eventItem.EventDate;
            existingEvent.Location = eventItem.Location;
            existingEvent.MaxCapacity = eventItem.MaxCapacity;
            existingEvent.Price = eventItem.Price;
            existingEvent.ImageUrl = eventItem.ImageUrl;
            existingEvent.IsActive = eventItem.IsActive;
        }
        return Task.FromResult(existingEvent);
    }

    public Task<bool> DeleteEventAsync(int id)
    {
        var eventItem = _events.FirstOrDefault(e => e.Id == id);
        if (eventItem != null)
        {
            _events.Remove(eventItem);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<int> GetTotalEventsAsync()
    {
        return Task.FromResult(_events.Count);
    }

    public Task<int> GetUpcomingEventsCountAsync()
    {
        var count = _events.Count(e => e.EventDate > DateTime.Now && e.IsActive);
        return Task.FromResult(count);
    }

    private void SeedData()
    {
        var sampleEvents = new List<Event>
        {
            new Event
            {
                Id = _nextId++,
                Title = "Tech Conference 2024",
                Description = "Annual technology conference featuring the latest in software development, AI, and cloud computing.",
                EventDate = DateTime.Now.AddDays(15),
                Location = "Convention Center, Istanbul",
                MaxCapacity = 200,
                Price = 150.00m,
                ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400",
                IsActive = true
            },
            new Event
            {
                Id = _nextId++,
                Title = "Marketing Workshop",
                Description = "Learn digital marketing strategies and social media best practices from industry experts.",
                EventDate = DateTime.Now.AddDays(7),
                Location = "Business Center, Ankara",
                MaxCapacity = 50,
                Price = 75.00m,
                ImageUrl = "https://images.unsplash.com/photo-1556761175-b413da4baf72?w=400",
                IsActive = true
            },
            new Event
            {
                Id = _nextId++,
                Title = "Music Festival",
                Description = "Three-day music festival featuring local and international artists across multiple genres.",
                EventDate = DateTime.Now.AddDays(30),
                Location = "Outdoor Park, Izmir",
                MaxCapacity = 1000,
                Price = 299.00m,
                ImageUrl = "https://images.unsplash.com/photo-1459749411175-04bf5292ceea?w=400",
                IsActive = true
            },
            new Event
            {
                Id = _nextId++,
                Title = "Cooking Masterclass",
                Description = "Learn to cook authentic Turkish cuisine with renowned chef Mehmet Gürs.",
                EventDate = DateTime.Now.AddDays(5),
                Location = "Culinary School, Istanbul",
                MaxCapacity = 25,
                Price = 125.00m,
                ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400",
                IsActive = true
            }
        };

        _events.AddRange(sampleEvents);
    }
}