using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiServer.Models.User;
using ApiServer.Services;
using Kamanri.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Controller]
    [Route("Match/")]
    public class MatchController:Controller
    {
        private readonly MatchService _matchService;
        public MatchController(MatchService matchService)
        {
            _matchService = matchService;
        }
        [HttpGet]
        [Route("Match")]
        public async Task<string> Match(string userID)
        {
            return (await _matchService.Match(new UserInfo(Convert.ToInt64(userID)))).ToJson();
        }
    }
}