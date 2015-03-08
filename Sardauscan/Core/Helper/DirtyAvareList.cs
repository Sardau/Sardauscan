#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
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
