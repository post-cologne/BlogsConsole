using NLog;

string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
logger.Info("Program started");

var db = new DataContext();
bool on = true;

do
{
    Console.WriteLine("Enter your selection:");
    Console.WriteLine("1: See all blogs");
    Console.WriteLine("2: Add blog");
    Console.WriteLine("3: Create post");
    Console.WriteLine("4: See posts");
    Console.WriteLine("5: Exit"); // Added exit option so program can stop
    string? input = Console.ReadLine();

    switch (input)
    {
        case "1":
            // Display all Blogs from the database
            var query = db.Blogs.OrderBy(b => b.Name);

            Console.WriteLine("All blogs in the database:");
            foreach (var item in query)
            {
                Console.WriteLine(item.Name);
            }
            break;

        case "2":
            // Create and save a new Blog
            Console.Write("Enter a name for a new Blog: ");
            var name = Console.ReadLine();

            var blog = new Blog { Name = name };

            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
            break;

    case "3":
            // Create and save a new Post

            break;

        case "4":
            // See posts


        case "5":
            on = false;
            break;

        default:
            Console.WriteLine("Invalid selection.");
            break;
    }

} while (on);
logger.Info("Program ended");