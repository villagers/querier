namespace Querier.Descriptors
{
    public interface IJoinDescriptor
    {
        public string? JoinRefTable { get; set; }
        public string? JoinRefColumn { get; set; }
        public string? JoinColumn { get; set; }
    }
}
