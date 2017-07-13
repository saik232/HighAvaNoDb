namespace HighAvaNoDb.Route
{
    public interface IHashAlgorithm
    {
        int Hash(string key);
    }

    public class SimpleHashAlgorithm
    {
    }

}
