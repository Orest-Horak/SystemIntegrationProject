using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DAL.Entities
{
    public class NewsEntity
    {
        public NewsEntity()
        {
            Comments = new List<CommentEntity>();
        }

        [BsonId]
        public ObjectId InternalId { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public ICollection<CommentEntity> Comments { get; set; }

        public bool? Like { get; set; }

    }
}
