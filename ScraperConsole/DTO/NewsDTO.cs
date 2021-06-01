using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    [Serializable]
    public class NewsDTO
    {
        public NewsDTO()
        {
            Comments = new List<CommentDTO>();
        }

        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public List<CommentDTO> Comments { get; set; }

        public bool? Like { get; set; }


    }
}
