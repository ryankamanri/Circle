using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using dotnet.Model;
using dotnet.Model.Relation;
using dotnet.Services.Database;

namespace dotnet.Services
{
    public class TagService
    {

        private DataBaseContext _dbc;

        /// <summary>
        /// 标签实体列表
        /// </summary>
        /// <value></value>
        public IList<Tag> Tags {get;private set;}



        /// <summary>
        /// 标签关键字与实体的字典
        /// </summary>
        /// <value></value>
        public Dictionary<string,List<Tag>> TagIndex {get;private set;}

        /// <summary>
        /// 标签ID与实体的字典
        /// </summary>
        /// <value></value>
        public Dictionary<long,Tag> TagDictionary{get;private set;}

        /// <summary>
        /// 标签树(以关系表的形式存储)
        /// </summary>
        /// <value></value>
        private Dictionary<long,long> TagTree;
        public TagService(DataBaseContext dbc)
        {
            _dbc = dbc;
            InitTags().Wait();
            InitTagIndex();
            InitTagDictionary();
            InitTagTree().Wait();
        }

        private async Task InitTags()
        {
            Tags = await _dbc.SelectAll<Tag>(new Tag());
        }

        

        /// <summary>
        /// 标签索引初始化
        /// </summary>
        private void InitTagIndex()
        {
            TagIndex = new Dictionary<string, List<Tag>>();
            foreach(var tag in Tags)
            {
                List<string> subset = tag.GetContinuousSubset();
                foreach(var subItem in subset)
                {
                    if(TagIndex.ContainsKey(subItem))
                        TagIndex[subItem].Add(tag);
                    
                    else TagIndex.Add(subItem,new List<Tag>(){tag});
                }
            }
        }

        private void InitTagDictionary()
        {
            TagDictionary = new Dictionary<long, Tag>();
            foreach(var tag in Tags)
            {
                TagDictionary.Add(tag.ID,tag);
            }
        }

        /// <summary>
        /// 标签树初始化
        /// </summary>
        private async Task InitTagTree()
        {
            TagTree = new Dictionary<long, long>();
            ID_IDList tagSon_tagParents = await _dbc.SelectAllRelations<Tag,Tag>(new Tag(),new Tag());
            foreach(var tagSon_tagParent in tagSon_tagParents)
            {
                TagTree.Add(tagSon_tagParent.ID,tagSon_tagParent.ID_2);
            }
        }

        /// <summary>
        /// 寻找子标签
        /// </summary>
        /// <param name="parentTag"></param>
        /// <returns></returns>
        public ICollection<Tag> FindChildTag(Tag parentTag)
        {
            //return (await _dbc.Mapping<Tag,Tag>(parentTag,new Tag(),ID_IDList.OutPutType.Key)).Keys;
            var childIDs = from tagTreeItem in TagTree
            where tagTreeItem.Value == parentTag.ID
            select tagTreeItem.Key;
            ICollection<Tag> tags = new List<Tag>();
            foreach(var childID in childIDs)
            {
                tags.Add(TagDictionary[childID]);
            }
            return tags;
        }

        
        /// <summary>
        /// 寻找父标签
        /// </summary>
        /// <param name="childTag"></param>
        /// <returns></returns>
        public Tag FindParentTag(Tag childTag)
        {
            return TagDictionary[TagTree[childTag.ID]];
        }

        /// <summary>
        /// 计算两个标签的相似度,利用标签树计算
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public double CalculateSimilarity(Tag t1,Tag t2)
        {
            long presentTag1ID = t1.ID,presentTag2ID = t2.ID;
            Stack<long> t1Stack = new Stack<long>(), t2Stack = new Stack<long>();
            while(presentTag1ID != -1)//表示未寻到根节点
            {
                t1Stack.Push(presentTag1ID);
                presentTag1ID = TagTree[presentTag1ID];
            }
            while(presentTag2ID != -1)//表示未寻到根节点
            {
                t2Stack.Push(presentTag2ID);
                presentTag2ID = TagTree[presentTag2ID];
            }
            double t1Depth = t1Stack.Count,t2Depth = t2Stack.Count,commonDepth = 0;
            while(t1Stack.Count != 0 && t2Stack.Count != 0 && t1Stack.Pop() == t2Stack.Pop()) commonDepth++;
            return (commonDepth * 2)/(t1Depth + t2Depth);
        }
    }
}