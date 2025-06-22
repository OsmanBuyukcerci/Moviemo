using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos;
using Moviemo.Dtos.Comment;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class CommentService : ICommentService
    {
        private readonly ILogger<CommentService> _Logger;

        private readonly AppDbContext _Context;

        public CommentService(AppDbContext Context, ILogger<CommentService> Logger)
        {
            _Logger = Logger;
            _Context = Context;
        }

        public async Task<List<CommentGetDto>?> GetAllAsync()
        {
            _Logger.LogInformation("Tüm yorum bilgileri alınıyor...");

            try
            {
                return await _Context.Comments
                    .Include(C => C.User)
                    .Include(C => C.Movie)
                    .Include(C => C.Votes)
                    .Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = C.User.Id,
                        MovieId = C.Movie.Id,
                        CreatedAt = C.CreatedAt,
                        UpdatedAt = C.UpdatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    })
                    .ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Tüm yorum bilgileri alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<CommentGetDto?> GetByIdAsync(long Id)
        {
            _Logger.LogInformation("Comment ID'si {Id} olan yorum alınıyor...", Id);

            try
            {
                return await _Context.Comments
                    .Include(C => C.User)
                    .Include(C => C.Movie)
                    .Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = C.User.Id,
                        MovieId = C.Movie.Id,
                        CreatedAt = C.CreatedAt,
                        UpdatedAt = C.UpdatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    })
                    .FirstOrDefaultAsync(C => C.Id == Id);
            } catch (Exception Ex) 
            {
                _Logger.LogError(Ex, "Comment ID'si {Id} olan yorum alınırken bir hata meydana geldi.", Id);
                return null;
            }
        }

        public async Task<List<CommentGetDto>?> GetByMovieIdAsync(long? MovieId)
        {
            _Logger.LogInformation("Filme ait tüm yorum bilgileri alınıyor...");

            try
            {
                return await _Context.Comments.Where(C => C.MovieId == MovieId).Select(C => new CommentGetDto
                {
                    Id = C.Id,
                    Body = C.Body,
                    UserId = C.User.Id,
                    MovieId = C.Movie.Id,
                    CreatedAt = C.CreatedAt,
                    UpdatedAt = C.UpdatedAt,
                    DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                    UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                }).ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Filme ait tüm yorum bilgileri alınırken bir sorun meydana geldi.");
                return null;
            }
        }

        public async Task<CommentCreateDto?> CreateAsync(CommentCreateDto Dto, long UserId)
        {
            _Logger.LogInformation("Yeni yorum oluşturuluyor: {@CommentCreateDto}", Dto);

            try
            {
                var Comment = new Comment
                {
                    Body = Dto.Body,
                    UserId = UserId,
                    MovieId = Dto.MovieId
                };

                await _Context.Comments.AddAsync(Comment);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Yorum başarıyla oluşturuldu.");

                return Dto;
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Yorum oluşturulurken bir hata meydana geldi.");
                return null; 
            }
        }

        public async Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, CommentUpdateDto Dto)
        {
            _Logger.LogInformation("Comment ID'si {Id} olan yorum güncelleniyor: {@CommentUpdateDto}", Id, Dto);

            try 
            { 
                var Comment = await _Context.Comments.FindAsync(Id);

                if (Comment == null)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotFound };

                if (Comment.UserId != UserId)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotOwner };

                    var DtoProperties = Dto.GetType().GetProperties();
                var CommentType = Comment.GetType();

                /* CommentUpdateDto'nun tek propertysi var ancak uygulamanın 
                 * scalable olması için böyle bıraktım */
                foreach (var Property in DtoProperties)
                {
                    var NewValue = Property.GetValue(Dto);
                    if (NewValue == null) continue;

                    var TargetProperty = CommentType.GetProperty(Property.Name);
                    if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                    TargetProperty.SetValue(Comment, NewValue);
                }

                Comment.UpdatedAt = DateTime.UtcNow;

                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Comment ID'si {Id} olan yorum başarıyla güncellendi.", Id);

                return new UpdateResponseDto { IsUpdated = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Yorum güncellenirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId)
        {
            _Logger.LogInformation("Comment ID'si {Id} olan yorum siliniyor...", Id);

            try
            {
                var Comment = await _Context.Comments.FindAsync(Id);

                if (Comment == null)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotFound };

                if (Comment.UserId != UserId)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotOwner  };

                _Context.Comments.Remove(Comment);
                await _Context.SaveChangesAsync();


                _Logger.LogInformation("Comment ID'si {Id} olan yorum başarıyla silindi.", Id);

                return new DeleteResponseDto { IsDeleted = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Yorum silinirken bir hata meydana geldi.");
                return null;
            }
        }
    }
}