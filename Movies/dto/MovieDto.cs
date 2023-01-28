using System.ComponentModel.DataAnnotations;

namespace Movies.dto
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
       
        public byte GenreId { get; set; }
    }
}
