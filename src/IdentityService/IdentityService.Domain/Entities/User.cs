namespace IdentityService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Mobile { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private User() { } // EF Core

    public User(string fullName, string mobile)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("FullName is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(mobile))
            throw new ArgumentException("Mobile is required.", nameof(mobile));

        Id = Guid.NewGuid();
        FullName = fullName;
        Mobile = mobile;
        IsActive = true;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
