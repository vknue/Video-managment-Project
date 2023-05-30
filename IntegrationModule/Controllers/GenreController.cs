using DAL.BLModels;
using DAL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepo;

        public GenreController(IGenreRepository genreRepo)
        {
            _genreRepo= genreRepo;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<Genre>> Get()
        {
            return Ok(_genreRepo.GetAll());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<Genre> Get(int id)
        {
            try
            {
                var genre = _genreRepo.GetById(id);
                if (genre is null) return NotFound();
                return Ok(genre);
            }
            catch
            {
                return Content("Something went wrong");
            };
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<Genre> Add([FromBody] Genre genre)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                int createdGenreId = _genreRepo.Insert(genre);

                return Ok(createdGenreId);
            }
            catch
            {
                return Content("Something went wrong!");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Genre genre)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (_genreRepo.GetById(id) is null) return NotFound();
                _genreRepo.Edit(id, genre);
                return Ok();
            }
            catch
            {
                return Content("Something went wrong");
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var genre = _genreRepo.GetById(id);
                if (genre is null) return NotFound();
                _genreRepo.Delete(id);
                return Ok();
            }
            catch
            {
                return Content("Something went wrong!");
            }

        }
    }
}
