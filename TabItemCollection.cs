using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TabControl
{
	public class TabItemCollection : IList, ICollection, IEnumerable
	{
		private TabHost owner;

		private List<TabItem> innerList;

		public bool IsFixedSize => false;

		public bool IsReadOnly => false;

		object IList.this[int index]
		{
			get
			{
				return innerList[index];
			}
			set
			{
				innerList[index] = (TabItem)value;
			}
		}

		public int Count => innerList.Count;

		public bool IsSynchronized => false;

		public object SyncRoot => false;

		public TabItem this[int index]
		{
			get
			{
				return innerList[index];
			}
			set
			{
				innerList[index] = value;
			}
		}

		public TabItemCollection(TabHost collectionOwner)
		{
			if (collectionOwner == null)
			{
				throw new ArgumentNullException("collectionOwner");
			}
			owner = collectionOwner;
			innerList = new List<TabItem>();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		public int Add(object value)
		{
			innerList.Add(value as TabItem);
			owner.ItemListChanged(value as TabItem, remove: false);
			return 0;
		}

		public void Clear()
		{
			TabItem[] items = innerList.ToArray();
			innerList.Clear();
			owner.ItemListChanged(items, remove: true);
		}

		public bool Contains(object value)
		{
			return innerList.Contains(value as TabItem);
		}

		public int IndexOf(object value)
		{
			return innerList.IndexOf(value as TabItem);
		}

		public void Insert(int index, object value)
		{
			innerList.Insert(index, value as TabItem);
			owner.ItemListChanged(value as TabItem, remove: false);
		}

		public void Remove(object value)
		{
			innerList.Remove(value as TabItem);
			owner.ItemListChanged(value as TabItem, remove: true);
		}

		public void RemoveAt(int index)
		{
			TabItem item = innerList[index];
			innerList.RemoveAt(index);
			owner.ItemListChanged(item, remove: true);
		}

		public void CopyTo(Array array, int index)
		{
			innerList.CopyTo(array as TabItem[], index);
		}

		public void Add(string tabText)
		{
			TabItem tabItem = new TabItem();
			tabItem.TabText = tabText;
			Add(tabItem);
		}

		public void Add(string tabText, Image image)
		{
			TabItem tabItem = new TabItem();
			tabItem.TabText = tabText;
			tabItem.Image = image;
			Add(tabItem);
		}

		public bool Contains(TabItem tab)
		{
			return innerList.Contains(tab);
		}

		public int IndexOf(TabItem tab)
		{
			return innerList.IndexOf(tab);
		}

		public void Insert(int index, TabItem tab)
		{
			innerList.Insert(index, tab);
			owner.ItemListChanged(tab, remove: false);
		}

		public void Remove(TabItem tab)
		{
			innerList.Remove(tab);
		}
	}
}
