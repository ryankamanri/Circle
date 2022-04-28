using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.ML.Data;
using StackExchange.Redis;

namespace MLServer.Models
{
    public class ClusterFeaturesDataItem
    {
        public const int TAG_COUNT = 58;
        public string UserID { get; set; }

        [ColumnName("Features")]
        [VectorType(TAG_COUNT)]
        public float[] Features { get; set; }

        public ClusterFeaturesDataItem()
        {
            
        }
        public ClusterFeaturesDataItem(IEnumerable<ClusterUserDataItem> clusterUserDataGroupByUser)
        {
            var features = new List<float>();
            foreach (var clusterUserDataItem in clusterUserDataGroupByUser)
            {
                UserID ??= clusterUserDataItem.UserID.ToString();
                features.Add(clusterUserDataItem.Count);
            }
            
            Features = features.ToArray();
 
        }

        public virtual void Print()
        {
            Console.WriteLine();
            Console.WriteLine($"UserID = {UserID}");
            Console.WriteLine($"Features Count = {Features.Length}");
            Console.WriteLine("Features : ");
            foreach (var feature in Features)
            {
                Console.Write($"{feature}\t");
            }
            Console.WriteLine();
            
        }
    }
}