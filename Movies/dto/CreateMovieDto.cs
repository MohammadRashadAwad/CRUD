namespace Movies.dto
{
    public class CreateMovieDto:MovieDto
    {
        public IFormFile Poster { get; set; }
    }
}
