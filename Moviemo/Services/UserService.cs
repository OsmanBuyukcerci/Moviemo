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
        private readonly ILogger<UserService> _Logger;

        private readonly AppDbContext _Context;

        private readonly ITokenService _TokenService;

        public UserService(ILogger<UserService> Logger, AppDbContext Context, ITokenService TokenService)
        {
            _Logger = Logger;
            _Context = Context;
            _TokenService = TokenService;
        }

        public async Task<List<UserGetDto>?> GetAllAsync()
        {
            _Logger.LogInformation("Tüm kullanıcı bilgileri alınıyor...");

            try
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
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Tüm kullanıcı bilgileri alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<UserGetDto?> GetByIdAsync(long Id)
        {
            _Logger.LogInformation("User ID'si {Id} olan kullanıcı bilgisi alınıyor...", Id);

            try
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
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<UserGetDto?> GetByUsernameAsync(string Username)
        {
            _Logger.LogInformation("Kullanıcı adı {Username} olan kullanıcı bilgisi alınıyor...", Username);

            try
            {
                return await _Context.Users.Where(U => U.Username == Username).Select(U => new UserGetDto
                {
                    Id = U.Id,
                    Name = U.Name,
                    Surname = U.Surname,
                    Username = U.Username,
                    Email = U.Email,
                    UserRole = U.UserRole
                }).FirstOrDefaultAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<CreateResponseDto?> CreateAsync(UserCreateDto Dto)
        {
            _Logger.LogInformation("Yeni kullanıcı oluşturuluyor: {@UserCreateDto}", Dto);

            try
            {
                if (await _Context.Users.AnyAsync(U => U.Username == Dto.Username))
                {
                    return new CreateResponseDto { Issue = CreateIssue.SameContent };
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

                return new CreateResponseDto { IsCreated = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı oluşturulurken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, UserUpdateDto Dto)
        {
            _Logger.LogInformation("User ID'si {Id} olan kullanıcı güncelleniyor: {@UserUpdateDto}", Id, Dto);

            try
            {
                var User = await _Context.Users.FindAsync(Id);

                if (User == null)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotFound };

                var UserThatUpdate = await _Context.Users.FindAsync(UserId);

                if (User.Id != UserId && UserThatUpdate.UserRole != UserRole.Manager)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotOwner };

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
                        return new UpdateResponseDto { Issue = UpdateIssue.SameContent };
                    }

                    TargetProperty.SetValue(User, NewValue);
                }

                await _Context.SaveChangesAsync();


                _Logger.LogInformation("User ID'si {Id} olan kullanıcı başarıyla güncellendi.", Id);

                return new UpdateResponseDto { IsUpdated = true };
            } 
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı güncellenirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId)
        {
            _Logger.LogInformation("User ID'si {Id} olan kullanıcı siliniyor.", Id);

            try
            {
                var User = await _Context.Users.FindAsync(Id);

                if (User == null)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotFound };

                if (User.Id != UserId)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotOwner };

                _Context.Users.Remove(User);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("User ID'si {Id} olan kullanıcı başarıyla silindi.", Id);

                return new DeleteResponseDto { IsDeleted = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı silinirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<LoginResponseDto?> LoginAsync(UserLoginDto Dto)
        {
            _Logger.LogInformation("Kullanıcı {Username} giriş yapıyor...", Dto.Username);

            try
            {
                var User = await _Context.Users.FirstOrDefaultAsync(U => U.Username == Dto.Username);

                if (User == null)
                    return new LoginResponseDto { Issue = LoginIssue.NotFound };

                if (new PasswordHasher<User>().VerifyHashedPassword(User, User.PasswordHash, Dto.Password)
                    == PasswordVerificationResult.Failed)
                {
                    return new LoginResponseDto { Issue = LoginIssue.IncorrectPassword};
                }

                _Logger.LogInformation("Kullanıcı {Username } başarıyla giriş yaptı.", Dto.Username);

                return new LoginResponseDto 
                { 
                    Username = Dto.Username,
                    IsLoggedIn = true , 
                    TokenResponse = await _TokenService.CreateTokenResponseAsync(User) 
                };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı giriş yaparken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(long UserId, RefreshTokenRequestDto Dto)
        {
            var Result = await _TokenService.RefreshTokensAsync(UserId, Dto.RefreshToken);

            return Result;
        }

        public async Task<PasswordChangeResponseDto?> ChangePasswordAsync(ChangePasswordDto Dto, long Id, long UserId)
        {
            _Logger.LogInformation("User ID'si {Id} olan kullanıcı parolasını değiştiriyor...", UserId);

            try
            {
                if (Id != UserId)
                {
                    return new PasswordChangeResponseDto { Issue = PasswordChangeIssue.Unauthorized };
                }

                var User = await _Context.Users.FindAsync(UserId);

                if (User != null)
                {
                    if (new PasswordHasher<User>().VerifyHashedPassword(User, User.PasswordHash, Dto.OldPassword)
                        == PasswordVerificationResult.Failed)
                    {
                        return new PasswordChangeResponseDto { Issue = PasswordChangeIssue.IncorrectOldPassword };
                    }
                }
                var HashedNewPassword = new PasswordHasher<User>()
                    .HashPassword(User, Dto.NewPassword);
                
                User.PasswordHash = HashedNewPassword;

                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Kullanıcı parolası başarıyla değiştirildi.");

                return new PasswordChangeResponseDto { };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Kullanıcı parolası değiştirilirken bir hata meydana geldi.");
                return null;
            }
        }
    }
}