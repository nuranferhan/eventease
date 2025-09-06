namespace EventEase.Services;

public class SessionService
{
    private readonly Dictionary<string, object> _sessionData = new();

    public event Action? OnStateChanged;

    public T? GetValue<T>(string key)
    {
        if (_sessionData.TryGetValue(key, out var value) && value is T)
        {
            return (T)value;
        }
        return default(T);
    }

    public void SetValue<T>(string key, T value)
    {
        if (value != null)
        {
            _sessionData[key] = value;
        }
        else
        {
            _sessionData.Remove(key);
        }
        
        NotifyStateChanged();
    }

    public bool HasValue(string key)
    {
        return _sessionData.ContainsKey(key);
    }

    public void RemoveValue(string key)
    {
        _sessionData.Remove(key);
        NotifyStateChanged();
    }

    public void ClearSession()
    {
        _sessionData.Clear();
        NotifyStateChanged();
    }

    public Dictionary<string, object> GetAllValues()
    {
        return new Dictionary<string, object>(_sessionData);
    }

    // Convenience methods for common data types
    public string? GetString(string key) => GetValue<string>(key);
    public int? GetInt(string key) => GetValue<int?>(key);
    public bool? GetBool(string key) => GetValue<bool?>(key);
    public DateTime? GetDateTime(string key) => GetValue<DateTime?>(key);

    public void SetString(string key, string value) => SetValue(key, value);
    public void SetInt(string key, int value) => SetValue(key, value);
    public void SetBool(string key, bool value) => SetValue(key, value);
    public void SetDateTime(string key, DateTime value) => SetValue(key, value);

    // User session management
    public void SetCurrentUser(string email, string fullName)
    {
        SetString("UserEmail", email);
        SetString("UserFullName", fullName);
        SetDateTime("LoginTime", DateTime.Now);
    }

    public bool IsUserLoggedIn()
    {
        return HasValue("UserEmail");
    }

    public string? GetCurrentUserEmail()
    {
        return GetString("UserEmail");
    }

    public string? GetCurrentUserName()
    {
        return GetString("UserFullName");
    }

    public void LogoutUser()
    {
        RemoveValue("UserEmail");
        RemoveValue("UserFullName");
        RemoveValue("LoginTime");
    }

    // Recent activities tracking
    public void AddRecentActivity(string activity)
    {
        var activities = GetValue<List<string>>("RecentActivities") ?? new List<string>();
        activities.Insert(0, $"{DateTime.Now:HH:mm:ss} - {activity}");
        
        // Keep only last 10 activities
        if (activities.Count > 10)
        {
            activities = activities.Take(10).ToList();
        }
        
        SetValue("RecentActivities", activities);
    }

    public List<string> GetRecentActivities()
    {
        return GetValue<List<string>>("RecentActivities") ?? new List<string>();
    }

    // Search history
    public void AddSearchTerm(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return;

        var searchHistory = GetValue<List<string>>("SearchHistory") ?? new List<string>();
        
        // Remove if already exists
        searchHistory.Remove(searchTerm);
        
        // Add to beginning
        searchHistory.Insert(0, searchTerm);
        
        // Keep only last 5 searches
        if (searchHistory.Count > 5)
        {
            searchHistory = searchHistory.Take(5).ToList();
        }
        
        SetValue("SearchHistory", searchHistory);
    }

    public List<string> GetSearchHistory()
    {
        return GetValue<List<string>>("SearchHistory") ?? new List<string>();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}