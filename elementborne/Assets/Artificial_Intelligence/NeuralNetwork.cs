using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork
{
    private List<Matrix> layers = new List<Matrix>();

    public NeuralNetwork(params int[] layers)
    {
        for (int i = 1; i < layers.Length; i++)
        {
            Matrix matrix = Matrix.Random(layers[i], layers[i - 1]);
            this.layers.Add(matrix);
        }
    }

    private double ReLu(double x)
    {
        if (x < 0)
            return 0;
        return x;
    }

    private double Sigmoid(double x)
    {
        return 1 / (1 + System.Math.Exp(-x));
    }

    private Matrix Predict(double[,] inputs)
    {
        Matrix input = new Matrix(inputs);
        Matrix z = null;
        for (int i = 0; i < layers.Count - 1; i++)
        {
            z = input * layers[i];
            z.function(ReLu);

            input = z;
        }

        z = input * layers[layers.Count - 1];
        z.function(Sigmoid);

        return z;
    }
}
