using System;
using System.IO;
using CafeLib.Aspnet.Identity;
using CafeLib.Data;
using CafeLib.Data.Sources.Sqlite;
using CafeLib.Identity.Sqlite.Extensions;
using CafeLib.Identity.Sqlite.Models;

namespace CafeLib.Identity.Sqlite.Data
{
    public class IdentityDatabase : IDatabase
    {
        private readonly IdentityStorage _storage;

        public string DatabaseName { get; }
        public string ConnectionString { get; }
        internal string DatabaseFilePath { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public IdentityDatabase(IServiceProvider serviceProvider)
        {
            ConnectionString = serviceProvider.GetIdentityConnectionString();
            DatabaseFilePath = ConnectionString.Split(new[] {'='})[1];
            DatabaseName = DatabaseFilePath;
            _storage = new IdentityStorage(ConnectionString, new SqliteOptions());

            if (!File.Exists(DatabaseFilePath))
            {
                _storage.CreateDatabase();
                var seeder = new IdentitySeeder();
                seeder.UsersSeed(_storage);
            }
        }

        public IStorage GetStorage()
        {
            return _storage;
        }
    }
}
