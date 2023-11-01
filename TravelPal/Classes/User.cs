using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Classes
{
    public class User : IUser
    {
        public List<Travel> Travels { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public Countries Location { get; set; }

        public User(string username, string password, Countries location, string firstName, string lastName)
        {
            Username = username;
            Password = password;
            Location = location;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
