using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyKindergarten.Model
{
    public class Activity : BaseClass
    {
		public Activity()
		{
			AddCategoryCommand = new RelayCommand(AddCategoryCommand_Execute);
		}

		private long? _ID;
		public long? ID
		{
			get { return _ID; }
			set { _ID = value; RaisePropertyChanged(nameof(ID)); }
		}

		private string _Title;
		public string Title
		{
			get { return _Title; }
			set { _Title = value; RaisePropertyChanged(nameof(Title)); }
		}

		private string _Descripton;
		public string Descripton
		{
			get { return _Descripton; }
			set { _Descripton = value; RaisePropertyChanged(nameof(Descripton)); }
		}


		private string _Icon;
		public string Icon
		{
			get { return _Icon; }
			set { _Icon = value; RaisePropertyChanged(nameof(Icon)); }
		}


		private string _OriginalSource;
		public string OriginalSource
		{
			get { return _OriginalSource; }
			set { _OriginalSource = value; RaisePropertyChanged(nameof(OriginalSource)); }
		}

		public ObservableCollection<ObjectContainer<Category>> Categories { get; } = new ObservableCollection<ObjectContainer<Category>>();

		public ObservableCollection<string> Files { get; } = new ObservableCollection<string>();


		private string _Buzzwords;
		public string Buzzwords
		{
			get { return _Buzzwords; }
			set { _Buzzwords = value; RaisePropertyChanged(nameof(Buzzwords)); }
		}


		#region Commands

		public RelayCommand AddCategoryCommand { get; }
		void AddCategoryCommand_Execute(object param)
		{
			Categories.Add(new ObjectContainer<Category>());
		}


		public RelayCommand RemoveCategoryCommand { get; }
		void RemoveCategoryCommand_Execute(object param)
		{
			if (param is ObjectContainer<Category> category)
				Categories.Remove(category);
		}

		bool RemoveCategoryCommand_CanExecute (object param)
		{
			return param is ObjectContainer<Category> && Categories.Count > 1;
		}

		#endregion
	}
}
