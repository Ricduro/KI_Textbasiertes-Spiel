using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConqueredKingdom
{
    public class Fight
    {
        private int ID, cnt;
        private static Random rnd;
        private string foe, str;
        private Stopwatch stopwatch = new Stopwatch();
        private int Exchanges, round_result;
        private int s_attack, t_attack;
        private PvE_npc non_AI;
        private PvP_npc pvp_AI;
        private int thrash_health, scythe_health;
        private int thrash_BMI, scythe_BMI;
        private float thrash_agility, scythe_agility;
        private bool isFighting, setBehavior;
        private int wins;
        private int att;
        private int attk1 = 0;
        private int attk2 = 0;
        private int attk3 = 0;
        
        public Fight()
        {
            rnd = new Random();
            att = 1;
            Exchanges = 0;
            cnt = 0;
            wins = 0;
        }

        // Text lines at the start of the fight
        public string Intro()
        {
            switch (ID)
            {
                case 1:
                    {
                        foe = "filthy Thracian";
                    }
                    break;
                case 2:
                    {
                        foe = "bloodthirsty Scythian";
                    }
                    break;
            }

            str = "A " + foe + " charges at you!\n" +
                  "Choose your attack with 'A', 'S' and 'D'.";
            
            return str;
        }

        // When isFighting is true, the fight can be started from Game.cs
        public void SetPlayerFighter()
        {
            isFighting = true;
            setBehavior = false;

            if (ID == 1)
            {
                scythe_health = non_AI.health;
                scythe_BMI = non_AI.BMI;
                scythe_agility = non_AI.agility;

                thrash_health = pvp_AI.health;
                thrash_BMI = pvp_AI.BMI;
                thrash_agility = pvp_AI.agility;
            }
            else
            {
                scythe_health = pvp_AI.health;
                scythe_BMI = pvp_AI.BMI;
                scythe_agility = pvp_AI.agility;

                thrash_health = non_AI.health;
                thrash_BMI = non_AI.BMI;
                thrash_agility = non_AI.agility;
            }
        }

        // This updates the fight between player and NPC
        public string Update()
        {
            string s = "";
            double b;
            stopwatch.Start();

            while (scythe_health > 0 && thrash_health > 0)
            {
                // Roll AI behavior: Either the neural network output or a random attack is used
                if (setBehavior == false)
                {
                    b = rnd.NextDouble();

                    if (b < 0.6)
                    {
                        if (ID == 1)
                        {
                            t_attack = pvp_AI.SetAttack(non_AI.BMI);
                        }
                        else { s_attack = pvp_AI.SetAttack(non_AI.BMI); }

                    }
                    else if (b >= 0.6)
                    {
                        if (ID == 1)
                        {
                            t_attack = rnd.Next(1, 4);
                        }
                        else { s_attack = rnd.Next(1, 4); }
                    }

                    setBehavior = true;
                }

                // Player can pick next move in this timeframe
                if (stopwatch.ElapsedMilliseconds < 3000)
                {
                    s = "";
                    
                    if (ID == 1)
                    {
                        att = GetInput();
                        s_attack = att;
                    }
                    else
                    {
                        att = GetInput();
                        t_attack = att;
                    }
                }

                // Here the actions are carried out
                if (stopwatch.ElapsedMilliseconds >= 3000)
                {
                    round_result = ResolveExchange(s_attack, t_attack);
                    

                    s = s + PrintResults(round_result, ID, s_attack, t_attack);

                    // Debug lines
                    if (round_result == 1)
                    {
                        Console.WriteLine("Scythian landed a blow!");
                    }
                    if (round_result == 2)
                    {
                        Console.WriteLine("Thracian landed a blow!");
                    }
                    if (round_result == 3)
                    {
                        Console.WriteLine("Scythian dodges.");
                    }
                    if (round_result == 4)
                    {
                        Console.WriteLine("Thracian dodges.");
                    }

                    Exchanges++;
                    stopwatch.Restart();
                    setBehavior = false;
                }

                return s;
            }

            isFighting = false;

            // Report the fight has ended
            if (ID == 1)
            {
                if (scythe_health <= 0)
                {
                    s = s + "\n\nYou died.";
                }
                else if (thrash_health <= 0)
                {
                    s = s + "\n\nYou defeated the Thracian.";
                }
            }
            else
            {
                if (scythe_health <= 0)
                {
                    s = s + "\n\nYou defeated the Scythian.";
                }
                else if (thrash_health <= 0)
                {
                    s = s + "\n\nYou died.";
                }
            }

            s = s + "\n\nPress Enter to keep playing.";
            return s;
        }

        internal bool KeepFighting()
        {
            if (isFighting)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // This method controls the adjustment of weights in the perceptron
        public bool TrainBot()
        {
            double desired = 0;           

            while (cnt < 100)        // Set amount of fights
            {
                if (thrash_health <= 0)
                {
                    scythe_health = 100;
                    thrash_health = 100;
                    cnt++;

                    if (ID == 2)
                    {
                        wins++;
                    }
                }

                if (scythe_health <= 0)
                {
                    scythe_health = 100;
                    thrash_health = 100;
                    cnt++;

                    if (ID == 1)
                    {
                        wins++;
                    }
                }
                
                if (ID == 1)
                {
                    t_attack = pvp_AI.SetAttack(non_AI.BMI);
                    if (t_attack == 1)
                    {
                        attk1++;
                    }
                    else if (t_attack == 2)
                    {
                        attk2++;
                    }
                    else if (t_attack == 3)
                    {
                        attk3++;
                    }
                    s_attack = non_AI.SetAttack(rnd);
                    
                    round_result = ResolveExchange(s_attack, t_attack);
                    
                    if (round_result == 1)
                    {
                        if (s_attack == 1)
                        {
                            desired = 0.5;
                        }
                        else if (s_attack == 2)
                        {
                            desired = 0.83;
                        }
                        else
                        {
                            desired = 0.16;
                        }

                        pvp_AI.Train(desired);
                    }
                }

                else if (ID == 2)
                {
                    t_attack = non_AI.SetAttack(rnd);
                    s_attack = pvp_AI.SetAttack(non_AI.BMI);
                    if (s_attack == 1)
                    {
                        attk1++;
                    }
                    else if (s_attack == 2)
                    {
                        attk2++;
                    }
                    else if(s_attack == 3)
                    {
                        attk3++;
                    }
                    
                    round_result = ResolveExchange(s_attack, t_attack);
                    
                    if (round_result == 2)
                    {
                        if (t_attack == 1)
                        {
                            desired = 0.5;
                        }
                        else if (t_attack == 2)
                        {
                            desired = 0.9;
                        }
                        else
                        {
                            desired = 0.1;
                        }

                        pvp_AI.Train(desired);
                    }
                }
                                
                Console.WriteLine(cnt);
                return true;
            }
            
            {
                Console.WriteLine("Bot won " + wins + " times.");
                Console.WriteLine("\nAttack 1 " + attk1 + " times.");
                Console.WriteLine("\nAttack 2 " + attk2 + " times.");
                Console.WriteLine("\nAttack 3 " + attk3 + " times.");

                return false;
            }

        }

        // Initialize the fighters
        public void SetFighters(int id)
        {
            pvp_AI = new PvP_npc(rnd);
            non_AI = new PvE_npc();

            InitializeFighters();

            ID = id;
        }

        // Called once, assigns fighter variables
        private void InitializeFighters()
        {
            double r = rnd.NextDouble();

            if (r < 0.5)
            {
                pvp_AI.BMI = 22;
                non_AI.BMI = 34;
            }
            else if (r <= 0.5)
            {
                pvp_AI.BMI = 34;
                non_AI.BMI = 22;
            }

            pvp_AI.health = 100;
            pvp_AI.agility = 10 / (float)pvp_AI.BMI;
            non_AI.health = 100;
            non_AI.agility = 10 / (float)pvp_AI.BMI;
        }

        // Player input methods
        private int GetInput()
        {
            int next_attack = att;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            { next_attack = 1; }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            { next_attack = 2; }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            { next_attack = 3; }

            return next_attack;
        }

        // Sends the result of each round onto the screen as narration
        private string PrintResults(int result, int id, int s_choice, int t_choice)
        {
            string report = "";

            #region text commentary
            switch (result)
            {
                case 1:
                    {
                        if (s_choice == t_choice)
                        {
                            if (id == 1)
                            { report = "\nYou block and strike him with your axe."; }
                            else
                            { report = "\nThe enemy blocks and strikes you with his axe."; }
                        }
                        else if (s_choice == 1)
                        {
                            if (id == 1)
                            { report = "\nYou chop the enemy with your axe."; }
                            else
                            { report = "\nEnemy chops you with an axe."; }
                        }
                        else if (s_choice == 2)
                        {
                            if (id == 1)
                            { report = "\nYou deal a CRITICAL HIT with your axe."; }
                            else
                            { report = "\nYou receive CRITICAL DAMAGE."; }
                        }
                        else
                        {
                            if (id == 1)
                            { report = "\nEnemy stumbles into your axe swing."; }
                            else
                            { report = "\nYou stumble and are chopped by the enemy."; }
                        }
                    }
                    break;
                    
                case 2:
                    {
                        if (t_choice == s_choice)
                        {
                            if (id == 1)
                            { report = "\nEnemy dodges and hits you with his sword."; }
                            else
                            { report = "\nYou dodge and hit him with your sword."; }
                        }
                        else if (t_choice == 1)
                        {
                            if (id == 1)
                            { report = "\nEnemy cuts you with his sword."; }
                            else
                            { report = "\nYou cut the enemy with your sword."; }
                        }
                        else if (t_choice == 2)
                        {
                            if (id == 1)
                            { report = "\nYou stumble into a sword stab."; }
                            else
                            { report = "\nEnemy stumbles and you stab him."; }
                        }
                        else
                        {
                            if (id == 1)
                            { report = "\nYou receive CRITICAL DAMAGE."; }
                            else
                            { report = "\nYou deal a CRITICAL HIT with your sword."; }
                        }

                    }
                    break;

                case 3:
                    {
                        if (id == 1)
                        { report = "\nYou dodge the enemy attack."; }
                        else
                        { report = "\nThe enemy dodges your attack."; }
                    }
                    break;

                case 4:
                    {
                        if (id == 1)
                        { report = "\nThe enemy dodges your attack."; }
                        else
                        { report = "\nYou dodge the enemy attack."; }
                    }
                    break;
            }
            #endregion

            return report;
        }

        // Fight logic that decides the outcome of every round
        private int ResolveExchange(int s, int t)
        {
            // Result guide: 1: scythian hits, 2: thracian hits, 3: scythian dodges, 4: thracian dodges
            if (s == 1 && t == 2)
            {
                // advantage thracian, 2 x scythian agility
                float s_max, t_max, e;
                s_max = (float)0.3 + (2 * scythe_agility);
                t_max = s_max + (float)(0.7 + thrash_agility);
                e = (float)rnd.NextDouble() * t_max;
                if (e <= s_max)
                {
                    return 3;
                }
                else
                {
                    scythe_health -= (20 + ( 4 * (thrash_BMI / 10)));
                    return 2;
                }
            }

            if (s == 1 && t == 3)
            {
                // advantage scythian, thracian dodge possible
                float s_max, t_max, e;
                s_max = (float)0.8 + scythe_agility;
                t_max = s_max + (float)(0.2 + thrash_agility);
                e = (float)rnd.NextDouble() * t_max;
                if (e <= s_max)
                {
                    thrash_health -= (20 + ( 4 * (scythe_BMI / 10)));
                    return 1;
                }
                else { return 4; }
            }

            if (s == 2 && t == 1)
            {
                thrash_health -= 30;
                return 1;
            }

            if (s == 2 && t == 3)
            {
                scythe_health -= 30;
                return 2;
            }

            if (s == 3 && t == 1)
            {
                // advantage thracian, scythian dodge possible
                float s_max, t_max, e;
                s_max = (float)0.2 + scythe_agility;
                t_max = s_max + (float)(0.8 + thrash_agility);
                e = (float)rnd.NextDouble() * t_max;
                if (e <= s_max)
                {
                    return 3;
                }
                else
                {
                    scythe_health -= (20 + ( 4 * (thrash_BMI / 10)));
                    return 2;
                }
            }

            if (s == 3 && t == 2)
            {
                // advantage scythian, 2 x thracian agility
                float s_max, t_max, e;
                s_max = (float)0.7 + scythe_agility;
                t_max = s_max + (float)(0.3 + (2 * thrash_agility));
                e = (float)rnd.NextDouble() * t_max;
                if (e <= s_max)
                {
                    thrash_health -= (20 + ( 4 * (scythe_BMI / 10)));
                    return 1;
                }
                else { return 4; }
            }

            // Otherwise both chose the same:
            else
            {
                float s_max, t_max, e;
                s_max = (float)0.5 + scythe_agility;
                t_max = s_max + (float)(0.5 + thrash_agility);
                e = (float)rnd.NextDouble() * t_max;
                if (e <= s_max)
                {
                    thrash_health -= (20 + ( 4 * (scythe_BMI / 10)));
                    return 1;
                }
                else
                {
                    scythe_health -= (20 + ( 4 * (thrash_BMI / 10)));
                    return 2;
                }
            }
        }
    }
}
