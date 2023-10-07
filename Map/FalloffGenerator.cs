using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int size, bool useEvaluateFunc){
        float[,] map = new float[size,size];

        for(int j = 0; j < size; j++){
            for(int i = 0; i < size; i++){
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x),Mathf.Abs(y));
                map[i,j] = Evaluate(value);
                // map[i,j] = useEvaluateFunc?Evaluate(value):value;
            }
        }
        return map;
    }

    static float Evaluate(float value){
        float a = 4;
        float b = 2.2f;
        b = 0.2f;
        return Mathf.Pow(value,a)/(Mathf.Pow(value,a)+Mathf.Pow(b-b*value,a));
    }
}
