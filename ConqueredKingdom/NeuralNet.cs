using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConqueredKingdom
{
    class NeuralNet
    {
        private int Input_Num;
        private Neuron nrn;
        private List<double> NetInputs;
        
        struct Neuron
        {
            public List<double> NetWeights;

            public Neuron(int Input_Num, Random rnd)
            {
                NetWeights = new List<double>();

                for (int i = 0; i < Input_Num + 1; i++)
                {
                    NetWeights.Add(rnd.NextDouble() - 0.5);
                }
            }

            public void AdjustWeight(double delta, int i)
            {
                NetWeights[i] = NetWeights[i] + delta;
            }
        }

        public NeuralNet()
        {
            NetInputs = new List<double>();
            NetInputs.Add(0);
            NetInputs.Add(0);
            NetInputs.Add(0);
            Input_Num = 3;
        }

        // Sends delta for correcting weights
        public void Train(double delta)
        {
            double e = 0.3;
            double weightDelta = 0;

            for (int i = 0; i < NetInputs.Count; i++)
            {
                weightDelta = delta * NetInputs[i] * e;
                nrn.AdjustWeight(weightDelta, i);
            }
        }

        public void CreateNet(Random rnd)
        {
            nrn = new Neuron(Input_Num, rnd);
        }

        // Main method of the NN
        public double Update(double health, double BMI, double e_BMI)
        {
            double netOutput = 0;

            // Add inputs to list
            if (NetInputs[0] == 0)
            {
                NetInputs[1] = BMI / 10;
                NetInputs[2] = e_BMI / 10;
            }

            NetInputs[0] = health / 30;

            // Sum all inputs multiplied by weights
            for (int i = 0; i < Input_Num; i++)
            {
                netOutput += NetInputs[i] * nrn.NetWeights[i];
            }

            // Add in bias
            netOutput += nrn.NetWeights[Input_Num];

            // Total is put through sigmoid function to get output            
            return Sigmoid(netOutput);
        }
        
        private double Sigmoid(double v)
        {
            return 1 / (1 + Math.Exp(-v));
        }
    }
}
