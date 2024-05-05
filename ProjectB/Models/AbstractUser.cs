using static BCrypt.Net.BCrypt;
using Newtonsoft.Json;
using ProjectB.Models;
using System;

/// <summary>
/// Blueprint for users, containing the fields shared by all user types. This class should be implemented by sub-classes,
/// which specify the type of user, and implement unique functionality and fields based on that type.
/// </summary>
/// <param name="username">The username assigned to the user.</param>
/// <param name="role">The role assigned to a user, used to determine what functionality should be available to said user.</param>
public abstract class AbstractUser(string username, UserRole role) : IEquatable<AbstractUser>, IEntity<string>
{

    [JsonProperty] protected string Username = username;
    [JsonProperty] protected UserRole Role = role;

    /// <summary>
    /// Gets the role assigned to the user.
    /// </summary>
    /// <returns>The role assigned to the user.</returns>
    public UserRole GetUserRole()
    {
        return Role;
    }

    /// <summary>
    /// Gets the ID assigned to the user. In this case, simply returns the username.
    /// </summary>
    /// <returns>The username of the user.</returns>
    public string GetId() => Username;

    /// <summary>
    /// Implements a way to check for equality between two instances of AbstractUser,
    /// </summary>
    /// <param name="other">The object which will be checked for equality with the object this method is called on.</param>
    /// <returns>True if the objects are equal, false if not.</returns>
    public abstract bool Equals(AbstractUser? other);

    /// <summary>
    /// Provides a more generic way to check for equality between an AbstractUser-instance and an instance of an unknown
    /// type.
    /// </summary>
    /// <param name="obj">Instance of an unknown type.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as AbstractUser);
    }

    /// <summary>
    /// Generates the hashcode of the user.
    /// </summary>
    /// <returns>The hashcode of the user.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Username, (int)Role);
    }

    /// <summary>
    /// Provides a flexible way to create a user object on the fly.
    /// This Builder is a rough implementation of the Builder-pattern, as described on RefactoringGuru
    /// (https://refactoring.guru/design-patterns/builder).
    /// </summary>
    private class Builder
    {
        private string? _username;
        private UserRole _role;
        private DateOnly _validForDate;
        private string? _password;

        /// <summary>
        /// Sets the username that will be set, when the Builder.Build()-method is called.
        /// </summary>
        /// <param name="username">The username that will be set.</param>
        /// <returns>This instance of the Builder.</returns>
        public Builder WithUsername(string username)
        {
            _username = username;
            return this;
        }
        /// <summary>
        /// Sets the password that will be set, when the Builder.Build()-method is called. Before setting the password,
        /// the given value is hashed, using BCrypt's HashPassword-method. If the role of the user is
        /// UserRole.Guest, the password will be ignored.
        /// </summary>
        /// <param name="password">The password that will be set.</param>
        /// <returns>This instance of the Builder.</returns>
        public Builder WithPassword(string password)
        {
            _password = EnhancedHashPassword(password);
            return this;
        }

        /// <summary>
        /// Sets the role of the user. This method comes with the side-effect that, if the UserRole is equal to
        /// UserRole.Guest at the time the Build-method is called, the _password-field will be ignored. Otherwise, the
        /// _validForDate-field will be ignored.
        /// </summary>
        /// <param name="role">The role of the user.</param>
        /// <returns>This instance of the Builder.</returns>
        public Builder WithUserRole(UserRole role)
        {
            _role = role;
            return this;
        }

        /// <summary>
        /// Sets the date for which the user is valid. This is only relevant for Guests.
        /// </summary>
        /// <param name="dateOnly">The date for which the user is valid.</param>
        /// <returns>This instance of the Builder-pattern.</returns>
        public Builder WithValidForDate(DateOnly dateOnly)
        {
            _validForDate = dateOnly;
            return this;
        }

        /// <summary>
        /// Build the user, and returns the result.
        /// </summary>
        /// <returns>Returns the created user.</returns>
        public AbstractUser Build()
        {
            if (_role == UserRole.Guest)
            {
                return new Guest(_username!, _validForDate);
            }

            return new Employee(_username!, _role, _password!);
        }
    }
}
