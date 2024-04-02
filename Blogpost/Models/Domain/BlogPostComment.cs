namespace Blogpost.Models.Domain
{
    public class BlogPostComment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserID { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
