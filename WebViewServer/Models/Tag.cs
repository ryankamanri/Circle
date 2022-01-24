
using System.Collections.Generic;
using Kamanri.Database.Models;

namespace WebViewServer.Models
{
    public class Tag : EntityView, IEqualityComparer<Tag>
    {

        public string _Tag { get; set; }



        public Tag()
        {

        }

        public Tag(long ID) : base(ID)
        {

        }

        public Tag(string tag)
        {
            _Tag = tag;

        }
        public Tag(long ID, string tag) : base(ID)
        {
            _Tag = tag;
        }



        public override string ToString()
        {
            return $"{ID},'{_Tag}'";
        }



        public bool Equals(Tag tag_1, Tag tag_2)
        {
            return tag_1.ID == tag_2.ID;
        }

        public int GetHashCode(Tag tag)
        {
            return (int)tag.ID;
        }


        /// <summary>
        /// 获取该标签的连续子集
        /// </summary>
        /// <returns></returns>
        public List<string> GetContinuousSubset()
        {
            List<string> subset = new List<string>();
            string subItem, subItemUpper, subItemLower;
            //从长度为1开始截取
            for (int i = 1; i <= _Tag.Length; i++)
            {
                //起始位置为j
                for (int j = 0; j + i <= _Tag.Length; j++)
                {
                    subItem = _Tag.Substring(j, i);
                    subItemUpper = subItem.ToUpper();
                    subItemLower = subItem.ToLower();

                    if (!subset.Contains(subItem))
                        subset.Add(subItem);
                    if (!subset.Contains(subItemUpper))
                        subset.Add(subItemUpper);
                    if (!subset.Contains(subItemLower))
                        subset.Add(subItemLower);
                }
            }
            return subset;
        }

    }
}