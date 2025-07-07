using POIApplication.Repository;

namespace POIApplication.UnitOfWork
{
    public interface IUnitOfWork
    {
        IMapObjectRepository MapObject { get; }
        Task CompleteAsync();
        void Dispose();
    }
}
