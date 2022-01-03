using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using MovieStore.Enums;
using MovieStore.Models.Database;
using MovieStore.Models.Settings;
using MovieStore.Models.TMDB;
using MovieStore.Services.Interfaces;

namespace MovieStore.Services
{
    public class TMDBMappingService : IDataMappingService
    {
        private readonly AppSettings _appSettings;
        private readonly IImageService _imageService;

        public TMDBMappingService(IOptions<AppSettings> appSettings, IImageService imageService)
        {
            _appSettings = appSettings.Value;
            _imageService = imageService;
        }

        public async Task<Movie> MapMovieDetailAsync(MovieDetail movie)
        {
            Movie newMovie = null;

            try
            {
                newMovie = new Movie()
                {
                    MovieId = movie.id,
                    Title = movie.title,
                    TagLine = movie.tagline,
                    Overview = movie.overview,
                    RunTime = movie.runtime,
                    VoteAverage = movie.vote_average,
                    ReleaseDate = DateTime.Parse(movie.release_date),
                    TrailerUrl = BuildTrailerPath(movie.videos),
                    Backdrop = await EncodeBackdropImageAsync(movie.backdrop_path),
                    BackdropType = BuildImageType(movie.backdrop_path),
                    Poster = await EncodePosterImageAsync(movie.poster_path),
                    PosterType = BuildImageType(movie.poster_path),
                    Rating = GetRating(movie.release_dates),
                };

                var castMembers = movie.credits.cast
                    .OrderByDescending(c => c.popularity)
                    .GroupBy(c => c.cast_id)
                    .Select(g => g.FirstOrDefault())
                    .Take(20)
                    .ToList();

                castMembers.ForEach(member =>
                {
                    newMovie.Cast.Add(new MovieCast()
                    {
                        CastID = member.id,
                        Department = member.known_for_department,
                        Name = member.name,
                        Character = member.character,
                        ImageUrl = BuildCastImage(member.profile_path),
                    });
                });

                var crewMembers = movie.credits.crew
                    .OrderByDescending(c => c.popularity)
                    .GroupBy(c => c.id)
                    .Select(g => g.First())
                    .Take(20)
                    .ToList();

                crewMembers.ForEach(member =>
                {
                    newMovie.Crew.Add(new MovieCrew()
                    {
                        CrewID = member.id,
                        Department = member.department,
                        Name = member.name,
                        Job = member.job,
                        ImageUrl = BuildCastImage(member.profile_path),
                    });
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in MapMovieDetailAsync: {e.Message}");
            }

            return newMovie;
        }


        public ActorDetail MapActorDetail(ActorDetail actor)
        {
           // Image
           actor.profile_path = BuildCastImage(actor.profile_path);

           // Bio
           if (string.IsNullOrWhiteSpace(actor.biography))
               actor.biography = "Not Available";

           // Place of birth
           if (string.IsNullOrWhiteSpace(actor.place_of_birth))
               actor.place_of_birth = "Not Available";

            // Birthday
            if (string.IsNullOrWhiteSpace(actor.birthday))
                actor.birthday = "Not Available";
            else
                actor.birthday = DateTime.Parse(actor.birthday).ToString("MMM dd, yyyy");

            return actor;
        }

        private string BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.results.FirstOrDefault(r => r.type.ToLower().Trim() == "trailer" && r.key != "")?.key;
            return string.IsNullOrWhiteSpace(videoKey)
                ? videoKey
                : $"{_appSettings.TMDBSettings.BaseYouTubePath}{videoKey}";
        }

        private async Task<byte[]> EncodeBackdropImageAsync(string path)
        {
            var backdropPath =
                $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieStoreSettings.DefaultBackdropSize}/{path}";
            return await _imageService.EncodeImageUrlAsync(backdropPath);
        }

        private static string BuildImageType(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? path : $"image/{Path.GetExtension(path).TrimStart('.')}";
        }

        private async Task<byte[]> EncodePosterImageAsync(string path)
        {
            var posterPath =
                $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieStoreSettings.DefaultPosterSize}/{path}";
            return await _imageService.EncodeImageUrlAsync(posterPath);
        }

        private MovieRating GetRating(Release_Dates dates)
        {
            var movieRating = MovieRating.NR;
            var certification = dates.results.FirstOrDefault(r => r.iso_3166_1 == "US");
            if (certification is not null)
            {
                var apiRating = certification.release_dates.FirstOrDefault(c => c.certification != "")?.certification
                    .Replace("-", "");

                if (!string.IsNullOrWhiteSpace(apiRating))
                {
                    movieRating = (MovieRating)Enum.Parse(typeof(MovieRating), apiRating, true);
                }
            }

            return movieRating;
        }

        private string BuildCastImage(string profilePath)
        {
            if (string.IsNullOrWhiteSpace(profilePath))
                return _appSettings.MovieStoreSettings.DefaultCastImage;

            return
                $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieStoreSettings.DefaultPosterSize}/{profilePath}";
        }
    }
}