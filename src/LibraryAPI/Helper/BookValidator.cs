using FluentValidation;
using LibraryAPI.Models;

namespace LibraryAPI.Helper
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(q => q.Id).NotNull();
            RuleFor(q => q.Title).NotNull().NotEmpty().MaximumLength(55);
            RuleFor(q => q.Author).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(q => q.YearofPublish).InclusiveBetween(1000, DateTime.Now.Year);
        }
        
    }
}