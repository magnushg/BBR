namespace Bouvet.BouvetBattleRoyale.Tjenester.Services
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    public class PostService : Service<Post>
    {
        public PostService(IRepository<Post> postRepository) : base(postRepository)
        {
        }
    }
}