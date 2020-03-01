using System;
using System.Collections.Generic;
using System.Text;

namespace MyKindergarten.Model
{
	public class ObjectContainer<T> : BaseClass
	{
		public ObjectContainer ()
        {
        }

		public ObjectContainer (T Object)
		{
			this.Object = Object;
		}

        private T _Object;
		public T Object
		{
			get { return _Object; }
			set { _Object = value; RaisePropertyChanged(nameof(Object)); }
		}

		public override string ToString()
		{
			return Object.ToString();
		}
	}
}
