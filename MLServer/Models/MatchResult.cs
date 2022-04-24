using System.Collections.Generic;
using MLServer.Models.BasicModels;

namespace MLServer.Models
{
    public class MatchResult
    {
        public float[] PCAFeatures { get; }
        public uint PredictedLabel { get; }
        public float[] Score { get; }
        public string UserID { get; }
        public IDictionary<string, float> TagID_Features { get; }
        public IList<string> MatchUserIDs { get; }

        public MatchResult(float[] pcaFeatures, uint predictedLabel, float[] score, string userId, IDictionary<string, float> tagID_features, IList<string> matchUserIDs)
        {
            PCAFeatures = pcaFeatures;
            PredictedLabel = predictedLabel;
            Score = score;
            UserID = userId;
            TagID_Features = tagID_features;
            MatchUserIDs = matchUserIDs;
        }
    }
}