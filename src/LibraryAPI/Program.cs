using FluentValidation;
using LibraryAPI.Data;
using LibraryAPI.Helper;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("Library"));
builder.Services.AddScoped<IValidator<Book>, BookValidator>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("api/v1/books", async(AppDbContext context) => {
   
   var books = await context.Books.ToListAsync();

   return Results.Ok(books);

});

app.MapGet("api/v1/books/{bookId}", async(AppDbContext context, int bookId) => {

    if(bookId <= 0) return Results.BadRequest();
        var bookExist = await context.Books.FindAsync(bookId);

        if(bookExist is null) return Results.NotFound();
          var book =  await context.Books.FirstOrDefaultAsync(x => x.Id == bookId);

          return Results.Ok(book);
});

app.MapPost("api/v1/books", async(AppDbContext context, IValidator<Book> bookValidator, Book book) => {

    if(book is null) return Results.BadRequest();

    var validatorResult = await bookValidator.ValidateAsync(book);

    if(!validatorResult.IsValid) 
        return Results.ValidationProblem(validatorResult.ToDictionary());

    await context.Books.AddAsync(book);

    await context.SaveChangesAsync();

    return Results.Created($"api/v1/books/{book.Id}", book);

});

app.MapPut("api/v1/books/{bookId}", async(AppDbContext context, IValidator<Book> bookValidator, int bookId, Book book) => {

    if(bookId <= 0 || book is null) return Results.BadRequest("Invalid request");

    var bookExist = await context.Books.FindAsync(bookId);  

    if(bookExist is null)  return Results.NotFound();

    var validatorResult = await bookValidator.ValidateAsync(book);

    if(!validatorResult.IsValid) 
        return Results.ValidationProblem(validatorResult.ToDictionary());

    bookExist.Id = bookId;
    bookExist.Title = book.Title;
    bookExist.Author = book.Author;
    bookExist.YearofPublish = book.YearofPublish;

    context.Books.Update(bookExist);

    await context.SaveChangesAsync();

    return Results.Accepted($"api/v1/books/{bookExist.Id}", bookExist);
});

app.MapDelete("api/v1/books/{bookId}", async(AppDbContext context, int bookId) => {

    if(bookId <= 0) return Results.BadRequest("Invalid request");

    var bookExist = await context.Books.FindAsync(bookId);  

    if(bookExist is null)  return Results.NotFound();

    context.Books.Remove(bookExist);

    await context.SaveChangesAsync();

    return Results.Accepted();
});

app.Run();

