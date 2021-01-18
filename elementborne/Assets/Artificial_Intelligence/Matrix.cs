using System;

public class Matrix
{
    private double[,] matrix = null;

    public int Row { get { return matrix.GetLength(0); } }
    public int Column { get { return matrix.GetLength(1); } }

    public static Matrix T(Matrix matrix)
    {
        double[,] temp = new double[matrix.Column, matrix.Row];
        Matrix tr = new Matrix(temp);

        for (int i = 0; i < matrix.Row; i++)
        {
            for (int j = 0; j < matrix.Column; j++)
            {
                tr[j, i] = matrix[i, j];
            }
        }

        return tr;
    }

    public Matrix(double[,] matrix)
    {
        this.matrix = matrix;
    }

    public double this[int row, int column]
    {
        get { return this.matrix[row, column]; }
        set { this.matrix[row, column] = value; }
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

    public static Matrix operator ^(Matrix matrix1, Matrix matrix2) 
    {
        return new Matrix(ElementWiseAdd(matrix1.matrix, matrix2.matrix));
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        return new Matrix(Dot(matrix1.matrix, matrix2.matrix));
    }

    private static double[,] ElementWiseAdd(double[,] matrix1, double[,] matrix2)
    {
        int row1 = matrix1.GetLength(0);
        int col1 = matrix1.GetLength(1);
        int row2 = matrix2.GetLength(0);
        int col2 = matrix2.GetLength(1);

        double[,] matrix = new double[row1, col1];

        if (col1 % col2 == 0 && row1 == row2)
        {
            for (int i = 0; i < row1; i++)
            {
                for (int k = 0; k < col1; k++)
                {
                    matrix[i, k] = matrix1[i, k] + matrix2[i % row2, k % col2];
                }
            }
        }

        return matrix;
    }

    private static double[,] Add(double[,] matrix1, double[,] matrix2)
    {
        double[,] matrix = new double[matrix1.GetLength(0), matrix1.GetLength(1)];
        
        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix1.GetLength(1); j++)
            {
                matrix[i, j] = matrix1[i, j] + matrix2[i, j];
            }
        }
        return matrix; 
    }

    private static double[,] Dot(double[,] matrix1, double[,] matrix2)
    {
        double[,] matrix = new double[matrix1.GetLength(0), matrix2.GetLength(1)];
        
        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix2.GetLength(1); j++)
            {
                for (int k = 0; k < matrix1.GetLength(1); k++)
                {
                    matrix[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }
        }
        return matrix;
    }

    public static Matrix UniformCross(Matrix matrix1, Matrix matrix2) 
    {
        Matrix uni = new Matrix(new double[matrix1.Row, matrix1.Column]);

        for (int i = 0; i < uni.Row; i++)
        {
            for (int j = 0; j < uni.Column; j++)
            {
                float rnd = UnityEngine.Random.Range(0, 1f);
                
                if (rnd <= 0.5)
                {
                    uni[i, j] = matrix1[i, j];
                }
                else
                {
                    uni[i, j] = matrix2[i, j];
                }
            }
        }

        return uni;
    }

    public static Matrix Mutate(Matrix matrix, float rate = 0.1f) 
    {
        Matrix mutated = new Matrix(new double[matrix.Row, matrix.Column]);

        for (int i = 0; i < mutated.Row; i++)
        {
            for (int j = 0; j < mutated.Column; j++)
            {
                if (UnityEngine.Random.Range(0, 1f) < rate)
                {
                    mutated[i, j] = UnityEngine.Random.Range(0, 1f);
                }
            }
        }

        return mutated;
    }

    public void function(Func<Double, Double> function)
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                this[i, j] = function(this[i, j]);
            }
        }
    }
}