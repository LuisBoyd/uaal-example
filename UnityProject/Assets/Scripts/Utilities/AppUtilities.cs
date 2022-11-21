using System;
using System.Collections.Generic;
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
            byte[] combined_bytes = new byte[unorderedBytes.Length];
            byte[] Sorted_bytes = new byte[combined_bytes.Length];

            int CollectionLength = MathUtils.DivisionInto(unorderedBytes.Length, SizeOfByteArray); //unorderedBytes.Count;
            
            for (int i = 0; i < CollectionLength; i++)
            {
                Buffer.BlockCopy(unorderedBytes, i * SizeOfByteArray, combined_bytes, i * SizeOfByteArray, SizeOfByteArray);
            }
            int Length_of_Line = Mathf.FloorToInt(Mathf.Sqrt(combined_bytes.Length));
            int Legnth_Of_Dimension = Mathf.FloorToInt(Mathf.Sqrt(CollectionLength));

            for (int i = 0; i < MathUtils.DivisionInto(combined_bytes.Length, Length_of_Line); i++)
            {
                int Legnth_of_individual_array = Length_of_Line / Legnth_Of_Dimension;
                for (int j = 0; j < Legnth_Of_Dimension; j++)
                {
                    Buffer.BlockCopy(combined_bytes, (i * Length_of_Line) + (j * Legnth_of_individual_array), Sorted_bytes,
                        (i * Length_of_Line) + (j * Legnth_of_individual_array), Legnth_of_individual_array);
                }
            }

            return Sorted_bytes;

            // for (int LoopCount = 0; LoopCount < length * length; LoopCount += length)
            // {
            //     
            // }
        }
        
        
    }
}