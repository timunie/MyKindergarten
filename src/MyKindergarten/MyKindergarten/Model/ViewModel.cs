using MyKindergarten.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace MyKindergarten.Model
{
    public class ViewModel
    {
        public static string ConnectionString => $"Data Source=\"{Settings.Default.DatabaseFile}\";Version=3;FailIfMissing=True;";
        static ViewModel()
        {
            RequeryCategories();
        }

        public static ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
        public static Dictionary<long, Category> DictCategories { get; } = new Dictionary<long, Category>();
        public static List<Category> AllCategories => DictCategories.Values.ToList();

        public static HashSet<string> Buzzwords { get; } = new HashSet<string>() { "Abc", "Def", "Ghi" };

        public static void RequeryCategories()
        {
            Categories.Clear();

            try
            {
                using var conn = new SQLiteConnection(ConnectionString, true);
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Category ORDER BY Title";
                var reader = cmd.ExecuteReader();

                Dictionary<long, long> ParentChildren = new Dictionary<long, long>();

                while (reader.Read())
                {
                    var item = new Category(reader);
                    DictCategories.Add(item.ID ?? 0, item);

                    if (reader.GetNullableLong("Parent") != null)
                    {
                        ParentChildren.Add(item.ID ?? 0, reader.GetLong("Parent"));
                    }
                    else
                    {
                        Categories.Add(item);
                    }
                }

                foreach (var item in ParentChildren)
                {
                    DictCategories[item.Value].SubCategories.Add(DictCategories[item.Key]);
                }
            }
            catch (Exception e)
            {
                DialogHelper.ShowErrorMessage(e);
            }

        }
    }
}
