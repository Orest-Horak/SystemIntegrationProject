using System;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO
{
    [Serializable]
    public class NewsDTO
    {
        public NewsDTO()
        {
            Comments = new List<CommentDTO>();
        }

        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }

        public bool? Like { get; set; }


    }
}
