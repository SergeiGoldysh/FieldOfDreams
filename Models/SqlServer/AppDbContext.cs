using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Models.SqlServer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Seed();
        }
        public DbSet<Question> Questiones { get; set; }
        public DbSet<Answer> Answeres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Hint> Hintes { get; set; }
        public DbSet<UserHint> UserHints { get; set; }
        public DbSet<GameReport> GameReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string? str = config["Action"];
            string? str1 = config["ConnectionString"];
            string? str2 = config
                .GetSection("ConnectionStrings")
                ["ConnectionString"];

            string? connectionString = config
                .GetConnectionString("ConnectionString");

            optionsBuilder
                .UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserHint>()
                .HasKey(userHint => new { userHint.UserId, userHint.HintId }); // Устанавливаем составной ключ

            modelBuilder.Entity<UserHint>()
                .HasOne(userHint => userHint.User)         // У одной связи UserHint есть один User
                .WithMany(user => user.UserHints)           // У одного пользователя может быть много UserHint
                .HasForeignKey(userHint => userHint.UserId) // Внешний ключ для связи с User
                .OnDelete(DeleteBehavior.Cascade);          // Каскадное удаление: при удалении пользователя удаляются все связанные UserHint

            modelBuilder.Entity<UserHint>()
                .HasOne(userHint => userHint.Hint)         // У одной связи UserHint есть одна подсказка
                .WithMany()                               // У одной подсказки может быть много UserHint
                .HasForeignKey(userHint => userHint.HintId) // Внешний ключ для связи с Hint
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>(entity =>
            {

                entity.HasMany(q => q.Answers)
                      .WithOne(a => a.Question)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
        public void Seed()
        {
            if (Hintes.Any())
                return;

            var hints = new List<Hint>
            {
                new Hint
                {
                    Name = "Fifty fifty",
                    Description = "Removes two incorrect answers",
                },
                new Hint
                {
                    Name = "Call a friend",
                    Description = "Call a friend and ask for help within 30 seconds",

                },
                new Hint
                {
                    Name = "Right to make mistakes",
                    Description = "Opportunity to answer incorrectly once",
                }
            };
            Hintes.AddRange(hints);
            SaveChanges();
        }

    }
}


