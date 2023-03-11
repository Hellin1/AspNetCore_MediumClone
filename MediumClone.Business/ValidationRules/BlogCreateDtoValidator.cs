using FluentValidation;
using MediumClone.Dtos.NlogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.ValidationRules
{
    public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateDtoValidator()
        { 
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.SelectedCategories).NotEmpty();
            RuleFor(x => x.AppUserId).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
