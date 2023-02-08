using FluentValidation;
using MediumClone.Dtos.NlogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.ValidationRules
{
    public class BlogListDtoValidator : AbstractValidator<BlogListDto>
    {
        public BlogListDtoValidator()
        {

        }
    }
}
