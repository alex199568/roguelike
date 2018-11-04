using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

namespace Level
{
    // TODO: check if it is possible to use Unity's grid
    public class CustomGrid<E> : IEnumerable<E> where E : class
    {
        private ArrayGrid<E> gridI = new ArrayGrid<E>();
        private ArrayGrid<E> gridII = new ArrayGrid<E>();
        private ArrayGrid<E> gridIII = new ArrayGrid<E>();
        private ArrayGrid<E> gridIV = new ArrayGrid<E>();

        private List<E> allItems = new List<E>();

        public bool IsEmpty
        {
            get { return allItems.Count == 0; }
        }

        public int Count
        {
            get { return allItems.Count; }
        }

        public int XMin
        {
            get { return -Math.Max(gridII.Width, gridIII.Width); }
        }

        public int XMax
        {
            get { return Math.Max(gridI.Width, gridIV.Width); }
        }

        public int YMin
        {
            get { return -Math.Max(gridIII.Height, gridIV.Height); }
        }

        public int YMax
        {
            get { return Math.Max(gridI.Height, gridII.Height); }
        }

        public void Add(int x, int y, E item)
        {
            ResolveGrid(x, y).Set(Math.Abs(x), Math.Abs(y), item);
            allItems.Add(item);
        }

        public void AddExisting(int x, int y, E item)
        {
            ResolveGrid(x, y).Set(Math.Abs(x), Math.Abs(y), item);
        }

        public E GetAt(int x, int y)
        {
            try
            {
                return ResolveGrid(x, y).Get(Math.Abs(x), Math.Abs(y));
            }
            catch (IndexOutOfRangeException)
            {
                // ...
            }

            return null;
        }

        public void Remove(int x, int y, E item)
        {
            try
            {
                allItems.Remove(item);
                ResolveGrid(x, y).Remove(Math.Abs(x), Math.Abs(y));
            }
            catch (IndexOutOfRangeException)
            {
                // ...
            }
        }

        public void Remove(int x, int y)
        {
            try
            {
                ResolveGrid(x, y).Remove(Math.Abs(x), Math.Abs(y));
            }
            catch (IndexOutOfRangeException)
            {
                // ...
            }
        }

        IEnumerator<E> IEnumerable<E>.GetEnumerator()
        {
            return ((IEnumerable<E>)allItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<E>)allItems).GetEnumerator();
        }

        private ArrayGrid<E> ResolveGrid(int x, int y)
        {
            if (x >= 0 && y >= 0)
            {
                return gridI;
            }
            if (x < 0 && y < 0)
            {
                return gridIII;
            }
            if (x >= 0)
            {
                return gridIV;
            }
            return gridII;
        }
    }
}
