using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class FormationGenerator
{
    public List<Vector> Generate(int rows, int cols, int players, int pieces)
    {
        int spawns = players * pieces;
        List<Vector> points = new List<Vector>();

        for(int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector point = new Vector
                {
                    x = i,
                    y = j
                };

                points.Add(point);
            }
        }

        Random rand = new Random();

        while (spawns < points.Count())
        {
            int num = rand.Next(points.Count());
            points.Remove(points[num]);
        }
        return points;
    }
}
