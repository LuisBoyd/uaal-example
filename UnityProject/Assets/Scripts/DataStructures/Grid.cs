using System;
using System.IO;
using UnityEngine;

namespace RCR.DataStructures
{
    public class Grid
    {
        private int m_sizeofGrid;
        private const int SIZEOFAREA = 2500;

        private Area[,] m_areas;
        
        
        public Grid(byte[] data)
        {
            if (data.Length % SIZEOFAREA == 0) //Check to see if there is no remainder bytes for the grid byte array
            {
                m_sizeofGrid = Mathf.RoundToInt(Mathf.Sqrt(data.Length / SIZEOFAREA)); //TODO look at possible loss of fraction
                m_areas = new Area[m_sizeofGrid, m_sizeofGrid];
                using (MemoryStream stream = new MemoryStream(data))
                {
                    int count = 0;
                    for (int x = 0; x < m_sizeofGrid; x++)
                    {
                        for (int y = 0; y < m_sizeofGrid; y++)
                        {
                            byte[] buffer = new byte[SIZEOFAREA];
                            count += stream.Read(buffer, count, SIZEOFAREA);
                            m_areas[x, y] = new Area(buffer);
                        }
                    }
                }
            }
            else
            {
                //TODO FAILED TO CREATE GRID
            }
        }
    }
}