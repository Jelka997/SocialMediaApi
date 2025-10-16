using System.Data.Common;

namespace SocialMedia.Domain
{
    public class Post
    {
        public Post()
        {
        }

        public Post(int id, string content, DateTime date, int userId)
        {
            Id = id;
            Content = content;
            Date = date;
            UserId = userId;
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
