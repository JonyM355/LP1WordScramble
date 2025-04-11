using System;
using System.Collections.Generic;
using System.Diagnostics;
using Spectre.Console;

namespace WordScramble
{
    public class Game
    {
        private readonly WordProvider wordProvider;
        private readonly GameResult[] gameStats;

        public Game()
        {
            wordProvider = new WordProvider();
            gameStats = new GameResult[5];
        }


        public void ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Word Scramble[/]")
                        .AddChoices("Start Game", "View Game Stats", "Quit"));

                switch (choice)
                {
                    case "Start Game":
                        StartGame();
                        break;
                    case "View Game Stats":
                        ShowGameStats();
                        break;
                    case "Quit":
                        return;
                }
            }
        }

        /// <summary>
        /// Starts a new game round where the player must unscramble a word.
        /// The game measures the time taken by the player to guess the word.
        /// </summary>
        /// <remarks>
        /// In this method, a random word is generated using the 
        /// <see cref="WordProvider"/> and scrambled. The player is asked to 
        /// guess the unscrambled word. The game checks if the player's guess 
        /// is correct. If correct, the result is stored in the game stats 
        /// board; otherwise, the correct word is displayed. The game stats 
        /// board only stores the last 5 results.
        /// </remarks>
        private void StartGame()
        {
            /// <summary>
            /// The randomly chosen word for the current round.
            /// </summary>
            string word = wordProvider.GetRandomWord();// ////////// => TO IMPLEMENT <= //////////// //

            /// <summary>
            /// The scrambled version of the word.
            /// </summary>
            string scrambledWord = wordProvider.GetScrambledWord(word);// ////////// => TO IMPLEMENT <= //////////// //

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold green]Unscramble the word:[/]");
            AnsiConsole.MarkupLine($"[italic yellow]{scrambledWord}[/]");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            /// <summary>
            /// The player's guess for the unscrambled word.
            /// </summary>
            string userInput = AnsiConsole.Ask<string>(
                "\n[bold cyan]Your Guess (type the word):[/] ");

            stopwatch.Stop();
            double timeTaken = stopwatch.Elapsed.TotalSeconds;

            /// <summary>
            /// Checks if the player's guess is correct.
            /// </summary>
            bool isCorrect = // ////////// => TO IMPLEMENT <= //////////// //

            if (isCorrect)
            {
                AnsiConsole.MarkupLine("\n[bold green]You Won![/]");
                AnsiConsole.MarkupLine(
                    $"[bold]Time Taken:[/] {timeTaken:F2} Seconds");

                // Shift existing entries
                for (int i = gameStats.Length - 1; i > 0; i--)
                {
                    // ////////// => TO IMPLEMENT <= //////////// //
                    gameStats[i] = gameStats[i - 1];
                }

                // Add new result at the beginning
                gameStats[0] = new GameResult();// ////////// => TO IMPLEMENT <= //////////// //
            }
            else
            {
                AnsiConsole.MarkupLine("\n[bold red]You Lost![/]");
                AnsiConsole.MarkupLine($"[bold]Correct Word:[/] {word}");
                AnsiConsole.MarkupLine(
                    $"[bold]Time Taken:[/] {timeTaken:F2} Seconds");
            }

            AnsiConsole.Markup(
                "\n[bold green]Press Enter to Return to the Menu...[/]");
            Console.ReadLine();
        }


        private void ShowGameStats()
        {
            AnsiConsole.Clear();
            Table table = new Table();
            table.AddColumn("#");
            table.AddColumn("Word");
            table.AddColumn("Time Taken (s)");

            for (int i = 0; i < gameStats.Length; i++)
            {
                if (gameStats[i] == null)
                {
                    table.AddRow(
                        (i + 1).ToString(),
                        "N/A",
                        "0.00"
                    );
                }
                
                table.AddRow(
                    (i + 1).ToString(),
                    gameStats[i].Word.ToString(),
                    gameStats[i].TimeTaken.ToString("F2")
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.Markup(
                "\n[bold green]Press Enter to Return to Menu...[/]");
            Console.ReadLine();
        }
    }
}