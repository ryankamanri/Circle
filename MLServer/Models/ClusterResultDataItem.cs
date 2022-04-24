using System;
using Kamanri.Database.Models.Attributes;

namespace MLServer.Models
{
    public class ClusterResultDataItem: ClusterFeaturesDataItem
    {
        [ColumnName("PCAFeatures")] 
        public float[] PCAFeatures { get; set; }
        [ColumnName("PredictedLabel")]
        public uint PredictedLabel { get; set; }

        [ColumnName("Score")] 
        public float[] Score { get; set; }
        
        public ClusterResultDataItem()
        {
            
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"PCAFeatures : ");
            foreach (var pcafeature in PCAFeatures)
            {
                Console.Write($"{pcafeature}\t");
            }
            Console.WriteLine();
            Console.WriteLine($"PredictedLabel = {PredictedLabel}");
            Console.WriteLine("Score : ");
            foreach (var score in Score)
            {
                Console.Write($"{score}\t");
            }
            Console.WriteLine();
            
            
        }
        
    }
}