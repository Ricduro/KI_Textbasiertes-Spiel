using System;

namespace ConqueredKingdom
{
    public class Menu
    {
        private String txtString;

        public Menu()
        { }

        public string Introduce()
        {
            txtString = "Welcome to Conquered Kingdom!\n\n" +
                        "Press '1' to get a quick rundown, press '2' to play.\n"+
                        "'Escape' quits the game.";
            return txtString;
        }

        public string Information()
        {
            txtString = "\n\nYour objective is to defeat your foe. You choose attacks with 'A', 'S' and 'D'.\n" +
                        "Every few seconds attacks are executed, be sure to select the attack you want.";
            return txtString;
        }

        internal string ChooseChar()
        {
            txtString = "You are a...\n('A' for Scythian, 'D' for Thracian)\n" + 
                        "Press Enter to continue.";
            return txtString;
        }

    }
}
