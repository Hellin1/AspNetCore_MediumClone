using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Entities.Domains
{
    public class AppUser : IdentityUser<int>
    {
        public string ImagePath { get; set; }

        public string Gender { get; set; }

        // blogs
        public List<Blog> Blogs { get; set; }

        // comments
        public List<Comment> Comments { get; set; }

    }
}
