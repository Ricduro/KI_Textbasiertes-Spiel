using System;
using System.Collections.Generic;

namespace ConqueredKingdom
{
    class NeuralNet
    {
        public int choice;
        private int Input_Num;
        private List<double> L_NetInputs;
        private List<double> L_HiddenOutputs;
        private List<double> L_Outputs;
        private int Output_Num = 3;
        private int Hidden_Num = 5;
        private List<Neuron> L_Neurons = new List<Neuron>();

        struct Neuron
        {
            public List<double> L_nWeights;

            public Neuron(int neuron_inputs, Random rnd)
            {
                L_nWeights = new List<double>();

                for (int i = 0; i < neuron_inputs + 1; i++)
                {
                    L_nWeights.Add((rnd.NextDouble() - 0.5) * 2);
                }
            }

            public void AdjustWeight(double delta, int i)
            {
                L_nWeights[i] = L_nWeights[i] - (delta);
            }
        }

        public NeuralNet()
        {
            L_NetInputs = new List<double>();
            L_HiddenOutputs = new List<double>();
            L_Outputs = new List<double>();
            Input_Num = 3;

            for (int i = 0; i < Input_Num; i++)
            {
                L_NetInputs.Add(i);
            }

            for (int j = 0; j < Hidden_Num; j++)
            {
                L_HiddenOutputs.Add(j);
            }

            for (int k = 0; k < Output_Num; k++)
            {
                L_Outputs.Add(k);
            }
        }

        // Sends delta for correcting weights
        public void Train(int de)
        {
            double e = 1;
            List<double> L_OutputDelta = new List<double>();
            double hiddenDelta = 0;

            // Desired neuron adjustment:
            // Output layer
            for (int i = 0; i < Hidden_Num; i++)
            {
                L_OutputDelta.Add(((L_Outputs[de - 1] - L_Outputs[choice - 1]) * L_HiddenOutputs[i] * e) - 1);
                L_Neurons[5 + (de - 1)].AdjustWeight(L_OutputDelta[i], i);
            }
            // Hidden layer
            for (int i = 0; i < Hidden_Num; i++)
            {
                for (int j = 0; j < Input_Num; j++)
                {
                    hiddenDelta = (L_OutputDelta[i] * L_NetInputs[j] * e) - 1;
                    L_Neurons[i].AdjustWeight(hiddenDelta, j);
                }
            }
        }

        public void CreateNet(Random rnd)
        {
            // Create NN

            for (int i = 0; i < Hidden_Num; i++)
            {
                Neuron n = new Neuron(3, rnd);
                L_Neurons.Add(n);
            }

            for (int j = 0; j < Output_Num; j++)
            {
                Neuron n = new Neuron(Hidden_Num, rnd);
                L_Neurons.Add(n);
            }
        }

        // Main method of the NN
        public int Update(double health, double BMI, double e_BMI)
        {
            int attackChoice;

            // Add inputs to list
            if (L_NetInputs[0] == 0)
            {
                L_NetInputs[1] = BMI;
                L_NetInputs[2] = e_BMI;
            }

            L_NetInputs[0] = health;

            for (int j = 0; j < Hidden_Num; j++)
            {
                for (int i = 0; i < Input_Num; i++)
                {
                    L_HiddenOutputs[j] += (L_NetInputs[i] * L_Neurons[j].L_nWeights[i]);    // Hidden layer outputs
                }

                L_HiddenOutputs[j] += (L_Neurons[j].L_nWeights[Input_Num]);                 // Bias

                L_HiddenOutputs[j] = Sigmoid(L_HiddenOutputs[j]);                           // Sigmoid activation
            }

            for (int j = Hidden_Num; j < Hidden_Num + Output_Num; j++)
            {
                for (int i = 0; i < Hidden_Num; i++)
                {
                    L_Outputs[j - 5] += (L_HiddenOutputs[i] * L_Neurons[j].L_nWeights[i]);  // Output neuron outputs
                }

                L_Outputs[j - 5] += (L_Neurons[j].L_nWeights[Input_Num]);                   // Bias

                L_Outputs[j - 5] = Sigmoid(L_Outputs[j - 5]);                               // Sigmoid activation
            }

            attackChoice = ChooseAttack(L_Outputs);

            return attackChoice;
        }

        private double Sigmoid(double v)
        {
            return 1 / (1 + Math.Exp(-v));
        }

        private int ChooseAttack(List<double> output)
        {
            // Sort outputs and select attack number corr. to greatest output
            if (L_Outputs[2] > L_Outputs[1] && L_Outputs[2] > L_Outputs[0])
            {
                choice = 3;
            }
            else if (L_Outputs[1] > L_Outputs[0] && L_Outputs[1] > L_Outputs[2])
            {
                choice = 2;
            }
            else if (L_Outputs[0] > L_Outputs[1] && L_Outputs[0] > L_Outputs[2])
            {
                choice = 1;
            }

            return choice;
        }
    }
}
