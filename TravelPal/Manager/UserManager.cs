﻿using System.Collections.Generic;
using System.Windows;
using TravelPal.Classes;
using TravelPal.Enums;

namespace TravelPal.Manager
{
    public static class UserManager
    {
        public static List<IUser>? UserList { get; set; } = new() // TODO: Fixa så att man kan välja alla länder i världen && Fyll i 2 resor
        {
            //new User("user", "password", Enums.Countries.Sweden, "User", "Usersson", new List<Travel> {
            //    new Vacation("Madrid", Countries.Spain, 2, new List<OtherItem>{"Sunscreen", 1 }, 2024-01-01, 2024-02-01),
            //    new WorkTrip("Hong Kong", Countries.Japan, 1, new List<IPackingListItem> (new OtherItem("Jacket", 1), new TravelDocument("Passport", true)), 2024-04-01, 2024-04-20) },
            //new Admin(),
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
        public static bool SignInUser(string username, string password)
        {
            // TODO: If nothing is filled in... "Please fill in username and password"
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
            if (isSameInput == false)
            {
                MessageBox.Show("Wrong username or password!", "Invalid login");
            }

            return isSameInput;
        }
    }
}
