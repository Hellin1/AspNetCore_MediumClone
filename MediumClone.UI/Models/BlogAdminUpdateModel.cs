using MediumClone.Entities.Domains;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediumClone.Dtos.Interfaces;

namespace MediumClone.UI.Models
{
    public class BlogAdminUpdateModel : IDto
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }


        public DateTime? UpdatedTime { get; set; } = DateTime.Now;

        // navigation props

        public List<BlogCategory> BlogCategories { get; set; }

        public SelectList Categories { get; set; }

        public List<Comment> Comments { get; set; }

        // user
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
