using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Entities.Domains
{
	public class Blog : BaseEntity
	{

        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdatedTime { get; set; }

        public List<BlogCategory> BlogCategories { get; set; }

        public List<Comment> Comments { get; set; }
     
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
