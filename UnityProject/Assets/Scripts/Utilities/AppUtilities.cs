using System;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;

namespace RCR.Utilities
{
    public static class AppUtilities
    {
        private const float AppVersion = 1.0f;

        // public static bool CheckForAppUpdate()
        // {
        //     //Make Request to Server for AppVersion
        //     //Compare it with local AppVersion if it's higher then update the app
        //     //TODO
        // }

        // public static bool CheckIfDatabaseLive()
        // {
        //     
        // }

        public static byte[] SortBytes(byte[] unorderedBytes, int SizeOfByteArray)
        {
            // byte[] Sorted_bytes = new byte[unorderedBytes.Length];
            //
            // //The lengthOfRow is the how long a Horizontal row of a Map Section is
            // int lengthOfRow = Mathf.FloorToInt(Mathf.Sqrt(SizeOfByteArray));
            //
            // //The length of line is the legnth of the combined row
            // int Length_of_Line = Mathf.FloorToInt(Mathf.Sqrt(Sorted_bytes.Length));
            // //How many map Sections are in a row
            // int Legnth_Of_Dimension = Mathf.FloorToInt(Mathf.Sqrt(MathUtils.DivisionInto(Sorted_bytes.Length, SizeOfByteArray)));
            //
            //
            // int destOffset = 0;
            // for (int i = 0; i < Length_of_Line; i++)
            // {
            //     for (int j = 0; j < Legnth_Of_Dimension; j++)
            //     {
            //         Buffer.BlockCopy(unorderedBytes, (i * lengthOfRow) + (j * SizeOfByteArray), Sorted_bytes,
            //             destOffset, lengthOfRow);
            //
            //         destOffset += lengthOfRow;
            //
            //         //j * sizeOfByteArray = skipping map section
            //         //i * lengthOfRow = offset within Map section
            //     }
            // }
            //
            // return Sorted_bytes;

            int MaxMapSectionCount = MathUtils.DivisionInto(unorderedBytes.Length, SizeOfByteArray);
            int mapRowCount = MathUtils.sqrt(SizeOfByteArray);
            int MaxRowEntries = MathUtils.sqrt(SizeOfByteArray);

            MapArray<byte> sortedBytes = new MapArray<byte>(unorderedBytes, MaxMapSectionCount,
                mapRowCount, MaxRowEntries);

            byte[] SortedBytes = new byte[unorderedBytes.Length];

            for (int MS = 0; MS < MaxMapSectionCount; MS++)
            {
                for (int RC = 0; RC < mapRowCount; RC++)
                {
                    for (int RE = 0; RE < MaxRowEntries; RE++)
                    {
                        //Buffer.BlockCopy(unorderedBytes, );
                        //TODO WORK OUT THE ORDERING OF THESE BYTES
                    }
                }
            }



            return null;
        }
        
        
    }
}