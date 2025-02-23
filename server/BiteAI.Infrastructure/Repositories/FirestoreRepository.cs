using System.Linq.Expressions;
using BiteAI.Services.Interfaces.Base;
using Google.Cloud.Firestore;

namespace BiteAI.Infrastructure.Repositories;

public class FirestoreRepository<T> : IRepository<T> where T : IEntity
{
    private readonly FirestoreDb _db;
    private readonly string _collectionName;

    public FirestoreRepository(FirestoreDb db)
    {
        this._db = db;
        this._collectionName = typeof(T).Name.ToLower() + "s"; // Convention: Collection name is plural of type name
    }

    private CollectionReference Collection => this._db.Collection(this._collectionName);

    public async Task<T> GetByIdAsync(string id)
    {
        var docRef = this.Collection.Document(id);
        var snapshot = await docRef.GetSnapshotAsync();

        if (!snapshot.Exists)
            return default;

        var entity = snapshot.ConvertTo<T>();
        entity.Id = snapshot.Id;
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var snapshot = await this.Collection.GetSnapshotAsync();
        return snapshot.Documents.Select(d =>
        {
            var entity = d.ConvertTo<T>();
            entity.Id = d.Id;
            return entity;
        });
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // Note: Firestore doesn't support LINQ directly
        // This implementation fetches all and filters in memory
        // For production, you'd want to use Firestore queries based on your needs
        var all = await this.GetAllAsync();
        return all.Where(predicate.Compile());
    }

    public async Task<T> AddAsync(T entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
        {
            var docRef = await this.Collection.AddAsync(entity);
            entity.Id = docRef.Id;
        }
        else
        {
            await this.Collection.Document(entity.Id).SetAsync(entity);
        }

        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            throw new ArgumentException("Entity must have an Id for updates");

        await this.Collection.Document(entity.Id).SetAsync(entity, SetOptions.MergeAll);
        return entity;
    }

    public async Task DeleteAsync(string id)
    {
        await this.Collection.Document(id).DeleteAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var docRef = this.Collection.Document(id);
        var snapshot = await docRef.GetSnapshotAsync();
        return snapshot.Exists;
    }
}