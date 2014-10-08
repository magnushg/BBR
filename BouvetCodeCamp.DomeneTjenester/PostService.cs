using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class PostService : Service<Post>
    {
        public PostService(IRepository<Post> postRepository) : base(postRepository)
        {
        }
    }
}