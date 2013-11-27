using System.Data.Entity;

namespace MyCore.Data {
    public interface IEntityRegister {
        void RegisterEntities(DbModelBuilder builder);
    }
}