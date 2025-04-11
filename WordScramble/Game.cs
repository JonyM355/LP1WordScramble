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

        private void StartGame()
        {
            string word = wordProvider.GetRandomWord();

            string scrambledWord = wordProvider.GetScrambledWord(word);

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

            bool isCorrect = string.Equals(
                        userInput,
                        word,
                        StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
            {
                AnsiConsole.MarkupLine("\n[bold green]You Won![/]");
                AnsiConsole.MarkupLine(
                    $"[bold]Time Taken:[/] {timeTaken:F2} Seconds");

                // Shift existing entries
                for (int i = gameStats.Length - 1; i > 0; i--)
                {
                    gameStats[i] = gameStats[i - 1];
                }

                // Add new result at the beginning
                gameStats[0] = new GameResult(word, timeTaken);
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