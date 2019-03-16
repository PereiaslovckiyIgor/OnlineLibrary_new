using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models
{
    public class ApplicationContext:DbContext
    {
        // Авторизация
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        // Книги 
        public DbSet<Categories.AllCategories> allCategories { get; set; }
        public DbSet<BooksCategories.BooksCategories> booksCategories { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    string adminRoleName = "admin";
        //    string userRoleName = "user";

        //    string adminEmail = "admin@mail.ru";
        //    string adminLogin = "Avalod";
        //    string adminPassword = "123456";

        //    // добавляем роли
        //    Role adminRole = new Role { Id = 1, Name = adminRoleName };
        //    Role userRole = new Role { Id = 2, Name = userRoleName };
        //    User adminUser = new User { Id = 1, Login = adminLogin, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

        //    modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
        //    modelBuilder.Entity<User>().HasData(new User[] { adminUser });
        //    base.OnModelCreating(modelBuilder);
        //}


        public ApplicationContext()
        {
            Database.EnsureCreated();
        }//ApplicationContext

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = IGORPC\MSSQL_IGOR; Database = LibOnline; User ID = sa; Password = 793638bujhm; MultipleActiveResultSets=true;");
        }//OnConfiguring


    }//ApplicationContext
}
