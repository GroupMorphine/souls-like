using System;

public class Genetic
{
    NeuralNetwork nn1;
    NeuralNetwork nn2;

    public Genetic(NeuralNetwork neural1, NeuralNetwork neural2)
    {
        if (neural1.weights.Count == neural2.weights.Count)
        {
            for (int i = 0; i < neural1.weights.Count; i++)
            {
                if ((neural1.weights[i].Row == neural2.weights[i].Row &&
                    neural2.weights[i].Column == neural2.weights[i].Column) &&
                   (neural1.biases[i].Row == neural2.biases[i].Row &&
                    neural2.biases[i].Column == neural2.biases[i].Column))
                {
                    this.nn1 = neural1;
                    this.nn2 = neural2;
                }
                else
                {
                    throw new Exception("The sizes are not same!");
                }
            }
        }
        else
        {
            throw new Exception("The sizes are not same!");
        }
    }

    public NeuralNetwork CrossOver()
    {
        NeuralNetwork network = nn1.Copy();
        
        for (int i = 0; i < network.weights.Count; i++)
        {
            network.weights[i] = Matrix.UniformCross(nn1.weights[i], nn2.weights[i]);
        }

        for (int i = 0; i < network.biases.Count; i++)
        {
            network.biases[i] = Matrix.UniformCross(nn1.biases[i], nn2.biases[i]);
        }

        return network;
    }

    public static NeuralNetwork Mutate(NeuralNetwork net, float mutation_rate = 0.1f) 
    {
        NeuralNetwork network = net.Copy();

        

        return network;
    }
}
