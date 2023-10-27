namespace TravelPal.Classes
{
    public class OtherItem : IPackingListItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public void _OtherItem(string name, int quantity)
        {

        }
        public string GetInfo()
        {
            return "";
        }


    }
}
