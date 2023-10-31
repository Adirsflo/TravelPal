using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Classes
{
    public class Travel
    {
        public string Destination { get; set; }
        public Countries Country { get; set; }
        public int Travellers { get; set; }
        public List<IPackingListItem> PackingList { get; set; }
        public DateTime StartDate { get; set; } // Ändra i XAML
        public DateTime EndDate { get; set; } // Ändra i XAML
        public int TravelDays { get; set; }


        public Travel(string destination, Countries country, int travellers, List<IPackingListItem> packingList, DateTime startDate, DateTime endDate, int travelDays)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            PackingList = packingList;
            StartDate = startDate;
            EndDate = endDate;
        }

        public virtual string GetInfo()
        {
            // FILL IN CODE
            return "I have not made a trip yet!";
        }

        private int CalculateTravelDays()
        {
            // FILL IN CODE??
            int travelDateFrom = Convert.ToInt32(StartDate);
            int travelDateTo = Convert.ToInt32(EndDate);
            return travelDateFrom - travelDateTo;
        }

    }
}
