using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class Movie
    {

        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
}
