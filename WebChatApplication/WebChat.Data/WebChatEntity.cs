using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.Models;

namespace WebChat.Data
{
    public class WebChatEntity : DbContext
    {
        public WebChatEntity()
            : base(ConfigurationManager.AppSettings["ChatDbConnectionString"])
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Channel> Channels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebChatEntity,WebChatEntityMigrator>());

         //   modelBuilder.Entity<Channel>()
         // .HasRequired(x => x.SecondUser)
         // .WithMany()
         // .HasForeignKey(x => x.SecondUserId)
         // .WillCascadeOnDelete(false);

               modelBuilder.Entity<Message>()
            .HasRequired(x => x.Retriever)
            .WithMany()
            .HasForeignKey(x => x.RetrieverId)
            .WillCascadeOnDelete(false);
        }
    }
}
