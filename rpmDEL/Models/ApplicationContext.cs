using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<user> _user { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<role> roles { get; set; }
        public DbSet<FileModel> files { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminFam = "admin";
            string adminName = "admin";
            string adminSurname = "admin";
            DateTime adminBD = DateTime.MinValue;
            string adminDesc = "Млекопитающее из семейства пандовых, подотряда псообразных, отряда хищных, которое, тем не менее, питается преимущественно растительностью; размером примерно соответствует крупным особям домашней кошки.";
            string adminL = "admin";
            string adminP = "admin";
            string adminEmail = "admin@mail.ru";

            // добавляем роли
            role adminRole = new role { Id = 1, Name = adminRoleName };
            role userRole = new role { Id = 2, Name = userRoleName };
            user adminUser = new user { _ID = 1, _Fam = adminFam, _Name = adminName, _Surname = adminSurname, _regday = adminBD, _Description = adminDesc, _login = adminL, _pass = adminP, _EMail = adminEmail, RoleId = adminRole.Id, _imgpath = "/images/Icon.jpg" };
            Post FadminPost = new Post { id = 1, Title = "Первый пост", Message = "Первый пост так-то", UserID = adminRole.Id };
            Post SadminPost = new Post { id = 2, Title = "Второй пост", Message = "_ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ _ВторойПост_ ", img1 = "/files/0c6c2b6f0167fa9447a578e9599910ba.jpg", img2 = "/files/5ecec14e09b33e0700d51e2b.png", img3 = "/files/785966f80d7f7aa7c3a6994e4d99dc36.jpg", UserID = adminRole.Id };

            modelBuilder.Entity<role>().HasData(new role[] { adminRole, userRole });
            modelBuilder.Entity<user>().HasData(new user[] { adminUser });
            modelBuilder.Entity<Post>().HasData(new Post[] { FadminPost, SadminPost });
            base.OnModelCreating(modelBuilder);
        }
    }
}
