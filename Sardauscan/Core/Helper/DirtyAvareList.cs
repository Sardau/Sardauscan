#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sardauscan.Core
{
	/// <summary>
	/// IList with a dlag when something change
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DirtyAvareList<T> : IList<T>
	{
		/// <summary>
		/// Default Ctor
		/// </summary>
		public DirtyAvareList(int capacity = 0)
		{
			m_InnerList = new List<T>(capacity);

		}

		public virtual void Update()
		{
			Dirty = false;
		}
		public void ForceDirty()
		{
			Dirty = true;
		}
		public virtual bool Dirty { get; private set; }
		List<T> m_InnerList;
		public int IndexOf(T item)
		{
			return m_InnerList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			m_InnerList.Insert(index, item);
			Dirty = true;
		}

		public void RemoveAt(int index)
		{
			m_InnerList.RemoveAt(index);
			Dirty = true;
		}

		public T this[int index]
		{
			get
			{
				return m_InnerList[index];
			}
			set
			{
				m_InnerList[index] = value;
				Dirty = true;
			}
		}

		public void Add(T item)
		{
			m_InnerList.Add(item);
			Dirty = true;
		}

		public void Clear()
		{
			m_InnerList.Clear();
			Dirty = true;
		}

		public bool Contains(T item)
		{
			return m_InnerList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			m_InnerList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return m_InnerList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			bool ret = m_InnerList.Remove(item);
			Dirty = true;
			return ret;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return m_InnerList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void AddRange(IEnumerable<T> collection)
		{
			m_InnerList.AddRange(collection);
			Dirty = true;
		}
		public void Sort()
		{
			m_InnerList.Sort();
			Dirty = true;
		}
		public void Sort(Comparison<T> comparison)
		{
			m_InnerList.Sort(comparison);
			Dirty = true;
		}

		public void Sort(IComparer<T> comparer)
		{
			m_InnerList.Sort(comparer);
			Dirty = true;
		}
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			m_InnerList.Sort(index, count, comparer);
			Dirty = true;
		}
		public T[] ToArray()
		{
			return m_InnerList.ToArray();
		}

	}
}
