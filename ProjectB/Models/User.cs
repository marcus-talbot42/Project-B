using Newtonsoft.Json;

namespace ProjectB.Models;

using BCrypt.Net;

public class User : IEquatable<User>
{
    [JsonProperty] private readonly string _username;
    [JsonProperty] private readonly string _password;
    [JsonProperty] private readonly UserRole _role;

    public User(string username, string password, UserRole role)
    {
        this._username = username;
        this._password = password;
        this._role = role;
    }

    public bool IsPasswordCorrect(string password)
    {
        return this._password == BCrypt.HashPassword(password);
    }

    public new string? ToString()
    {
        return $"User {{\"username\":\"{_username}\", \"password\":\"{_password}\", \"role\":\"{_role}\"}}";
    }

    public class Builder
    {
        private string? _username;
        private string? _password;
        private UserRole _role;

        public Builder WithUsername(string username)
        {
            this._username = username;
            return this;
        }

        public Builder WithPassword(string password)
        {
            this._password = BCrypt.HashPassword(password);
            return this;
        }

        public Builder WithUserRole(UserRole role)
        {
            this._role = role;
            return this;
        }

        public User Build()
        {
            return new User(_username!, _password!, _role);
        }
    }

    public bool Equals(User? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _password == other._password && _role == other._role && _username == other._username;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_password, (int)_role, _username);
    }
}