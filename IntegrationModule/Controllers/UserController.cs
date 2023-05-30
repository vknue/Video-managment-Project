using DAL.BLModels;
using DAL.Repositories;
using DAL.Models;
using IntegrationModule.VModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase 
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<BLUser>> Get()
        {
            return Ok(_userRepo.GetAll());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<BLUser> Get(int id)
        { 
            try
            {
                var user = _userRepo.GetById(id);
                if (user is null) return NotFound();
                return Ok(user);
            }
            catch(Exception e)
            {
                return Content(e.Message);
            };
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<BLUser> Add([FromBody] UserCreateRequest user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                BLUser createdUser = _userRepo.CreateUser(user.Username, user.FirstName, user.LastName, user.Email, user.Password, user.CountryOfResidence, user.PhoneNumber);

                return Ok(createdUser);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            try
            {

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try {
                var user = _userRepo.GetById(id);
                if (user is null) return NotFound();
                _userRepo.DeactivateUser(id);
                return Ok();
            }
            catch
            {
                return Content("Something went wrong!");
            }
            
        }
    }
}
