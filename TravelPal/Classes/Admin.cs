﻿using TravelPal.Enums;

namespace TravelPal.Classes
{
    class Admin : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Countries Location { get; set; }
    }
}