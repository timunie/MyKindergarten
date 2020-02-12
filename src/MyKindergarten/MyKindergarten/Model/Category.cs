using MyKindergarten.ExtensionMethods;
using MyKindergarten.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MyKindergarten.Model
{
    public class Category : BaseClass
    {

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
