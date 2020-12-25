using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MasterMind___Console
{
    class GameManager
    {
        private int CodeLength;
        private int NumTurns;
        private int BlockHeight;
        private int NumberColours;
        private bool AllowBlanks;
        private Code[] PegMap;
        private string[] ScoreMap;
        private bool solved;
        private int turn;

        private bool warned;
        private int messageLength;

        //Constructor
        public GameManager(int CodeLength = 4, int NumTurns = 10, int BlockHeight = 2, int NumberColours = 8, bool AllowBlanks = true)
        {
            this.CodeLength = CodeLength;
            this.NumTurns = NumTurns;
            this.BlockHeight = BlockHeight;
            this.AllowBlanks = AllowBlanks;
            if (NumberColours<=10)
                this.NumberColours = NumberColours;
            else
                this.NumberColours = 10;
            PegMap = new Code[NumTurns+1];
            PegMap[0] = new Code(CodeLength, NumberColours, AllowBlanks);
            for (int x = 1; x<=NumTurns; x = x + 1)
            {
                PegMap[x] = new Code(CodeLength);
            }
            ScoreMap = new string[NumTurns + 1];
            ScoreMap[0] = "";
            for (int x = 1; x <= NumTurns; x = x + 1)
            {
                ScoreMap[x] = "";
            }
            solved = false;
            turn = 1;
        }

        //Draw and Populate the Game Grid
        public void DrawGameBoard(string status = "busy") //"busy" "win" or "lose"
        {
            DrawSecretRow(status);

            //Draw the rest of the grid
            for (int x = 1; x <= NumTurns; x = x + 1)
            {
                DrawOneHorizSectionLine();
                for (int y = 1; y <= BlockHeight; y = y + 1)
                {
                    if (y == 1)
                        DrawOneVertSectionLine(x, ScoreMap[x]);
                    else
                        DrawOneVertSectionLine(x);
                }
            }
            DrawOneHorizSectionLine();
        }

        private void DrawOneHorizSectionLine(string addText = "")
        {
            for (int x = 1; x <= CodeLength; x = x + 1)
            {
                Console.Write("+");
                for (int y = 1; y <= BlockHeight * 2; y = y + 1)
                {
                    Console.Write("-");
                }
            }
            Console.WriteLine("+      {0}", addText); //Last Column
        }

        private void DrawOneVertSectionLine(int row, string addText = "")
        {            
            for (int x = 1; x <= CodeLength; x = x + 1)
            {
                Console.Write("|");
                for (int y = 1; y <= BlockHeight * 2; y = y + 1)
                {
                    ChangeBackground(PegMap[row].GetCode()[x-1]);
                    Console.Write(" ");
                    ChangeBackground();
                }
            }
            if ((NumTurns+1-row) == turn && row != 0) Console.WriteLine("|  <--   {0}", addText);
            else Console.WriteLine("|        {0}",addText); //Last Column
        }

        //Draw the Secret Row
        private void DrawSecretRow(string status)
        {
            if (/*!solved && turn <= NumTurns*/ status == "busy")
            {
                DrawOneHorizSectionLine();
                for (int x = 1; x <= BlockHeight; x = x + 1)
                {
                    for (int y = 1; y <= CodeLength; y = y + 1)
                    {
                        Console.Write("|");
                        for (int z = 1; z <= BlockHeight * 2; z = z + 1)
                        {
                            Console.Write("?");
                        }
                    }
                    Console.WriteLine("|");
                }
            }
            else
            {
                if (status == "win" && BlockHeight >= 2)
                {
                    DrawOneHorizSectionLine("          _                      _ | |");              //"          _                      _ | |"
                    for (int y = 1; y <= BlockHeight; y = y + 1)
                    {
                        switch (y)
                        {
                            case 1:
                                DrawOneVertSectionLine(0, "|    | |_ |  |   |`\\ /`\\ |\\ | |_ | |"); // "|    | |_ |  |   |`\\ /`\\ |\\ | |_ | |"
                                break;
                            case 2:
                                DrawOneVertSectionLine(0, " \\/\\/  |_ |_ |_  |_/ \\_/ | \\| |_ o o"); // " \\/\\/  |_ |_ |_  |_/ \\_/ | \\| |_ o o"
                                break;
                            default:
                                DrawOneVertSectionLine(0);
                                break;
                        }
                    }
                }
                else if (status == "win")
                {
                    DrawOneHorizSectionLine();
                    DrawOneVertSectionLine(0, "WELL DONE!!");
                }

                else if (status == "lose")
                {
                    DrawOneHorizSectionLine();
                    for (int y = 1; y <= BlockHeight; y = y + 1)
                    {
                        DrawOneVertSectionLine(0);                      
                    }
                }
            }
            DrawOneHorizSectionLine();
            Console.WriteLine();
        }

        //Change the console background to print the different colour "pegs"
        private void ChangeBackground(int PegColorNumber = 999)
        {
            switch (PegColorNumber)
            {
                case -1:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case 5:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case 6:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case 7:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case 8:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case 9:
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case 0:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }

        //Display Legend (Colour to Number Conversion)
        public void ShowPegColours()
        {
            Console.WriteLine("\nPeg Colour Palette:");
            Console.WriteLine("==================");
            Console.Write("|");
            // Print Peg colours 1 to 9
            for (int x = 1; x <= NumberColours; x = x + 1)
            {
                if (x < 10)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    ChangeBackground(x);
                    Console.Write("  {0}  ", x);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    ChangeBackground();
                    Console.Write("|");
                }
            }
            // Print Peg colour 0 (i.e. 10)
            if (NumberColours == 10)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                ChangeBackground(0);
                Console.Write("  0  ");
                Console.ForegroundColor = ConsoleColor.Gray;
                ChangeBackground();
                Console.Write("|");
            }
            // Print Delete/Blank peg
            Console.ForegroundColor = ConsoleColor.DarkGray;
            ChangeBackground(-1);
            Console.Write(" DEL ");
            Console.ForegroundColor = ConsoleColor.Gray;
            ChangeBackground();
            Console.Write("|");
            Console.WriteLine("\n");
        }

        //GET METHODS

        public bool GetSolved()
        {
            return solved;
        }

        public int GetTurn()
        {
            return turn;
        }

        public int GetNumTurns()
        {
            return NumTurns;
        }

        //END OF GET METHODS


        private void PrintCode(int[] codeBuilder, bool first = false, string message = "")
        {
            int numCharactersMakingUpCode = (codeBuilder.Length - 1) * ((BlockHeight * 2) + 1);

            if (!first)
            {
                // Erase previous message if necessary
                if (messageLength != 0)
                {
                    // Resize window to at least fit all on one line to prevent backspace bug if line wraps
                    if (Console.WindowWidth < numCharactersMakingUpCode + messageLength)
                    {
                        // for the:          '|'+          code             +     message   + some extra
                        Console.WindowWidth = 1 + numCharactersMakingUpCode + messageLength + 2;
                    }

                    for (int x = 1; x <= messageLength; x++)
                    {
                        // https://stackoverflow.com/a/5195807
                        Console.Write("\b \b");
                    }
                }

                // Erase what is currently there up to the first "|"
                for (int x = 1; x <= numCharactersMakingUpCode; x++)
                {
                    // Doesn't overwrite existing chars on backspace, only on forward pass again - reduces flicker
                    Console.Write("\b");
                }
            }

            // Print the new one
            for (int index = 1; index < codeBuilder.Length; index++)
            {
                WriteColourBlock(codeBuilder[index], (codeBuilder[0] == index));
            }

            // Print message
            if (message != "")
            {
                messageLength = 2 + message.Length;
                Console.Write("  " + message);
            }
            else
            {
                messageLength = 0;
            }
        }

        //Writes a colour block to the Console (Does not validate for allowed colour!)
        private void WriteColourBlock(int colRef, bool active = false)
        {
            ChangeBackground(colRef);
            // Change foreground - Prevent black foreground and background
            Console.ForegroundColor = (colRef == -1) ? ConsoleColor.Gray : ConsoleColor.Black;

            // First char of block
            Console.Write((active)? "[" : " ");

            // Middle chars of block
            for (int x = 2; x < BlockHeight * 2; x = x + 1)
            {
                Console.Write(" ");
            }

            // Last char of block
            Console.Write((active) ? "]" : " ");

            // Reset foreground to default
            Console.ForegroundColor = ConsoleColor.Gray;

            ChangeBackground();
            Console.Write("|");
        }


        private void UpdateCodeBuilderColour(int changeToColour, int[] codeBuilder)
        {
            // Validate - check new peg is in number of colour range
            if (changeToColour == -1 ||
               (changeToColour <= NumberColours && changeToColour != 0 && NumberColours < 10) ||
               (changeToColour <= NumberColours && NumberColours == 10))
            {
                // Make the change
                codeBuilder[codeBuilder[0]] = changeToColour;
                // Re-print on screen
                PrintCode(codeBuilder);
            }
            // Else no change
        }

        // INPUT CONTROL
        public void EnterAndSubmitCode()
        {
            // Intialize Code building Manager
            int[] codeBuilder = new int[CodeLength + 1]; // codeBuilder[0] holds the (1-based) index of the currently selected peg
            codeBuilder[0] = 1;
            for (int x = 1; x <= CodeLength; x++)
            {
                codeBuilder[x] = -1;
            }

            // Inital Console Setup
            Console.CursorVisible = false;
            Console.Write("|");

            // Print the initial code for this turn
            messageLength = 0;
            PrintCode(codeBuilder, true);

            // Intialize Warned
            warned = false;
            
            bool repeat = true;
            while (repeat)
            {
                ConsoleKey pressedKey = Console.ReadKey(true).Key;
                
                // Update warned status
                switch (pressedKey)
                {
                    case ConsoleKey.Enter:
                        break;
                    default:
                        warned = false;
                        break;
                }

                // Actual key press logic
                switch (pressedKey)
                {
                    //0-9 Colour entry controls
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        UpdateCodeBuilderColour(0, codeBuilder);
                        break;

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        UpdateCodeBuilderColour(1, codeBuilder);
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        UpdateCodeBuilderColour(2, codeBuilder);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        UpdateCodeBuilderColour(3, codeBuilder);
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        UpdateCodeBuilderColour(4, codeBuilder);
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        UpdateCodeBuilderColour(5, codeBuilder);
                        break;

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        UpdateCodeBuilderColour(6, codeBuilder);
                        break;

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        UpdateCodeBuilderColour(7, codeBuilder);
                        break;

                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        UpdateCodeBuilderColour(8, codeBuilder);
                        break;

                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        UpdateCodeBuilderColour(9, codeBuilder);
                        break;
                    
                    //Left - Move active cell one left (if not at start of the code)
                    case ConsoleKey.LeftArrow:
                        if (codeBuilder[0] > 1) // remember index 1 is left-most code value
                        {
                            codeBuilder[0]--;
                            PrintCode(codeBuilder);
                        }
                        break;

                    //Right - Move active cell one right (if not at end of the code)
                    case ConsoleKey.RightArrow:
                        if (codeBuilder[0] < CodeLength) // remember index CodeLength is right-most code value (1-based)
                        {
                            codeBuilder[0]++;
                            PrintCode(codeBuilder);
                        }
                        break;

                    //Backspace - Delete Colour
                    case ConsoleKey.Backspace:
                    //Delete - Delete currently selected (if not at start of the code)
                    case ConsoleKey.Delete:
                        UpdateCodeBuilderColour(-1, codeBuilder);
                        break;

                    //Enter - Submit Code for marking (if the code length is correct)
                    case ConsoleKey.Enter:
                        // Check if blanks are allowed, if not ensure code is complete (EXTRA FEATURE - TO DO)
                        bool proceed = true;
                        if (!AllowBlanks && !warned)
                        {
                            for (int index = 1; index < codeBuilder.Length; index++)
                            {
                                if (codeBuilder[index] == -1)
                                {
                                    proceed = false;
                                    break;
                                }
                            }
                        }
                        
                        if (proceed)
                        {
                            turn += 1;
                            repeat = false;
                            PegMap[NumTurns + 2 - turn].SubmitCode(codeBuilder);
                            MarkAttempt(codeBuilder);
                        }
                        else
                        {
                            warned = true;
                            PrintCode(codeBuilder, false, "You leaving blanks? <Enter to confirm>");
                        }
                        break;
                }
            }  
        }

        private void MarkAttempt(int[] attemptBuilder)
        {                                                                                                          // ½ 00BD
            //Set up the three strings used in Manipulation          
            string scoreBuilder = "";
            string attempt = "";
            string answer = "";

            // Convert answer [] to a string
            for (int x = 0; x < CodeLength; x = x + 1)
            {
                int val = PegMap[0].GetCode()[x];
                answer += (val == -1) ? "_" : val.ToString();
            }

            // Convert attemptBuilder [] to String
            for (int x = 1; x <= CodeLength; x = x + 1)
            {
                int val = attemptBuilder[x];
                attempt += (val == -1) ? "_" : val.ToString();
            }

            //Initialize counters
            int matches = 0, tally = 0;

            //Score Exact Matches
            for (int x = 0; x < attempt.Length; x = x + 1)
            {
                if (attempt[x].Equals(answer[x]))
                {
                    attempt = attempt.Remove(x, 1);
                    answer = answer.Remove(x, 1);
                    matches += 1;
                    tally += 1;
                    x = x - 1;
                }
            }

            if (matches > 0 && Convert.ToString(matches).Length == 1)
                scoreBuilder = String.Format(" {0} Exact    ", matches);
            else if (matches > 0 && Convert.ToString(matches).Length == 2)
                scoreBuilder = String.Format("{0} Exact    ", matches);
            else
                scoreBuilder = "            ";

            if (tally == CodeLength)
            {
                solved = true;
                ScoreMap[NumTurns + 2 - turn] = scoreBuilder;
                return;
            }

            //Score Correct Colours in Incorrect Places
            matches = 0;
            for (int x = 0; x < attempt.Length; x = x + 1)
            {
                for (int y = 0; y < answer.Length; y = y+1)
                {
                    if (attempt[x].Equals(answer[y]))
                    {
                        attempt = attempt.Remove(x, 1);
                        answer = answer.Remove(y, 1);
                        y = 0;
                        x = -1;
                        matches += 1;
                        tally += 1; 
                        break;
                    }
                }
            }

            if (matches > 0 && Convert.ToString(matches).Length == 1)
                scoreBuilder += String.Format(" {0} Partial    ", matches);
            else if (matches > 0 && Convert.ToString(matches).Length == 2)
                scoreBuilder += String.Format("{0} Partial     ", matches);
            else
                scoreBuilder += "              ";

            //Add Incorrect Pegs to Score
            if (tally < CodeLength)
                if (Convert.ToString(CodeLength-tally).Length == 1) 
                    scoreBuilder += String.Format(" {0} Incorrect", CodeLength - tally);
                else
                    scoreBuilder += String.Format("{0} Incorrect", CodeLength - tally);


            //Submit Final Score
            ScoreMap[NumTurns + 2 - turn] = scoreBuilder;
        }

    }
}
