using BiteAI.Services.Contracts.Profiles;
using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Data.Entities.Enums;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Mappings;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.EntityFrameworkCore;

namespace BiteAI.Services.Services;

public class UserProfileService(AppDbContext db, IIdentityService identityService) : IUserProfileService
{
    public async Task<Result<UserProfileDto?>> GetMyProfileAsync(CancellationToken cancellationToken = default)
    {
        var userResult = await identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure) return Result.ErrorFromResult<UserProfileDto?>(userResult);
        var user = userResult.Data!;

        var profile = await db.UserProfiles.AsNoTracking()
            .Include(p => p.Allergies)
            .Include(p => p.FoodDislikes)
            .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (profile == null) return Result.Fail<UserProfileDto?>(OperationError.NotFound("Profile not found"));

        return Result.Success(profile.ToDto());
    }

    public async Task<Result<UserProfileDto?>> GetProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var profile = await db.UserProfiles.AsNoTracking()
            .Include(p => p.Allergies)
            .Include(p => p.FoodDislikes)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        if (profile == null) return Result.Fail<UserProfileDto?>(OperationError.NotFound("Profile not found"));
        return Result.Success(profile.ToDto());
    }

    public async Task<Result<UserProfileDto?>> CreateMyProfileAsync(CreateUserProfileDto dto, CancellationToken cancellationToken = default)
    {
        var userResult = await identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure) return Result.ErrorFromResult<UserProfileDto?>(userResult);
        var user = userResult.Data!;

        var exists = await db.UserProfiles.AnyAsync(p => p.UserId == user.Id, cancellationToken);
        if (exists) return Result.Fail<UserProfileDto?>(OperationError.Conflict("Profile already exists"));

        // Resolve enum lists to entity collections
        var allergyEntities = (dto.Allergies == null || dto.Allergies.Count == 0)
            ? new List<AllergyTypeEntity>()
            : await db.AllergyTypes.Where(a => dto.Allergies.Contains(a.Value)).ToListAsync(cancellationToken);
        var foodDislikeEntities = (dto.FoodDislikes == null || dto.FoodDislikes.Count == 0)
            ? new List<FoodDislikeTypeEntity>()
            : await db.FoodDislikeTypes.Where(fd => dto.FoodDislikes.Contains(fd.Value)).ToListAsync(cancellationToken);

        var entity = new UserProfile
        {
            UserId = user.Id,
            Gender = dto.Gender,
            Age = dto.Age,
            WeightInKg = dto.WeightInKg,
            HeightInCm = dto.HeightInCm,
            ActivityLevel = dto.ActivityLevel,
            PreferredDietType = dto.PreferredDietType,
            Allergies = allergyEntities,
            FoodDislikes = foodDislikeEntities,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };
        entity.IsProfileComplete = CalculateIsComplete(entity);

        await db.UserProfiles.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.ToDto());
    }

    public async Task<Result<UserProfileDto?>> UpdateMyProfileAsync(UpdateUserProfileDto dto, CancellationToken cancellationToken = default)
    {
        var userResult = await identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure) return Result.ErrorFromResult<UserProfileDto?>(userResult);
        var user = userResult.Data!;

        var entity = await db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (entity == null) return Result.Fail<UserProfileDto?>(OperationError.NotFound("Profile not found"));

        if (dto.Gender.HasValue) entity.Gender = dto.Gender;
        if (dto.Age.HasValue) entity.Age = dto.Age;
        if (dto.WeightInKg.HasValue) entity.WeightInKg = dto.WeightInKg;
        if (dto.HeightInCm.HasValue) entity.HeightInCm = dto.HeightInCm;
        if (dto.ActivityLevel.HasValue) entity.ActivityLevel = dto.ActivityLevel;
        if (dto.PreferredDietType.HasValue) entity.PreferredDietType = dto.PreferredDietType;
        if (dto.Allergies != null)
        {
            entity.Allergies = await db.AllergyTypes.Where(a => dto.Allergies.Contains(a.Value)).ToListAsync(cancellationToken);
        }
        if (dto.FoodDislikes != null)
        {
            entity.FoodDislikes = await db.FoodDislikeTypes.Where(fd => dto.FoodDislikes.Contains(fd.Value)).ToListAsync(cancellationToken);
        }

        entity.UpdatedAt = DateTime.UtcNow;
        entity.IsProfileComplete = CalculateIsComplete(entity);

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.ToDto());
    }

    public async Task<Result> DeleteMyProfileAsync(CancellationToken cancellationToken = default)
    {
        var userResult = await identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure) return Result.ErrorFromResult(userResult);
        var user = userResult.Data!;

        var entity = await db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (entity == null) return Result.Fail(OperationError.NotFound("Profile not found"));

        db.UserProfiles.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success("Profile deleted");
    }

    public async Task<Result<bool>> HasProfileAsync(CancellationToken cancellationToken = default)
    {
        var userResult = await identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure) return Result.ErrorFromResult<bool>(userResult);
        var user = userResult.Data!;

        var exists = await db.UserProfiles.AsNoTracking().AnyAsync(p => p.UserId == user.Id, cancellationToken);
        return Result.Success(exists);
    }

    private static bool CalculateIsComplete(UserProfile p)
    {
        return p is { Gender: not null, Age: not null, WeightInKg: not null, HeightInCm: not null, ActivityLevel: not null };
    }
}