using MovieStudioWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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
                films = films.Where(f => f.Genres.Any(g => g.GenreID == genreId.Value));
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
            Film film = db.Films
                .Include(f => f.Director)
                .Include(f => f.Studio)
                .Include(f => f.FilmActors.Select(fa => fa.Actor))
                .SingleOrDefault(f => f.FilmID == id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        private void PopulateAssignedData(FilmViewModel viewModel)
        {
            var allGenres = db.Genres;
            var filmGenres = new HashSet<int>(viewModel.Film.Genres.Select(g => g.GenreID));
            var allActors = db.Actors;
            var filmActors = new HashSet<int>(viewModel.Film.FilmActors.Select(a => a.ActorID));
            var filmActorRoles = viewModel.Film.FilmActors.ToDictionary(a => a.ActorID, a => a.Role);

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
                    Assigned = filmActors.Contains(actor.ActorID),
                    Role = filmActorRoles.ContainsKey(actor.ActorID) ? filmActorRoles[actor.ActorID] : null
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
                UpdateFilmActors(selectedActors, Request.Form, film);
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
                .Include(f => f.Genres)
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
                    .Include(f => f.Genres)
                    .Include(f => f.FilmActors)
                    .Where(f => f.FilmID == viewModel.Film.FilmID)
                    .Single();

                if (TryUpdateModel(film, "Film", new string[] { "Title", "ReleaseYear", "DurationMinutes", "Budget", "BoxOffice", "Rating", "Synopsis", "StudioID", "DirectorID" }))
                {
                    UpdateFilmGenres(selectedGenres, film);
                    UpdateFilmActors(selectedActors, Request.Form, film);

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
                filmToUpdate.Genres.Clear();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var filmGenres = new HashSet<int>(
                filmToUpdate.Genres.Select(c => c.GenreID));
            foreach (var genre in db.Genres)
            {
                if (selectedGenresHS.Contains(genre.GenreID.ToString()))
                {
                    if (!filmGenres.Contains(genre.GenreID))
                    {
                        filmToUpdate.Genres.Add(genre);
                    }
                }
                else
                {
                    if (filmGenres.Contains(genre.GenreID))
                    {
                        var genreToRemove = filmToUpdate.Genres.Single(c => c.GenreID == genre.GenreID);
                        filmToUpdate.Genres.Remove(genreToRemove);
                    }
                }
            }
        }

        private void UpdateFilmActors(string[] selectedActors, NameValueCollection form, Film filmToUpdate)
        {
            if (selectedActors == null)
            {
                filmToUpdate.FilmActors.Clear();
                return;
            }

            var selectedActorsHS = new HashSet<string>(selectedActors);
            var filmActors = new HashSet<int>(
                filmToUpdate.FilmActors.Select(c => c.ActorID));
            foreach (var actor in db.Actors)
            {
                var isSelected = selectedActorsHS.Contains(actor.ActorID.ToString());
                var roleValue = form?[$"actorRoles[{actor.ActorID}]"];
                if (isSelected)
                {
                    if (!filmActors.Contains(actor.ActorID))
                    {
                        filmToUpdate.FilmActors.Add(new FilmActor
                        {
                            FilmID = filmToUpdate.FilmID,
                            ActorID = actor.ActorID,
                            Role = string.IsNullOrWhiteSpace(roleValue) ? "Актер" : roleValue
                        });
                    }
                    else
                    {
                        var actorToUpdate = filmToUpdate.FilmActors.Single(c => c.ActorID == actor.ActorID);
                        if (!string.IsNullOrWhiteSpace(roleValue))
                        {
                            actorToUpdate.Role = roleValue;
                        }
                        else if (string.IsNullOrWhiteSpace(actorToUpdate.Role))
                        {
                            actorToUpdate.Role = "Актер";
                        }
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
