using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos.Comment;
using Moviemo.Dtos.Review;
using Moviemo.Dtos.User;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _Context;

        public UserService(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<List<UserGetDto>> GetAllAsync()
        {
            return await _Context.Users
                .Include(U => U.Reviews)
                .ThenInclude(R => R.Movie)
                .Include(U => U.Comments)
                .ThenInclude(C => C.Movie)
                .Select(U => new UserGetDto
                {
                    Id = U.Id,
                    Name = U.Name,
                    Surname = U.Surname,
                    Username = U.Username,
                    Email = U.Email,
                    UserRole = U.UserRole,
                    Reviews = U.Reviews.Select(R => new ReviewGetDto
                    {
                        Id = R.Id,
                        Body = R.Body,
                        UserId = U.Id,
                        MovieId = R.Movie.Id,
                        UserScore = R.UserScore,
                        CreatedAt = R.CreatedAt
                    }).ToList(),
                    Comments = U.Comments
                    .Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = U.Id,
                        MovieId = C.Movie.Id,
                        CreatedAt = C.CreatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<UserGetDto?> GetByIdAsync(long Id)
        {
            return await _Context.Users
                .Include(U => U.Comments)
                .ThenInclude(C => C.Movie)
                .Select(U => new UserGetDto
                {
                    Id = U.Id,
                    Name = U.Name,
                    Surname = U.Surname,
                    Username = U.Username,
                    Email = U.Email,
                    UserRole = U.UserRole,
                    Reviews = U.Reviews.Select(R => new ReviewGetDto
                    {
                        Id = R.Id,
                        Body = R.Body,
                        UserId = U.Id,
                        MovieId = R.Movie.Id,
                        UserScore = R.UserScore,
                        CreatedAt = R.CreatedAt
                    }).ToList(),
                    Comments = U.Comments
                    .Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = U.Id,
                        MovieId = C.Movie.Id,
                        CreatedAt = C.CreatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    }).ToList()
                })
                .FirstOrDefaultAsync(U => U.Id == Id);
        }

        public async Task<UserCreateDto> CreateAsync(UserCreateDto Dto)
        {
            var User = new User
            {
                Name = Dto.Name,
                Surname = Dto.Surname,
                Username = Dto.Username,
                Password = Dto.Password,
                Email = Dto.Email,
                UserRole = Dto.UserRole
            };

            await _Context.Users.AddAsync(User);
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<bool> UpdateAsync(long Id, UserUpdateDto Dto)
        {
            var User = await _Context.Users.FindAsync(Id);

            if (User == null) return false;

            var DtoProperties = Dto.GetType().GetProperties();
            var UserType = User.GetType();

            foreach (var Property in DtoProperties)
            {
                var NewValue = Property.GetValue(Dto);
                if (NewValue == null) continue;

                var TargetProperty = UserType.GetProperty(Property.Name);
                if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                TargetProperty.SetValue(User, NewValue);
            }

            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long Id)
        {
            var User = await _Context.Users.FindAsync(Id);

            if (User == null) return false;

            _Context.Users.Remove(User);
            await _Context.SaveChangesAsync();

            return true;
        }
    }
}