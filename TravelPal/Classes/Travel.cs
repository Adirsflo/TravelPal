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

        // TODO: Lägg in resterande från UML diagrammet
        // Travel({Props})
        // {virtual} + GetInfo(): string
        // private CalculateTravelDays(): int

    }
}
