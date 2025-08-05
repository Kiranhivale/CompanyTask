using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Task.Models;

namespace Task.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDbContext _context;
        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View(); // List will be filled using jQuery/Ajax
            }
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var movies = _context.Movies.FromSqlRaw("EXEC sp_GetAllMovies").ToList();
            return Json(movies);
        }

        [HttpPost]
        public IActionResult Add(Movie movie)
        {
            _context.Database.ExecuteSqlRaw("EXEC sp_AddMovie @p0, @p1, @p2", movie.Title, movie.Genre, movie.ReleaseDate);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Update(Movie movie)
        {
            _context.Database.ExecuteSqlRaw("EXEC sp_UpdateMovie @p0, @p1, @p2, @p3", movie.MovieId, movie.Title, movie.Genre, movie.ReleaseDate);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.Database.ExecuteSqlRaw("EXEC sp_DeleteMovie @p0", id);
            return Json(new { success = true });
        }

        public void DeleteMultipleMovies(List<int> movieIds)
        {
            var idList = string.Join(",", movieIds); // Convert list to comma-separated string

            var parameter = new SqlParameter("@MovieIds", idList);

            _context.Database.ExecuteSqlRaw("EXEC DeleteMultipleMovies @MovieIds", parameter);
        }
    }
}
