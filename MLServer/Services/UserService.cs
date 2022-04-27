using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kamanri.Database;
using Kamanri.Http;
using Microsoft.Extensions.Logging;
using MLServer.Models;
using MLServer.Models.BasicModels;
using MLServer.Models.BasicModels.User;

namespace MLServer.Services
{
    public interface IUserService
    {
        public Task<MatchResult> Match(string userID);
    }
    public class UserService: IUserService
    {
        private readonly IMLService _mlService;

        private readonly ILogger<UserService> _logger;

        private readonly DatabaseContext _dbc;

        public UserService(IMLService mlService, ILoggerFactory loggerFactory, DatabaseContext dbc)
        {
            _mlService = mlService;
            _logger = loggerFactory.CreateLogger<UserService>();
            _dbc = dbc;
        }

        public async Task<MatchResult> Match(string userID)
        {
            while (!_mlService.IsReady)
            {
                Thread.Sleep(100);
            }
            var result = _mlService.Predict(Convert.ToInt64(userID));
            _logger.LogDebug("Predict Result: ");
            result.Print();
            var matchUsers = _mlService.GetClusterUsers(result);
            _logger.LogDebug("Match Users: ");
            foreach (var matchUser in matchUsers)
            {
                matchUser.Print();
            }
            var matchUserIDList = from matchUser in matchUsers
                where matchUser.UserID != userID
                select matchUser.UserID;

            var tagID_features = new Dictionary<string, float>();
            var tag_tagList = await _dbc.SelectAllRelations<Tag, Tag>();

            for(var i = 0; i < result.Features.Length; i++)
            {
                tagID_features.Add(tag_tagList[i].ID.ToString(), result.Features[i]);
            }

            return new MatchResult(
                pcaFeatures: result.PCAFeatures,
                predictedLabel: result.PredictedLabel,
                score: result.Score,
                userId: userID,
                matchUserIDs: matchUserIDList.ToList(),
                tagID_features: tagID_features
            );
        }
        
    }
}