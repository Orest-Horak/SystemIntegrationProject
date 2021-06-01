using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    [Serializable]
    public class CommentDTO
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime DateOfPost { get; set; }
    }
}
