using MediumClone.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.DataAccess.Configurations
{
    public class BlogConfigurations : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            // prop özellikleri geçilecek
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(400);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Content).HasColumnType("text");


            builder.HasMany(x => x.BlogCategories).WithOne(x => x.Blog).HasForeignKey(x => x.BlogId);
            builder.HasMany(x => x.Comments).WithOne(x => x.Blog).HasForeignKey(x => x.BlogId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
