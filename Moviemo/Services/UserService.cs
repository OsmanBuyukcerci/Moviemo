using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos;
using Moviemo.Dtos.Comment;
using Moviemo.Dtos.Review;
using Moviemo.Dtos.Token;
using Moviemo.Dtos.User;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _Context;

        private readonly ITokenInterface _TokenService;

        public UserService(AppDbContext Context, ITokenInterface TokenService)
        {
            _Context = Context;
            _TokenService = TokenService;
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
                        CreatedAt = R.CreatedAt,
                        UpdatedAt = R.UpdatedAt,
                    }).ToList(),
                    Comments = U.Comments
                    .Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = U.Id,
                        MovieId = C.Movie.Id,
                        CreatedAt = C.CreatedAt,
                        UpdatedAt = C.UpdatedAt,
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
                        CreatedAt = R.CreatedAt,
                        UpdatedAt = R.UpdatedAt
                    }).ToList(),
                    Comments = U.Comments
                    .Select(C => new CommentGetDto
                    {
                        Id = C.Id,
                        Body = C.Body,
                        UserId = U.Id,
                        MovieId = C.Movie.Id,
                        CreatedAt = C.CreatedAt,
                        UpdatedAt = C.UpdatedAt,
                        DownvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Downvote),
                        UpvoteCounter = C.Votes.Count(V => V.VoteType == VoteType.Upvote)
                    }).ToList()
                })
                .FirstOrDefaultAsync(U => U.Id == Id);
        }

        public async Task<UserCreateDto?> CreateAsync(UserCreateDto Dto)
        {
            if (await _Context.Users.AnyAsync(U => U.Username == Dto.Username))
            {
                return null;
            }

            var User = new User
            {
                Name = Dto.Name,
                Surname = Dto.Surname,
                Username = Dto.Username,
                Email = Dto.Email,
                UserRole = Dto.UserRole
            };

            var HashedPassword = new PasswordHasher<User>()
                .HashPassword(User, Dto.Password);

            User.PasswordHash = HashedPassword;

            await _Context.Users.AddAsync(User);
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<UpdateResponseDto> UpdateAsync(long Id, long UserId, UserUpdateDto Dto)
        {
            var ResponseDto = new UpdateResponseDto
            {
                IsUpdated = false,
                Issue = UpdateIssue.None
            };

            var User = await _Context.Users.FindAsync(Id);

            if (User == null)
            {
                ResponseDto.Issue = UpdateIssue.NotFound;
                return ResponseDto;
            }

            else if (User.Id != UserId)
            {
                ResponseDto.Issue = UpdateIssue.Unauthorized;
                return ResponseDto;
            }

            var DtoProperties = Dto.GetType().GetProperties();
            var UserType = User.GetType();

            foreach (var Property in DtoProperties)
            {
                var NewValue = Property.GetValue(Dto);
                if (NewValue == null) continue;

                var TargetProperty = UserType.GetProperty(Property.Name);

                if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                if (Property.Name == "Username" && 
                    User.Username != Dto.Username &&
                    Dto.Username != null &&
                    await _Context.Users.AnyAsync(U => U.Username == Dto.Username))
                {
                    ResponseDto.Issue = UpdateIssue.SameUsername;
                    return ResponseDto;
                }

                if (Property.Name == "Password" && Dto.Password != null)
                {
                    NewValue = new PasswordHasher<User>()
                        .HashPassword(User, Dto.Password);
                }


                TargetProperty.SetValue(User, NewValue);
            }

            await _Context.SaveChangesAsync();

            ResponseDto.IsUpdated = true;

            return ResponseDto;
        }

        public async Task<DeleteResponseDto> DeleteAsync(long Id, long UserId)
        {
            var ResponseDto = new DeleteResponseDto
            {
                IsDeleted = false,
                Issue = DeleteIssue.None
            };

            var User = await _Context.Users.FindAsync(Id);

            if (User == null)
            {
                ResponseDto.Issue = DeleteIssue.NotFound;
                return ResponseDto;
            }

            else if (User.Id != UserId)
            {
                ResponseDto.Issue = DeleteIssue.Unauthorized;
                return ResponseDto;
            }

            _Context.Users.Remove(User);
            await _Context.SaveChangesAsync();

            ResponseDto.IsDeleted = true;

            return ResponseDto;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto Dto)
        {
            var User = await _Context.Users.FirstOrDefaultAsync(U => U.Username == Dto.Username);

            if (User == null) return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(User, User.PasswordHash, Dto.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await _TokenService.CreateTokenResponseAsync(User);
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto Dto)
        {
            var Result = await _TokenService.RefreshTokensAsync(Dto);
            return Result;
        }
    }
}