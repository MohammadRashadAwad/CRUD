using System.ComponentModel.DataAnnotations;

namespace Movies.dto
{
    public class CreateGenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
