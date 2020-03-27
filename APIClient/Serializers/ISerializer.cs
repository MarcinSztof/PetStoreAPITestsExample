namespace APIClient.Serializers
{
    public interface ISerializer
    {
        T Deserialize<T>(string body);
        string Serialize(object body);
    }
}
