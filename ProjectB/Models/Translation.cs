namespace ProjectB.Models;

public class Translation(string id, Dictionary<string, string> pairs) : IEntity<string>
{

    public Dictionary<string, string> GetPairs()
    {
        return pairs;
    } 
    
    public string GetId()
    {
        return id;
    }
}