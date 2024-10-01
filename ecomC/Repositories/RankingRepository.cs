using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecomC.Models;

public class RankingRepository : IRankingRepository
{
    private readonly IMongoCollection<Ranking> _rankings;

    public RankingRepository(IMongoDatabase mongoDatabase)
    {
        _rankings = mongoDatabase.GetCollection<Ranking>("Rankings");
    }

    public async Task<IEnumerable<Ranking>> GetRankingsByVendorId(string vendorId) =>
        await _rankings.Find(r => r.VendorId == vendorId).ToListAsync();

    public async Task<Ranking> GetRankingById(string rankingId) =>
        await _rankings.Find(r => r.RankingId == rankingId).FirstOrDefaultAsync();

    public async Task CreateRanking(Ranking ranking) =>
        await _rankings.InsertOneAsync(ranking);

    public async Task UpdateComment(string rankingId, string newComment)
    {
        var update = Builders<Ranking>.Update.Set(r => r.Comment, newComment);
        await _rankings.UpdateOneAsync(r => r.RankingId == rankingId, update);
    }
}