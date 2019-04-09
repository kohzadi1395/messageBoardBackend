using System.IO;
using MessageBoardBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MessageBoardBackend.Core
{
    public class ApiContext : DbContext
    {
        public ApiContext()
        {

        }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(@"Server=.;Database=MessageBoard;Trusted_Connection=True;");
            }
            
        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
