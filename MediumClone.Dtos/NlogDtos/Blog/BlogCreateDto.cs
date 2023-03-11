using MediumClone.Dtos.Interfaces;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Dtos.NlogDtos
{
    public class BlogCreateDto : IDto
    { 
        public string Title { get; set; }
        public string Content { get; set; }

        // navigation props
        public List<BlogCategory> BlogCategories { get; set; }

        public List<CategoryListDto> Categories { get; set; }

        public List<string> SelectedCategories { get; set; }

        public int AppUserId { get; set; }
    }

}
