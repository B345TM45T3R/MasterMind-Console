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
        private Code[] PegMap;
        private string[] ScoreMap;
        private bool solved;
        private int turn;

        //Constructor
        public GameManager(int CodeLength = 4, int NumTurns = 10, int BlockHeight = 2, int NumberColours = 8)
        {
            this.CodeLength = CodeLength;
            this.NumTurns = NumTurns;
            this.BlockHeight = BlockHeight;
            if (NumberColours<=10)
                this.NumberColours = NumberColours;
            else
                this.NumberColours = 10;
            PegMap = new Code[NumTurns+1];
            PegMap[0] = new Code(CodeLength, NumberColours);
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
            Console.Write("==================\n|");
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
            if (NumberColours == 10)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                ChangeBackground(0);
                Console.Write("  0  ");
                Console.ForegroundColor = ConsoleColor.Gray;
                ChangeBackground();
                Console.Write("|");
            }
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


        //INPUT CONTROL
        public void EnterAndSubmitCode()
        {
            Console.CursorVisible = true;
            Console.Write("|");
            string codeBuilder = "";
            bool repeat = true;
            while (repeat)
            {
                switch (Console.ReadKey(true).KeyChar)
                {
                    //0-9 Colour entry controls
                    case '0':
                        TypeColours(0, ref codeBuilder);
                        break;

                    case '1':
                        TypeColours(1, ref codeBuilder);
                        break;

                    case '2':
                        TypeColours(2, ref codeBuilder);
                        break;
                    case '3':
                        TypeColours(3, ref codeBuilder);
                        break;

                    case '4':
                        TypeColours(4, ref codeBuilder);
                        break;

                    case '5':
                        TypeColours(5, ref codeBuilder);
                        break;

                    case '6':
                        TypeColours(6, ref codeBuilder);
                        break;

                    case '7':
                        TypeColours(7, ref codeBuilder);
                        break;

                    case '8':
                        TypeColours(8, ref codeBuilder);
                        break;

                    case '9':
                        TypeColours(9, ref codeBuilder);
                        break;

                    //Backspace - Delete Colour
                    case (char)ConsoleKey.Backspace:
                        if (codeBuilder.Length !=0)
                        for (int x = 1; x <= 3; x = x + 1)
                        {
                            for (int y = 1; y <= BlockHeight * 2 + 1; y = y + 1)
                            {
                                if (x == 2) Console.Write(" ");
                                else Console.Write("\b");
                            }
                        }
                        if(codeBuilder.Length >= 1) codeBuilder = codeBuilder.Remove(codeBuilder.Length-1,1);
                        break;

                    //Enter - Submit Code for marking (if the code length is correct)
                    case (char)ConsoleKey.Enter:
                        if (codeBuilder.Length == CodeLength)
                        {
                            turn += 1;
                            repeat = false;
                            PegMap[NumTurns + 2 - turn].SubmitCode(codeBuilder);
                            MarkAttempt(codeBuilder);
                        }
                        break;
                }
            }  
        }

        private void MarkAttempt(string attempt)
        {
                                                                                                          // ½ 00BD
            //Set up the three strings used in Manipulation (string attempt is supplied as parameter)          
            string scoreBuilder = "";
            string answer = "";
            for (int x = 0; x < CodeLength; x = x + 1)
            {
                answer += PegMap[0].GetCode()[x];
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

        //Validating and printing Colour key presses
        private void TypeColours(int colRef, ref string codeBuilder)
        {
            if (codeBuilder.Length < CodeLength && ((NumberColours >= colRef && (colRef != 0 && NumberColours < 10)) || (NumberColours >= colRef && NumberColours == 10)))
            {
                ChangeBackground(colRef);
                for (int x = 1; x <= BlockHeight*2; x = x + 1)
                    Console.Write(" ");
                codeBuilder += colRef;
                ChangeBackground();
                Console.Write("|");
            }
        }

    }
}
