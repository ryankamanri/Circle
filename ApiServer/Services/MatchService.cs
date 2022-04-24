using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiServer.Models;
using ApiServer.Models.User;
using Kamanri.Database;
using Kamanri.Http;
using MLServer.Models;

namespace ApiServer.Services
{
    public class MatchService
    {
        private readonly Api _api;
        private readonly DatabaseContext _dbc;
        public MatchService(Api api, DatabaseContext dbc)
        {
            _api = api;
            _dbc = dbc;
        }

        public async Task<Form> Match(UserInfo userInfo)
        {
            var mlResult = await _api.Post<MatchResult>("/User/Match", new Form()
            {
                { "userID", userInfo.ID }
            });

            var tag_featuresList = (from tagID_feature in mlResult.TagID_Features
                where tagID_feature.Value > 0
                select tagID_feature).ToList();
                
            tag_featuresList.Sort((x, y) => (x.Value - y.Value > 0) switch
            {
                true => 1,
                _ => -1
            });

            var matchTagList = new List<Tag>();
            
            foreach (var tag_features in tag_featuresList)
            {
                var tag = await _dbc.Select<Tag>(new Tag(Convert.ToInt64(tag_features.Key)));
                matchTagList.Add(tag);
            }
            
            // add user

            var matchUserInfoList = new List<UserInfo>();
            
            foreach (var matchUserID in mlResult.MatchUserIDs)
            {
                var matchUserInfo = await _dbc.Select(new UserInfo(Convert.ToInt64(matchUserID)));
                matchUserInfoList.Add(matchUserInfo);
            }

            return new Form()
            {
                { "MatchTagList", matchTagList },
                { "MatchUserInfoList", matchUserInfoList }
            };

        }
    }
}