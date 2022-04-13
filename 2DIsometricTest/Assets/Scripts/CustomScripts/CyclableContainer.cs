using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CyclableContainer<T> : ICollection<T>, IEnumerable<T>
{
    [SerializeField] private List<T> m_contents = new List<T>();
    [SerializeField] private int m_currentIndex = 0;

    public T currentElement
    {
        get
        {
            if (m_contents.Count == 0) return default(T);
            if (m_currentIndex < 0 || m_currentIndex >= m_contents.Count) return default(T);
            return m_contents[m_currentIndex];
        }
    }

    public CyclableContainer()
    {
        m_contents = new List<T>();
        m_currentIndex = 0;
    }

    public CyclableContainer(IEnumerable<T> contents)
    {
        m_contents = new List<T>(contents);
        m_currentIndex = 0;
    }

    #region List Functions

    public void Add(T content) => m_contents.Add(content);

    public void AddRange(IEnumerable<T> contents) => m_contents.AddRange(contents);

    public bool Remove(T content)
    {
        if (!m_contents.Contains(content)) return false;
        int index = m_contents.IndexOf(content);

        if (index <= m_currentIndex)
            m_currentIndex--;

        m_contents.RemoveAt(index);
        return true;
    }

    public void Clear()
    {
        m_contents.Clear();
        m_currentIndex = 0;
    }

    public bool Contains(T content) => m_contents.Contains(content);

    public int IndexOf(T content) => m_contents.IndexOf(content);

    public void Insert(int index, T content)
    {
        if (index <= m_currentIndex)
            m_currentIndex++;

        m_contents.Insert(index, content);
    }

    public void InsertRange(int index, IEnumerable<T> content)
    {
        if (index <= m_currentIndex)
            m_currentIndex += GetIEnumerableCount(content);

        m_contents.InsertRange(index, content);
    }

    public void CopyTo(T[] content, int index) => m_contents.CopyTo(content, index);

    public int Count => m_contents.Count;

    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator() => m_contents.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_contents).GetEnumerator();

    private int GetIEnumerableCount(IEnumerable<T> content)
    {
        ICollection<T> c = content as ICollection<T>;
        if (c != null) return c.Count;

        int result = 0;
        using (IEnumerator<T> e = content.GetEnumerator())
        {
            while (e.MoveNext())
                result++;
        }
        return result;
    }

    #endregion

    public void MoveNext()
    {
        if (m_contents.Count == 0) return;
        m_currentIndex++;
        if (m_currentIndex >= m_contents.Count)
            m_currentIndex = 0;
    }

    public void MovePrevious()
    {
        if (m_contents.Count == 0) return;
        m_currentIndex--;
        if (m_currentIndex < 0)
            m_currentIndex = m_contents.Count - 1;
    }

    public T PeekNext()
    {
        if (m_contents.Count == 0) return default(T);
        int i = m_currentIndex + 1;
        if (i >= m_contents.Count)
            i = 0;

        return m_contents[i];
    }

    public T PeekPrevious()
    {
        if (m_contents.Count == 0) return default(T);
        int i = m_currentIndex - 1;
        if (i < 0)
            i = m_contents.Count - 1;

        return m_contents[i];
    }

    public void GoToFirst() => m_currentIndex = 0;

    public void GoToLast()
    {
        if (m_contents.Count == 0)
        {
            m_currentIndex = 0;
            return;
        }

        m_currentIndex = m_contents.Count - 1;
    }
}
