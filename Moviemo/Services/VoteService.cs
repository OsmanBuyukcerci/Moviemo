using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos.Vote;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class VoteService : IVoteService
    {
        private readonly AppDbContext _Context;

        public VoteService(AppDbContext Context) 
        {
            _Context = Context;
        }

        public async Task<List<VoteGetDto>> GetAllAsync()
        {
            return await _Context.Votes
                .Include(V => V.User)
                .Include(V => V.Comment)
                .Select(V => new VoteGetDto 
                {
                    Id = V.Id,
                    UserId = V.User.Id,
                    CommentId = V.Comment.Id
                })
                .ToListAsync();
        }

        public async Task<VoteGetDto?> GetByIdAsync(long Id)
        {
            return await _Context.Votes
                .Include(V => V.User)
                .Include(V => V.Comment)
                .Select(V => new VoteGetDto
                {
                    Id = V.Id,
                    UserId = V.User.Id,
                    CommentId = V.Comment.Id
                })
                .FirstOrDefaultAsync(V => V.Id == Id);
        }

        public async Task<VoteCreateDto> CreateAsync(VoteCreateDto Dto)
        {
            var Vote = new Vote 
            { 
                UserId = Dto.UserId,
                VoteType = Dto.VoteType,
                CommentId = Dto.CommentId,
            };

            await _Context.Votes.AddAsync(Vote);
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<bool> UpdateAsync(long Id, VoteUpdateDto Dto)
        {
            var Vote = await _Context.Votes.FindAsync(Id);

            if (Vote== null) return false;

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

            return true;
        }

        public async Task<bool> DeleteAsync(long Id)
        {
            var Vote = await _Context.Votes.FindAsync(Id);

            if (Vote == null) return false;

            _Context.Votes.Remove(Vote);

            return true;
        }
    }
}