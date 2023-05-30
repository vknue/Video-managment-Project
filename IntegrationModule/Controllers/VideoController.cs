using DAL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _videoRepo;

        public VideoController(IVideoRepository videoRepo)
        {
            _videoRepo = videoRepo;
        }
        // GET: api/<VideosController>
        [HttpGet]
        public ActionResult<IEnumerable<Video>> Get()
        => Ok(_videoRepo.GetAll());

        // GET api/<VideosController>/5
        [HttpGet("{id}")]
        public ActionResult<Video> Get(int id)
        {
            var video = _videoRepo.GetById(id);
            if (video is null) return NotFound();
            return video;
        }
        
        // GET api/<VideosController>/5
        [HttpGet("[action]")]
        public ActionResult<Video> Paged(int page, int size,string orderBy = "id", string orderDirection = "asc")
        {
            List<Video> videos = (List<Video>)_videoRepo.GetAll();


            switch (orderBy) { 
                case "name":
                    videos.OrderBy(s => s.Name);
                    break;
                case "totalTime":
                    videos.OrderBy(x => x.TotalSeconds);
                    break;
                default:
                    videos.OrderBy(s => s.Id);
                    break;
            }

            if (string.Compare(orderDirection, "desc", true) == 0)
            {
                videos.Reverse();
            }


            var retVal = videos.Skip(page * size).Take(size);

            return Ok(videos);
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Video>> Filtered(string filter)
        {
            var filteredVideos = _videoRepo.GetAll();
            filteredVideos = filteredVideos.Where(x =>
                    x.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase));

            return Ok(filteredVideos);
        }

        // POST api/<VideosController>
        [HttpPost]
        public ActionResult<int> Insert([FromBody] Video value)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var id = _videoRepo.Insert(value);
                return Ok(id);
            }
            catch
            {
                return Content("Something went wrong");
            }
        }

        // PUT api/<VideosController>/5
        [HttpPut("{id}")]
        public ActionResult Edit(int id, [FromBody] Video value)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (_videoRepo.GetById(id) is null) return NotFound();
                _videoRepo.Edit(id, value);
                return Ok();
            }
            catch
            {
                return Content("Something went wrong");
            }
        }

        // DELETE api/<VideosController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_videoRepo.GetById(id) is null) return NotFound();
                _videoRepo.Delete(id);
                return Ok();
            }catch
            {
                return Content("Something went wrong");
            }
        }
    }
}
