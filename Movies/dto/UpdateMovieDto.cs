namespace Movies.dto
{
    public class UpdateMovieDto:MovieDto
    {
        public IFormFile ?Poster { get; set; }
    }
}
