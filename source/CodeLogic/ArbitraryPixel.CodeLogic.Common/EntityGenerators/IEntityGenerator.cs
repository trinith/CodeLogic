using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;

namespace ArbitraryPixel.CodeLogic.Common.EntityGenerators
{
    public interface IEntityGenerator<TEntity> where TEntity : IEntity
    {
        TEntity[] GenerateEntities(IEngine host, int numEntities);
        void RepositionEntity(TEntity entity);
    }
}
