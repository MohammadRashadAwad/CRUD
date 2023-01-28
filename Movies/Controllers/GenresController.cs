using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.dto;
using Movies.Models;
using Movies.Service;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            this._genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genre = await _genreService.GetAll();
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateGenreDto dto)
        {
            var genre = new Genre() { Name = dto.Name };
          await _genreService.Add(genre);
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id,[FromBody]CreateGenreDto dto)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return BadRequest($"No Genre was found the id {id}");
            genre.Name = dto.Name;
            _genreService.Update(genre);

            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var del = await _genreService.GetById(id);
            if (del == null)
                return BadRequest($"No Genre was found the id {id}");
            _genreService.Delete(del);
            return Ok("the delete is done");
        }
    }
}
