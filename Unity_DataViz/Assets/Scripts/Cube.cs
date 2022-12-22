using System;

[Serializable]
public class Cube
{
    public int config;
    public int[] pointIndices;

    public Cube(int config, int[] pointIndices)
    {
        this.config = config;
        this.pointIndices = pointIndices;
    }

    public static float[] GetConfigsFloat(Cube[] cubes)
    {
        var configs = new float[cubes.Length];
        for (var i = 0; i < cubes.Length; i++)
        {
            configs[i] = cubes[i].config;
        }

        return configs;
    }
    public static int[] GetConfigsInt(Cube[] cubes)
    {
        var configs = new int[cubes.Length];
        for (var i = 0; i < cubes.Length; i++)
        {
            configs[i] = cubes[i].config;
        }

        return configs;
    }
}
