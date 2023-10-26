namespace TravelPal.Classes
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        public WorkTrip(string meetingDetails)
        {
            MeetingDetails = meetingDetails;
        }

        //  TODO: Fixa metoderna
        public void Trip(MeetingDetails) // Korrekt? 
        {
        }

        public string GetInfo()
        {
            return "";
        }
    }
}
