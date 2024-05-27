using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>
{

    T[] _Items;
    int _CurrentItemCount;

    public Heap(int maxHeapSize)
    {
        _Items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = _CurrentItemCount;
        _Items[_CurrentItemCount] = item;
        SortUp(item);
        _CurrentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _Items[0];
        _CurrentItemCount--;
        _Items[0] = _Items[_CurrentItemCount];
        _Items[0].HeapIndex = 0;
        SortDown(_Items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return _CurrentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(_Items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < _CurrentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < _CurrentItemCount)
                {
                    if (_Items[childIndexLeft].CompareTo(_Items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(_Items[swapIndex]) < 0)
                {
                    Swap(item, _Items[swapIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }

        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = _Items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        _Items[itemA.HeapIndex] = itemB;
        _Items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
