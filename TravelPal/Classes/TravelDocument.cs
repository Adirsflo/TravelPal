namespace TravelPal.Classes
{
    public class TravelDocument : IPackingListItem
    {
        public string Name { get; set; }
        public bool Required { get; set; }

        public TravelDocument(string name, bool required)
        {
            Name = name;
            Required = required;
        }

        public void Document(string name, bool required)
        {

        }

        public string GetInfo()
        {
            return "";
        }
    }
}
