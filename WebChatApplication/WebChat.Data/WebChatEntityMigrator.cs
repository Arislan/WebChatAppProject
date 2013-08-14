using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using WebChat.Models;

namespace WebChat.Data
{
    class WebChatEntityMigrator : DbMigrationsConfiguration<WebChatEntity>
    {
         public WebChatEntityMigrator()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

         protected override void Seed(WebChatEntity context)
        {
          
        }
    }
}
