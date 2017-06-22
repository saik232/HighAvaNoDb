namespace HighAvaNoDb.Domain
{
    public interface INode
    {
        string Path { set; get; }
        string NodeName { set; get; }
        string Data { get; }
    }
}
