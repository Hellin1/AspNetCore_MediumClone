using FluentValidation;
using MediumClone.Dtos.NlogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.ValidationRules
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        { // gonna change
            //RuleFor(x => x.BlogId).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            //RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
