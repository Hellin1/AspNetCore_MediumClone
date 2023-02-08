using MediumClone.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Dtos.NlogDtos
{
	public class CategoryCreateDto : IDto
	{
        public string Title { get; set; }
    }
}
