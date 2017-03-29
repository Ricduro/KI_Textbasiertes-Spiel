using System;

namespace ConqueredKingdom
{
    class PvE_npc
    {
        public int health;
        public int BMI;
        public float agility;

        public int SetAttack(Random rnd)
        {
            int att_int;

            //float e = (float)rnd.NextDouble();
            float e = (float)rnd.NextDouble();
            att_int = 0;

            if (e <= 0.33)
            {
                att_int = 1;
            }
            else if (e > 0.33 && e < 0.66)
            {
                att_int = 2;
            }
            else if (e >= 0.66)
            {
                att_int = 3;
            }
            return att_int;
        }
    }
}
