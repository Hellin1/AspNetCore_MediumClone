using MediumClone.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Dtos.NlogDtos
{
    public class CategoryListDto : IDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsSelected { get; set; }
    }
}
