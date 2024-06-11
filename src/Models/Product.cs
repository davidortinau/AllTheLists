namespace AllTheLists.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Company {get;set;}
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Type {get;set;}

    public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);
}
