namespace TravelPal.Classes
{
    public class TravelDocument : IPackingListItem
    {
        public string Name { get; set; }
        public bool Required { get; set; }

        public void Document(Name, Required) // FIX
        {

        }

        public string GetInfo()
        {
            return "";
        }
    }
}
