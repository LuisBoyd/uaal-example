using System;
using System.Collections.Generic;
using System.Linq;
using Core.models.maths;
using RuntimeModels;
using UnityEngine;

namespace Core.Services.ColliderGeneration
{
    public class MapBorderGenerator
    {
        private readonly RuntimeUserMap _runtimeUserMap;
        
        public MapBorderGenerator(RuntimeUserMap runtimeUserMap)
        {
            _runtimeUserMap = runtimeUserMap;
        }

        // public IEnumerable<Line> GenerateMapBorderLines()
        // {
        //     List<Line> MapLines = _runtimeUserMap.GetAllWorldPlotLines().ToList();
        //     if (MapLines.Count < 2) //if we dont have at least 3 vectors then we can't even generate a triangle polygon. and a line itself has 2 vectors.
        //         throw new Exception($"You need at least 2 Lines or 3 vectors to generate the border polygon");
        //
        //     Vector3 originalStartPoint = MapLines[0]._startPoint; //if it's to difficult to calculate the original start point by position have the vector3 carry a Unique ID
        //     List<Line> Hull = new List<Line>();
        //
        //     for (int i = 0; i < MapLines.Count; i++)
        //     {
        //         Line currentLine = MapLines[i]; //Get the first line it matters not where
        //         if (currentLine._endPoint == originalStartPoint)
        //         {
        //             Hull.Add(currentLine); //we found the end of the loop and we can add it onto the hull.
        //             break; //after this we break as we don't need to search anymore.
        //         }
        //
        //         List<Line> matchingLines = MapLines.Where(line => line._startPoint == currentLine._endPoint).ToList(); //Get all the lines where the starting point of those lines is the end point of the currentline.
        //         if(matchingLines.Count == 0)
        //             continue; //continue on with the algorithm although something may have gone wrong here
        //         while (mat)
        //         {
        //             
        //         }
        //     }
        // }
    }
}