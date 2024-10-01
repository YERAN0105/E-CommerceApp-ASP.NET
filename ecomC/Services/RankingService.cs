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

    public async Task CreateRanking(Ranking ranking) =>
        await _rankingRepository.CreateRanking(ranking);

    public async Task UpdateComment(string rankingId, string newComment) =>
        await _rankingRepository.UpdateComment(rankingId, newComment);
}