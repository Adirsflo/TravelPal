using System;
using System.Collections.Generic;
using TravelPal.Enums;

namespace TravelPal.Classes
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        public WorkTrip(string destination, Countries country, int travellers, List<IPackingListItem> packingList, DateTime startDate, DateTime endDate, string meetingDetails) : base(destination, country, travellers, packingList, startDate, endDate)
        {
            MeetingDetails = meetingDetails;
        }

        public void Trip(string meetingDetails)
        {
        }

        public string GetInfo()
        {
            return "";
        }
    }
}
