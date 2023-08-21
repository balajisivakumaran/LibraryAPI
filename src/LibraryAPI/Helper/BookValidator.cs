using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LibraryAPI.Models;

namespace LibraryAPI.Helper
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor( q => q.Id).NotNull();
            RuleFor(q => q.Title).NotEmpty().MaximumLength(30);
            RuleFor(q => q.Author).NotEmpty().MaximumLength(30);
            RuleFor(q => q.YearofPublish).InclusiveBetween(1000, DateTime.Now.Year);
        }
        
    }
}