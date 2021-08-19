using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Code copied from RC4_M3_C2
//Copied from Vicente Soler https://github.com/ADRC4/Voxel/blob/master/Assets/Scripts/Util/Util.cs

public enum Axis { X, Y, Z };
public enum BoundaryType { Inside = 0, Left = -1, Right = 1, Outside = 2 }

static class Util
{
    public static Vector3 Average(this IEnumerable<Vector3> vectors)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var vector in vectors)
        {
            sum += vector;
            count++;
        }

        sum /= count;
        return sum;
    }

    public static T MinBy<T>(this IEnumerable<T> items, Func<T, double> selector)
    {
        double minValue = double.MaxValue;
        T minItem = items.First();

        foreach (var item in items)
        {
            var value = selector(item);

            if (value < minValue)
            {
                minValue = value;
                minItem = item;
            }
        }

        return minItem;
    }

    public static bool ValidateIndex(Vector3Int gridSize, Vector3Int index)
    {
        if (index.x < 0 || index.x > gridSize.x - 1)
        {
            return false;
        }

        else if (index.y < 0 || index.y > gridSize.y - 1)
        {
            return false;
        }

        else if (index.z < 0 || index.z > gridSize.z - 1)
        {
            return false;
        }

        return true;
    }

    public static float Normalise(float v, float a1, float a2, float b1, float b2)
    {
        float result = b1 + (v - a1) * (b2 - b1) / (a2 - a1);

        return result;
    }

    public static Dictionary<(Function, Function), float> FunctionAttraction = new Dictionary<(Function, Function), float>()
    {
        {(Function.Dining,Function.LivingRoom), 0f },
        {(Function.Kitchen,Function.Dining), 0f },
        {(Function.Kitchen,Function.Bedroom), 1f },
        {(Function.Bathroom,Function.Bedroom), 0f },
        {(Function.LivingRoom,Function.Kitchen), 0f },
        {(Function.Bedroom,Function.Closet), 0f },
        {(Function.Bathroom,Function.Dining), 0f },
        {(Function.Kitchen,Function.Courtyard), 1f },
        {(Function.LivingRoom,Function.Bedroom), 0f },
        {(Function.LivingRoom,Function.Closet), 2f },
        {(Function.Dining,Function.Bedroom), 2f },
        {(Function.Kitchen,Function.Closet), 2f },
        {(Function.Kitchen,Function.Bathroom), 3f },
        {(Function.Closet,Function.Courtyard), 4f },
        {(Function.Bathroom,Function.Courtyard), 4f },
        {(Function.Bedroom,Function.Courtyard), 3f },
        {(Function.Dining,Function.Closet), 4f }
    };

    /// <summary>
    /// Defines a dictionary of minimum and maximum areas per room function
    /// </summary>
    public static Dictionary<Function, (float, float)> AreasPerFunction = new Dictionary<Function, (float, float)>()
    {
        {Function.Bathroom, (4, 8) },
        {Function.Kitchen, (6, 12) },
        {Function.Bedroom, (8.5f, 15f) },
        {Function.Dining, (4, 8) },
        {Function.LivingRoom, (9, 16) }
    };
}