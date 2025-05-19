using System.Reflection;
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
        private readonly AppDbContext _Context;

        public CommentService(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<List<CommentGetDto>> GetAllAsync()
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
                .ToListAsync();
        }

        public async Task<CommentGetDto?> GetByIdAsync(long Id)
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
        }

        public async Task<CommentCreateDto> CreateAsync(CommentCreateDto Dto)
        {
            var Comment = new Comment
            {
                Body = Dto.Body,
                UserId = Dto.UserId,
                MovieId = Dto.MovieId
            };

            await _Context.Comments.AddAsync(Comment);
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<UpdateResponseDto> UpdateAsync(long Id, long UserId, CommentUpdateDto Dto)
        {
            var ResponseDto = new UpdateResponseDto
            {
                IsUpdated = false,
                Issue = UpdateIssue.None
            };

            var Comment = await _Context.Comments.FindAsync(Id);

            if (Comment == null)
            {
                ResponseDto.Issue = UpdateIssue.NotFound;
                return ResponseDto;
            } 
            
            else if (Comment.UserId != UserId)
            {
                ResponseDto.Issue = UpdateIssue.Unauthorized;
                return ResponseDto;
            }

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

            ResponseDto.IsUpdated = true;

            return ResponseDto;
        }

        public async Task<DeleteResponseDto> DeleteAsync(long Id, long UserId)
        {
            var ResponseDto = new DeleteResponseDto { 
                IsDeleted = false,
                Issue = DeleteIssue.None
            };

            var Comment = await _Context.Comments.FindAsync(Id);

            if (Comment == null)
            {
                ResponseDto.Issue = DeleteIssue.NotFound;
                return ResponseDto;
            }

            else if (Comment.UserId != UserId)
            {
                ResponseDto.Issue = DeleteIssue.Unauthorized;
                return ResponseDto;
            }

            _Context.Comments.Remove(Comment);
            await _Context.SaveChangesAsync();

            ResponseDto.IsDeleted = true;

            return ResponseDto;
        }
    }
}