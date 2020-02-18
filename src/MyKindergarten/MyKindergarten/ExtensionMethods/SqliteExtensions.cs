using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace MyKindergarten.ExtensionMethods
{
    public static class SqliteExtensions
    {

        public static long? GetNullableLong (this SQLiteDataReader reader, string ColumnName)
        {
            int i = reader.GetOrdinal(ColumnName);
            if (reader.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return reader.GetInt64(i);
            }
        }

        public static long GetLong(this SQLiteDataReader reader, string ColumnName, long? Default = null)
        {
            int i = reader.GetOrdinal(ColumnName);
            if (reader.IsDBNull(i))
            {
                return Default ?? throw new ArgumentNullException(ColumnName);
            }
            else
            {
                return reader.GetInt64(i);
            }
        }

        public static string GetString(this SQLiteDataReader reader, string ColumnName)
        {
            int i = reader.GetOrdinal(ColumnName);
            if (reader.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return reader.GetString(i);
            }
        }

    }
}
