using FluentValidation;
using FluentValidation.TestHelper;

namespace LibraryAPI.UnitTests;

public class BookModelTests
{
    private readonly BookValidator _bookValidator;

    public BookModelTests()
    {
        _bookValidator = new BookValidator();
    }
    
    [Theory]
    [InlineData("")]
    public void Can_BookTitle_Go_Empty(string title)
    {
        //Arrange
        var book = new Book {
            Id = 1,
            Title = title,
            Author = "J.K.Rowling",
            YearofPublish = 1990
        };

        //Act
        TestValidationResult<Book> response = 
                        _bookValidator.TestValidate(book);

        //Assert
        response.ShouldHaveValidationErrorFor(e => e.Title)
            .WithErrorMessage("'Title' must not be empty.")
              .WithSeverity(Severity.Error);
    }

    [Theory]
    [InlineData("")]
    public void Can_BookAuthor_Go_Empty(string author)
    {
        //Arrange
        var book = new Book {
            Id = 1,
            Title = "Harry Potter and the Philosopher's Stone",
            Author = author,
            YearofPublish = 1990
        };

        //Act
        TestValidationResult<Book> response = 
                        _bookValidator.TestValidate(book);

        //Assert
        response.ShouldHaveValidationErrorFor(e => e.Author)
            .WithErrorMessage("'Author' must not be empty.")
              .WithSeverity(Severity.Error);
    }

    [Theory]
    [InlineData(999)]
    public void Can_YearofPublish_Go_OutofLowerBound(int year)
    {
        //Arrange
        var book = new Book {
            Id = 1,
            Title = "Harry Potter and the Philosopher's Stone",
            Author = "J.K.Rowling",
            YearofPublish = year
        };

        //Act
        TestValidationResult<Book> response = 
                        _bookValidator.TestValidate(book);

        //Assert
        response.ShouldHaveValidationErrorFor(e => e.YearofPublish)
            .WithErrorMessage($"'Yearof Publish' must be between 1000 and {DateTime.Now.Year}. You entered {year}.")
              .WithSeverity(Severity.Error);
    }

    [Fact]
    public void Can_YearofPublish_Go_OutofUpperBound()
    {
        //Arrange
        int year = DateTime.Now.Year + 1;

        var book = new Book {
            Id = 1,
            Title = "Harry Potter and the Philosopher's Stone",
            Author = "J.K.Rowling",
            YearofPublish = year
        };

        //Act
        TestValidationResult<Book> response = 
                        _bookValidator.TestValidate(book);

        //Assert
        response.ShouldHaveValidationErrorFor(e => e.YearofPublish)
            .WithErrorMessage($"'Yearof Publish' must be between 1000 and {DateTime.Now.Year}. You entered {year}.")
              .WithSeverity(Severity.Error);
    }
}