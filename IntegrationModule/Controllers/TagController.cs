using DAL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagContoller : ControllerBase
    {
        private readonly ITagRepository _tagRepo;

        public TagContoller(ITagRepository tagRepo)
        {

            _tagRepo = tagRepo;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<Tag>> Get()
        {
            return Ok(_tagRepo.GetAll());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<Tag> Get(int id)
        {
            try
            {
                var tag = _tagRepo.GetById(id);
                if (tag is null) return NotFound();
                return Ok(tag);
            }
            catch
            {
                return Content("Something went wrong");
            };
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<Tag> Add([FromBody] Tag tag)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                int createdTagId = _tagRepo.Insert(
                    new Tag()
                    {
                        Name = tag.Name
                    });

                return Ok(createdTagId);
            }
            catch
            {
                return Content("Something went wrong!");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Tag tag)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (_tagRepo.GetById(id) is null) return NotFound();
                _tagRepo.Edit(id, tag);
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
                var tag = _tagRepo.GetById(id);
                if (tag is null) return NotFound();
                _tagRepo.Delete(id);
                return Ok();
            }
            catch
            {
                return Content("Something went wrong!");
            }

        }
    }
}
