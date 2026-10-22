
namespace Haruka.Scripts.Data;

public class HarukaCharacterSkinData
{
    public string Id { get; }
    public string Name { get; }
    public string ModelPath { get; }

    public HarukaCharacterSkinData(string id, string name, string path)
    {
        Id = id; Name = name; ModelPath = path;
    }
}