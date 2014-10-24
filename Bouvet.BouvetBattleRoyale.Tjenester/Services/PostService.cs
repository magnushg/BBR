namespace BouvetCodeCamp.DomeneTjenester.Services
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.DomeneTjenester.Interfaces;

    public class PostService : Service<Post>
    {
        public PostService(IRepository<Post> postRepository) : base(postRepository)
        {
        }
    }
}