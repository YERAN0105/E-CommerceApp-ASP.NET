using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecomC.DTOs;
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

    [HttpPost("create-ranking")]
    public async Task<IActionResult> CreateRanking([FromBody] RankingDTO rankingDTO)
    {
        var ranking = new Ranking
        {
            VendorId = rankingDTO.VendorId,
            Rating = rankingDTO.Rating,
            Comment = rankingDTO.Comment,
            CustomerId = rankingDTO.CustomerId
        };

        await _rankingService.CreateRanking(ranking);
        return CreatedAtAction(nameof(CreateRanking), new { id = ranking.RankingId }, ranking);
    }

    [HttpPut("{rankingId}/comment")]
    public async Task<IActionResult> UpdateComment(string rankingId, [FromBody] string newComment)
    {
        await _rankingService.UpdateComment(rankingId, newComment);
        return NoContent();
    }
}