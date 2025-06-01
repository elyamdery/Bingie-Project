using Bingie.Data;
using Bingie.Models;
using Microsoft.EntityFrameworkCore;

namespace Bingie.Services
{
    public interface IBingeService
    {
        Task<BingeEntry> AddBingeEntryAsync(BingeEntry entry);
        Task<BingeEntry> UpdateBingeEntryAsync(BingeEntry entry);
        Task<bool> DeleteBingeEntryAsync(int entryId, int userId);
        Task<BingeEntry?> GetBingeEntryByIdAsync(int entryId, int userId);
        Task<IEnumerable<BingeEntry>> GetUserBingeEntriesAsync(int userId);
        Task<IEnumerable<BingeEntry>> GetUserBingeEntriesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<Dictionary<string, int>> GetActivityStatisticsAsync(int userId);
    }

    public class BingeService : IBingeService
    {
        private readonly AppDBContext _context;

        public BingeService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<BingeEntry> AddBingeEntryAsync(BingeEntry entry)
        {
            try
            {
                // Validate entry
                if (entry == null)
                    throw new ArgumentNullException(nameof(entry));

                if (string.IsNullOrWhiteSpace(entry.Activity))
                    throw new ArgumentException("Activity is required.");

                if (entry.UserId <= 0)
                    throw new ArgumentException("Valid User ID is required.");

                // Verify user exists
                var userExists = await _context.Users.AnyAsync(u => u.Id == entry.UserId);
                if (!userExists)
                    throw new ArgumentException("User does not exist.");

                entry.CreatedAt = DateTime.UtcNow;
                
                _context.BingeEntries.Add(entry);
                await _context.SaveChangesAsync();

                return entry;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add binge entry: {ex.Message}", ex);
            }
        }

        public async Task<BingeEntry> UpdateBingeEntryAsync(BingeEntry entry)
        {
            try
            {
                var existingEntry = await _context.BingeEntries
                    .FirstOrDefaultAsync(b => b.Id == entry.Id && b.UserId == entry.UserId);

                if (existingEntry == null)
                    throw new ArgumentException("Binge entry not found or access denied.");

                // Update properties
                existingEntry.Activity = entry.Activity;
                existingEntry.Description = entry.Description;
                existingEntry.Date = entry.Date;
                existingEntry.Duration = entry.Duration;
                existingEntry.IntensityRating = entry.IntensityRating;
                existingEntry.Mood = entry.Mood;
                existingEntry.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return existingEntry;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update binge entry: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteBingeEntryAsync(int entryId, int userId)
        {
            try
            {
                var entry = await _context.BingeEntries
                    .FirstOrDefaultAsync(b => b.Id == entryId && b.UserId == userId);

                if (entry == null)
                    return false;

                _context.BingeEntries.Remove(entry);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete binge entry: {ex.Message}", ex);
            }
        }

        public async Task<BingeEntry?> GetBingeEntryByIdAsync(int entryId, int userId)
        {
            try
            {
                return await _context.BingeEntries
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.Id == entryId && b.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve binge entry: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<BingeEntry>> GetUserBingeEntriesAsync(int userId)
        {
            try
            {
                return await _context.BingeEntries
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve user binge entries: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<BingeEntry>> GetUserBingeEntriesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.BingeEntries
                    .Where(b => b.UserId == userId && b.Date >= startDate && b.Date <= endDate)
                    .OrderByDescending(b => b.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve binge entries by date range: {ex.Message}", ex);
            }
        }

        public async Task<Dictionary<string, int>> GetActivityStatisticsAsync(int userId)
        {
            try
            {
                return await _context.BingeEntries
                    .Where(b => b.UserId == userId)
                    .GroupBy(b => b.Activity)
                    .ToDictionaryAsync(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve activity statistics: {ex.Message}", ex);
            }
        }
    }
}
