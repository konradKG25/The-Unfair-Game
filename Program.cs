using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

public static class UnfairGameBase
{
    public static Random Random = new Random();
    public static List<Team> TeamList = new List<Team>();
    public static int HowManyQuestions;
    public static int mathA;
    public static int mathB;
    public static int mathOperation;
    public static int mathAnswer;
    public static int LastTeam;
    public static bool allowedToSeeNumbers;

    public static int Random4DigitNumber()
    {
        return Random.Next(1111, 9999);
    }

    public static int PointsRandom()
    {
        int a = Random.Next(1, 20);
        int b = Random.Next(1, 3);
        if (b != 3)
        {
            return a;
        }
        else if (b == 3)
        {
            int c = a * 2;
            a = a - c;
        }
        return a;
    }

    public static int Random2DigitNumber()
    {
        return Random.Next(11, 99);
    }

    public static int Random2igitNumber()
    {
        return Random.Next(2, 81);
    }

    public static string GetMathQuestionString()
    {
        string operationA = "";
        if (mathOperation == 1)
        {
            operationA = "+";
        }
        else if (mathOperation == 2)
        {
            operationA = "-";
        }
        else if (mathOperation == 3)
        {
            operationA = "x";
        }
        else if (mathOperation == 4)
        {
            operationA = "/";
        }
        return (mathA + operationA + mathB);
    }

    public static void SetupTeams()
    {
        Console.WriteLine("How Many Teams do you want?");
        string? howManyTeamsInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(howManyTeamsInput) || !int.TryParse(howManyTeamsInput, out int howManyTeams))
        {
            Console.WriteLine("Invalid input. Please enter a valid number of teams.");
            SetupTeams();
            return;
        }

