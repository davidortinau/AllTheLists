using System.Diagnostics;
using AllTheLists.Models;
using AllTheLists.Models.Learning;
using Fonts;

namespace AllTheLists;

public partial class App : Application
{
    private static List<Product> _products;

    private static string[] shoeImages = new string[] { "shoe_01.png", "shoe_02.png", "shoe_03.png", "shoe_04.png", "shoe_05.png", "shoe_06.png", "shoe_07.png", "shoe_08.png" };
    
    public static List<Product> GenerateProducts()
    {

		if(_products != null)
			return _products;

        int count = 10000;
		_products = new List<Product>();
        for (int i = 0; i < count; i++)
        {
            var ran = Random.Shared.Next(1, 1000);
            _products.Add(
                new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Price = ran,
                    Description = "This is a sample product description.",
                    ImageUrl = ran % 2 == 0 ? "" : $"https://picsum.photos/id/{ran}/80",
                    Image = shoeImages[Random.Shared.Next(0, shoeImages.Length)],
                    Company = $"Company {i}",
                    Type = $"Type {i}",
                    SalesCategory = Random.Shared.Next(0, 3) switch
                    {
                        0 => "NEW",
                        1 => "BEST SELLER",
                        _ => ""
                    },
                    Category = Random.Shared.Next(0, 4) switch
                    {
                        0 => "Men's Sportswear",
                        1 => "Men's Originals",
                        2 => "Originals",
                        _ => "Sportswear"
                    },
                    ColorWays = Random.Shared.Next(1, 16)
                });
        }
        return _products;
    }

    private static List<ProductDisplay> _productDisplays;

    public static List<ProductDisplay> GenerateProductDisplays()
    {
        if (_productDisplays != null)
            return _productDisplays;

        int count = 1000;
        _productDisplays = new List<ProductDisplay>();
        for (int i = 0; i < count; i++)
        {
            if (i < 4)
            {
                _productDisplays.Add(new ProductDisplay
                {
                    Products = GenerateProducts().GetRange(i * 2, 2)
                });
            }
            else if (i % 3 == 1)
            {
                _productDisplays.Add(new ProductDisplay
                {
                    Products = GenerateProducts().GetRange(i * 2 - 1, 1)
                });
            }
            else
            {
                _productDisplays.Add(new ProductDisplay
                {
                    Products = GenerateProducts().GetRange(i * 2 - 2, 2)
                });
            }

            Debug.WriteLine($"Product Display {i} has {GenerateProducts().GetRange(i * 2, 2).Count} products");
        }
        return _productDisplays;
    }  

    private static string _lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur faucibus tortor nisi, at luctus massa molestie sit amet. Curabitur quam ligula, auctor sit amet viverra in, mattis eget libero. Nam pulvinar ante non mauris volutpat faucibus vitae ac diam. Fusce nec fringilla dolor. Nulla vitae justo quis mauris rutrum rutrum quis sed nisl. Nam vehicula erat et enim hendrerit, sollicitudin molestie nibh venenatis. Donec sapien sem, imperdiet et dui gravida, imperdiet viverra velit. Nulla sed quam porttitor, rhoncus felis quis, fermentum est. Quisque eu nulla urna. Ut vitae mauris vitae turpis malesuada lobortis nec sit amet massa. Vivamus vehicula arcu ac tempus aliquet. In mollis hendrerit consequat.";

    public static List<Review> GenerateReviews()
    {
        int count = 100;
        var reviews = new List<Review>();
        for (int i = 0; i < count; i++)
        {
            reviews.Add(new Review
            {
                Author = $"Author {i}",
                Comment = _lipsum.Substring(0, Random.Shared.Next(50, (_lipsum.Length - 1))),
                Car = $"Car {i}",
                ChargerType = $"Charger Type {i}",
                CreatedAt = DateTime.Now,
                Status = i % 2 == 0
            });
        }
        return reviews;
    }

    private static List<CheckIn> _checkIns;

    public static List<CheckIn> GenerateCheckIns()
    {
        if (_checkIns != null)
            return _checkIns;

        int count = 500;
        _checkIns = new List<CheckIn>();
        for (int i = 0; i < count; i++)
        {
            _checkIns.Add(new CheckIn
            {
                Author = $"Author {i}",
                AuthorIcon = $"https://picsum.photos/id/{Random.Shared.Next(1, 1000)}/40",
                Venue = new Venue
                {
                    Name = $"Venue {i}",
                    Address = $"Address {i}"
                },
                Product = GenerateProducts()[Random.Shared.Next(0, GenerateProducts().Count - 1)],
                Comment = _lipsum.Substring(0, Random.Shared.Next(0, 30)),
                Rating = Random.Shared.Next(1, 5),
                ServingStyle = $"Serving Style {i}",
                CreatedAt = DateTime.Now,
                SocialImage = $"https://picsum.photos/id/{Random.Shared.Next(1, 1000)}/300"
            });
        }
        return _checkIns;
    }

    private static List<Unit> _units;  

    public static string[] unitTitles = new string[]{
        "Basic Greetings",
        "Talking about Your Day",
        "Making Appointments with Friends",
        "Ordering in the Restaurant",
        "Shopping in Korea",
        "Asking for Directions",
        "Asking About the Past",
        "Talking about Weekend Plans",
        "Shopping in Korea",
        "Making Appointments with Friends",
        "Going to the Hospital"
    };

    public static string[] chapterTitles = new string[]{
        "Greeting for the First Time",
        "Starting an Introduction",
        "Talking about the Weather",
        "Talking about the Your Nationality",
        "Talking about Your Age",
        "Test"
    };

    public static string[] lessonTitles = new string[]{
        "Hello & Thank you",
        "What is your name?",
        "Where are you from?",
        "How old are you?",
        "What do you do?",
        "What is your phone number?",
        "What is your email address?",
        "What is your address?",
        "What are you doing?"
    };  

    public static string[] icons = new string[]{
        FontAwesome.Book,
        FontAwesome.Video,
        FontAwesome.Music,
        FontAwesome.Tv,
        FontAwesome.Film,
        FontAwesome.Gamepad,
        FontAwesome.Headphones,
        FontAwesome.Microphone,
    };

    public static List<Unit> GenerateUnits()
    {   

        if (_units != null)
            return _units;

        int count = 4;
        _units = new List<Unit>();
        for (int i = 0; i < count; i++)
        {
            _units.Add(new Unit
            {
                UnitNumber = i,
                Title = unitTitles[i],
                SubTitle = $"Sub Title {i}",
                Icon = $"{icons[Random.Shared.Next(0, icons.Length - 1)]}",
                Chapters = GenerateChapters(Random.Shared.Next(1, chapterTitles.Length))
            });
        }
        return _units;
    }

    private static List<Chapter> GenerateChapters(int v)
    {
        var chapters = new List<Chapter>();
        for (int i = 0; i < v; i++)
        {
            chapters.Add(new Chapter
            {
                Title = chapterTitles[i],
                Lessons = GenerateLessons(Random.Shared.Next(1, lessonTitles.Length))
            });
        }
        return chapters;
    }

    private static List<Lesson> GenerateLessons(int count)
    {
        var lessons = new List<Lesson>();
        for (int i = 0; i < count; i++)
        {
            lessons.Add(new Lesson
            {
                Title = lessonTitles[i]                
            });
        }
        return lessons;
    }

    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage());
	}
}
