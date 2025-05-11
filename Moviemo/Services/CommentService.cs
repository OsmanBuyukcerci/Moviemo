using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
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

        public async Task<bool> UpdateAsync(long Id, CommentUpdateDto Dto)
        {
            var Comment = await _Context.Comments.FindAsync(Id);

            if (Comment == null) return false;

            var DtoProperties = Dto.GetType().GetProperties();
            var CommentType = Comment.GetType();

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

            return true;
        }

        public async Task<bool> DeleteAsync(long Id)
        {
            var Comment = await _Context.Comments.FindAsync(Id);

            if (Comment == null) return false;

            _Context.Comments.Remove(Comment);
            await _Context.SaveChangesAsync();

            return true;
        }
    }
}