        for (int i = 0; i < howManyTeams; i++) TeamList.Add(new Team(TeamList.Count + 1, 0));
        SetupQuestions();
    }

    public static void SetupQuestions()
    {
        Console.WriteLine("How many questions do you want?");
        string? howManyQuestionsInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(howManyQuestionsInput) || !int.TryParse(howManyQuestionsInput, out int howManyQuestions) || howManyQuestions <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid positive number of questions.");
            SetupQuestions();
            return;
        }
        HowManyQuestions = howManyQuestions;

        BeginTheGAME();
    }

    public static void BeginTheGAME()
    {
        AskRandomQuestion();
    }

    public static void AskRandomQuestion()
    {
        int m = Random.Next(1, 4);
        if (m == 1)
        {
            mathA = Random4DigitNumber();
            mathB = Random4DigitNumber();
            mathOperation = 1;
            mathAnswer = mathA + mathB;
        }
        else if (m == 2)
        {
            mathA = Random4DigitNumber();
            mathB = Random4DigitNumber();
            if (mathA < mathB)
            {
                (mathA, mathB) = (mathB, mathA);
            }
            else if (mathA == mathB)
            {
                AskRandomQuestion();
                return;
            }
            mathOperation = 2;
            mathAnswer = mathA - mathB;
        }
        else if (m == 3)
        {
            mathA = Random4DigitNumber();
            mathB = Random2DigitNumber();
            mathOperation = 3;
            mathAnswer = mathA * mathB;
        }
        else if (m == 4)
        {
            mathA = Random4DigitNumber();
            mathB = Random2igitNumber();
            mathOperation = 4;
            mathAnswer = mathA / mathB;
            if (mathAnswer < 0)
            {
                AskRandomQuestion();
            }
            decimal.Truncate(mathAnswer);
        }
        Ask();
    }

    public static void Ask()
    {
        if (LastTeam == TeamList.Count)
        {
            LastTeam = 0;
        }
        LastTeam++;
        Console.Clear();
        Console.WriteLine(GetMathQuestionString() + "for Team " + LastTeam);
        string? userAnswerInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userAnswerInput) || !int.TryParse(userAnswerInput, out int userAnswer))
        {
            Console.WriteLine("Invalid input. Please enter a valid number of questions.");
            SetupQuestions();
            return;
        }

        if (userAnswer == mathAnswer)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Correct!");
            HowManyQuestions--;
            Console.Read();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            WhoToGiveOrKeep();
        }
        else
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("WRONG!");
            HowManyQuestions--;
            Console.Read();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            AskRandomQuestion();
        }
    }

    public static void WhoToGiveOrKeep()
    {
        if (!allowedToSeeNumbers)
        {
            Console.WriteLine("Give or Keep? (g to Give, k to Keep)");
            string? sss = Console.ReadLine();
            if (sss == "g")
            {
                int Points;
                Console.WriteLine("Team" + LastTeam + ", What Team to Give to?");
                string? aaa = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(aaa) || !int.TryParse(aaa, out int aaaa))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Team Number.");
                    WhoToGiveOrKeep();
                    return;
                }
                Points = PointsRandom();
                Console.WriteLine(PointsRandom() + "Points");
                Console.Read();
                Console.ReadLine();
                Team team = TeamList[aaaa - 1];
                if (Points < 0)
                {
                    team.TeamPoints = team.TeamPoints - Points;
                }
                else
                {
                    team.TeamPoints = team.TeamPoints + Points;
                }
            }
            else if (sss == "k")
            {
                int Points;
                Points = PointsRandom();
                Console.WriteLine(PointsRandom() + "Points");
                Console.Read();
                Console.ReadLine();
                Team team = TeamList[LastTeam - 1];
                if (Points < 0)
                {
                    team.TeamPoints = team.TeamPoints - Points;
                }
                else
                {
                    team.TeamPoints = team.TeamPoints + Points;
                }
            }

            if (HowManyQuestions != 0)
            {
                AskRandomQuestion();
            }
            else
            {
                List<int> teamScores = new List<int>();
                foreach (Team team in TeamList)
                {
                    teamScores.Add(team.TeamPoints);
                }

                int winningTeam = 0;
                int highestScore = teamScores[0];

                for (int i = 1; i < teamScores.Count; i++)
                {
                    if (teamScores[i] > highestScore)
                    {
                        highestScore = teamScores[i];
                        winningTeam = i + 1;
                    }
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("TEAM" + winningTeam + "WINS!");
                Console.Read();
            }
        }
        else
        {
            int Points;
            Points = PointsRandom();
            Console.WriteLine(PointsRandom() + "Points");
            Console.Read();
            Console.ReadLine();
            Console.WriteLine("Give or Keep? (g to Give, k to Keep)");
            string? sss = Console.ReadLine();
            if (sss == "g")
            {
                Console.WriteLine("Team" + LastTeam + ", What Team to Give to?");
                string? aaa = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(aaa) || !int.TryParse(aaa, out int aaaa))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Team Number.");
                    WhoToGiveOrKeep();
                    return;
                }
                Console.ReadLine();
                Team team = TeamList[aaaa - 1];
                if (Points < 0)
                {
                    team.TeamPoints = team.TeamPoints - Points;
                }
                else
                {
                    team.TeamPoints = team.TeamPoints + Points;
                }
            }
            else if (sss == "k")
            {
                Team team = TeamList[LastTeam - 1];
                if (Points < 0)
                {
                    team.TeamPoints = team.TeamPoints - Points;
                }
                else
                {
                    team.TeamPoints = team.TeamPoints + Points;
                }
            }

            if (HowManyQuestions != 0)
            {
                AskRandomQuestion();
            }
            else
            {
                List<int> teamScores = new List<int>();
                foreach (Team team in TeamList)
                {
                    teamScores.Add(team.TeamPoints);
                }

                int winningTeam = 0;
                int highestScore = teamScores[0];

                for (int i = 1; i < teamScores.Count; i++)
                {
                    if (teamScores[i] > highestScore)
                    {
                        highestScore = teamScores[i];
                        winningTeam = i + 1;
                    }
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("TEAM" + winningTeam + "WINS!");
                Console.Read();
            }
        }
    }
}

class TheUnfairGame
{
    static void Main()
    {
        string configPath = "game.config";

        if (File.Exists(configPath))
        {
            string[] lines = File.ReadAllLines(configPath);

            foreach (string line in lines)
            {
                if (line.StartsWith("allowedToSeeNumber:"))
                {
                    string value = line.Split(':')[1].Trim();
                    UnfairGameBase.allowedToSeeNumbers = value == "1";
                    break;
                }
            }
        }

        Console.WriteLine("Welcome To The Unfair Game!");
        Console.WriteLine(" ");
        Console.WriteLine("There are teams that you pick how many, then each team gets asked random maths questions. If you get one correct, that team gets to pick whether they want to keep or give the points they are getting (but by default, you can't see the points. but you can change it by going into the 'game.config' file and editing it.) Some points might be positive or negative. This game is for Y5 (school years) and above people. Enjoy :)");

        UnfairGameBase.SetupTeams();
    }
}

public class Team
{
    public int TeamNum;
    public int TeamPoints;
    public Team(int TeamNum, int TeamPoints)
    {
        this.TeamNum = TeamNum;
        this.TeamPoints = TeamPoints;
    }
}