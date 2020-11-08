using System;
using System.Collections;
using System.Collections.Generic;

public class Matrix
{
    private double[,] matrix = null;

    public int Row { get { return matrix.GetLength(0); } }
    public int Column { get { return matrix.GetLength(1); } }

    public Matrix(double[,] matrix)
    {
        this.matrix = matrix;
    }

    public double this[int r, int c]
    {
        get { return this.matrix[r, c]; }
        set { this.matrix[r, c] = value; }
    }

    public static Matrix Random(int row, int column)
    {
        Matrix matrix = new Matrix(new double[row, column]);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                matrix[i, j] = UnityEngine.Random.Range(0, 1f);
            }
        }

        return matrix;
    }

    public static Matrix operator +(Matrix matrix1, Matrix matrix2) 
    {
        return new Matrix(Add(matrix1.matrix, matrix2.matrix));
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        return new Matrix(Dot(matrix1.matrix, matrix2.matrix));
    }

    private static double[,] Add(double[,] matrix1, double[,] matrix2)
    {
        double[,] mx = new double[matrix1.GetLength(0), matrix1.GetLength(1)];

        for (int i = 0; i < mx.GetLength(0); i++)
        {
            for (int j = 0; j < mx.GetLength(1); j++)
            {
                mx[i, j] = matrix1[i, j] + matrix2[i, j];
            }
        }

        return mx;
    }

    private static double[,] Dot(double[,] matrix1, double[,] matrix2)
    {
        return null;
    }

    public void function(Func<Double, Double> function)
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                matrix[i, j] = function(matrix[i, j]);
            }
        }
    }

    public void PrintMatrix()
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                UnityEngine.Debug.Log(this[i, j] + " ");
            }
            UnityEngine.Debug.Log("\n");
        }
    }
}
