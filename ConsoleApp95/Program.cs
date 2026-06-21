using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp2
{
    internal class Program
    {
        static string currentUser = "";
        static int points;
        static Random rng = new Random();
        static Dictionary<string, (string type, string definition, string example)> wordDictionary;

        static void Main(string[] args)
        {
            wordDictionary = DictionaryInput();
            mainmenu();
            Console.ReadKey();
        }
        static void mainmenu()
        {
            bool try2 = false;
            while (!try2)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine("        Welcome to FILIPI-HULA         ");
                Console.WriteLine("=====================================");
                Console.WriteLine("[1].Login");
                Console.WriteLine("[2].Register");
                Console.WriteLine("[3].Exit");
                Console.Write("choose an option: ");
                string output = Console.ReadLine();
                switch (output)
                {
                    case "1":
                        login();
                        try2 = true;
                        break;
                    case "2":
                        regist();
                        try2 = true;
                        break;
                    case "3":
                        Console.WriteLine("Thank you for Playing!");
                        try2 = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Option pls enter either 1, 2 or 3");
                        try2 = false;
                        break;
                }
            }
        }

        static void login()
        {
            Console.Write("Enter Username: ");
            string usern = Console.ReadLine();
            Console.Write("Enter Password: ");
            string pass = Console.ReadLine();

            if (usern == "" || pass == "")
            {
                Console.Clear();
                Console.WriteLine("Username and Password cannot be empty");
                return;
            }

            if (!File.Exists("login.txt"))
            {
                Console.Clear();
                Console.WriteLine("No accounts found. Please register first.");
                return;
            }

            string[] lines = File.ReadAllLines("login.txt");
            bool userExists = lines.Any(line => line.Split(',')[0] == usern);

            if (!userExists)
            {
                Console.Clear();
                Console.WriteLine("No accounts found. Please register first.");
                mainmenu();
            }
            // to display points on top next to the username 
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 2 && parts[0] == usern && parts[1] == pass)
                {
                    Console.Clear();
                    currentUser = usern;

                    // to Load users points if they have to the leaderboard 
                    points = 0; // reset first
                    if (File.Exists("Leaderboard.txt"))
                    {
                        foreach (string lbLine in File.ReadAllLines("Leaderboard.txt"))
                        {
                            string[] lbParts = lbLine.Split(',');
                            if (lbParts.Length >= 2 && lbParts[0] == currentUser)
                            {
                                int.TryParse(lbParts[1], out points);
                                break;
                            }
                        }
                        Console.WriteLine("Login successful! Welcome, " + usern + "!");
                        gamemenu(wordDictionary);
                        return;
                    }
                }
            }

            Console.WriteLine("Invalid username or password. Try again.");
            return;
        }
        static void regist()
        {
            bool regis = false;
            Console.Clear();
            while (!regis)
            {
                Console.WriteLine("========Account Registration=====");
                Console.Write("Enter new username: ");
                string newuser = Console.ReadLine();
                Console.Write("Enter new password: ");
                string newpass = Console.ReadLine();
                Console.Write("Confirm Password: ");
                string confpass = Console.ReadLine();
                Console.WriteLine("===================================");
                if (newuser.Trim() == "" || newpass.Trim() == "" || confpass.Trim() == "")
                {
                    Console.Clear();
                    Console.WriteLine("Username and Password cannot be empty!");
                    continue;
                }
                if (confpass != newpass)
                {
                    Console.Clear();
                    Console.WriteLine("Password does not match!");
                    continue;
                }
                if (File.Exists("login.txt") && File.ReadAllText("login.txt").Contains(newuser + ","))
                {
                    Console.Clear();
                    Console.WriteLine("Account already exists!");
                    continue;
                }
                else
                {
                    string[] newaccount = { newuser + "," + confpass };
                    File.AppendAllLines("login.txt", newaccount);
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(500);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Loading....");
                        Console.ResetColor();
                        Console.Clear();
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Registration Successfull!");
                    Console.ResetColor();
                    mainmenu();
                    break;
                }
            }
        }
        static void gamemenu(Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            Console.Clear();
            bool try3 = false;
            while (!try3)
            {
                Console.WriteLine("Playing as: " + currentUser + " " + "Points: " + " " + points);
                Console.WriteLine("=====================================");
                Console.WriteLine("        Welcome to FILIPI-HULA         ");
                Console.WriteLine("=====================================");
                Console.WriteLine("Choose your gamemode");
                Console.WriteLine("[1]. Hangman");
                Console.WriteLine("[2]. Reading Comprehension");
                Console.WriteLine("[3]. Leaderboard");
                Console.WriteLine("[4]. Dictionary");
                Console.WriteLine("[5]. Go back");
                Console.Write("Enter choice: ");
                string output2 = Console.ReadLine();
                switch (output2)
                {
                    case "1":
                        HangmanMenu(currentUser,wordDictionary);
                        try3 = true;
                        break;
                    case "2":
                        Readingcomp();
                        try3 = true;
                        break;
                    case "3":
                        Leaderboard();
                        break;
                    case "4":
                        DictionaryView(wordDictionary);
                        break;
                    case "5":
                        currentUser = "";
                        try3 = true;
                        Console.Clear();
                        mainmenu();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Option pls enter either 1, 2, 3, 4 or 5");
                        try3 = false;
                        break;
                }
            }
        }
        static void Leaderboard()
        {
            string[] leaderboard = File.ReadAllLines("Leaderboard.txt");

            // Bubble sort to sort from highest to lowest
            for (int i = 0; i < leaderboard.Length - 1; i++)
            {
                for (int j = 0; j < leaderboard.Length - 1 - i; j++)
                {
                    if (leaderboard[j] == "" || leaderboard[j + 1] == "") continue;

                    string[] partsA = leaderboard[j].Split(',');
                    string[] partsB = leaderboard[j + 1].Split(',');

                    int pointsA = int.Parse(partsA[1]);
                    int pointsB = int.Parse(partsB[1]);
                    // Swap if current is less than next
                    if (pointsA < pointsB)
                    {
                        string temp = leaderboard[j];
                        leaderboard[j] = leaderboard[j + 1];
                        leaderboard[j + 1] = temp;
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("=================================================");
            Console.WriteLine("                    Leaderboard                  ");
            Console.WriteLine("=================================================");
            Console.WriteLine(String.Format("{0,-5}{1,-20}{2,-10}", "#", "USER", "POINTS"));
            Console.WriteLine("-------------------------------------------------");
            int rank = 1;
            for (int i = 0; i < leaderboard.Length; i++)
            {
                if (leaderboard[i] != "")
                {
                    string[] parts = leaderboard[i].Split(',');
                    Console.WriteLine(String.Format("{0,-5}{1,-20}{2,-10}", rank, parts[0], parts[1]));
                    rank++;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
            gamemenu(wordDictionary);
        }
        static void DrawHangman(int stage)
        {

            Console.WriteLine(" +---+");
            Console.WriteLine(" |   |");
            Console.WriteLine($" |   {(stage >= 1 ? "O" : " ")}"); // If stage is 1 or more -> print "O", if not print blank
            Console.WriteLine($" |  {(stage >= 3 ? "/" : " ")}{(stage >= 2 ? "|" : " ")}{(stage >= 4 ? "\\" : " ")}");
            Console.WriteLine($" |  {(stage >= 5 ? "/" : " ")} {(stage >= 6 ? "\\" : " ")}");
            Console.WriteLine(" |");
            Console.WriteLine("_|_");
        }

        static void HangmanMenu(string currentUser,Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {

            bool exithangmanmenu = false;
            //CALLL OUT POINT HERE

            while (!exithangmanmenu)
            {

                for (int wrongGuesses = 1; wrongGuesses <= 6; wrongGuesses++)
                {
                    Console.Clear();
                    Console.Write("Playing as: " + currentUser); Console.Write("        Points: " + points);
                    Console.WriteLine("\n=====================================");
                    Console.WriteLine("        Hangman Game         ");
                    Console.WriteLine("=====================================");

                    DrawHangman(wrongGuesses);
                    Thread.Sleep(500);
                }
                Console.WriteLine("Choose your hangman mode:");
                Console.WriteLine("[1] Easy");
                Console.WriteLine("[2] Normal");
                Console.WriteLine("[3] Hard");
                Console.WriteLine("[0] Go Back to Main Menu");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        HangmanEasy(currentUser,wordDictionary);
                        break;
                    case "2":
                        Console.Clear();
                        HangmanNormal(currentUser, wordDictionary);
                        break;
                    case "3":
                        Console.Clear();
                        HangmanHard(currentUser, wordDictionary);
                        break;
                    case "0":
                        Console.Clear();
                        gamemenu(wordDictionary);
                        exithangmanmenu = true;
                        break;
                    default:
                        Console.WriteLine(" [Please enter a correct option.]");
                        break;
                }
            }
        }
        static void LoadingScreen()
        {
            for (int wrongGuesses = 1; wrongGuesses <= 6; wrongGuesses++)
            {
                Console.Clear();


                DrawHangman(wrongGuesses);
                Console.WriteLine();
                Console.WriteLine("[Loading a new game....]");
                if (wrongGuesses == 1)
                {
                    Console.WriteLine("|| 10%");
                }
                else if (wrongGuesses == 3)
                {
                    Console.WriteLine("||||||| 30%");
                }
                else if (wrongGuesses == 4)
                {
                    Console.WriteLine("|||||||||||||||| 50%");
                }
                else if (wrongGuesses == 6)
                {
                    Console.WriteLine("|||||||||||||||||||||||||||||||| 100%");
                }
                Thread.Sleep(500);
            }
        }
        static Dictionary<string, (string type, string definition, string example)> DictionaryInput()
        {
            string[] words = File.ReadAllLines("Dictionaries.txt");

            var wordDictionary = new Dictionary<string, (string type, string definition, string example)>();

            for (int w = 0; w < words.Length; w++)
            {
                string[] wordcategory = words[w].Split(',');

                if (wordcategory.Length >= 4) //Checks if the amt is enough so it wouldnt cause error
                {
                    wordDictionary.Add(wordcategory[0], (wordcategory[1], wordcategory[2], wordcategory[3]));
                }
            }
            return wordDictionary;
        }
        static (string, string) WordRandomizer(Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            Random random = new Random();

            int randomIndex = random.Next(wordDictionary.Count);

            var randomEntry = wordDictionary.ElementAt(randomIndex);

            return (randomEntry.Key, randomEntry.Value.definition);
        }
        static void HangmanEasy(string currentUser, Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            var (hangmanword, hangmandefinition) = WordRandomizer(wordDictionary);
            List<char> CorrectLetters = new List<char>();
            List<char> WrongLetters = new List<char>();
            int wrongGuesses = 0;
            bool playing = true;

            LoadingScreen();

            while (playing)
            {
                Console.Clear();
                Console.Write("Playing as: " + currentUser); Console.Write("        Points: " + points);
                Console.WriteLine("\n=====================================");
                Console.WriteLine("        HANGMAN EASY MODE       ");
                Console.WriteLine("=====================================");
                Console.WriteLine("> Are you ready to play?");
                Console.WriteLine("> Guess the filipino word in the blanks below!");
                DrawHangman(wrongGuesses);
                Console.WriteLine();
                foreach (char c in hangmanword)
                {
                    if (CorrectLetters.Contains(c))
                    {
                        Console.Write(c);
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }

                Console.WriteLine("\n\nMeaning: " + hangmandefinition);

                if (WrongLetters != null)
                {
                    Console.Write("Wrong guesses: ");
                    foreach (char w in WrongLetters)
                    {
                        Console.Write(w);
                    }
                }

                Console.Write("\nGuess a letter: ");
                string input = Console.ReadLine().ToLower();

                if (input.Length != 1) //it needs to be char
                {
                    continue;
                }

                char letter = input[0];

                if (CorrectLetters.Contains(letter) || WrongLetters.Contains(letter)) //it needs to be not on the list
                {
                    continue;
                }

                if (hangmanword.Contains(letter))
                {
                    CorrectLetters.Add(letter);
                }
                else
                {
                    WrongLetters.Add(letter);
                    wrongGuesses++;
                }


                // Check win
                bool won = true;
                foreach (char c in hangmanword)
                {
                    if (!CorrectLetters.Contains(c))
                    {
                        won = false;
                        break;
                    }
                }

                if (won)
                {
                    Console.WriteLine("\nYou Win! + 5 Points!");
                    Console.WriteLine("The word was: " + hangmanword);
                    points += 5;
                    calcu();
                    playing = false;
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return;
                }

                if (wrongGuesses >= 6)
                {
                    Console.WriteLine("\nYou Lose! - 2 Points.");
                    Console.WriteLine("The word was: " + hangmanword);
                    calcu();
                    points -= 2;
                    playing = false;
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return;
                }
            }
            return;
        }


        static void HangmanNormal(string currentUser, Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            var (hangmanword, hangmandefinition) = WordRandomizer(wordDictionary);
            List<char> CorrectLetters = new List<char>();
            List<char> WrongLetters = new List<char>();
            int wrongGuesses = 0;
            bool playing = true;
            bool definitionAsked = false;
            bool definitionUnlocked = false;
            LoadingScreen();
            while (playing)
            {

                Console.Clear();
                Console.Write("Playing as: " + currentUser); Console.Write("        Points: " + points);
                Console.WriteLine("\n=====================================");
                Console.WriteLine("        HANGMAN NORMAL MODE       ");
                Console.WriteLine("=====================================");
                Console.WriteLine("> Are you ready to play?");
                Console.WriteLine("> Guess the filipino word in the blanks below!");
                DrawHangman(wrongGuesses);
                foreach (char c in hangmanword)
                {
                    if (CorrectLetters.Contains(c))
                    {
                        Console.Write(c);
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }

                if (!definitionAsked)
                {
                    Console.WriteLine("\nDo you want to unlock the definition? Doing this action will result in building your stickman by 1. Y/N");
                    string unlock = Console.ReadLine().ToUpper();

                    if (unlock == "Y")
                    {
                        Console.WriteLine("\nMeaning: " + hangmandefinition);
                        wrongGuesses++;
                        definitionUnlocked = true;
                    }

                    definitionAsked = true;
                    continue;
                }

                if (definitionUnlocked)
                {
                    Console.WriteLine("\nMeaning: " + hangmandefinition);
                }



                if (WrongLetters != null)
                {
                    Console.Write("\n\nWrong guesses: ");
                    foreach (char w in WrongLetters)
                    {
                        Console.Write(w);
                    }
                }

                Console.Write("\nGuess a letter: ");
                string input = Console.ReadLine().ToLower();

                if (input.Length != 1) //it needs to be char
                {
                    continue;
                }

                char letter = input[0];

                if (CorrectLetters.Contains(letter) || WrongLetters.Contains(letter)) //it needs to be not on the list
                {
                    continue;
                }

                if (hangmanword.Contains(letter))
                {
                    CorrectLetters.Add(letter);
                }
                else
                {
                    WrongLetters.Add(letter);
                    wrongGuesses++;
                }
                // Check win
                bool won = true;
                foreach (char c in hangmanword)
                {
                    if (!CorrectLetters.Contains(c))
                    {
                        won = false;
                        break;
                    }
                }
                if (won)
                {
                    Console.WriteLine("\nYou Win!");
                    Console.WriteLine("The word was: " + hangmanword);
                    points += 10;
                    playing = false;
                    calcu();
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return;
                }
                if (wrongGuesses >= 6)
                {
                    Console.WriteLine("\nYou Lose!");
                    Console.WriteLine("The word was: " + hangmanword);
                    points -= 5;
                    playing = false;
                    calcu();
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return;
                }
            }
            return;
        }
        static void HangmanHard(string currentUser, Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            var (hangmanword, hangmandefinition) = WordRandomizer(wordDictionary);
            List<char> CorrectLetters = new List<char>();
            List<char> WrongLetters = new List<char>();
            int wrongGuesses = 0;
            bool playing = true;
            LoadingScreen();
            while (playing)
            {
                Console.Clear();
                Console.Write("Playing as: " + currentUser); Console.Write("        Points: " + points);
                Console.WriteLine("\n=====================================");
                Console.WriteLine("        HANGMAN HARD MODE       ");
                Console.WriteLine("=====================================");
                Console.WriteLine("> Are you ready to play?");
                Console.WriteLine("> Guess the filipino word in the blanks below!");
                DrawHangman(wrongGuesses);
                foreach (char c in hangmanword)
                {
                    if (CorrectLetters.Contains(c))
                    {
                        Console.Write(c);
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }

                if (WrongLetters != null)
                {
                    Console.Write("\nWrong guesses: ");
                    foreach (char w in WrongLetters)
                    {
                        Console.Write(w);
                    }
                }
                Console.WriteLine();
                Console.Write("\nGuess a letter: ");
                string input = Console.ReadLine().ToLower();

                if (input.Length != 1) //it needs to be char
                {
                    continue;
                }

                char letter = input[0];

                if (CorrectLetters.Contains(letter) || WrongLetters.Contains(letter)) //it needs to be not on the list
                {
                    continue;
                }

                if (hangmanword.Contains(letter))
                {
                    CorrectLetters.Add(letter);
                }
                else
                {
                    WrongLetters.Add(letter);
                    wrongGuesses++;
                }


                // Check win
                bool won = true;
                foreach (char c in hangmanword)
                {
                    if (!CorrectLetters.Contains(c))
                    {
                        won = false;
                        break;
                    }
                }

                if (won)
                {
                    Console.WriteLine("\nYou Win!");
                    Console.WriteLine("The word was: " + hangmanword);
                    points += 20;
                    playing = false;
                    calcu();
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return;
                }

                if (wrongGuesses >= 6)
                {
                    Console.WriteLine("\nYou Lose!");
                    Console.WriteLine("The word was: " + hangmanword);
                    points -= 8;
                    playing = false;
                    calcu();
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return;
                }
            }
            return;
        }
        static void Readingcomp()
        {
            Console.Clear();
            bool try4 = false;
            while (!try4)
            {
                Console.WriteLine("==============Reading comprehension===========");
                Console.WriteLine("Choose your Level of Difficulty");
                Console.WriteLine("[1]. Easy Level");
                Console.WriteLine("[2]. Medium Level");
                Console.WriteLine("[3]. Hard Level");
                Console.WriteLine("[4]. Exit");
                Console.WriteLine("===============================================");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine().ToUpper();
                switch (choice)
                {
                    case "1":
                        easy();
                        try4 = true;
                        break;
                    case "2":
                        medium();
                        try4 = true;
                        break;
                    case "3":
                        hard();
                        try4 = true;
                        break;
                    case "4":
                        try4 = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Option pls enter either 1, 2, 3 or 4");
                        try4 = false;
                        break;
                }
            }
        }
        // It returns a List of stories, where each story has a Title, Text, and List of Questions
        static List<(string Title, string Text, List<(string Prompt, string[] Choices, char Answer)> Questions)>
            LoadStories(string filePath) // is basically our easy, normal and hard txt file so whatever filename we pass we call either easy, normal or hard
        {
            // create an empty list that holds the story
            var stories = new List<(string, string, List<(string, string[], char)>)>();
            // checks if the file exists if not it will display an empty list
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return stories; // stops the method and returns the empty list 
            }
            // reads the text file 
            string[] lines = File.ReadAllLines(filePath);
            // temporary variables that hold the current stories data 
            string currentTitle = ""; // holds the title 
            List<string> storyLines = new List<string>();// holds each line of the story
            List<(string, string[], char)> currentQuestions = null; // holds the current question of the story
            string currentPrompt = ""; //holds the current questions 
            string[] currentChoices = new string[4];// holds A B C D choices for current question
            char currentAnswer = ' ';// holds correct answer 
            bool readingQuestions = false;// its a flag to for the program to know if we are in the question yet
            bool inStory = false;// a flag to know if the program has started reading it

            // goes through the file, one file at a time
            foreach (string line in lines)
            {
                // the indicator that a new story is starting
                if (line.Trim() == "---STORY---")// trim to remove extra spaces
                {
                    // If we were already reading a story, save it before starting the new one
                    if (inStory)
                    {
                        // saves the last question of the previous story
                        if (currentPrompt != "")
                            // adds the completed story to our list
                            currentQuestions.Add((currentPrompt, currentChoices, currentAnswer));
                        stories.Add((currentTitle, string.Join("\n", storyLines).Trim(), currentQuestions));
                        // string.Join("\n", storyLines) combines all story lines back into one big text
                    }
                    // to reset everything to start a new story
                    currentTitle = "";
                    storyLines = new List<string>();
                    currentQuestions = new List<(string, string[], char)>();
                    currentPrompt = "";
                    currentChoices = new string[4];
                    currentAnswer = ' ';
                    readingQuestions = false;// back to reading story text and not questions
                    inStory = true;// we are now inside the story
                }
                else if (line.Trim() == "---QUESTIONS---")
                {
                    readingQuestions = true;  // flips the flag so we know to read questions now
                }
                // If line starts with "Title:" and we're not in questions section yet
                // grab everything after "Title:" as the title
                // e.g. "Title: Ang Matsing" → currentTitle = "Ang Matsing"
                else if (line.StartsWith("Title:") && !readingQuestions)
                {
                    currentTitle = line.Substring(6).Trim(); // to skip the word "Title"
                }
                // If we're NOT in questions section and we're inside a story
                // and it's not a Title line, it must be story text so add it to storyLines
                else if (!readingQuestions && inStory && !line.StartsWith("Title:"))
                {
                    storyLines.Add(line);// adds this line to the story text
                }
                // If we ARE in questions section and line starts with "Q:"
                // it means a new question is starting
                else if (readingQuestions && line.StartsWith("Q:"))
                {
                    // Save the previous question first (if there was one)
                    if (currentPrompt != "")
                        currentQuestions.Add((currentPrompt, currentChoices, currentAnswer));
                    // Start the new question
                    currentPrompt = line.Substring(2).Trim();// skips "Q:" (2 characters)
                    currentChoices = new string[4];// reset choices for new question
                    currentAnswer = ' '; // reset answer for new question
                }
                // If line length > 1 and second character is "." it means it's a choice line
                // e.g. "A. Maglaro" → line[0]='A', line[1]='.'
                else if (readingQuestions && line.Length > 1 && line[1] == '.')
                {
                    int index = line[0] - 'A';
                    if (index >= 0 && index < 4)
                        currentChoices[index] = line.Trim();
                    // e.g. "A. Maglaro" goes to currentChoices[0]
                    //      "B. Lumangoy" goes to currentChoices[1]
                }
                // If line starts with "ANS:" grab the correct answer letter
                // e.g. "ANS: B" → currentAnswer = 'B'
                else if (readingQuestions && line.StartsWith("ANS:"))
                {
                    currentAnswer = line.Substring(4).Trim()[0];  // Substring(4) skips "ANS:" then [0] grabs just the first character e.g. 'B'
                }
            }
            // Saves last story
            if (inStory)
            {
                if (currentPrompt != "")
                    currentQuestions.Add((currentPrompt, currentChoices, currentAnswer));
                stories.Add((currentTitle, string.Join("\n", storyLines).Trim(), currentQuestions));
            }

            return stories;
        }
        //  DISPLAY 
        static void DisplayStory(string title, string text, List<(string Prompt, string[] Choices, char Answer)> questions, int pointsIfPass, int pointsifNot)
        {

            bool goBackToStory = false;

            do
            {
                goBackToStory = false;
                int score = 0;

                foreach (var q in questions)
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║           " + title.PadRight(45) + "║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n" + text + "\n");
                    Console.ResetColor();
                    Console.WriteLine("--------------------------------------------");
                    // Question below the story
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n" + q.Prompt);
                    Console.ResetColor();
                    foreach (var choice in q.Choices)
                        Console.WriteLine(choice);
                    Console.Write("\nYour answer: ");
                    string input = Console.ReadLine().Trim().ToUpper();
                    if (input == "BACK")
                    {
                        goBackToStory = true;
                        break;
                    }
                    if (input.Length > 0 && input[0] == q.Answer)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Correct!");
                        Console.ResetColor();
                        points += pointsIfPass;
                        score++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        points -= pointsifNot;
                        Console.WriteLine($"Wrong! The correct answer is {q.Answer}.");
                        Console.ResetColor();
                    }
                    Console.ReadKey();
                }
                if (!goBackToStory)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nYou got {score}/{questions.Count} correct!");
                    Console.WriteLine("Total Points: " + points);
                    Console.ResetColor();
                    calcu();
                    gamemenu(wordDictionary);
                    break;
                }

            } while (goBackToStory);
        }
        static void DictionaryView(Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            int amt = 0;
            Console.WriteLine("\n=====================================");
            Console.WriteLine("        Dictionary        ");
            Console.WriteLine("=====================================");

            foreach (var entry in wordDictionary.OrderBy(x => x.Key))
            {
                Console.WriteLine("\nWord: " + entry.Key);
                Console.WriteLine("Type: " + entry.Value.type);
                Console.WriteLine("Definition: " + entry.Value.definition);
                Console.WriteLine("Example: " + entry.Value.example);
                Console.WriteLine("-------------------------------------");
            }

            Console.WriteLine("\nPress any key to go back to main menu...");
            Console.ReadKey();
        }
        // point calculation
        static void calcu()
        {
            string file = "Leaderboard.txt";
            string[] lines = File.Exists(file) ? File.ReadAllLines(file) : new string[0]; // this is basically just a shortened version of if else 
            bool userfound = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (currentUser == parts[0])
                {
                    int existingpoints = int.Parse(parts[1]);

                    if (parts.Length >= 2 && currentUser == parts[0])
                    {
                        lines[i] = currentUser + "," + points;
                    }
                    userfound = true;
                    break;
                }
            }
            if (!userfound)
            {
                File.AppendAllText(file, currentUser + "," + points + "\n");
            }
            if (userfound)
            {
                File.WriteAllLines(file, lines);
            }
        }

        //  LEVELS 
        static void easy()
        {
            var stories = LoadStories("easy1.txt");
            if (stories.Count == 0) return;
            var picked = stories[rng.Next(stories.Count)];
            DisplayStory(picked.Title, picked.Text, picked.Questions, 10, 4);
        }
        static void medium()
        {
            var stories = LoadStories("medium.txt");
            if (stories.Count == 0) return;
            var picked = stories[rng.Next(stories.Count)];
            DisplayStory(picked.Title, picked.Text, picked.Questions, 20, 8);
        }
        static void hard()
        {
            var stories = LoadStories("hard.txt");
            if (stories.Count == 0) return;
            var picked = stories[rng.Next(stories.Count)];
            DisplayStory(picked.Title, picked.Text, picked.Questions, 40, 12);
        }
    }
}
