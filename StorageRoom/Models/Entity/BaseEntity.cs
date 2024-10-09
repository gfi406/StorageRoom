namespace StorageRoom.Models.Entity
{
    public class BaseEntity
    {        
        public Guid Id { get; private set; }

        // Конструктор ⬇️⬇️⬇️⬇️⬇️
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
