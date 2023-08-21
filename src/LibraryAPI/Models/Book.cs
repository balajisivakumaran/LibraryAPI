using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Book
    {
        [Key]
        [DisplayName("BookId")]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        public int? YearofPublish { get; set;}

    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class YearRangeAttribute : RangeAttribute
{
    public YearRangeAttribute(int minimum) : base(minimum, DateTime.Now.Year)
    {}
}