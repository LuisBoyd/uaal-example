using System;
using UnityEngine;

namespace Core.Services
{
    public class UnityProgressReport : IProgress<float>
    {
        public void Report(float value)
        {
            // Debug.Log($"Current Process is {value}");
        }
    }
}