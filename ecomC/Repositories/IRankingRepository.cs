using System.Collections.Generic;
using System.Threading.Tasks;
using ecomC.Models;

public interface IRankingRepository
{
    Task<IEnumerable<Ranking>> GetRankingsByVendorId(string vendorId);
    Task<Ranking> GetRankingById(string rankingId);
    Task CreateRanking(Ranking ranking);
    Task UpdateComment(string rankingId, string newComment);
    Task<IEnumerable<Ranking>> GetRankingsByCustomerId(string customerId);
}