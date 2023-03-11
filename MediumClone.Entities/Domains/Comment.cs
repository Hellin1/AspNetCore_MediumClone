using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Entities.Domains
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime? UpdatedTime { get; set; }

        public int BlogId { get; set; }

        public Blog Blog { get; set; }
        
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
