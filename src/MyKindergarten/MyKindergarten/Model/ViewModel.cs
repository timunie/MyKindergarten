using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyKindergarten.Model
{
    public class ViewModel
    {
        static ViewModel()
        {
            Categories.Add(new Category() { ID = 0, Title = "ABC" });
            Categories.Add(new Category() { ID = 1, Title = "DEF" });

            var cat = new Category() { ID = 2, Title = "Mit Unterkategorie" };
            cat.SubCategories.Add(new Category() { ID = 3, Title = "Sub 1" });
            cat.SubCategories.Add(new Category() { ID = 4, Title = "Sub 2" });

            Categories.Add(cat);
        }

        public static ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
    }
}
