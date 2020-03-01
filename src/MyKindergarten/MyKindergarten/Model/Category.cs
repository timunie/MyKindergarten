using MyKindergarten.ExtensionMethods;
using MyKindergarten.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyKindergarten.Model
{
    public class Category : BaseClass
    {

		public Category(SQLiteDataReader reader) : this()
		{
			ID = reader.GetNullableLong("ID");
			Title = reader.GetString("Title");
			Description = reader.GetString("Description");
			Icon = reader.GetString("Icon");
		}

		public Category()
		{
			SubCategories.CollectionChanged += SubCategories_CollectionChanged;
			Validate();
		}

		private long? _ID;
		public long? ID
		{
			get { return _ID; }
			set { _ID = value; RaisePropertyChanged(nameof(ID)); }
		}

		private Category _Parent;
		public Category Parent
		{
			get { return _Parent; }
			set { _Parent = value; RaisePropertyChanged(nameof(Parent)); }
		}

		private string _Title;
		public string Title
		{
			get { return _Title; }
			set { _Title = value; RaisePropertyChanged(nameof(Title)); Validate(); }
		}


		private string _Description;
		public string Description
		{
			get { return _Description; }
			set { _Description = value; RaisePropertyChanged(nameof(Description)); }
		}

		private string _Icon;
		public string Icon
		{
			get { return _Icon; }
			set { _Icon = value; RaisePropertyChanged(nameof(Icon)); }
		}

		public ObservableCollection<Category> SubCategories { get; set; } = new ObservableCollection<Category>();

		private void SubCategories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (Category item in e.NewItems)
				{
					item.Parent = this;
				}
			}

			if (e.OldItems != null)
			{
				foreach (Category item in e.OldItems)
				{
					item.Parent = this;
				}
			}
		}


		public override string ToString()
		{
			return $"{ID?.ToString("00")} - {Title}";
		}

		#region Commands

		// Edit 
		public static RelayCommand EditCommand { get; } = new RelayCommand(EditCommand_Execute, EditCommand_CanExecute);
		static void EditCommand_Execute (object param)
		{
			if (param is Category category)
			{
				var dialog = new EditCategory() { DataContext = category };
				dialog.ShowDialog();
			}
		}

		static bool EditCommand_CanExecute(object param)
		{
			return param is Category;
		}

		// Add
		public static RelayCommand AddCommand { get; } = new RelayCommand(AddCommand_Execute);
		public static RelayCommand AddSubcategoryCommand { get; } = new RelayCommand(AddCommand_Execute, AddSubcategoryCommand_CanExecute);
		static void AddCommand_Execute(object param)
		{
			EditCategory dialog;
			if (param is Category category)
			{
				dialog = new EditCategory() { DataContext = category };
			}
			else
			{
				dialog = new EditCategory() { DataContext = new Category() };
			}
				dialog.ShowDialog();
		}

		static bool AddSubcategoryCommand_CanExecute(object param)
		{
			return param is Category;
		}

		public Task<bool> Save()
		{
			try
			{
				using var conn = new SQLiteConnection(ViewModel.ConnectionString, true);
				conn.Open();

				using var cmd = conn.CreateCommand();

				cmd.CommandText = "REPLACE INTO Category (ID, Title, Parent, Icon, Description) VALUES (@ID, @Title, @Parent, @Icon, @Description);";
				
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@Title", Title);
				cmd.Parameters.AddWithValue("@Parent", Parent?.ID);
				cmd.Parameters.AddWithValue("@Icon", Icon);
				cmd.Parameters.AddWithValue("@Description", Description);

				cmd.ExecuteNonQuery();
				return Task.FromResult(true);
			}
			catch(Exception e)
			{
				DialogHelper.ShowErrorMessage(e);
				return Task.FromResult(false);
			}
		}

		#endregion

		void Validate()
		{
			ClearErrors(nameof(Title));
			ClearErrors(nameof(Icon));

			if (Title.IsNullOrWhitespace())
			{
				AddError(nameof(Title), Lang.General.Err_XX_MayNotBeEmpty.Format(Lang.General.Title));
			}

			if (Icon.IsNullOrWhitespace())
			{
				AddError(nameof(Icon), Lang.General.Err_XX_MayNotBeEmpty.Format(Lang.General.Title));
			}
			else if (!File.Exists(Icon))
			{
				// AddError(nameof(Icon), Lang.General.Err_FileNotFound);
			}
		}

	}
}
