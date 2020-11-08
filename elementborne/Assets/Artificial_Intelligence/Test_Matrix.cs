using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Matrix : MonoBehaviour
{    
    void Start()
    {
        Matrix m1 = Matrix.Random(2, 3);
        Matrix m2 = Matrix.Random(2, 3);

        Matrix result = m1 + m2;

        result.PrintMatrix();
    }
}
