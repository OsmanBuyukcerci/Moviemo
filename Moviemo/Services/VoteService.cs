using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos;
using Moviemo.Dtos.Vote;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class VoteService : IVoteService
    {
        private readonly ILogger<VoteService> _Logger;

        private readonly AppDbContext _Context;

        public VoteService(ILogger<VoteService> Logger, AppDbContext Context) 
        {
            _Logger = Logger;
            _Context = Context;
        }

        public async Task<List<VoteGetDto>?> GetAllAsync()
        {
            _Logger.LogInformation("Tüm oy bilgileri alınıyor...");

            try
            {
                return await _Context.Votes
                .Include(V => V.User)
                .Include(V => V.Comment)
                .Select(V => new VoteGetDto
                {
                    Id = V.Id,
                    UserId = V.User.Id,
                    CommentId = V.Comment.Id,
                    VoteType = V.VoteType
                })
                .ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Tüm oy bilgileri alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<VoteGetDto?> GetByIdAsync(long Id)
        {
            _Logger.LogInformation("Vote ID'si {Id} olan vote alınıyor...", Id);

            try
            {
                return await _Context.Votes
                .Include(V => V.User)
                .Include(V => V.Comment)
                .Select(V => new VoteGetDto
                {
                    Id = V.Id,
                    UserId = V.User.Id,
                    CommentId = V.Comment.Id,
                    VoteType = V.VoteType
                })
                .FirstOrDefaultAsync(V => V.Id == Id);
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Oy bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<VoteGetDto?> GetByUserAndCommentIdAsync(long? UserId, long? CommentId)
        {
            _Logger.LogInformation("User ID'si {UserId} Comment ID'si {CommentId} olan oy alınıyor...", UserId, CommentId);

            try
            {
                return await _Context.Votes.Where(V => V.UserId == UserId).Where(V => V.CommentId == CommentId).Select(V => new VoteGetDto
                {
                    Id = V.Id,
                    UserId = V.User.Id,
                    CommentId = V.Comment.Id,
                    VoteType = V.VoteType
                }).FirstOrDefaultAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Filme ait tüm yorum bilgileri alınırken bir sorun meydana geldi.");
                return null;
            }
        }

        public async Task<CreateResponseDto?> CreateAsync(VoteCreateDto Dto, long UserId)
        {
            _Logger.LogInformation("Yeni oy oluşturuluyor: {@VoteCreateDto}", Dto);

            try
            {
                var User = await _Context.Users.Include(U => U.Votes).FirstOrDefaultAsync(U => U.Id == UserId);

                var Vote = new Vote
                {
                    UserId = UserId,
                    VoteType = Dto.VoteType,
                    CommentId = Dto.CommentId,
                };

                await _Context.Votes.AddAsync(Vote);
                await _Context.SaveChangesAsync();

                return new CreateResponseDto { IsCreated = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Oy oluşturulurken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, VoteUpdateDto Dto)
        {
            _Logger.LogInformation("Vote ID'si {Id} olan oy güncelleniyor: {@VoteUpdateDto}", Id, Dto);

            try
            {
                var Vote = await _Context.Votes.FindAsync(Id);

                if (Vote == null)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotFound };

                if (Vote.UserId != UserId)
                    return new UpdateResponseDto() { Issue = UpdateIssue.NotOwner };

                var DtoProperties = Dto.GetType().GetProperties();
                var VoteType = Vote.GetType();

                foreach (var Property in DtoProperties)
                {
                    var NewValue = Property.GetValue(Dto);
                    if (NewValue == null) continue;

                    var TargetProperty = VoteType.GetProperty(Property.Name);
                    if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                    TargetProperty.SetValue(Vote, NewValue);
                }

                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Vote ID'si {Id} olan oy başarıyla güncellendi.", Id);

                return new UpdateResponseDto { IsUpdated = true };
            } 
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Oy bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId)
        {
            _Logger.LogInformation("Vote ID'si {Id} olan oy siliniyor...", Id);
             
            try
            {
                var Vote = await _Context.Votes.FindAsync(Id);

                if (Vote == null)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotFound };

                if (Vote.UserId != UserId)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotOwner };

                _Context.Votes.Remove(Vote);

                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Vote ID'si {Id} olan oy başarıyla silindi.", Id);

                return new DeleteResponseDto { IsDeleted = true };
            } 
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Oy silinirken bir hata meydana geldi.");
                return null;
            }

            
        }
    }
}