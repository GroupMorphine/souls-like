using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork
{
    public List<Matrix> weights = new List<Matrix>();
    private int[] layerSizes;
    public NeuralNetwork(params int[] layerSizes)
    {
        this.layerSizes = layerSizes;
        for (int i = 1; i < layerSizes.Length; i++)
        {
            Matrix matrix = Matrix.Random(layerSizes[i], layerSizes[i - 1]);
            this.weights.Add(matrix);
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

    public Matrix Predict(double[,] inputs)
    {
        Matrix input = new Matrix(inputs);
        Matrix z = null;
        for (int i = 0; i < weights.Count - 1; i++)
        {
            z = input * weights[i];
            z.function(ReLu);

            input = z;
        }

        z = input * weights[weights.Count - 1];
        z.function(Sigmoid);

        return z;
    }

    public NeuralNetwork Copy() 
    {
        List<Matrix> copy_layers = new List<Matrix>();

        for (int i = 0; i < layerSizes.Length; i++)
        {
            Matrix matrix = Matrix.Random(layerSizes[i], layerSizes[i - 1]);
            copy_layers.Add(matrix);
        }

        for (int i = 0; i < copy_layers.Count; i++)
        {
            for (int j = 0; j < copy_layers[i].Row; j++)
            {
                for (int k = 0; k < copy_layers[i].Column; k++)
                {
                    copy_layers[i][j, k] = weights[i][j, k];
                }
            }
        }

        NeuralNetwork copy_nn = new NeuralNetwork(layerSizes);
        copy_nn.weights = copy_layers;

        return copy_nn;
    }
}