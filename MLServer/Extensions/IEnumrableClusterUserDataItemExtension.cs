using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Channels;
using Kamanri.Database.Models.Relation;
using MLServer.Models;

namespace MLServer.Extensions
{
    public static class IEnumrableClusterUserDataItemExtension
    {
        public static IEnumerable<ClusterUserDataItem> GroupByUserAndTag(this IEnumerable<ClusterUserDataItem> clusterData)
        {
            return from c in clusterData
                group c by (c.UserID, c.TagID)
                into groupc
                select groupc.Aggregate((c1, c2) => new ClusterUserDataItem()
                {
                    UserID = c1.UserID,
                    TagID = c1.TagID,
                    IsInterested = c1.IsInterested + c2.IsInterested,
                    IsSelf = c1.IsSelf + c2.IsSelf,
                    IsPostOwned = c1.IsPostOwned + c2.IsPostOwned,
                    IsPostLike = c1.IsPostLike + c2.IsPostLike,
                    IsPostCollect = c1.IsPostCollect + c2.IsPostCollect,
                    PostCommentCount = c1.PostCommentCount + c2.PostCommentCount,
                    Count = c1.Count + c2.Count
                });
        }
        
        /// <summary>
        /// 给标签树添加权重
        /// </summary>
        /// <param name="clusterData"></param>
        /// <param name="tagTree"></param>
        /// <returns></returns>
        public static IEnumerable<ClusterUserDataItem> AddTagTreeWeight(this IEnumerable<ClusterUserDataItem> clusterData,
            ID_IDList tagTree)
        {
            var clusterUserDataItems = clusterData.ToList();
            foreach (var clusterUserDataItem in clusterUserDataItems)
            {
                var ancestorTagID = clusterUserDataItem.TagID;
                while (ancestorTagID != -1)
                {
                    ancestorTagID = tagTree.First(tag_tag => tag_tag.ID == ancestorTagID).ID_2;
                    if(ancestorTagID == -1) break;
                    
                    // calculate the children count from ancient tag
                    var childrenCount = GetChildrenCount(tagTree, ancestorTagID);

                    var ancestorClusterUserDataItem = clusterUserDataItems.First(userDataItem => userDataItem.TagID == ancestorTagID && userDataItem.UserID == clusterUserDataItem.UserID);
                    ancestorClusterUserDataItem.IsInterested += clusterUserDataItem.IsInterested / childrenCount;
                    ancestorClusterUserDataItem.IsSelf += clusterUserDataItem.IsSelf / childrenCount;
                    ancestorClusterUserDataItem.IsPostCollect += clusterUserDataItem.IsPostCollect / childrenCount;
                    ancestorClusterUserDataItem.IsPostLike += clusterUserDataItem.IsPostLike / childrenCount;
                    ancestorClusterUserDataItem.IsPostOwned += clusterUserDataItem.IsPostOwned / childrenCount;
                    ancestorClusterUserDataItem.PostCommentCount += clusterUserDataItem.PostCommentCount / childrenCount;
                    ancestorClusterUserDataItem.Count += clusterUserDataItem.Count / childrenCount;
                }
            }
            
            return clusterUserDataItems;

        }

        private static int GetChildrenCount(ID_IDList tagTree, long ancestor)
        {
            if (tagTree.FirstOrDefault(tag_tag => tag_tag.ID_2 == ancestor) == default)
            {
                return 1;
            }

            var firstChildren = from tag_tag in tagTree
                where tag_tag.ID_2 == ancestor
                select tag_tag.ID;

            var count = 0;
            
            foreach (var child in firstChildren)
            {
                count += GetChildrenCount(tagTree, child);
            }

            return count;
        }

        public static IEnumerable<ClusterFeaturesDataItem> ToClusterFeaturesDataItems(this IEnumerable<ClusterUserDataItem> clusterData)
        {
            foreach (var proceededUserData in clusterData.GroupBy(data => data.UserID))
            {
                yield return new ClusterFeaturesDataItem(proceededUserData);
            }
        }
    }
}