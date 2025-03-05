namespace Ecology.Data.DataLayerModels
{
    public class Pagginator<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
    }
}
