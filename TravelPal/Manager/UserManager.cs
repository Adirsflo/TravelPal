using System;
using System.Collections.Generic;
using System.Windows;
using TravelPal.Classes;
using TravelPal.Enums;

namespace TravelPal.Manager
{
    public static class UserManager
    {
        public static List<IUser>? UserList { get; set; } = new()
        {
            new Admin(),
            new User("user", "password", Enums.Countries.Sweden, "User", "Usersson")
            {
                Travels = new List<Travel>()
                {
                    new Vacation("Madrid", Countries.Spain, 2, new List<IPackingListItem> {new OtherItem("Sunscreen", 2), new OtherItem("Ball", 2), new TravelDocument("Passport", false)}, new DateTime(2024, 10, 02), new DateTime(2024, 11, 01), true),
                    new WorkTrip("Hongkong", Countries.China, 1, new List<IPackingListItem> {new OtherItem("Jacket", 1), new OtherItem("Laptop", 1), new TravelDocument("Passport", true)}, new DateTime(2024, 04, 01), new DateTime(2024, 04, 20), "Bussines meeting for developing new strategy")
                }
            },
        };
        public static IUser? signedInUser { get; set; }
        public static bool AddUser(string username, string firstName, string lastName, Countries country, string newPassword)
        {
            bool isValidated = ValidateUsername(username);

            if (isValidated)
            {
                // Add user to userList
                UserList.Add(new User(username, newPassword, country, firstName, lastName));
                return true;
            }
            return isValidated;
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
        public static bool SignInUser(string username, string password)
        {
            bool isSameInput = false;
            foreach (var user in UserList)
            {
                if (username.ToLower() == user.Username && password == user.Password)
                {
                    // Signing in user if information is correct
                    signedInUser = user;
                    isSameInput = true;
                }
            }
            if (username == "" && password == "" && isSameInput == false)
            {
                MessageBox.Show("Please fill in username and password!", "Invalid login");
            }
            else if (isSameInput == false)
            {
                MessageBox.Show("Wrong username or password!", "Invalid login");
            }

            return isSameInput;
        }
    }
}
