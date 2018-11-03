﻿using UnityEngine;
using System.Collections;
using System;

namespace Level
{
    // TODO: check if it is possible to use Unity's grid
    public class CustomGrid<E>
    {
        private ArrayGrid<E> gridI = new ArrayGrid<E>();
        private ArrayGrid<E> gridII = new ArrayGrid<E>();
        private ArrayGrid<E> gridIII = new ArrayGrid<E>();
        private ArrayGrid<E> gridIV = new ArrayGrid<E>();

        public bool IsEmpty
        {
            get { return gridI.IsEmpty && gridII.IsEmpty && gridIII.IsEmpty && gridIV.IsEmpty; }
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

        public void Add(int x, int y, E cell)
        {
            try
            {
                ResolveGrid(x, y).Set(Math.Abs(x), Math.Abs(y), cell);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogWarning($"could not add cell at {x}, {y}");
            }
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

            return default(E);
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