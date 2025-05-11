using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos.Comment;
using Moviemo.Dtos.Movie;
using Moviemo.Dtos.Review;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _Context;

        public MovieService(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<List<MovieGetDto>> GetAllAsync()
        {
            return await _Context.Movies
                .Include(M => M.Comments)
                .ThenInclude(C => C.User)
                .Select(M => new MovieGetDto 
                {
                    Id = M.Id,
                    Title = M.Title,
                    Overview = M.Overview,
                    PosterPath = M.PosterPath,
                    TrailerUrl = M.TrailerUrl,
                    Reviews = M.Reviews.Select(R => new ReviewGetDto
                    {
                        Id = R.Id,
                        Body = R.Body,
                        UserId = R.User.Id,
                        MovieId = M.Id,
                        UserScore = R.UserScore,
                        CreatedAt = R.CreatedAt,
                        UpdatedAt = R.CreatedAt,
                    }).ToList(),
                    Comments = M.Comments.Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = C.UserId,
                        MovieId = M.Id,
                        CreatedAt = C.CreatedAt,
                        UpdatedAt = C.CreatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<MovieGetDto?> GetByIdAsync(long Id)
        {
            return await _Context.Movies
                .Include(M => M.Comments)
                .ThenInclude(C => C.User)
                .Include(M => M.Reviews)
                .ThenInclude(R => R.User)
                .Select(M => new MovieGetDto
                {
                    Id = M.Id,
                    Title = M.Title,
                    Overview = M.Overview,
                    PosterPath = M.PosterPath,
                    TrailerUrl = M.TrailerUrl,
                    Reviews = M.Reviews.Select(R => new ReviewGetDto
                    {
                        Id = R.Id,
                        Body = R.Body,
                        UserId = R.User.Id,
                        MovieId = M.Id,
                        UserScore = R.UserScore,
                        CreatedAt = R.CreatedAt,
                        UpdatedAt = R.UpdatedAt,
                    }).ToList(),
                    Comments = M.Comments.Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = C.UserId,
                        MovieId = M.Id,
                        CreatedAt = C.CreatedAt,
                        UpdatedAt= C.UpdatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    }).ToList()
                })
                .FirstOrDefaultAsync(M => M.Id == Id);
        }

        public async Task<MovieCreateDto> CreateAsync(MovieCreateDto Dto)
        {
            var Movie = new Movie
            {
                Title = Dto.Title,
                Overview = Dto.Overview,
                PosterPath = Dto.PosterPath,
                TrailerUrl = Dto.TrailerUrl
            };

            await _Context.Movies.AddAsync(Movie);
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<bool> UpdateAsync(long Id, MovieUpdateDto Dto)
        {
            var Movie = await _Context.Movies.FindAsync(Id);

            if (Movie == null) return false;

            var DtoProperties = Dto.GetType().GetProperties();
            var MovieType = Movie.GetType();

            foreach (var Property in DtoProperties)
            {
                var NewValue = Property.GetValue(Dto);
                if (NewValue == null) continue;

                var TargetProperty = MovieType.GetProperty(Property.Name);
                if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                TargetProperty.SetValue(Movie, NewValue);
            }

            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long Id)
        {
            var Movie = await _Context.Movies.FindAsync(Id);

            if (Movie == null) return false;

            _Context.Movies.Remove(Movie);
            await _Context.SaveChangesAsync();

            return true;
        }
    }
}