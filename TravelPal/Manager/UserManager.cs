using System.Collections.Generic;
using System.Windows;
using TravelPal.Classes;
using TravelPal.Enums;

namespace TravelPal.Manager
{
    public static class UserManager
    {
        public static List<IUser>? UserList { get; set; } = new() // TODO: Fixa så att man kan välja alla länder i världen && Fyll i 2 resor
        {
            new User("user", "password", Enums.Countries.SaudiArabia, "User", "Usersson", new List<Travel>
            ())
        };
        public static IUser? signedInUser { get; set; }

        public static bool AddUser(string username, string firstName, string lastName, Countries country, string newPassword)
        {
            bool isValidated = ValidateUsername(username);

            if (isValidated)
            {
                // Add user to userList
                UserList.Add(new User(username, newPassword, country, firstName, lastName, null));
                return true;
            }
            return isValidated;
        }
        public static void RemoveUser(IUser User)
        {
            // FILL IN CODE
        }
        public static bool UpdateUsername(IUser username, IUser password)
        {
            // FILL IN CODE
            bool answer = true;
            return answer;
        }
        private static bool ValidateUsername(string username)
        {
            foreach (var user in UserList)
            {
                // If username is taken, throw error message
                if (user.Username == username)
                {
                    MessageBox.Show("Username is already taken", "User not added");
                    return false;
                }
            }
            return true;
        }
        public static bool SignInUser(IUser username, IUser password)
        {
            // FILL IN CODE
            bool answer = true;
            return answer;
        }
    }
}
