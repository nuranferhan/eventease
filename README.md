# EventEase - Comprehensive Event Management System

A Blazor Server application for managing events, registrations, and attendance tracking with a modern, responsive UI.

## Overview

<div align="center">
  <img width="85%" alt="EventEase Screenshot" src="https://github.com/user-attachments/assets/e56daa88-1fba-48ed-a651-df68679712c5" />
</div>

EventEase provides seamless event creation, registration management, and real-time attendance tracking.  
Built with Blazor Server, it demonstrates component architecture, state management, and performance optimization.

## Project Structure

```
EventEase/
├── Components/
│   ├── EventCard.razor              # Reusable event display component
│   ├── RegistrationForm.razor       # Complete registration form with validation
│   └── AttendanceTracker.razor      # Real time attendance management
├── Pages/
│   ├── Index.razor                  # Dashboard with featured events
│   ├── Events.razor                 # Event browsing and management
│   ├── Registration.razor           # Event registration portal
│   ├── Attendance.razor             # Attendance tracking interface
│   ├── _Host.cshtml                 # Host page for Blazor Server
│   └── Shared/
│       ├── _Layout.cshtml           # Main layout template
│       ├── MainLayout.razor         # Blazor main layout component
│       └── NavMenu.razor            # Navigation sidebar
├── Models/
│   ├── Event.cs                     # Event entity with validation
│   ├── Registration.cs              # Registration model
│   └── AttendanceRecord.cs          # Attendance tracking model
├── Services/
│   ├── EventService.cs              # Event management service
│   ├── RegistrationService.cs       # Registration and attendance service
│   └── SessionService.cs            # Session state management
├── wwwroot/
│   └── css/
│       └── site.css                 # Custom CSS styles
├── Program.cs                       # Application startup and configuration
├── App.razor                        # Blazor app component
└── EventEase.csproj                 # Project file
````

## Key Features

- **Dashboard** – Hero stats, featured events, quick actions  
- **Event Management** – CRUD, filters, search, capacity & status control  
- **Registration** – Multi-step forms, validation, confirmation codes  
- **Attendance Tracking** – Real-time check-in/out, attendee search, CSV export  
- **Component Architecture** – EventCard, RegistrationForm, AttendanceTracker  
- **Responsive Design** – Mobile-first with Bootstrap & custom CSS  
- **Security & Performance** – Input validation, CSRF protection, lazy loading, efficient rendering  

## Getting Started

### Prerequisites
- .NET 6 SDK or later  
- Visual Studio 2022 / VS Code  

### Quick Start
1. Clone the repository  
2. Run `dotnet run` in the project folder  
3. Open `https://localhost:5001` in your browser  

## Configuration

### Service Registration (`Program.cs`)
```csharp
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<RegistrationService>();
builder.Services.AddScoped<SessionService>();
````

## Data Models

* **Event** – Title, description, date, location, capacity, price, status
* **Registration** – Personal info, contact details, validation, confirmation code
* **Attendance** – Check-in/out times, duration, status, notes

## Usage Guide

### Creating Events

1. Go to **Events** page
2. Click **Create Event**
3. Fill details and save

### Managing Registrations

1. Select an event
2. Click **Register**
3. Complete the form and get confirmation

### Tracking Attendance

1. Open **Attendance** page
2. Select active event
3. Check-in/check-out attendees
4. Export CSV if needed
