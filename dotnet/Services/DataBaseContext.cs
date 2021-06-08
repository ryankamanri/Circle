using System;
using dotnet.Services;
using dotnet.Model;
using System.Collections.Generic;

namespace dotnet.Services
{

    
    public class DataBaseContext
    {
        private SQL _sql;

        public ID_IDList SortedUsers_Posts {get;private set;}

        public ID_IDList SortedUsers_Tags {get;private set;}

        public ID_IDList SortedPosts_Tags {get;private set;}

        public ID_IDList Users_SortedPosts {get;private set;}

        public ID_IDList Users_SortedTags {get;private set;}

        public ID_IDList Posts_SortedTags {get;private set;}
        
        public DataBaseContext(SQL sql)
        {
            _sql = sql;
            SortedUsers_Posts = ID_ID.GetList( _sql.Query("select * from schema1.users_posts"));
            SortedUsers_Tags = ID_ID.GetList(_sql.Query("select * from schema1.users_tags"));
            SortedPosts_Tags = ID_ID.GetList(_sql.Query("select * from schema1.posts_tags"));
            Users_SortedPosts = ID_ID.GetList( _sql.Query("select * from schema1.users_posts"));
            Users_SortedTags = ID_ID.GetList(_sql.Query("select * from schema1.users_tags"));
            Posts_SortedTags = ID_ID.GetList(_sql.Query("select * from schema1.posts_tags"));

            SortedUsers_Posts.Sort(new Comparison<ID_ID>((up1,up2) => ((int)(up1.ID - up2.ID))));
            SortedUsers_Tags.Sort(new Comparison<ID_ID>((ut1,ut2) => ((int)(ut1.ID - ut2.ID))));
            SortedPosts_Tags.Sort(new Comparison<ID_ID>((pt1,pt2) => ((int)(pt1.ID - pt2.ID))));

            Users_SortedPosts.Sort(new Comparison<ID_ID>((up1,up2) => ((int)(up1.ID_2 - up2.ID_2))));
            Users_SortedTags.Sort(new Comparison<ID_ID>((ut1,ut2) => ((int)(ut1.ID_2 - ut2.ID_2))));
            Posts_SortedTags.Sort(new Comparison<ID_ID>((pt1,pt2) => ((int)(pt1.ID_2 - pt2.ID_2))));
        }


    }

}
