using MediumClone.Entities.Domains;
using System;

namespace MediumClone.UI.Models
{
    public class CommentAdminListModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        // navigation props
        public int BlogId { get; set; }

        public Blog Blog { get; set; }
        // user
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }

    }
}
