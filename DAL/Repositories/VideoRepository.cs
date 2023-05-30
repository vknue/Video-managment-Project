using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetAll();
        Video GetById(int id);
        void Delete(int id);
        int Insert(Video video);
        void Edit(int id, Video video);
    }

    public class VideoRepository : IVideoRepository
    {

        private readonly RwaMoviesContext _db;

        public VideoRepository(RwaMoviesContext db)
        {
            _db = db;
        }

        public Video GetById(int id)
            => _db.Videos.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Video> GetAll()
            => _db.Videos;

        public void Delete(int id)
        {
            var target = _db.Videos.FirstOrDefault (x => x.Id == id);
            _db.Videos.Remove(target);
            _db.SaveChanges();
        }

        public int Insert(Video video)
        {
            int id = GetAll().ToList().Count == 0 ? 1 : GetAll().Max(x => x.Id) + 1;
            video.CreatedAt = DateTime.UtcNow;
            _db.Videos.Add(video);
            _db.SaveChanges();
            return id;
        }

        public void Edit(int id, Video video)
        {
            Video target = _db.Videos.First(x => x.Id == id);
            target.Image = video.Image;
            target.Description = video.Description;
            target.Genre = video.Genre;
            target.Name = video.Name;
            target.StreamingUrl = video.StreamingUrl;
            target.VideoTags = video.VideoTags; ;
            target.ImageId = video.ImageId;
            _db.SaveChanges();
        }
    }

}
