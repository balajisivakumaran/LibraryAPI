using Xunit.Extensions.Ordering;

namespace LibraryAPI.IntTests;


public class LibraryAPIIntegrationTests
{
    private RestClient _restClient;
    public LibraryAPIIntegrationTests()
    {
        _restClient = new RestClient(new Uri("https://librarymanagerapi.azurewebsites.net/"));
    }

    [Fact, Order(1)]
    public void Check_StatusCode_Returned_IsOK()
    {
        //Arrange
        RestRequest restRequest = new RestRequest("api/v1/books", Method.Get);

        //Act
        RestResponse<Book> response = _restClient.Execute<Book>(restRequest);

        //Assert
        response.StatusCode.Equals(HttpStatusCode.OK);
    }

    [Fact, Order(2)]
    public void Ensure_ListOfBookRecords_IsReturned_FromDataSeed()
    {
        //Arrange
        /*Books list is data seeded*/
        List<string> bookAuthors = new List<string>{"J.R.R. Tolkien", "J.K. Rowling", "F. Scott Fitzgerald"};

        RestRequest restRequest = new RestRequest("api/v1/books", Method.Get);

        //Act
        RestResponse<List<Book>> responses = _restClient.Execute<List<Book>>(restRequest);

        //Assert
        /*Magic number: 3 Total pre-seeded data=3. Test is looking if the pre-seeded data count tallys*/
        responses.Data!.Count.Should().Be(3);

        foreach (var bookAuthor in bookAuthors)
        {
            responses.Data?.ForEach(q => q.Author.Equals(bookAuthor));
        }        
    }

    [Theory, Order(3)]
    [MemberData(nameof(TestDataGenerator.AddBook), MemberType = typeof(TestDataGenerator))]
    public void Ensure_Enduser_IsAbleTo_AddaBook(string title, string author, int year)
    {
        //Arrange
        var book = new Book{
            Title = title,
            Author = author,
            YearofPublish = year
        };

        RestRequest restRequest = new RestRequest("api/v1/books", Method.Post);

        var serializeBook = JsonSerializer.Serialize<Book>(book);

        restRequest.AddJsonBody(serializeBook);

        //Act
        RestResponse<Book> response = _restClient.Execute<Book>(restRequest);

        //Assert
        response.Data?.Id.Should().BeGreaterThan(0);
        response.Data?.Title.Should().Be(title);
        response.Data?.Author.Should().Be(author);
        response.Data?.YearofPublish.Should().Be(year);
      
    }

    [Theory, Order(4)]
    [MemberData(nameof(TestDataGenerator.UpdateBook), MemberType = typeof(TestDataGenerator))]
    public void Ensure_Enduser_IsAbleTo_UpdateBook(int id, string title, string author, int year)
    {
        //Arrange
        var book = new Book{
            Title = title,
            Author = author,
            YearofPublish = year
        };

        RestRequest restRequest = new RestRequest($"api/v1/books/{id}", Method.Put);

        var serializeBook = JsonSerializer.Serialize<Book>(book);

        restRequest.AddJsonBody(serializeBook);

        //Act
        RestResponse<Book> response = _restClient.Execute<Book>(restRequest);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      
    }

    //Test to verify DELETE verb and also does the tear down
    [Fact, Order(5)]
    public void Ensure_Enduser_IsAbleTo_DeleteBooks()
    {
        //Arrange        
        RestRequest restRequest = new RestRequest("api/v1/books", Method.Get);
        RestResponse<List<Book>> responses = _restClient.Execute<List<Book>>(restRequest);
        
        /*Magic number: 3 Total pre-seeded data=3, the below selects any ids greater than 3*/
        List<int> bookIds = responses.Data!.Where(q => q.Id > 3).Select(q => q.Id).ToList();
        RestResponse? response;

        //Act
        if(bookIds.Any())
        {
            foreach (var bookId in bookIds)
            {
                RestRequest restDeleteRequest = new RestRequest($"api/v1/books/{bookId}", Method.Delete);
            
                response = _restClient.Execute(restDeleteRequest);

                //Assert
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            }
        }
     
    }
}