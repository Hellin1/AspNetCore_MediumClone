using MediumClone.Dtos.Interfaces;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Dtos.NlogDtos
{
    public class BlogListDto : IDto
    { // gonna change
        public int Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdatedTime { get; set; }

        // navigation props

        public List<BlogCategory> BlogCategories { get; set; }

        public List<Comment> Comments { get; set; }

        // user
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
