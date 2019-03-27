using Microsoft.EntityFrameworkCore;

namespace LibOnline.Areas.Admin.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Statistics.Statistics> statistics { get; set; }
        public DbSet<Aythor.Author> authors { get; set; }



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