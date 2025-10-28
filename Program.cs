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
            Console.WriteLine("What blog will this post be added to? Enter the blog Id: ");
            int userBlogChoice;

            while (!int.TryParse(Console.ReadLine(), out userBlogChoice) || userBlogChoice <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid blog Id: ");
            }

            var selectedBlog = db.Blogs.Find(userBlogChoice);

            if (selectedBlog == null)
            {
                Console.WriteLine("That blog Id does not exist.");
            }
            else
            {
                var post = new Post { BlogId = userBlogChoice };
                // Create new post
                Console.Write("Enter a title for a new Post: ");
                post.Title = Console.ReadLine();

                Console.Write("Enter content for a new Post: ");
                post.Content = Console.ReadLine();

                db.AddPost(post);
                logger.Info("Post added - {title}", post.Title);
                logger.Info("Post content - {content}", post.Content);
                logger.Info("Post blog Id - {blogId}", userBlogChoice);
            }
            break;
            break;

        case "4":
            // See posts
            var query2 = db.Posts.OrderBy(b => b.Title);

            Console.WriteLine("All posts in the database:");
            foreach (var item in query2)
            {
                Console.WriteLine($"Title: {item.Title}");
                Console.WriteLine($"Content: {item.Content}");
                Console.WriteLine($"BlogId: {item.BlogId}");
                Console.WriteLine("-------------------");
            }
            break;

        case "5":
            on = false;
            break;

        default:
            Console.WriteLine("Invalid selection.");
            break;
    }

} while (on);
logger.Info("Program ended");