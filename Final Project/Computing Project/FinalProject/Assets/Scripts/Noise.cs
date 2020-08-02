using UnityEngine;
using System.Collections;

public delegate float Nfunction(Vector3 point, float frequency);
//Noise Class
//Noise class implementation from creating texture tutorial: http://catlikecoding.com/unity/tutorials/noise/
public class Noise {

    //hash values
    private static int[] hash =
    {
       151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180

    };


    private static float[] gradients1D = {
        1f, -1f
    };

    private const int gradientsMask1D = 1;

    private const int hashMask = 255;

    //Compute squrare root of 2
    private static float sqr2 = Mathf.Sqrt(2f);

    //2D Gradient coefficients
    private static Vector2[] gradients2D = {
        new Vector2( 1f, 0f),
        new Vector2(-1f, 0f),
        new Vector2( 0f, 1f),
        new Vector2( 0f,-1f),
        new Vector2( 1f, 1f).normalized,
        new Vector2(-1f, 1f).normalized,
        new Vector2( 1f,-1f).normalized,
        new Vector2(-1f,-1f).normalized
    };

    //Dot function to compute and return a value for  g(x, y) = ax + by where a and b are either -1, 0 or 1
    private static float Dot(Vector3 g, float x, float z)
    {
        return g.x * x + g.z * z;
    }


    private const int gradientsMask2D = 7;

    public static Nfunction[] noiseDimensions =
{
    Perlin1D,
    Perlin2D
   
};

    //smooth function to smooth transitions in the interpolated values.
    //based on Ken Perlins 2002 revision 
    private static float Smooth(float t)
    {
        //smooth curve function that has a function that has a second and first derrivative of 0 at both ends so the rate of change is always 0 at gradient boundaries.
        //6t5 - 15t4 + 10t3. function
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }
  


public static float Perlin1D(Vector3 V1, float f)
{
        V1 *= f;
        int noise0 = Mathf.FloorToInt(V1.x);
        float t0 = V1.x - noise0;
        float t1 = t0 - 1f;
        noise0 &= hashMask;
        int noise1 = noise0 + 1;
        int h0 = hash[noise0];
        int h1 = hash[noise1];
        float t = Smooth(t0);
        float g0 = gradients1D[hash[noise0] & gradientsMask1D];
        float g1 = gradients1D[hash[noise1] & gradientsMask1D];
        float v0 = g0 * t0;
        float v1 = g1 * t1;
        return Mathf.Lerp(v0, v1, t)*2f;
    }

    public static float Perlin2D(Vector3 V1, float f)
    {
        //Convert the input vector to floating values
        V1 *= f;
        //get the floor value of the floating vector values
        int ix0 = Mathf.FloorToInt(V1.x);
        //(Use Z value instead of x because using a 3D engine and y represents the hight.
        int iy0 = Mathf.FloorToInt(V1.z);
        //Compute gradient values
        //calculate relative distance between  the grid point vector and floating point vector 
        float tx0 = V1.x - ix0;
        float ty0 = V1.z - iy0;
        //
        float tx1 = tx0 - 1f;
        float ty1 = ty0 - 1f;
        ix0 &= hashMask;
        iy0 &= hashMask;
        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;

        int h0 = hash[ix0];
        int h1 = hash[ix1];
        //assign associated random gradient vectors.
        Vector2 g00 = gradients2D[hash[h0 + iy0] & gradientsMask2D];
        Vector2 g10 = gradients2D[hash[h1 + iy0] & gradientsMask2D];
        Vector2 g01 = gradients2D[hash[h0 + iy1] & gradientsMask2D];
        Vector2 g11 = gradients2D[hash[h1 + iy1] & gradientsMask2D];

        //using the  dot function where  g(x, y) = ax + by where a and b 
        float v00 = Dot(g00, tx0, ty0);
        float v10 = Dot(g10, tx1, ty0);
        float v01 = Dot(g01, tx0, ty1);
        float v11 = Dot(g11, tx1, ty1);

        float tx = Smooth(tx0);
        float ty = Smooth(ty0);
        //return linear interpolation of the pairs of the top two points and bottom two points, and the smoothed values of tx and ty
        return Mathf.Lerp(
            Mathf.Lerp(v00, v10, tx),
            Mathf.Lerp(v01, v11, tx),
            ty) * sqr2;
        //  since the maximum value is still reached at the center off a cell with four diagonal gradients pointing at its center.
    }


}
