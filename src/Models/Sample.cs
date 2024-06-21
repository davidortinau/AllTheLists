namespace AllTheLists.Models;

public class Sample
{
    public string Name { get; set; }
    public Type Page { get; set; }

    public override string ToString()
    {
        return Name;
    }
}