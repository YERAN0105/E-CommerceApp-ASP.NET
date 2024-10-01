using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecomC.Models;

[ApiController]
[Route("api/[controller]")]
public class RankingController : ControllerBase
{
    private readonly RankingService _rankingService;

    public RankingController(RankingService rankingService)
    {
        _rankingService = rankingService;
    }

    [HttpGet("vendor/{vendorId}")]
    public async Task<IActionResult> GetRankingsByVendorId(string vendorId)
    {
        var rankings = await _rankingService.GetRankingsByVendorId(vendorId);
        return Ok(rankings);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRanking([FromBody] Ranking ranking)
    {
        await _rankingService.CreateRanking(ranking);
        return CreatedAtAction(nameof(GetRankingsByVendorId), new { vendorId = ranking.VendorId }, ranking);
    }

    [HttpPut("{rankingId}/comment")]
    public async Task<IActionResult> UpdateComment(string rankingId, [FromBody] string newComment)
    {
        await _rankingService.UpdateComment(rankingId, newComment);
        return NoContent();
    }
}