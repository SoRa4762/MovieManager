namespace MovieManager.Models
{
    //abstract cuz it needs to be shared! not to be instantiated directly
    public abstract class EntityBase
    {
        //init is a new feature in C# 9.0
        //init so that it is immutable once set
        public Guid Id { get; private init; } = Guid.NewGuid();
        public DateTimeOffset Created { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastModified { get; private set; } = DateTimeOffset.UtcNow;
        public void UpdateLastModified()
        {
            LastModified = DateTimeOffset.UtcNow;
        }
    }
}
