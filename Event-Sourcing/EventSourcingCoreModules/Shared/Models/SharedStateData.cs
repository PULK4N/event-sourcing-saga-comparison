namespace EventSourcing.Shared.Models
{
    public interface ISharedStateData
    {
        public bool IsDeleted { get; set; }
    }
}
