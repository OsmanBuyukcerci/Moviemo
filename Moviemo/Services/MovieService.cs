using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos;
using Moviemo.Dtos.Comment;
using Moviemo.Dtos.Movie;
using Moviemo.Dtos.Review;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class MovieService : IMovieService
    {
        private readonly ILogger<MovieService> _Logger;

        private readonly AppDbContext _Context;

        public MovieService(AppDbContext Context, ILogger<MovieService> Logger)
        {
            _Logger = Logger;
            _Context = Context;
        }

        public async Task<List<MovieGetDto>?> GetAllAsync()
        {
            _Logger.LogInformation("Tüm film bilgileri alınıyor...");

            try
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
                            UpdatedAt = R.UpdatedAt,
                        }).ToList(),
                        Comments = M.Comments.Select(C => new CommentGetDto
                        {
                            Id = C.Id,
                            Body = C.Body,
                            UserId = C.UserId,
                            MovieId = M.Id,
                            CreatedAt = C.CreatedAt,
                            UpdatedAt = C.UpdatedAt,
                            DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                            UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                        }).ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Tüm film bilgileri alınırken bir hata meydana geldi.");
                return null;
            };
        }

        public async Task<MovieGetDto?> GetByIdAsync(long Id)
        {
            _Logger.LogInformation("Movie ID'si {Id} olan film bilgisi alınıyor...", Id);

            try
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
                        UpdatedAt = C.UpdatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    }).ToList()
                })
                .FirstOrDefaultAsync(M => M.Id == Id);
            } catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Film bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<MovieCreateDto?> CreateAsync(MovieCreateDto Dto)
        {
            _Logger.LogInformation("Yeni film oluşturuluyor: {@MovieCreateDto}", Dto);

            try
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

                _Logger.LogInformation("Film başarıyla oluşturuldu.");

                return Dto;
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Film oluşturulurken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<UpdateResponseDto?> UpdateAsync(long Id, MovieUpdateDto Dto)
        {
            _Logger.LogInformation("Movie ID'si {Id} olan film güncelleniyor: {@MovieUpdateDto}", Id, Dto);

            try
            {
                var Movie = await _Context.Movies.FindAsync(Id);

                if (Movie == null)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotFound };

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


                _Logger.LogInformation("Movie ID'si {Id} olan film başarıyla güncellendi.", Id);

                return new UpdateResponseDto { IsUpdated = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Film güncellenirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<DeleteResponseDto?> DeleteAsync(long Id)
        {
            _Logger.LogInformation("Movie ID'si {Id} olan yorum siliniyor...", Id);

            try
            {
                var Movie = await _Context.Movies.FindAsync(Id);

                if (Movie == null)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotFound };

                _Context.Movies.Remove(Movie);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Movie ID'si {Id} olan film başarıyla silindi.", Id);

                return new DeleteResponseDto { IsDeleted = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Film silinirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<List<SearchResponseDto>?> SearchAsync(string Query)
        {
            _Logger.LogInformation("{Query} başlığı aranıyor...", Query);

            try
            {
                var Results = await _Context.Movies
                    .Where(M => M.Title.Contains(Query))
                    .Select(M => new SearchResponseDto
                    {
                        Id = M.Id,
                        Title = M.Title,
                        PosterPath = M.PosterPath
                    })
                    .ToListAsync();

                return Results;
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Arama yapılırken bir hata meydana geldi.");
                return null;
            }

        }

        public async Task<MoviePageDto?> GetByPageSizeAsync(int PageIndex, int PageSize)
        {
            _Logger.LogInformation("Sayfa: {PageIndex} - Adet: {PageSize} alınıyor...", PageIndex, PageSize);

            try
            {
                if (PageIndex <= 0 || PageSize <= 0)
                {
                    throw new Exception("Sayfa Numarası veya Sayfa Boyutu hatalı girildi.");
                }

                var TotalMovies = await _Context.Movies.CountAsync();

                var Movies = await _Context.Movies
                    .Skip((PageIndex - 1) * PageSize)
                    .Take(PageSize)
                    .Select(M => new MovieGetDto
                    {
                        Id = M.Id,
                        Title = M.Title,
                        Overview = M.Overview,
                        PosterPath = M.PosterPath,
                        TrailerUrl = M.TrailerUrl
                    })
                    .ToListAsync();
                    
                return new MoviePageDto
                {
                    Data = Movies,
                    Total = TotalMovies,
                    PageSize = PageSize,
                    PageIndex = PageIndex
                };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Sayfa bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }
    }
}