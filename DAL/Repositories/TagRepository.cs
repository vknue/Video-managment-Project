using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAll();
        Tag GetById(int id);
        void Delete(int id);
        int Insert(Tag tag);
        void Edit(int id, Tag tag);
    }

    public class TagRepository : ITagRepository
    {

        private readonly RwaMoviesContext _db;

        public TagRepository(RwaMoviesContext db)
        {
            _db = db;
        }

        public Tag GetById(int id)
            => _db.Tags.FirstOrDefault(x => x.Id == id, null);

        public IEnumerable<Tag> GetAll()
            => _db.Tags;

        public void Delete(int id)
        {
            _db.Tags.Remove(_db.Tags.First(x => x.Id == id));
            _db.SaveChanges();
        }

        public int Insert(Tag tag)
        {
            int id = GetAll().ToList().Count == 0 ? 1 : GetAll().Max(x => x.Id) + 1;
            tag.Id = id;
            _db.Tags.Add(tag);
            _db.SaveChanges();
            return id;
        }

        public void Edit(int id, Tag tag)
        {
            Tag target = _db.Tags.First(x => x.Id == id);
            target.Name = tag.Name;
            _db.SaveChanges();
        }
    }
}
