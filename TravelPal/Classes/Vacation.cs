﻿using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Classes
{
    public class Vacation : Travel
    {
        public bool AllInclusive { get; set; }

        public Vacation(string destination, Countries country, int travellers, List<IPackingListItem> packingList, DateTime startDate, DateTime endDate, bool allInclusive) : base(destination, country, travellers, packingList, startDate, endDate)
        {
            AllInclusive = allInclusive;
        }
    }
}
