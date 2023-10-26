namespace TravelPal.Classes
{
    public interface IPackingListItem
    {
        public string Name { get; set; }

        public string GetInfo();
    }
}
