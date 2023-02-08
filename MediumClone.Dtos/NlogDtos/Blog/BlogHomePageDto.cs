using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediumClone.Dtos.Interfaces;
using MediumClone.Dtos.NlogDtos;

namespace MediumClone.Dtos.NlogDtos
{
    public class BlogHomePageDto : IDto
    {
        public List<BlogListDto> TrendingBlogs { get; set; }

        public List<BlogListDto> HomePageBlogs { get; set; }

        public List<CategoryListDto> Categories { get; set; }

    }
}
