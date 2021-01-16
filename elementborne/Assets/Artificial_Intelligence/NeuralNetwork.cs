using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork
{
    public List<Matrix> weights = new List<Matrix>();
    public List<Matrix> biases = new List<Matrix>();
    private int[] layerSizes;
    public NeuralNetwork(params int[] layerSizes)
    {
        this.layerSizes = layerSizes;

        for (int i = 1; i < layerSizes.Length; i++)
        {
            Matrix weight = Matrix.Random(layerSizes[i], layerSizes[i - 1]);
            Matrix bias = Matrix.Random(layerSizes[i], 1);

            this.biases.Add(bias);
            this.weights.Add(weight);
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
            z = input * weights[i] ^ biases[i];
            z.function(ReLu);

            input = z;
        }

        z = input * weights[weights.Count - 1] ^ biases[biases.Count - 1];
        z.function(Sigmoid);

        return z;
    }

    public NeuralNetwork Copy() 
    {
        List<Matrix> copy_weights = new List<Matrix>();
        List<Matrix> copy_biases = new List<Matrix>();

        for (int i = 0; i < layerSizes.Length; i++)
        {
            Matrix weight = Matrix.Random(layerSizes[i], layerSizes[i - 1]);
            Matrix bias = Matrix.Random(layerSizes[i], 1);

            copy_biases.Add(bias);
            copy_weights.Add(weight);
        }

        for (int i = 0; i < copy_weights.Count; i++)
        {
            for (int j = 0; j < copy_weights[i].Row; j++)
            {
                for (int k = 0; k < copy_weights[i].Column; k++)
                {
                    copy_weights[i][j, k] = weights[i][j, k];
                }
            }
        }

        for (int i = 0; i < copy_biases.Count; i++)
        {
            for (int j = 0; j < copy_biases[i].Row; j++)
            {
                for (int k = 0; k < copy_biases[i].Column; k++)
                {
                    copy_biases[i][j, k] = biases[i][j, k];
                }
            }
        }

        NeuralNetwork copy_nn = new NeuralNetwork(layerSizes);
        copy_nn.weights = copy_weights;
        copy_nn.biases = copy_biases;

        return copy_nn;
    }
}