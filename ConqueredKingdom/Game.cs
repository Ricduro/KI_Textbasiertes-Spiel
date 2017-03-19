using System;
using SFML;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace ConqueredKingdom
{
    public class Game
    {
        private static RenderWindow window = null;
        private enum States { St_menu, St_pickCharacter, St_train, St_fight };
        private States gameState;
        private Menu menu;
        private Fight fight;
        private Text text;
        private string s;
        private int menuType, id;
        private bool training, playing;


        public Game()
        {
            menuType = 1;
            id = 1;
            training = true;
            playing = true;
            menu = new Menu();
            fight = new Fight();

            text = new Text(s, new Font(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "/arial.ttf"), 20);
            text.Position = new SFML.System.Vector2f(100, 70);
            text.Color = Color.Black;
        }

        public void Run()
        {
            // Set up window
            if (window == null)
            {
                window = new RenderWindow(new VideoMode(1280, 720), "Conquered Kingdom",
                Styles.Close, new ContextSettings(24, 8, 2));
            }
            window.SetVerticalSyncEnabled(true);
            window.SetActive();

            // Set up event handlers
            window.Closed += new EventHandler(OnClosed);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            while (window.IsOpen)
            {
                // Update:
                Update();
                window.DispatchEvents();

                // Draw:
                window.Clear(Color.White);
                Draw(window);
                window.Display();
            }
        }

        // Updates the Gamestate
        public void Update()
        {
            if (gameState == States.St_menu)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                {
                    menuType = 2;
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                {
                    menuType = 3;
                    gameState = States.St_pickCharacter;
                }
                
                switch (menuType)
                {
                    case 1:
                        {
                            s = menu.Introduce();
                        }
                        break;
                    case 2:
                        {
                            s = menu.Introduce() + menu.Information();
                        }
                        break;
                    case 3:
                        {
                            s = menu.ChooseChar();
                        }
                        break;
                    default:
                        {
                            s = "Conquered Kingdom";
                        }
                        break;
                }
            }

            if (gameState == States.St_pickCharacter)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    id = 1;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    id = 2;
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                    fight.SetFighters(id);
                    gameState = States.St_train;
                }
            }

            if (gameState == States.St_train)
            {
                // NN trains fighting a bot:
                s = "Please wait ..." + "\n(Training in progress)";
                if (training == true)
                {
                    training = fight.TrainBot();
                }
                else
                {
                    fight.SetPlayerFighter();
                    s = fight.Intro();
                    gameState = States.St_fight;
                }
            }

            if (gameState == States.St_fight)
            {
                if (playing)
                {
                    s += fight.Update();
                    playing = fight.KeepFighting();
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                    s = fight.Intro();
                    fight.SetPlayerFighter();
                    playing = true;
                }
            }

            text.DisplayedString = s;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(text);
        }

        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
        
        private static void OnClosed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow) sender;
            window.Close();
        }
    }
}
