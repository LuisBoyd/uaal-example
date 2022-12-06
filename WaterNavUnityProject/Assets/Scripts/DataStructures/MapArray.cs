using System;
using System.Collections.Generic;
using System.Linq;
using RCR.Utilities;
using UnityEngine;

namespace DataStructures
{
    /// <summary>
    /// A Wrapper class to store the bytes for the map
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapArray<T>
    {
        private T[] m_data;
        private int MaxmapSectionCount;
        private int mapRowCount;
        private int maxRowEntries;
        private Dictionary<int, T[,]> m_SortedbyteKeyPair;

        public MapArray(T[] input, int maxMapSectionCount, int mapRowCount, int maxRowEntries)
        {
            m_data = input;
            this.MaxmapSectionCount = maxMapSectionCount;
            this.mapRowCount = mapRowCount;
            this.maxRowEntries = maxRowEntries;
            m_SortedbyteKeyPair = new Dictionary<int, T[,]>();

            for (int MS = 0; MS < MaxmapSectionCount; MS++)
            {
                T[,] entry = new T[this.mapRowCount, this.maxRowEntries];
                Buffer.BlockCopy(input, MS * (this.mapRowCount *  this.maxRowEntries), entry, 0, (this.mapRowCount * this.maxRowEntries));
                m_SortedbyteKeyPair.Add(MS,entry);
            }
        }

        public T this[int index0, int index1, int index2]
        {
            get
            {
                if (CheckNotOutOfrangeException(index0, index1, index2))
                {
                   return m_SortedbyteKeyPair[index0][index1, index2];
                }
                return default;
            }
            set
            {
                if (CheckNotOutOfrangeException(index0, index1, index2))
                    m_SortedbyteKeyPair[index0][index1, index2] = value;
            }
        }


        private bool CheckNotOutOfrangeException(int index0, int index1, int index2)
        {
            if (index0 < MaxmapSectionCount && index1 < mapRowCount && index2 < maxRowEntries)
                return true;
            else
                return false;
        }
        private bool CheckNotOutOfrangeException(int index0, int index1)
        {
            if (index0 < MaxmapSectionCount && index1 < mapRowCount)
                return true;
            else
                return false;
        }

        private T[] GetColumn(T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
        }
        private T[] GetRow(T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }
        
        public T[] SortedArray
        {
            get
            {
                T[] ReturnCollection = new T[MaxmapSectionCount * mapRowCount * maxRowEntries];
                int mapScetionsPerRow = MathUtils.sqrt(MaxmapSectionCount);

                int FractionCounter = 0; //in the case of a 5x5 map fractionCounter Max value should be 5
                int YDirectioncounter = 0; //What mapSection are we in in the Y direction
                
                for (int TotalRows = 0; TotalRows < mapRowCount * mapScetionsPerRow; TotalRows++) // 1
                {
                    if (TotalRows != 0 && TotalRows % mapRowCount == 0)
                        YDirectioncounter++;

                    for (int MapRow = 0; MapRow < mapScetionsPerRow; MapRow++)
                    {
                        T[] IndividualRow = GetRow(m_SortedbyteKeyPair[MapRow + (YDirectioncounter * mapScetionsPerRow)], TotalRows % mapRowCount);
                        try
                        {
                            Buffer.BlockCopy(IndividualRow, 0, ReturnCollection, (TotalRows * (mapRowCount * mapScetionsPerRow)) + (MapRow * maxRowEntries), IndividualRow.Length);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        
                    }
                }
                return ReturnCollection;
            }
        }


    }
}