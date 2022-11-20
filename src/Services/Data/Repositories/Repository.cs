namespace Services.Data.Repositories;

public class Repository<TEntity> where TEntity : class
{
    private readonly CoreDbContext dbContext;

    public Repository(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken ct)
    {
        dbContext.Set<TEntity>().Add(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task RemoveAsync(TEntity entity, CancellationToken ct)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
