using Microsoft.EntityFrameworkCore;

namespace LibOnline.Models
{
    public class ApplicationContext:DbContext
    {
        // Авторизация
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        // Книги 
        public DbSet<Categories.Category> allCategories { get; set; }
        public DbSet<Books.BooksCategories> booksCategories { get; set; }
        public DbSet<Books.BookDescription> bookDescriptions { get; set; }
        public DbSet<Books.Page> booksPage { get; set; }
        public DbSet<Books.UserBook> userBooks { get; set; }
        public DbSet<General.PageFontSizes> pageFontSizes { get; set; }
        public DbSet<General.CategoriesPagination> categoriesPagination { get; set; }
        public DbSet<General.MenuPagination> menuPagination { get; set; }
        public DbSet<General.PagePagination> pagePaginations { get; set; }


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


    }//ApplicationContext
}
