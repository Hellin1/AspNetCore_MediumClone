using MediumClone.DataAccess.Configurations;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.DataAccess.Contexts
{
	public class NlogContext : IdentityDbContext<AppUser,AppRole,int>
	{
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogCategory> BlogCategories { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public NlogContext(DbContextOptions<NlogContext> options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogConfigurations());
            modelBuilder.ApplyConfiguration(new BlogCategoryConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            base.OnModelCreating(modelBuilder);
        }


    }
}
