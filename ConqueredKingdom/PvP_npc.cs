using System;

namespace ConqueredKingdom
{
    class PvP_npc
    {
        private NeuralNet npc_AI;
        public int health;
        public int BMI;
        public float agility;

        public PvP_npc(Random rnd)
        {
            npc_AI = new NeuralNet();
            npc_AI.CreateNet(rnd);
        }

        public void Train(int desired)
        {
            npc_AI.Train(desired);            
        }

        // Feed updated input into a NN for each attack
        public int SetAttack(int e_bmi)
        {
            int attk;

            attk = npc_AI.Update(health, BMI, e_bmi);
            
            // return attack num
            return attk;
        }
    }
}
