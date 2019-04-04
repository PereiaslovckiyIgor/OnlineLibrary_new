using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Statistics.Statistics> statistics { get; set; }
        public DbSet<Aythor.Author> authors { get; set; }
        public DbSet<Category.Category> сategories { get; set; }
        public DbSet<Access.User> users { get; set; }
        public DbSet<Access.Role> roles { get; set; }
        public DbSet<Comments.Comment> comments { get; set; }
        public DbSet<Comments.Comments> allComments { get; set; }
        public DbSet<Book.Book> books { get; set; }
        public DbSet<Book.BookMain> bookMains { get; set; }
        public DbSet<Book.BookToInsert> bookToInserts { get; set; }
        public DbSet<Book.Image> images { get; set; }
        public DbSet<Book.BooksAuthors> booksAuthors { get; set; }
        public DbSet<Book.BookCategories> booksCategories { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }


        public ApplicationContext()
        {
            Database.EnsureCreated();
        }//ApplicationContext

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = IGORPC\SQLEXPRESS; Database = LibOnline; User ID = sa; Password = 793638bujhm; MultipleActiveResultSets=true;");
        }//OnConfiguring
    }
}