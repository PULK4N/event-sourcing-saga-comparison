namespace EventSourcing.Models
{
    public interface ISharedStateData
    {
        public bool IsDeleted { get; set; }
    }
}
