using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieStudioWebApplication.Models;

namespace MovieStudioWebApplication.Controllers
{
    public class FilmsController : Controller
    {
        private MovieDbContext db = new MovieDbContext();

        // GET: Films
        public ActionResult Index(string searchString, int? genreId, int? releaseYear)
        {
            var films = db.Films.Include(f => f.Director).Include(f => f.Studio);

            // Searching
            if (!String.IsNullOrEmpty(searchString))
            {
                films = films.Where(f => f.Title.Contains(searchString));
            }

            // Filtering by Genre
            if (genreId.HasValue)
            {
                films = films.Where(f => f.FilmGenres.Any(fg => fg.GenreID == genreId.Value));
            }

            // Filtering by Release Year
            if (releaseYear.HasValue)
            {
                films = films.Where(f => f.ReleaseYear == releaseYear.Value);
            }

            ViewBag.GenreId = new SelectList(db.Genres, "GenreID", "Name", "Жанр");
            ViewBag.ReleaseYear = new SelectList(db.Films.Select(f => f.ReleaseYear).Distinct(), "Год выпуска");

            return View(films.ToList());
        }

        // GET: Films/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        private void PopulateAssignedData(FilmViewModel viewModel)
        {
            var allGenres = db.Genres;
            var filmGenres = new HashSet<int>(viewModel.Film.FilmGenres.Select(g => g.GenreID));
            var allActors = db.Actors;
            var filmActors = new HashSet<int>(viewModel.Film.FilmActors.Select(a => a.ActorID));

            viewModel.Genres = new List<AssignedGenreData>();
            viewModel.Actors = new List<AssignedActorData>();

            foreach (var genre in allGenres)
            {
                viewModel.Genres.Add(new AssignedGenreData
                {
                    GenreID = genre.GenreID,
                    Name = genre.Name,
                    Assigned = filmGenres.Contains(genre.GenreID)
                });
            }

            foreach (var actor in allActors)
            {
                viewModel.Actors.Add(new AssignedActorData
                {
                    ActorID = actor.ActorID,
                    Name = actor.FirstName + " " + actor.LastName,
                    Assigned = filmActors.Contains(actor.ActorID)
                });
            }
        }

        // GET: Films/Create
        public ActionResult Create()
        {
            var viewModel = new FilmViewModel
            {
                Film = new Film(),
            };
            PopulateAssignedData(viewModel);
            ViewBag.DirectorID = new SelectList(db.Directors, "DirectorID", "FirstName");
            ViewBag.StudioID = new SelectList(db.Studios, "StudioID", "Name");
            return View(viewModel);
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FilmViewModel viewModel, string[] selectedGenres, string[] selectedActors)
        {
            if (ModelState.IsValid)
            {
                var film = viewModel.Film;
                UpdateFilmGenres(selectedGenres, film);
                UpdateFilmActors(selectedActors, film);
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedData(viewModel);
            ViewBag.DirectorID = new SelectList(db.Directors, "DirectorID", "FirstName", viewModel.Film.DirectorID);
            ViewBag.StudioID = new SelectList(db.Studios, "StudioID", "Name", viewModel.Film.StudioID);
            return View(viewModel);
        }

        // GET: Films/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films
                .Include(f => f.FilmGenres)
                .Include(f => f.FilmActors)
                .Where(f => f.FilmID == id)
                .Single();

            if (film == null)
            {
                return HttpNotFound();
            }

            var viewModel = new FilmViewModel
            {
                Film = film
            };

            PopulateAssignedData(viewModel);
            ViewBag.DirectorID = new SelectList(db.Directors, "DirectorID", "FirstName", film.DirectorID);
            ViewBag.StudioID = new SelectList(db.Studios, "StudioID", "Name", film.StudioID);
            return View(viewModel);
        }

        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FilmViewModel viewModel, string[] selectedGenres, string[] selectedActors)
        {
            if (ModelState.IsValid)
            {
                var film = db.Films
                    .Include(f => f.FilmGenres)
                    .Include(f => f.FilmActors)
                    .Where(f => f.FilmID == viewModel.Film.FilmID)
                    .Single();

                if (TryUpdateModel(film, "Film", new string[] { "Title", "ReleaseYear", "DurationMinutes", "Budget", "BoxOffice", "Rating", "Synopsis", "StudioID", "DirectorID" }))
                {
                    UpdateFilmGenres(selectedGenres, film);
                    UpdateFilmActors(selectedActors, film);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            PopulateAssignedData(viewModel);
            ViewBag.DirectorID = new SelectList(db.Directors, "DirectorID", "FirstName", viewModel.Film.DirectorID);
            ViewBag.StudioID = new SelectList(db.Studios, "StudioID", "Name", viewModel.Film.StudioID);
            return View(viewModel);
        }

        private void UpdateFilmGenres(string[] selectedGenres, Film filmToUpdate)
        {
            if (selectedGenres == null)
            {
                filmToUpdate.FilmGenres.Clear();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var filmGenres = new HashSet<int>
                (filmToUpdate.FilmGenres.Select(c => c.GenreID));
            foreach (var genre in db.Genres)
            {
                if (selectedGenresHS.Contains(genre.GenreID.ToString()))
                {
                    if (!filmGenres.Contains(genre.GenreID))
                    {
                        filmToUpdate.FilmGenres.Add(new FilmGenre { FilmID = filmToUpdate.FilmID, GenreID = genre.GenreID });
                    }
                }
                else
                {
                    if (filmGenres.Contains(genre.GenreID))
                    {
                        var genreToRemove = filmToUpdate.FilmGenres.Single(c => c.GenreID == genre.GenreID);
                        filmToUpdate.FilmGenres.Remove(genreToRemove);
                    }
                }
            }
        }

        private void UpdateFilmActors(string[] selectedActors, Film filmToUpdate)
        {
            if (selectedActors == null)
            {
                filmToUpdate.FilmActors.Clear();
                return;
            }

            var selectedActorsHS = new HashSet<string>(selectedActors);
            var filmActors = new HashSet<int>
                (filmToUpdate.FilmActors.Select(c => c.ActorID));
            foreach (var actor in db.Actors)
            {
                if (selectedActorsHS.Contains(actor.ActorID.ToString()))
                {
                    if (!filmActors.Contains(actor.ActorID))
                    {
                        filmToUpdate.FilmActors.Add(new FilmActor { FilmID = filmToUpdate.FilmID, ActorID = actor.ActorID, Role = "Актер" });
                    }
                }
                else
                {
                    if (filmActors.Contains(actor.ActorID))
                    {
                        var actorToRemove = filmToUpdate.FilmActors.Single(c => c.ActorID == actor.ActorID);
                        filmToUpdate.FilmActors.Remove(actorToRemove);
                    }
                }
            }
        }


        // GET: Films/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
