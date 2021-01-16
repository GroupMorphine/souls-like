using System;
using System.Collections;
using System.Collections.Generic;

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
                if (neural1.weights[i].Row == neural2.weights[i].Row &&
                    neural2.weights[i].Column == neural2.weights[i].Column)
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
            for (int j = 0; j < network.weights[i].Row; j++)
            {
                for (int k = 0; k < network.weights[i].Column; k++)
                {
                    if (true)
                    {

                    }
                }
            }
        }

        return network;
    }
}
