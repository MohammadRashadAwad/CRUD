using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.dto;
using Movies.Models;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly List<string> allowedExtenstions = new List<string>() { ".jpg", ".png" };
        private readonly long maxLengthPoster= 1048576;

        public MoviesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movie = await context.Movies.Include(m=>m.Genre)
                .OrderByDescending(o=>o.Rate)
                .Select(m=>new MovieDetailsDto() {
                    Id=m.Id,
                    Title=m.Title,
                    GenreId=m.GenreId,
                    GenreName=m.Genre.Name,
                    Poster=m.Poster,
                    Rate=m.Rate,
                    Storyline=m.Storyline,
                     Year=m.Year
                })
                .ToListAsync();
            return Ok(movie);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieByIDAsync(int id)
        {
            
            var movie = await context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(x=>x.Id==id);
            if (movie == null)
                return NotFound();
            var dto = new MovieDetailsDto() { 
            Id=movie.Id,
            GenreId=movie.GenreId,
            Title=movie.Title,
            Poster=movie.Poster,
            Year=movie.Year,
            Rate=movie.Rate,
            Storyline=movie.Storyline,
            GenreName=movie.Genre.Name,
            };
            return Ok(dto);
        }
        [HttpGet("GetMoviesByGenre")]
        public async Task<IActionResult> GetMoviesByGenreAsync(byte genreId)
        {
            var movie = await context.Movies
                .Where(w=>w.GenreId==genreId)
                .Include(m=>m.Genre)
                .Select(m=> new MovieDetailsDto
                {   Id=m.Id,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name,
                    Title = m.Title,
                    Year=m.Year,
                    Rate=m.Rate,
                    Storyline=m.Storyline,
                    Poster=m.Poster,
                   
                   
                } )
                .ToListAsync();
            return Ok(movie);
        }
      
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]CreateMovieDto dto)
        {
            if (!allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
              return BadRequest("only .jpg and .png images allowed");
            
            if (dto.Poster.Length > maxLengthPoster)
                return BadRequest("Max allowed size for poster is 1MB");
            var isValidGenre = await context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre ID ");
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
                
            
; 
            var movie=new Movie()
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                Storyline = dto.Storyline,
                Poster = dataStream.ToArray(),
            };
           await context.Movies.AddAsync(movie);
            context.SaveChanges();
            return Ok(movie);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return BadRequest($"No Movie was found the id {id}");
            context.Movies.Remove(movie);
            context.SaveChanges();
            return Ok("the delete is done");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id ,[FromForm]UpdateMovieDto dto) 
        {

            var movie = await context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
          if(dto.Poster != null)
            {
                if (!allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return BadRequest("only .jpg and .png images allowed");
                }
                if (dto.Poster.Length > maxLengthPoster)
                    return BadRequest("Max allowed size for poster is 1MB");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }
            var isValidGenre = await context.Movies.AnyAsync(g=>g.GenreId == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre ID");
           
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Year;
            movie.Storyline = dto.Storyline;
            movie.GenreId = dto.GenreId;
            context.SaveChanges();
            return Ok(movie);
        }
    
    }
}
