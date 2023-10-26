using System.Collections.Generic;
using TravelPal.Classes;

namespace TravelPal.Manager
{
    public static class UserManager
    {
        public static List<IUser> Users { get; set; }
        public static IUser signedInUser { get; set; }

        public static bool AddUser(IUser)
        {
            bool answer = true;
            return answer;
        }
        public static void RemoveUser(IUser)
        {
        }
        public static bool UpdateUsername(IUser username, IUser password)
        {
            bool answer = true;
            return answer;
        }
        private static bool ValidateUsername(IUser username)
        {
            bool answer = true;
            return answer;
        }
        public static bool SignInUser(IUser username, IUser password)
        {
            bool answer = true;
            return answer;
        }
    }
}
