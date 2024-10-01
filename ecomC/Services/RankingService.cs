using ecomC.Models;

public class RankingService
{
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository)
    {
        _rankingRepository = rankingRepository;
    }

    public async Task<IEnumerable<Ranking>> GetRankingsByVendorId(string vendorId) =>
        await _rankingRepository.GetRankingsByVendorId(vendorId);
    
    public async Task<IEnumerable<Ranking>> GetRankingsByCustomer(string customerId) =>
        await _rankingRepository.GetRankingsByCustomerId(customerId);

    public async Task CreateRanking(Ranking ranking) =>
        await _rankingRepository.CreateRanking(ranking);

    public async Task UpdateComment(string rankingId, string newComment) =>
        await _rankingRepository.UpdateComment(rankingId, newComment);
    
    public async Task<double> GetAverageRanking(string vendorId)
    {
        var rankings = await GetRankingsByVendorId(vendorId);
        if (rankings == null || !rankings.Any())
            return 0;

        return rankings.Average(r => r.Rating);
    }

}