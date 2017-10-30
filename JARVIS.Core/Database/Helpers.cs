using System;
using Microsoft.Data.Sqlite;

namespace JARVIS.Core.Database
{
    public static class Helpers
    {
        public static R Single<R>(this SqliteDataReader reader, Func<SqliteDataReader, R> selector)
        {
            R result = default(R);
            if (reader.Read())
                result = selector(reader);
            if (reader.Read())
                throw new Exception("multiple rows returned from query");
            return result;
        }
    }
}
