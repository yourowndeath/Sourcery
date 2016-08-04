using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SettlersEngine
{



    internal class PriorityQueue<T> where T : IIndexedObject
    {
        protected List<T> InnerList = new List<T>();
        protected IComparer<T> mComparer;

        public PriorityQueue()
        {
            mComparer = Comparer<T>.Default;
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            mComparer = comparer;
        }

        public PriorityQueue(IComparer<T> comparer, int capacity)
        {
            mComparer = comparer;
            InnerList.Capacity = capacity;
        }

        protected void SwitchElements(int i, int j)
        {
            T h = InnerList[i];
            InnerList[i] = InnerList[j];
            InnerList[j] = h;

            InnerList[i].Index = i;
            InnerList[j].Index = j;
        }

        protected virtual int OnCompare(int i, int j)
        {
            return mComparer.Compare(InnerList[i], InnerList[j]);
        }

        public int Push(T item)
        {
            int p = InnerList.Count, p2;
            item.Index = InnerList.Count;
            InnerList.Add(item); // E[p] = O

            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            return p;
        }

        public T Pop()
        {
            T result = InnerList[0];
            int p = 0, p1, p2, pn;

            InnerList[0] = InnerList[InnerList.Count - 1];
            InnerList[0].Index = 0;

            InnerList.RemoveAt(InnerList.Count - 1);

            result.Index = -1;

            do
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);

            return result;
        }

        public void Update(T item)
        {
            int count = InnerList.Count;

            // usually we only need to switch some elements, since estimation won't change that much.
            while ((item.Index - 1 >= 0) && (OnCompare(item.Index - 1, item.Index) > 0))
            {
                SwitchElements(item.Index - 1, item.Index);
            }

            while ((item.Index + 1 < count) && (OnCompare(item.Index + 1, item.Index) < 0))
            {
                SwitchElements(item.Index + 1, item.Index);
            }
        }

        public T Peek()
        {
            if (InnerList.Count > 0)
                return InnerList[0];
            return default(T);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public int Count
        {
            get { return InnerList.Count; }
        }
    }
}