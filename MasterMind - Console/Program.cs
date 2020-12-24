﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MasterMind___Console
{
    class Program
    {
        const int WIDTH = 92;
        const ConsoleColor DEFAULT_TITLE_FONT_COL = ConsoleColor.Cyan;
        const ConsoleColor DEFAULT_TITLE_BACK_COL = ConsoleColor.DarkCyan;
        const string title = "Master Mind for the Console - created by B34STM4ST3R";

        //Main
        static void Main(string[] args)
        {           
            Console.Title = "MasterMind for the Command Line Terminal";

            try
            {
                Console.SetWindowPosition(0, 0);
                Console.WindowHeight = Console.LargestWindowHeight;
            }
            catch
            { }

            try
            {
                Console.SetBufferSize(WIDTH, 300);
                Console.WindowWidth = WIDTH;           
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine(CenterText("If you are running Windows 10, please open the game in"));
                Console.WriteLine(CenterText("Compatibility Mode for Windows 7\n\n"));
                Console.WriteLine(CenterText("Or, if the above didn't work:\n\n"));
                Console.WriteLine(CenterText("Please change your window size manually via the Console settings"));
                Console.WriteLine(CenterText("(Access menu by clicking on the Logo in the Top-left Corner of the screen\n\n"));
                Console.WriteLine(CenterText("INSTRUCTIONS"));
                Console.WriteLine(CenterText("============"));
                Console.WriteLine(CenterText("1. CLICK: the 'M' Logo > Properties > Layout (tab) ...\n"));
                Console.WriteLine(CenterText("2. Set 'Screen Buffer size': Width = 92, Height = 300\n"));
                Console.WriteLine(CenterText("3. Set 'Window Size': Width = 92, Height = (as high as will fit on your screen)"));
                Console.WriteLine(CenterText("4. Click Ok\n\n"));
                Idle("continue to the game");
                Console.Clear();
            }
            
            PrintTitleCentred(GenerateTitle("Welcome to Master Mind", height: 5, borderCorner: "=", borderVert: ""));
            Idle("proceed");
            MainMenu();
        }

        //Main Menu
        static void MainMenu()
        {
            string[] AutoSave = new string[4];
            for (int x = 0; x < 4; x = x + 1)
            {
                AutoSave[x] = "???";
            }

            bool Repeat = true;
            while (Repeat)
            {
                Console.Clear();
                Console.CursorVisible = false;
                PrintTitleCentred(GenerateTitle("Main Menu", height: 5, borderCorner:"=", borderVert: ""));
                Console.WriteLine("\n");
                PrintTitleCentred(GenerateTitle("1      Start Game      1", width: 30), ConsoleColor.Green, ConsoleColor.DarkGreen);
                PrintTitleCentred(GenerateTitle("2      How to Play     2", width: 30), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                PrintTitleCentred(GenerateTitle("3    Exit to Windows   3", width: 30), ConsoleColor.White, ConsoleColor.DarkGray);

                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':                        
                        StartGame(SetGameSettings(AutoSave));
                        break;

                    case '2':
                        HowToPlay();
                        break;

                    case '3':
                        Repeat = false;
                        break;

                    default: break;
                }
            }            
        }

        //Menu Item 1
        //Game Setup
        static GameManager SetGameSettings(string[] saves)
        {
            string customLength, customTurns, customSize, customColours;
            bool Repeat = true;
            bool Default = true;
            customLength = saves[0];
            customTurns = saves[1];
            customSize = saves[2];
            customColours = saves[3];

            while (Repeat)
            {
                Console.Clear();
                PrintTitleCentred(GenerateTitle("Game Preferences", height: 5, borderCorner: "=", borderVert: ""));
                Console.WriteLine("\n");
                PrintTitleCentred(GenerateTitle("1   Traditional Rules   1", width: 31), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                PrintGameSettings();
                PrintTitleCentred(GenerateTitle("2   Saved Custom Game   2", width: 31), ConsoleColor.Green, ConsoleColor.DarkGreen);
                PrintGameSettings(customLength, customTurns, customColours, customSize);
                Console.WriteLine(CenterText("<press 3 to setup new custom game>"));

                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':
                        Default = true;
                        Repeat = false;
                        break;

                    case '2':
                        if (customSize != "???")
                        {
                            Default = false;
                            Repeat = false;
                            break;
                        }
                        else break;

                    case '3':
                        Console.Clear();
                        PrintTitleCentred(GenerateTitle("Game Preferences - Custom Game Setup", height: 5, borderCorner: "=", borderVert: ""));
                        Console.WriteLine("\n");                        
                        SelectPreferences(out customLength, out customTurns, out customColours, out customSize);
                        Console.WriteLine("\n");
                        PrintTitleCentred(GenerateTitle("Game Preferences", height: 5, borderCorner: "=", borderVert: ""),ConsoleColor.White,ConsoleColor.DarkGray);
                        PrintTitleCentred(GenerateTitle("       Custom Game       ", width: 30), ConsoleColor.Green, ConsoleColor.DarkGreen);
                        PrintGameSettings(customLength, customTurns, customColours, customSize);
                        Console.WriteLine("\n");
                        Console.WriteLine(CenterText("Are you happy with the above settings? Press ENTER to start the game"));
                        Console.WriteLine(CenterText("Press any other key to restart game setup\n\n\n\n\n\n\n\n\n\n"));
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                Default = false;
                                Repeat = false;
                                break;

                            default:
                                Repeat = true;
                                customLength = saves[0];
                                customTurns = saves[1];
                                customSize = saves[2];
                                customColours = saves[3];
                                break;                           
                        }
                        break;
                }
            }
            if (Default)
            {
                GameManager GM = new GameManager();
                return GM;
            }
            else
            {
                saves[0] = customLength;
                saves[1] = customTurns;
                saves[2] = customSize;
                saves[3] = customColours;
                GameManager GM = new GameManager(int.Parse(customLength), int.Parse(customTurns), int.Parse(customSize), int.Parse(customColours));
                return GM;
            }
        }

        //Receive preferences from User (And verify correct input)
        static void SelectPreferences(out string customLength, out string customTurns, out string customColours, out string customSize)
        {
            Console.CursorVisible = true;
            //Code Length
            PrintTitleCentred(GenerateTitle("Code Length", width: 30), ConsoleColor.White, ConsoleColor.DarkGray);
            Console.WriteLine(CenterText("Please enter a number from 2 to 14"));
            customLength = GetValidNumber(2,14);

            //Turns
            PrintTitleCentred(GenerateTitle("Number of Turns allowed", width: 30), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            Console.WriteLine(CenterText("Please enter a number from 5 to 30"));
            Console.WriteLine(CenterText("Recommended maximum: 23 turns"));
            customTurns = GetValidNumber(5, 30);

            //Number Colours
            PrintTitleCentred(GenerateTitle("Number of Peg colours", width: 30), ConsoleColor.Green, ConsoleColor.DarkGreen);
            Console.WriteLine(CenterText("Please enter a number from 2 to 10"));
            customColours = GetValidNumber(2, 10);

            //Size of Board
            PrintTitleCentred(GenerateTitle("Board Display size", width: 30), ConsoleColor.Cyan, ConsoleColor.DarkCyan);
            if (int.Parse(customLength) > 10)
            {
                Console.WriteLine(CenterText("Due to the length of the code the board will be displayed as \"Tiny\""));
                Console.WriteLine();
                Idle();
                customSize = "1";
            }
            else
            {
                Console.WriteLine(CenterText("Please choose the size of the display (enter the number)"));
                Console.WriteLine();
                bool footnote = false;
                Console.WriteLine(CenterText("Tiny (1)"));
                if (int.Parse(customTurns) < 15)
                    Console.WriteLine(CenterText("Big  (2)"));
                else
                {
                    Console.WriteLine(CenterText("Big  (2)") + "  -  Not Recommended *");
                    footnote = true;
                }
                    
                if (int.Parse(customLength) < 9)
                {
                    if (int.Parse(customTurns) < 11)
                        Console.WriteLine(CenterText("Huge (3)"));
                    else
                    {
                        Console.WriteLine(CenterText("Huge (3)") + "  -  Not Recommended *");
                        footnote = true;
                    }                       
                }
                if (footnote)
                {
                    Console.WriteLine("\n\n" + CenterText("* Vertical scrolling will probably be required, which may affect your gaming experience"));
                }

                bool Repeat = true;
                string temp = "2";
                while (Repeat)
                {
                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case '1':
                            temp = "1";
                            Console.WriteLine("1");
                            Repeat = false;
                            break;

                        case '2':
                            temp = "2";
                            Console.WriteLine("2");
                            Repeat = false;
                            break;

                        case '3':
                            if (int.Parse(customLength) < 9)
                            {
                                temp = "3";
                                Console.WriteLine("3");
                                Repeat = false;
                            }                               
                            break;

                        default: break;
                    }
                }
                customSize = temp;
            }
            Console.CursorVisible = false;
        }

        static string GetValidNumber(int lowerBound, int upperBound)
        {
            int number = -13;        
            string input = Console.ReadLine();
            int.TryParse(input,out number);
            if ((number < lowerBound || number > upperBound) || int.TryParse(input, out number) == false)
            while ((number < lowerBound || number > upperBound) || int.TryParse(input, out number) == false)
            {
                Console.WriteLine("Invalid parameter. Try again");
                input = Console.ReadLine();
                int.TryParse(input,out number);
            }
            return number.ToString();
        }

        //Print selected settings
        static void PrintGameSettings(string CodeLength = "4", string NumTurns = "10", string NumberColours = "8", string BlockHeight = "2")
        {
                             
            Console.WriteLine(CenterText("Code Length:        " + AlignValue(CodeLength)));
            Console.WriteLine(CenterText("Max. Turns allowed: " + AlignValue(NumTurns)));
            Console.WriteLine(CenterText("Available Colours:  " + AlignValue(NumberColours)));
            if (BlockHeight.Equals("1"))
                BlockHeight = "Tiny";
            else if (BlockHeight.Equals("2"))
                BlockHeight = "Big";
            else if (BlockHeight.Equals("3"))
                BlockHeight = "Huge";

            Console.WriteLine(CenterText("Draw the Board:     " + AlignValue(BlockHeight)));
            Console.WriteLine();
            Console.WriteLine();
        }

        
        //Begin Game
        static void StartGame(GameManager GM)
        {
            for (int x = 1, n = GM.GetNumTurns(); x <= n + 1; x = x + 1)
            {
                Console.Clear();
                PrintTitleCentred(GenerateTitle(title, height: 5, borderCorner: "=", borderVert: ""));
                GM.DrawGameBoard();
                GM.ShowPegColours();
                if (GM.GetTurn() <= n)
                {                
                    Heading1("Turn " + GM.GetTurn() + ":");
                    GM.EnterAndSubmitCode();
                    if (GM.GetSolved()) break;
                }                
            }
            if (!GM.GetSolved())
            {
                PrintTitleCentred(GenerateTitle("Sorry, But you lose...", width: 50), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                Idle("reveal the code");
                Console.Clear();
                PrintTitleCentred(GenerateTitle(title, height: 5, borderCorner: "=", borderVert: ""));
                GM.DrawGameBoard("lose");
                GM.ShowPegColours();
                PrintTitleCentred(GenerateTitle("Sorry, But you lose...", width: 50), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            }
            else
            {
                Console.Clear();
                PrintTitleCentred(GenerateTitle(title, height: 5, borderCorner: "=", borderVert: ""));
                GM.DrawGameBoard("win");
                GM.ShowPegColours();
                Console.WriteLine();
                PrintTitleCentred(GenerateTitle("Well Done!!! You cracked the Code", width: 50), ConsoleColor.Green, ConsoleColor.DarkGreen);
            }
            //Console.WriteLine();            
            Idle("return to the main menu");
        }

        static void Heading1(string text)
        {
            Console.WriteLine(text);
            for (int x = 1, n = text.Length; x <= n; x = x + 1)
            {
                Console.Write("=");
            }
            Console.WriteLine("\n");
        }

        //Menu Item 2
        static void HowToPlay()
        {
            Console.Clear();
            PrintTitleCentred(GenerateTitle("How to Play (Concise/Incomplete)", height: 5, borderCorner: "=", borderVert: ""));
            PrintTitleCentred(GenerateTitle("Objective: Crack the code within the set number of turns"), ConsoleColor.Green, ConsoleColor.DarkGreen);
            PrintTitleCentred(GenerateTitle("The Scoring System", width: 30), ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            PrintTitleCentred(GenerateTitle("# Exact", width: 15, borderHoriz: "-"), ConsoleColor.White, ConsoleColor.DarkGray);
            Console.WriteLine(CenterText("Number of exact matches. An Exact match is where a peg in your code"));
            Console.WriteLine(CenterText("is the correct colour and in the correct position"));
            PrintTitleCentred(GenerateTitle("# Partial", width: 15, borderHoriz: "-"), ConsoleColor.White, ConsoleColor.DarkGray);
            Console.WriteLine(CenterText("Number of partial matches. A Partial match is where a peg in your code"));
            Console.WriteLine(CenterText("is the correct colour but in the incorrect position"));
            PrintTitleCentred(GenerateTitle("# Incorrect", width: 15, borderHoriz: "-"), ConsoleColor.White, ConsoleColor.DarkGray);
            Console.WriteLine(CenterText("Number of incorrect pegs. An Incorrect peg is a peg in your code which is the wrong colour"));
            Console.WriteLine("\n");
            PrintTitleCentred(GenerateTitle("That's all for now..."), ConsoleColor.Gray, ConsoleColor.DarkGray);
            Console.WriteLine(CenterText("Mess around with the game, experiment... and you'll soon figure out what's cooking!"));
            Console.WriteLine("\n");
            Idle();
        }
        
        //DECORATIVE FUNCTIONS
        static void PrintTitleCentred(string[] text, ConsoleColor fontColour = DEFAULT_TITLE_FONT_COL, ConsoleColor backgroundColour = DEFAULT_TITLE_BACK_COL)
        {
            Console.WriteLine();

            //Save colours
            ConsoleColor tempFont = Console.ForegroundColor;
            ConsoleColor tempBack = Console.BackgroundColor;

            if (text[0].Length == WIDTH)
            {
                //Apply new colouring
                Console.ForegroundColor = fontColour;
                Console.BackgroundColor = backgroundColour;

                string temp = "";
                for (int x = 0, n = text.Length; x < n; x = x + 1)
                    temp += text[x];
                Console.WriteLine(temp);
            }                
            else
            {
                int buffer = CenterText(text[0]).Length - text[0].Length;
                for (int x = 0; x < text.Length; x = x + 1)
                {

                    //Revert colours
                    Console.ForegroundColor = tempFont;
                    Console.BackgroundColor = tempBack;

                    string Buffer = CenterText(text[x]).Remove(buffer);
                    string Colour = CenterText(text[x]).Substring(buffer);

                    Console.Write(Buffer);

                    //Apply new colouring
                    Console.ForegroundColor = fontColour;
                    Console.BackgroundColor = backgroundColour;

                    Console.WriteLine(Colour);
                }                                        
            }
                
            //Revert colours
            Console.ForegroundColor = tempFont;
            Console.BackgroundColor = tempBack;

            Console.WriteLine();
        }

        
        static string[] GenerateTitle(string text, string borderCorner = "+", string borderVert = "|", string borderHoriz = "=", int width = WIDTH, int height = 3)
        {
            if (height != 3 && height != 5) height = 3;

            string[] titleBuilder = new string[height];
            for (int x = 0; x < height; x = x + 1)
            {
                titleBuilder[x] = "";
            }

            int pos = 0;
            
            //First Line          
            titleBuilder[pos] += borderCorner;
            for (int x = 1, n = width - (2 * borderCorner.Length); x <= n; x = x + 1)
            {
                titleBuilder[pos] += borderHoriz[0];
            }
            titleBuilder[pos] += borderCorner;

            //Second Line
            if (height == 5)
            {
                pos++;
                titleBuilder[pos] += borderVert;
                for (int x = 1, n = (width - (2 * borderVert.Length)); x <= n; x = x + 1)
                {
                    titleBuilder[pos] += " ";
                }
                titleBuilder[pos] += borderVert;
            }

            //Middle Line
            pos++;
            titleBuilder[pos] += borderVert;

            int buffer = (width - (2 * borderVert.Length) - text.Length) / 2;            
            for (int x = 1; x <= buffer; x = x + 1)
            {
                titleBuilder[pos] += " ";
            }
            titleBuilder[pos] += text.ToUpper();

            for (int x = 1, n = (width - (2 * borderVert.Length) - buffer - text.Length); x <= n; x = x + 1)
            {
                titleBuilder[pos] += " ";
            }
            titleBuilder[pos] += borderVert;

            //Second-Last Line
            if (height == 5)
            {
                pos++;
                titleBuilder[pos] += borderVert;
                for (int x = 1, n = (width - (2 * borderVert.Length)); x <= n; x = x + 1)
                {
                    titleBuilder[pos] += " ";
                }
                titleBuilder[pos] += borderVert;
            }

            //Last Line
            pos++;
            titleBuilder[pos] += borderCorner;
            for (int x = 1, n = width - (2 * borderCorner.Length); x <= n; x = x + 1)
            {
                titleBuilder[pos] += borderHoriz[0];
            }
            titleBuilder[pos] += borderCorner;

            return titleBuilder;
        }

        static void Idle(string action = "continue")
        {
            Console.CursorVisible = false;
            Console.Write(CenterText("<press any key to " + action + ">"));
            Console.ReadKey(true);
            Console.CursorVisible = true;
        }

        static string CenterText(string text)
        {
            int textLength = text.Length;
            for (int x = 1, n = (WIDTH - textLength) / 2; x <= n; x = x + 1)
            {
                text = " " + text;
            }
            return text;
        }

        static string AlignValue(string value)
        {
            string temp = value;
            for (int x = 1, n = 4 - value.ToString().Length; x <= n; x = x + 1)
            {
                temp = " " + temp;
            }
            return temp;
        }
    }
}
