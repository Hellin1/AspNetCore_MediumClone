using MediumClone.Dtos.NlogDtos;
using System.Collections.Generic;

namespace MediumClone.UI.Models
{
    public class BlogAdminListModel
    {
        public List<BlogListDto> LatestBlogs { get; set; }

        public List<BlogListDto> BlogsOrderedById { get; set; }
    }
}
