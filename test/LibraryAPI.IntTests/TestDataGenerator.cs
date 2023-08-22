using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.IntTests
{
    public static class TestDataGenerator
    {
        public static IEnumerable<object[]> AddBook()
        {
            return new List<object[]>{
                new object[] { "The Catcher in the Rye","J.D. Salinger",1951}
            };
        }

        public static IEnumerable<object[]> UpdateBook()
        {
            return new List<object[]>{
                new object[] { 3, "The Great Gatsby", "", 1925}
            };
        }
    }
}