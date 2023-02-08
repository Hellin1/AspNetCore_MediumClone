using MediumClone.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Dtos.NlogDtos
{
    public class CommentCreateDto : IDto
    {
        public string Content { get; set; }
        public int    UserId  { get; set; }
        public int    BlogId  { get; set; }
    }
}
