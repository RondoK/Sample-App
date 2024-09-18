using App.Data;
using App.Data.Models;
using FastApi.EF;

namespace App.Services;

// It can be better designed, but I am not sure what functionality and how I want to extend
// so for I'll just use this for now
public class BadService<T> where T : class
{
    private Context _context;
    private TimeProvider _timeProvider;

    public BadService(Context context, TimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }

    public async Task<T> Create(T entity)
    {
        if (entity is IHaveCreateInfo createInfo)
        {
            createInfo.CreatedAt = _timeProvider.GetUtcNow();
            //TODO : Set Creator Id
        }

        await _context.SaveNewAsync(entity);
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        if (entity is IHaveUpdateInfo updateInfo)
        {
            updateInfo.LastUpdatedAt = _timeProvider.GetUtcNow();
            // TODO : Set updater Id
        }

        await _context.SaveUpdateAsync(entity);

        return entity;
    }
    
    
    
}