using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAll();
        Genre GetById(int id);
        void Delete(int id);
        int Insert(Genre genre);
        void Edit(int id, Genre genre);
    }

    public class GenreRepository : IGenreRepository
    {

        private readonly RwaMoviesContext _db;

        public GenreRepository(RwaMoviesContext db)
        {
            _db = db;
        }

        public Genre GetById(int id)
            => _db.Genres.FirstOrDefault(x => x.Id == id, null);

        public IEnumerable<Genre> GetAll()
            => _db.Genres;

        public void Delete(int id)
        {
            _db.Genres.Remove(_db.Genres.First(x => x.Id == id));
            _db.SaveChanges();
        }

        public int Insert(Genre genre)
        {
            int id = GetAll().ToList().Count == 0 ? 1 : GetAll().Max(x => x.Id) + 1;
            genre.Id = id;
            _db.Genres.Add(genre);
            _db.SaveChanges();
            return id;
        }

        public void Edit(int id, Genre genre)
        {
            Genre target = _db.Genres.First(x => x.Id == id);
            target.Name = genre.Name;
            target.Description = genre.Description;
            _db.SaveChanges();
        }
    }
}
