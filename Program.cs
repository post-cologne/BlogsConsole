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
    Console.WriteLine("Enter q to quit");
    string? input = Console.ReadLine()?.ToLower();

    switch (input)
    {
        case "1":
            // Display all Blogs from the database
            var query = db.Blogs.OrderBy(b => b.BlogId);

            Console.WriteLine("Option 1 selected.\nDisplaying all blogs in the database:");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.BlogId}: {item.Name}");
            }
            break;

        case "2":
            // Create and save a new Blog
            Console.Write("Option 2 selected.\nEnter a name for a new Blog: ");
            var name = Console.ReadLine();

            var blog = new Blog { Name = name };

            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
            break;

        case "3":
            Console.WriteLine("Option 3 selected\nWhat blog will this post be added to? Enter the blog Id: ");
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

        case "4":
            var blogs = db.Blogs.OrderBy(b => b.Name).ToList();
            Console.WriteLine("Option 4 selected.\nDisplaying all blogs to choose from:");
            if (blogs.Count == 0)
            {
                Console.WriteLine("No blogs found.");
                break;
            }

            Console.WriteLine("Select the Blog whose posts you want to view:");
            foreach (var b in blogs)
            {
                Console.WriteLine($"{b.BlogId}: {b.Name}");
            }

            int blogChoice;
            while (!int.TryParse(Console.ReadLine(), out blogChoice) || !blogs.Any(b => b.BlogId == blogChoice))
            {
                Console.WriteLine("Invalid blog Id. Please enter one from the list above:");
            }

            var chosenBlog = db.Blogs.Find(blogChoice);
            var blogPosts = db.Posts.Where(p => p.BlogId == blogChoice).OrderBy(p => p.Title).ToList();

            if (chosenBlog == null)
            {
                Console.WriteLine("Selected blog not found.");
                break;
            }
            
            Console.WriteLine($"In {chosenBlog.Name} there are {blogPosts.Count} posts.\n");

            if (blogPosts.Count == 0)
            {
                Console.WriteLine("No posts found for this blog.");
            }
            else
            {
                foreach (var post in blogPosts)
                {
                    Console.WriteLine($"Blog: {chosenBlog.Name}");
                    Console.WriteLine($"Title: {post.Title}");
                    Console.WriteLine($"Content: {post.Content}");
                    Console.WriteLine("-------------------");
                }
            }

            logger.Info("Viewed posts for BlogId {blogId} - {blogName}", chosenBlog.BlogId, chosenBlog.Name);
            break;

        case "q":
            Console.WriteLine("Option q selected.\nExiting program.");
            on = false;
            break;

        default:
            Console.WriteLine("Invalid selection.");
            break;
    }

} while (on);

logger.Info("Program ended");
