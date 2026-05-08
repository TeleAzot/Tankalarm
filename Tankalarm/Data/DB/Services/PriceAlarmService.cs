using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Tankalarm.Data.DB.Models;

namespace Tankalarm.Data.DB.DBServices
{
    public class PriceAlarmService
    {
        private SQLiteAsyncConnection? _database;

        public async Task InitAsync()
        {
            if (_database != null)
                return;

            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "app.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<PriceAlarm>();
        }

        public async Task<IEnumerable<PriceAlarm>> GetPriceAlarmsAsync()
        {
            await InitAsync();

            return await _database!
                .Table<PriceAlarm>()
                .ToListAsync();
        }

        public async Task<int> SavePriceAlarmAsync(PriceAlarm priceAlarm)
        {
            await InitAsync();

            if (priceAlarm.Id != 0)
                return await _database!.UpdateAsync(priceAlarm);

            return await _database!.InsertAsync(priceAlarm);
        }

        public async Task<int> DeletePriceAlarmAsync(PriceAlarm priceAlarm)
        {
            await InitAsync();

            return await _database!.DeleteAsync(priceAlarm);
        }
    }
}