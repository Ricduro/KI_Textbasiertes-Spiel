using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConqueredKingdom
{
    class PvP_npc
    {
        private NeuralNet npc_AI;
        public int health;
        public int BMI;
        public float agility;
        public double output;

        public PvP_npc(Random rnd)
        {
            npc_AI = new NeuralNet();
            npc_AI.CreateNet(rnd);
        }

        public void Train(double desired)
        {
            double delta = 0;
            delta = desired - output;

            npc_AI.Train(delta);
        }

        // Feed updated input into a NN for each attack
        public int SetAttack(int e_bmi)
        {
            int attk;
            output = npc_AI.Update(health, BMI, e_bmi);

            if (output >= 0 && output <= 0.33)
            {
                attk = 1;
            }
            else if (output > 0.33 && output <= 0.66)
            {
                attk = 2;
            }
            else if (output > 0.66 && output <= 1)
            {
                attk = 3;
            }
            else
            {
                attk = -1;
            }

            // return attack num
            return attk;
        }
    }
}
