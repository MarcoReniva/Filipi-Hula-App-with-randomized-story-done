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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.WriteLine(@"  ║    ___ ___ _    ___ ___ ___       _  _ _   _ _      _       ║");
                Console.WriteLine(@"  ║   | __|_ _| |  |_ _| _ \_ _|___  | || | | | | |    /_\      ║");
                Console.WriteLine(@"  ║   | _| | || |__ | ||  _/| |____| | __ | |_| | |__ / _ \     ║");
                Console.WriteLine(@"  ║   |_| |___|____|___|_| |___|     |_||_|\___/|____/_/ \_\    ║");
                Console.WriteLine(@"  ╠════════════╦════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║  +======+  ║                                                ║");
                Console.WriteLine(@"  ║  |      |  ║                                                ║");
                Console.WriteLine(@"  ║  O      |  ║             [ 1 ] > START / LOGIN              ║");
                Console.WriteLine(@"  ║ /|\     |  ║             [ 2 ] > CREATE ACCOUNT             ║");
                Console.WriteLine(@"  ║ / \     |  ║             [ 3 ] > EXIT GAME                  ║");
                Console.WriteLine(@"  ║         |  ║                                                ║");
                Console.WriteLine(@"  ║         |  ║                                                ║");
                Console.WriteLine(@"  ║ ===========║                                                ║");
                Console.WriteLine(@"  ╚════════════╩════════════════════════════════════════════════╝");
                Console.WriteLine();

                // --- INPUT PROMPT ---
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    Enter your command (1-3) : ");
                Console.ResetColor();

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
                        Console.WriteLine("Invalid Option please enter either 1, 2 or 3");
                        try2 = false;
                        break;
                }
            }
        }

        static void login()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine(@"  ║                       [ SYSTEM AUTHENTICATION ]                     ║");
            Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine(@"  ║                                                                     ║");
            Console.WriteLine(@"  ║             .--------.                                              ║");
            Console.WriteLine(@"  ║            /  ____  \                                               ║");
            Console.WriteLine(@"  ║           |  /    \  |                                              ║");
            Console.WriteLine(@"  ║           | |      | |     AWAITING USER CREDENTIALS...             ║");
            Console.WriteLine(@"  ║          _| |______| |_                                             ║");
            Console.WriteLine(@"  ║         /              \   ====================================     ║");
            Console.WriteLine(@"  ║        |  .----------.  |                                           ║");
            Console.WriteLine(@"  ║        |  |   ____   |  |  * UNAUTHORIZED ACCESS PROHIBITED *       ║");
            Console.WriteLine(@"  ║        |  |   |  |   |  |                                           ║");
            Console.WriteLine(@"  ║        |  |   |  O   |  |  ====================================     ║");
            Console.WriteLine(@"  ║        |  |   | /|\  |  |                                           ║");
            Console.WriteLine(@"  ║        |  |   | / \  |  |  PLEASE ENTER YOUR LOGIN DATA BELOW.      ║");
            Console.WriteLine(@"  ║        |  |  /____\  |  |                                           ║");
            Console.WriteLine(@"  ║        |  '----------'  |                                           ║");
            Console.WriteLine(@"  ║         \______________/                                            ║");
            Console.WriteLine(@"  ║                                                                     ║");
            Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // --- INPUT FIELDS ---
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("       > USERNAME : ");
            Console.ResetColor();
            string usern = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("       > PASSWORD : ");
            Console.ResetColor();
            string pass = Console.ReadLine();

            // --- 1. VALIDATION: EMPTY FIELDS (Fixed WarningPopup arguments) ---
            if (string.IsNullOrWhiteSpace(usern) || string.IsNullOrWhiteSpace(pass))
            {
                Console.Clear();
                WarningPopup("EMPTY DATA", "Username and Password cannot be empty.");
                return;
            }

            // --- 2. VALIDATION: FILE MISSING (Fixed WarningPopup arguments) ---
            if (!File.Exists("login.txt"))
            {
                Console.Clear();
                WarningPopup("DATABASE NOT FOUND", "No accounts found. Please register first.");
                return;
            }

            // --- 3. VALIDATION: USER CHECK (Fixed WarningPopup arguments) ---
            string[] lines = File.ReadAllLines("login.txt");
            bool userExists = lines.Any(line => line.Split(',')[0] == usern);

            if (!userExists)
            {
                Console.Clear();
                WarningPopup("USER NOT RECOGNIZED", "Account not found. Please register first.");
                return; // Use return instead of mainmenu() to avoid infinite stacking loops
            }

            // --- 4. PASSWORD VERIFICATION (Fixed logic loop) ---
            bool loginSuccess = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 2 && parts[0] == usern && parts[1] == pass)
                {
                    loginSuccess = true;
                    break; // Stop looking, we found the right password!
                }
            }

            // --- 5. LOGIN RESULT ---
            if (loginSuccess)
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
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                // Unlocked Padlock ASCII art!
                Console.WriteLine(@"                                   ____  ");
                Console.WriteLine(@"                                  /    \ ");
                Console.WriteLine(@"                                 |      |");
                Console.WriteLine(@"                                 |      |");
                Console.WriteLine(@"                              ___|      |");
                Console.WriteLine(@"                             /          \ ");
                Console.WriteLine(@"                            |  .------.  |");
                Console.WriteLine(@"                            |  | [OK] |  |");
                Console.WriteLine(@"                            |  |      |  |");
                Console.WriteLine(@"                            |  '------'  |");
                Console.WriteLine(@"                             \__________/ ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(@"  ║                       [ A C C E S S   G R A N T E D ]               ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════════════╝");

                Console.ResetColor();
                Console.WriteLine($"\n                       Welcome back, {currentUser}!");
                Console.WriteLine($"                       Current Points: {points}");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                       game loading...\n");
                Console.ResetColor();

                System.Threading.Thread.Sleep(1500); // Gives them a second to enjoy the unlock screen

                gamemenu(wordDictionary);
            }
            else
            {
                // If the loop finished and loginSuccess is STILL false, the password was wrong!
                Console.Clear();
                WarningPopup("ACCESS DENIED", "Invalid password. Intruder Alert.");
                return;
            }
        }

        static void WarningPopup(string errorTitle, string errorDetail)
        {
            Console.Clear();
            Console.Beep(800, 200);
            Console.Beep(600, 300);

            Console.WriteLine("\n\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"        ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine(@"        ║                 [ S Y S T E M  A L E R T ]             ║");
            Console.WriteLine(@"        ╠════════════════════════════════════════════════════════╣");
            Console.WriteLine(@"        ║                        _______                         ║");
            Console.WriteLine(@"        ║                       /       \                        ║");
            Console.WriteLine(@"        ║                      /    !    \                       ║");
            Console.WriteLine(@"        ║                     /___________\                      ║");
            Console.WriteLine(@"        ║                                                        ║");

            // Centers the error title dynamically
            int titlePadding = (54 - errorTitle.Length) / 2;
            Console.Write(@"        ║");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("".PadLeft(Math.Max(0, titlePadding)) + errorTitle + "".PadRight(Math.Max(0, 54 - titlePadding - errorTitle.Length)));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"║");

            // Centers the error detail dynamically
            int detailPadding = (54 - errorDetail.Length) / 2;
            Console.Write(@"        ║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("".PadLeft(Math.Max(0, detailPadding)) + errorDetail + "".PadRight(Math.Max(0, 54 - detailPadding - errorDetail.Length)));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"║");

            Console.WriteLine(@"        ║                                                        ║");
            Console.WriteLine(@"        ║                   [  Tap to Continue  ]                ║");
            Console.WriteLine(@"        ╚════════════════════════════════════════════════════════╝");

            // --- 3D DROP SHADOW EFFECT ---
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"          ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");

            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            mainmenu();
            
        }

        static void regist()
        {
            bool regis = false;
            Console.Clear();
            while (!regis)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;

                // --- CUSTOM ID BADGE ASCII ART ---
                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(@"  ║                     [ NEW USER REGISTRATION ]                       ║");
                Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║                                                                     ║");
                Console.WriteLine(@"  ║         [  NEW ACCOUNT  ]                                           ║");
                Console.WriteLine(@"  ║          _______________                                            ║");
                Console.WriteLine(@"  ║         |  ___________  |                                           ║");
                Console.WriteLine(@"  ║         | |     _     | |         INPUT CREDENTIALS BELOW.          ║");
                Console.WriteLine(@"  ║         | |    ( )    | |                                           ║");
                Console.WriteLine(@"  ║         | |   /| |\   | |                                           ║");
                Console.WriteLine(@"  ║         | |  /_\_/_\  | |  ====================================     ║");
                Console.WriteLine(@"  ║         | |___________| |        *  AWAITING NEW PLAYER  *          ║");
                Console.WriteLine(@"  ║         |    _______    |  ====================================     ║");
                Console.WriteLine(@"  ║         |   |_______|   |                                           ║");
                Console.WriteLine(@"  ║         |_______________|                                           ║");
                Console.WriteLine(@"  ║                                                                     ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                // --- INPUT FIELDS ---
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("       > USERNAME : ");
                Console.ResetColor();
                string newuser = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("       > PASSWORD  : ");
                Console.ResetColor();
                string newpass = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("       > CONFIRM PASSWORD : ");
                Console.ResetColor();
                string confpass = Console.ReadLine();

                // --- PAUSE ---
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n       Loading...");
                System.Threading.Thread.Sleep(600);
                Console.ResetColor();

                Console.WriteLine("===================================");
                if (newuser.Trim() == "" || newpass.Trim() == "" || confpass.Trim() == "")
                {
                    WarningPopup("INVALID DATA", "Username and Passwords cannot be empty!");
                    continue; // Restarts the while loop
                }

                // --- 2. VALIDATION: PASSWORD MATCH ---
                if (confpass != newpass)
                {
                    WarningPopup("SECURITY ALERT", "Passwords do not match! Please try again.");
                    continue;
                }

                // --- 3. VALIDATION: USER EXISTS ---
                if (File.Exists("login.txt") && File.ReadAllText("login.txt").Contains(newuser + ","))
                {
                    WarningPopup("INVALID DATA", "Username already exists in the database.");
                    continue;
                }
                else
                {
                    // --- REGISTRATION SUCCESS LOGIC ---
                    string[] newaccount = { newuser + "," + confpass };
                    File.AppendAllLines("login.txt", newaccount);

                    // --- HACKER PROGRESS BAR ANIMATION ---
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n\n\n\n\n");
                    Console.WriteLine(@"                    =======================================");
                    Console.WriteLine(@"                         [     CREATING NEW ACCOUNT     ]");
                    Console.WriteLine(@"                    =======================================");
                    Console.Write("\n                      PROCESSING: [");

                    Console.ForegroundColor = ConsoleColor.Green;
                    for (int i = 0; i < 20; i++)
                    {
                        Console.Write("█"); // Fills the progress bar
                        System.Threading.Thread.Sleep(100); // 100ms per block (2 seconds total)
                    }
                    Console.ResetColor();
                    Console.WriteLine("] 100%");
                }

                // --- SUCCESS REGIS ---
                System.Threading.Thread.Sleep(500);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(@"                                   *      *   *");
                Console.WriteLine(@"                                 *   \    |   /   *");
                Console.WriteLine(@"                                * - -  [ OK ] - - *");
                Console.WriteLine(@"                                 *   /    |   \   *");
                Console.WriteLine(@"                                   *      *   *");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(@"  ║                     [ REGISTRATION SUCCESSFUL ]                     ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════════════╝");

                Console.ResetColor();
                Console.WriteLine($"\n                       Account created for: {newuser}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n                              Tap to Continue...");
                Console.ResetColor();
                Console.ReadKey();

                regis = true; // Breaks the loop
                Console.Clear();
                mainmenu(); // Safely returns to mainmenu() without causing a stack overflow
            }
        }

        static void gamemenu(Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            Console.Clear();
            bool try3 = false;
            while (!try3)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;

                string playerText = $"> USER: {currentUser}";
                string pointsText = $"> SCORE: {points} PTS";
                int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                if (spaces < 0) spaces = 0;

                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.Write(@"  ║  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(playerText);
                Console.Write(new string(' ', spaces));
                Console.Write(pointsText);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"  ║");
                Console.WriteLine(@"  ╠════════════╦════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║  .------.  ║                                                ║");
                Console.WriteLine(@"  ║  | GAME |  ║                                                ║");
                Console.WriteLine(@"  ║  | MENU |  ║             [ 1 ] > PLAY HANGMAN               ║");
                Console.WriteLine(@"  ║  |______|  ║             [ 2 ] > READING COMPREHENSION      ║");
                Console.WriteLine(@"  ║    _  _    ║             [ 3 ] > VIEW LEADERBOARD           ║");
                Console.WriteLine(@"  ║  _| || |_  ║             [ 4 ] > WORD DICTIONARY            ║");
                Console.WriteLine(@"  ║ |_  __  _| ║             [ 5 ] > LOGOUT & RETURN            ║");
                Console.WriteLine(@"  ║   |_||_|   ║                                                ║");
                Console.WriteLine(@"  ║       (B)  ║      >> SELECT YOUR NEXT CHALLENGE <<          ║");
                Console.WriteLine(@"  ║    (A)     ║                                                ║");
                Console.WriteLine(@"  ╚════════════╩════════════════════════════════════════════════╝");
                Console.WriteLine();

                // --- INPUT PROMPT ---
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    Enter your command (1-5) : ");
                Console.ResetColor();
                string output2 = Console.ReadLine();
                switch (output2)
                {
                    case "1":
                        HangmanMenu(currentUser, wordDictionary);
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
                        Console.WriteLine("Invalid Option please enter either 1, 2, 3, 4 or 5");
                        try3 = false;
                        break;
                }
            }
        }
        static void Leaderboard()
        {
            string[] leaderboard = File.ReadAllLines("Leaderboard.txt");

            // Bubble sort 
            for (int i = 0; i < leaderboard.Length - 1; i++)
            {
                for (int j = 0; j < leaderboard.Length - 1 - i; j++)
                {
                    if (leaderboard[j] == "" || leaderboard[j + 1] == "") continue;

                    string[] partsA = leaderboard[j].Split(',');
                    string[] partsB = leaderboard[j + 1].Split(',');

                    int pointsA = int.Parse(partsA[1]);
                    int pointsB = int.Parse(partsB[1]);

                    if (pointsA < pointsB)
                    {
                        string temp = leaderboard[j];
                        leaderboard[j] = leaderboard[j + 1];
                        leaderboard[j + 1] = temp;
                    }
                }
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine(@"  ║             ___________                                             ║");
            Console.WriteLine(@"  ║            '._==_==_=_.'      [ GLOBAL LEADERBOARD ]                ║");
            Console.WriteLine(@"  ║            .-\:      /-.      ======================                ║");
            Console.WriteLine(@"  ║           | (|:.     |) |          TOP PLAYERS                      ║");
            Console.WriteLine(@"  ║            '-|:.     |-'                                            ║");
            Console.WriteLine(@"  ║              \::.    /                                              ║");
            Console.WriteLine(@"  ║               '::. .'                                               ║");
            Console.WriteLine(@"  ║                 ) (                                                 ║");
            Console.WriteLine(@"  ║               _.' '._                                               ║");
            Console.WriteLine(@"  ║              `"""""" ""`                                            ║");
            Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine(@"  ║  RANK  │           USERNAME           │         TOTAL SCORE         ║");
            Console.WriteLine(@"  ╠════════╪══════════════════════════════╪═════════════════════════════╣");

            // --- PLAYERS (TOP 10 ONLY) ---
            int rank = 1;
            bool hasPlayers = false;

            for (int i = 0; i < leaderboard.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(leaderboard[i]))
                {
                    hasPlayers = true;
                    string[] parts = leaderboard[i].Split(',');

                    string name = parts[0];
                    string pts = parts.Length > 1 ? parts[1] : "0";

                    // Limit letters to 26 chars so it doesn't mess up the box huhu
                    if (name.Length > 26) name = name.Substring(0, 23) + "...";

                    Console.Write(@"  ║ ");


                    if (rank == 1) Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (rank == 2) Console.ForegroundColor = ConsoleColor.Gray;
                    else if (rank == 3) Console.ForegroundColor = ConsoleColor.DarkRed;
                    else Console.ForegroundColor = ConsoleColor.White;


                    Console.Write(string.Format("{0,-6} │  {1,-26}  │  {2,-25}", $"#{rank}", name, $"{pts} PTS"));

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@"║");

                    rank++;

                    // Limit to Top 10 so the screen doesn't overflow
                    if (rank > 10) break;
                }
            }

            if (!hasPlayers)
            {
                Console.Write(@"  ║");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("       [ NO RECORDS FOUND. BE THE FIRST TO SET A HIGH SCORE! ]       ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"║");
            }


            Console.WriteLine(@"  ╚════════╧══════════════════════════════╧═════════════════════════════╝");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    Tap to go back   ");
            Console.ResetColor();

            Console.ReadKey();
            return;
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
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    string playerText = $"> USER: {currentUser}";
                    string pointsText = $"> SCORE: {points} PTS";
                    int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                    if (spaces < 0) spaces = 0;


                    Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                    Console.Write(@"  ║  ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(playerText);
                    Console.Write(new string(' ', spaces));
                    Console.Write(pointsText);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@"  ║");
                    Console.WriteLine(@"  ╠════════════╦════════════════════════════════════════════════╣");
                    Console.WriteLine(@"  ║  +======+  ║                                                ║");
                    Console.WriteLine(@"  ║  |      |  ║                                                ║");
                    Console.WriteLine(@"  ║  O      |  ║             [ 1 ] > EASY (BEGINNER)            ║");
                    Console.WriteLine(@"  ║ /|\     |  ║             [ 2 ] > NORMAL (OPERATIVE)         ║");
                    Console.WriteLine(@"  ║ / \     |  ║             [ 3 ] > HARD (NIGHTMARE)           ║");
                    Console.WriteLine(@"  ║         |  ║             [ 4 ] > RETURN                     ║");
                    Console.WriteLine(@"  ║         |  ║                                                ║");
                    Console.WriteLine(@"  ║ ========╩= ║      »» SELECT DIFFICULTY PROTOCOL ««          ║");
                    Console.WriteLine(@"  ╚════════════╩════════════════════════════════════════════════╝");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    Enter command (0-3) : ");
                    Console.ResetColor();

                    string choice = Console.ReadLine();


                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n    Loading Easy Mode...");
                            System.Threading.Thread.Sleep(600);
                            exithangmanmenu = true;
                            points = HangmanEasy(currentUser, points, wordDictionary);
                            break;

                        case "2":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n    Loading Normal Mode...");
                            System.Threading.Thread.Sleep(600);
                            points = HangmanNormal(currentUser, points, wordDictionary);
                            break;

                        case "3":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n    Loading Hard Mode...");
                            System.Threading.Thread.Sleep(800);
                            points = HangmanHard(currentUser, points, wordDictionary);
                            break;

                        case "4":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\n    Returning...");
                            System.Threading.Thread.Sleep(800);
                            gamemenu(wordDictionary);
                            return;

                        default:

                            WarningPopup("INVALID OPTION", "Please enter 0, 1, 2, or 3.");
                            break;
                    }
                }
                gamemenu(wordDictionary);
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
        static int HangmanEasy(string currentUser, int points, Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            var (hangmanwordOrig, hangmandefinition) = WordRandomizer(wordDictionary);
            string hangmanword = hangmanwordOrig.ToLower();

            List<char> CorrectLetters = new List<char>();
            List<char> WrongLetters = new List<char>();
            int wrongGuesses = 0;
            bool playing = true;
            string errorMessage = "";

            LoadingScreen();

            while (playing)
            {
                Console.Clear();
                string playerText = $"> USER: {currentUser}";
                string pointsText = $"> SCORE: {points} PTS";
                int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                if (spaces < 0) spaces = 0;

                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.Write(@"  ║  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(playerText);
                Console.Write(new string(' ', spaces));
                Console.Write(pointsText);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"  ║");
                Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║                    [ DIFFICULTY: EASY ]                     ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                // --- 2. HANGMAN STAGE ---
                DrawHangman(wrongGuesses);
                Console.WriteLine();

                // --- 3. THE PUZZLE WORD ---
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("    [ W O R D ]");
                Console.Write("      ");
                Console.ForegroundColor = ConsoleColor.Yellow;

                bool won = true;
                foreach (char c in hangmanword)
                {
                    if (CorrectLetters.Contains(c))
                    {
                        Console.Write(char.ToUpper(c) + " "); // Prints revealed letters in UPPERCASE
                    }
                    else
                    {
                        Console.Write("_ "); // Prints blanks
                        won = false; // If we hit a blank, they haven't won yet
                    }
                }
                Console.WriteLine("\n");

                // --- 4. CLUE / INTEL ---
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("    [ H I N T  ( M E A N I N G ) ]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                // Basic word wrap trick to keep the definition looking clean
                Console.WriteLine("      " + hangmandefinition);
                Console.WriteLine();

                // --- 5. COMPROMISED DATA (WRONG GUESSES) ---
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    [ W R O N G   L E T T E R S ]");
                Console.Write("      ");
                if (WrongLetters.Count == 0)
                {
                    Console.WriteLine("None");
                }
                else
                {
                    foreach (char w in WrongLetters)
                    {
                        Console.Write(char.ToUpper(w) + "  ");
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");

                // --- 6. WIN / LOSS LOGIC ---
                if (won)
                {
                    points += 5;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"        \ \    / /_ _|  __|_   _|__  _ \ \ \ / / ");
                    Console.WriteLine(@"         \ \  / / | || |    | | / _ \ |_) \ V /  ");
                    Console.WriteLine(@"          \ \/ /  | || |__  | || (_) |  _ <| |   ");
                    Console.WriteLine(@"           \__/  |___|\____| |_| \___/|_| \_\_|  ");
                    Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n          MISSION ACCOMPLISHED! THE WORD WAS: {hangmanwordOrig.ToUpper()}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n                  REWARD: [ + 5 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Press ANY KEY to return to the Hangman Menu...");
                    Console.ReadKey();
                    return points;
                }

                if (wrongGuesses >= 6)
                {
                    points -= 2;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"           ___   _   __  __ ___    _____   _____ ___  ");
                    Console.WriteLine(@"          / __| /_\ |  \/  | __|  / _ \ \ / / __| _ \ ");
                    Console.WriteLine(@"         | (_ |/ _ \| |\/| | _|  | (_) \ V /| _||   / ");
                    Console.WriteLine(@"          \___/_/ \_\_|  |_|___|  \___/ \_/ |___|_|_\ ");
                    Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n          MISSION FAILED. THE TARGET WORD WAS: {hangmanwordOrig.ToUpper()}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n                  PENALTY: [ - 2 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Press ANY KEY to return to the Hangman Menu...");
                    Console.ReadKey();
                    return points;
                }

                // --- 7. ERROR MESSAGE DISPLAY ---
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"    [!] {errorMessage}");
                    errorMessage = ""; // Reset after displaying
                }

                // --- 8. INPUT PROMPT ---
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    > ENTER A LETTER : ");
                Console.ResetColor();
                string input = Console.ReadLine()?.ToLower();

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
                    playing = false;
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return points;
                }

                if (wrongGuesses >= 6)
                {
                    Console.WriteLine("\nYou Lose! - 2 Points.");
                    Console.WriteLine("The word was: " + hangmanword);
                    points -= 2;
                    playing = false;
                    Console.WriteLine("Press any key to go back to hangman menu...");
                    Console.ReadKey();
                    return points;
                }
            }
            return points;
        }


        static int HangmanNormal(string currentUser, int points, Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {
            var (hangmanwordOrig, hangmandefinition) = WordRandomizer(wordDictionary);
            string hangmanword = hangmanwordOrig.ToLower();

            List<char> CorrectLetters = new List<char>();
            List<char> WrongLetters = new List<char>();
            int wrongGuesses = 0;
            bool playing = true;
            string errorMessage = "";

            // Normal mode specific mechanics
            bool definitionAsked = false;
            bool definitionUnlocked = false;

            // Themed loading effect
            Console.Clear();
            LoadingScreen();

            while (playing)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;

                // --- 1. HUD (HEADS UP DISPLAY) ---
                string playerText = $"> USER: {currentUser}";
                string pointsText = $"> SCORE: {points} PTS";
                int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                if (spaces < 0) spaces = 0;

                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.Write(@"  ║  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(playerText);
                Console.Write(new string(' ', spaces));
                Console.Write(pointsText);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"  ║");
                Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║                   [ DIFFICULTY: NORMAL ]                    ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                // --- 2. HANGMAN STAGE ---
                DrawHangman(wrongGuesses);
                Console.WriteLine();

                // --- 3. THE PUZZLE WORD ---
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("    [ T A R G E T   W O R D ]");
                Console.Write("      ");
                Console.ForegroundColor = ConsoleColor.Yellow;

                bool won = true;
                foreach (char c in hangmanword)
                {
                    if (CorrectLetters.Contains(c))
                    {
                        Console.Write(char.ToUpper(c) + " ");
                    }
                    else
                    {
                        Console.Write("_ ");
                        won = false;
                    }
                }
                Console.WriteLine("\n");

                // --- 4. CLUE / INTEL (NORMAL MECHANIC) ---
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("    [ H I N T   ( M E A N I N G ) ]");

                if (!definitionAsked)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("      [ AWAITING DEFINITION TO BE UNLOCKED ]");
                }
                else if (definitionUnlocked)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("      " + hangmandefinition); // Shows the clue
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("      [ HINT DECLINED. PLAYING BLIND. ]");
                }
                Console.WriteLine();

                // --- 5. COMPROMISED DATA (WRONG GUESSES) ---
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    [ W R O N G   L E T T E R S ]");
                Console.Write("      ");
                if (WrongLetters.Count == 0)
                {
                    Console.WriteLine("None");
                }
                else
                {
                    foreach (char w in WrongLetters)
                    {
                        Console.Write(char.ToUpper(w) + "  ");
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");

                // --- 6. WIN / LOSS LOGIC (NORMAL REWARDS) ---
                if (won)
                {
                    points += 10; // Normal Mode Reward!
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"        \ \    / /_ _|  __|_   _|__  _ \ \ \ / / ");
                    Console.WriteLine(@"         \ \  / / | || |    | | / _ \ |_) \ V /  ");
                    Console.WriteLine(@"          \ \/ /  | || |__  | || (_) |  _ <| |   ");
                    Console.WriteLine(@"           \__/  |___|\____| |_| \___/|_| \_\_|  ");
                    Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n          MISSION ACCOMPLISHED! THE WORD WAS: {hangmanwordOrig.ToUpper()}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n                  REWARD: [ + 10 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Tap to return to the Hangman Menu...");
                    Console.ReadKey();
                    return points;
                }

                if (wrongGuesses >= 6)
                {
                    points -= 5; // Normal Mode Penalty!
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"           ___   _   __  __ ___    _____   _____ ___  ");
                    Console.WriteLine(@"          / __| /_\ |  \/  | __|  / _ \ \ / / __| _ \ ");
                    Console.WriteLine(@"         | (_ |/ _ \| |\/| | _|  | (_) \ V /| _||   / ");
                    Console.WriteLine(@"          \___/_/ \_\_|  |_|___|  \___/ \_/ |___|_|_\ ");
                    Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n          MISSION FAILED. THE TARGET WORD WAS: {hangmanwordOrig.ToUpper()}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n                  PENALTY: [ - 5 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Tap to return to the Hangman Menu...");
                    Console.ReadKey();
                    return points;
                }

                // --- 7. ERROR MESSAGE DISPLAY ---
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"    [!] {errorMessage}");
                    errorMessage = "";
                }

                // --- 8. DYNAMIC INPUT PROMPT (DECRYPT VS GUESS) ---
                if (!definitionAsked)
                {
                    // Decryption choice prompt
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("    [!] WARNING: GETTING HINT ADDS +1 TO HANGMAN STAGE.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    > UNLOCK HINT? (Y/N) : ");
                    Console.ResetColor();

                    string unlock = Console.ReadLine()?.ToUpper();

                    if (unlock == "Y")
                    {
                        wrongGuesses++;
                        definitionUnlocked = true;
                        definitionAsked = true;
                    }
                    else if (unlock == "N")
                    {
                        definitionAsked = true;
                    }
                    else
                    {
                        errorMessage = "Invalid command. Enter 'Y' for Yes or 'N' for No.";
                    }
                    continue; // Refresh screen after making the choice!
                }
                else
                {
                    // Standard letter guessing prompt
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    > ENTER A LETTER : ");
                    Console.ResetColor();
                    string input = Console.ReadLine()?.ToLower();

                    // --- 9. INPUT VALIDATION ---
                    if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                    {
                        errorMessage = "Invalid target. Please enter a single letter.";
                        continue;
                    }

                    char letter = input[0];

                    if (CorrectLetters.Contains(letter) || WrongLetters.Contains(letter))
                    {
                        errorMessage = $"Letter '{char.ToUpper(letter)}' has already been used!";
                        continue;
                    }

                    // --- 10. GAMEPLAY LOGIC ---
                    if (hangmanword.Contains(letter))
                    {
                        CorrectLetters.Add(letter);
                    }
                    else
                    {
                        WrongLetters.Add(letter);
                        wrongGuesses++;
                    }
                }
            }

            return points;
        }
        static int HangmanHard(string currentUser, int points, Dictionary<string, (string type, string definition, string example)> wordDictionary)

        {
            var (hangmanwordOrig, hangmandefinition) = WordRandomizer(wordDictionary);
            string hangmanword = hangmanwordOrig.ToLower();

            List<char> CorrectLetters = new List<char>();
            List<char> WrongLetters = new List<char>();
            int wrongGuesses = 0;
            bool playing = true;
            string errorMessage = "";

            // Normal mode specific mechanics
            bool definitionAsked = false;
            bool definitionUnlocked = false;

            // Themed loading effect
            Console.Clear();
            LoadingScreen();

            while (playing)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;

                // --- 1. HUD (HEADS UP DISPLAY) ---
                string playerText = $"> AGENT: {currentUser}";
                string pointsText = $"> SCORE: {points} PTS";
                int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                if (spaces < 0) spaces = 0;

                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.Write(@"  ║  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(playerText);
                Console.Write(new string(' ', spaces));
                Console.Write(pointsText);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"  ║");
                Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║                   [ DIFFICULTY: Hard ]                      ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                // --- 2. HANGMAN STAGE ---
                DrawHangman(wrongGuesses);
                Console.WriteLine();
                // --- 3. THE PUZZLE WORD ---
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("    [ T A R G E T   W O R D ]");
                Console.Write("      ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                bool won = true;
                foreach (char c in hangmanword)
                {
                    if (CorrectLetters.Contains(c))
                    {
                        Console.Write(char.ToUpper(c) + " ");
                    }
                    else
                    {
                        Console.Write("_ ");
                        won = false;
                    }
                }
                // --- 5. COMPROMISED DATA (WRONG GUESSES) ---
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n");
                Console.WriteLine("    [ W R O N G   L E T T E R S ]");
                Console.Write("      ");
                if (WrongLetters.Count == 0)
                {
                    Console.WriteLine("None");
                }
                else
                {
                    foreach (char w in WrongLetters)
                    {
                        Console.Write(char.ToUpper(w) + "  ");
                    }
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");

                // --- 6. WIN / LOSS LOGIC (NORMAL REWARDS) ---
                if (won)
                {
                    points += 10; // Normal Mode Reward!
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"        \ \    / /_ _|  __|_   _|__  _ \ \ \ / / ");
                    Console.WriteLine(@"         \ \  / / | || |    | | / _ \ |_) \ V /  ");
                    Console.WriteLine(@"          \ \/ /  | || |__  | || (_) |  _ <| |   ");
                    Console.WriteLine(@"           \__/  |___|\____| |_| \___/|_| \_\_|  ");
                    Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n          MISSION ACCOMPLISHED! THE WORD WAS: {hangmanwordOrig.ToUpper()}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n                  REWARD: [ + 10 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Press ANY KEY to return to the Hangman Menu...");
                    Console.ReadKey();
                    return points;
                }

                if (wrongGuesses >= 6)
                {
                    points -= 5; // Normal Mode Penalty!
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"           ___   _   __  __ ___    _____   _____ ___  ");
                    Console.WriteLine(@"          / __| /_\ |  \/  | __|  / _ \ \ / / __| _ \ ");
                    Console.WriteLine(@"         | (_ |/ _ \| |\/| | _|  | (_) \ V /| _||   / ");
                    Console.WriteLine(@"          \___/_/ \_\_|  |_|___|  \___/ \_/ |___|_|_\ ");
                    Console.WriteLine("\n  ═══════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n          MISSION FAILED. THE TARGET WORD WAS: {hangmanwordOrig.ToUpper()}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n                  PENALTY: [ - 5 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Press ANY KEY to return to the Hangman Menu...");
                    Console.ReadKey();
                    return points;
                }

                // --- 7. ERROR MESSAGE DISPLAY ---
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"    [!] {errorMessage}");
                    errorMessage = "";
                }
                else
                {
                    // Standard letter guessing prompt
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    ▶ ENTER A LETTER : ");
                    Console.ResetColor();
                    string input = Console.ReadLine()?.ToLower();

                    // --- 9. INPUT VALIDATION ---
                    if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                    {
                        errorMessage = "Invalid target. Please enter a single letter.";
                        continue;
                    }

                    char letter = input[0];

                    if (CorrectLetters.Contains(letter) || WrongLetters.Contains(letter))
                    {
                        errorMessage = $"Letter '{char.ToUpper(letter)}' has already been used!";
                        continue;
                    }

                    // --- 10. GAMEPLAY LOGIC ---
                    if (hangmanword.Contains(letter))
                    {
                        CorrectLetters.Add(letter);
                    }
                    else
                    {
                        WrongLetters.Add(letter);
                        wrongGuesses++;
                    }
                }
            }

            return points;
        }
        static void Readingcomp()
        {
            Console.Clear();
            bool try4 = false;
            while (!try4)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.WriteLine(@"  ║                 [ READING COMPREHENSION ]                   ║");
                Console.WriteLine(@"  ╠════════════╦════════════════════════════════════════════════╣");
                Console.WriteLine(@"  ║  .------.  ║                                                ║");
                Console.WriteLine(@"  ║  |      |  ║        CHOOSE YOUR LEVEL OF DIFFICULTY         ║");
                Console.WriteLine(@"  ║  |______|  ║                                                ║");
                Console.WriteLine(@"  ║   _||||_   ║        [ 1 ] . EASY LEVEL                      ║");
                Console.WriteLine(@"  ║  |______|  ║        [ 2 ] . MEDIUM LEVEL                    ║");
                Console.WriteLine(@"  ║            ║        [ 3 ] . HARD LEVEL                      ║");
                Console.WriteLine(@"  ║            ║        [ 4 ] . EXIT                            ║");
                Console.WriteLine(@"  ║  ========  ║                                                ║");
                Console.WriteLine(@"  ╚════════════╩════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    Enter your choice (1-4) : ");
                Console.ResetColor();

                string choice = Console.ReadLine()?.ToUpper();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n    Loading Easy Level...");
                        System.Threading.Thread.Sleep(600);
                        easy();
                        try4 = true;
                        break;
                    case "2":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n    Loading Medium Level...");
                        System.Threading.Thread.Sleep(600);
                        medium();
                        try4 = true;
                        break;
                    case "3":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n    Loading Medium Level...");
                        System.Threading.Thread.Sleep(600);
                        hard();
                        try4 = true;
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("\n    Exiting Reading Comprehension...");
                        System.Threading.Thread.Sleep(500);
                        gamemenu(wordDictionary);
                        try4 = true;
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n    [!] Invalid Option pls enter either 1, 2, 3 or 4");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(1500);
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
        static List<string> WordWrap(string text, int maxLineLength)
        {
            var list = new List<string>();
            string[] paragraphs = text.Split(new[] { "\n" }, StringSplitOptions.None);
            foreach (string paragraph in paragraphs)
            {
                if (string.IsNullOrWhiteSpace(paragraph))
                {
                    list.Add("");
                    continue;
                }
                string[] words = paragraph.Split(' ');
                string currentLine = "";
                foreach (string word in words)
                {
                    if ((currentLine + word).Length > maxLineLength)
                    {
                        list.Add(currentLine.TrimEnd());
                        currentLine = "";
                    }
                    currentLine += word + " ";
                }
                if (currentLine.Length > 0)
                    list.Add(currentLine.TrimEnd());
            }
            return list;
        }
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
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    // --- 1. HUD & HEADER ---
                    string playerText = $"> USER: {currentUser}";
                    string pointsText = $"> SCORE: {points} PTS";
                    int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                    if (spaces < 0) spaces = 0;

                    Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                    Console.Write(@"  ║  ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(playerText);
                    Console.Write(new string(' ', spaces));
                    Console.Write(pointsText);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@"  ║");
                    Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════╣");
                    Console.WriteLine(@"  ║                  [ READING COMPREHENSION ]                  ║");
                    Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════╝");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"    [ THE STORY : {title.ToUpper()} ]");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(@"  ───────────────────────────────────────────────────────────────");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    List<string> wrappedStory = WordWrap(text, 60);
                    foreach (string line in wrappedStory)
                    {
                        Console.WriteLine("    " + line); // Indented cleanly
                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(@"  ───────────────────────────────────────────────────────────────");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@"  [ QUESTION ]");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    List<string> wrappedQuestion = WordWrap($"Q: {q.Prompt}", 60);
                    foreach (var line in wrappedQuestion)
                    {
                        Console.WriteLine("    " + line);
                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (var choice in q.Choices)
                    {
                        Console.WriteLine("      " + choice);

                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(@"  ───────────────────────────────────────────────────────────────");
                    Console.WriteLine("    [ Type 'x' to exit ]");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    > ENTER YOUR ANSWER (A/B/C/D) : ");
                    Console.ResetColor();

                    string input = Console.ReadLine()?.Trim().ToUpper();

                    if (input == "x")
                    {
                        goBackToStory = true;
                        break;
                    }
                    if (input.Length > 0 && input[0] == q.Answer)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n    [✓] CORRECT! The answer is {q.Answer}. (+{pointsIfPass} PTS)");
                        Console.ResetColor();
                        points += pointsIfPass;
                        score++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        points -= pointsifNot;
                        Console.WriteLine($"\n    [X] INCORRECT! The correct answer was {q.Answer}. (-{pointsifNot} PTS)");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n    Press any key to continue...");
                    Console.ReadKey();

                }
                if (!goBackToStory)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"        ╔════════════════════════════════════════════════╗");
                    Console.WriteLine(@"        ║           [O V E R A L L  R E S U L T ]        ║");
                    Console.WriteLine(@"        ╠════════════════════════════════════════════════╣");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine($"        ║ STORY TITLE    : {title.PadRight(31)} ║");
                    Console.WriteLine($"        ║ SCORE   : {score} / {questions.Count} Correct                 ║");
                    Console.WriteLine($"        ║ TOTAL POINTS : {points.ToString().PadRight(4)} PTS                       ║");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@"        ╚════════════════════════════════════════════════╝");

                    if (score == questions.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n                  STATUS: EXCELLENT");
                    }
                    else if (score > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n                  STATUS: GOOD JOB");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n                  STATUS: FAILED");
                    }

                    Console.ResetColor();
                    calcu();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.ReadKey();
                    gamemenu(wordDictionary);
                    break;
                }

            } while (goBackToStory);
        }
        static void DictionaryView(Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {

            // Convert dictionary to a sorted list for pagination
            var sortedEntries = wordDictionary.OrderBy(x => x.Key).ToList();

            if (sortedEntries.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n    [!] DATABASE: No words found.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(2000);
                return;
            }

            int itemsPerPage = 3; // Number of words to display per screen
            int totalPages = (int)Math.Ceiling((double)sortedEntries.Count / itemsPerPage);
            int currentPage = 1;
            bool viewing = true;

            int amt = 0;
            while (viewing)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;


                // --- 1. ASCII ART HEADER ---
                Console.WriteLine(@"  ╔═════════════════════════════════════════════════════════════╗");
                Console.WriteLine(@"  ║   ___ ___ ___ _____ ___ ___  _  _   _   _____   __          ║");
                Console.WriteLine(@"  ║  |   \_ _/ __|_   _|_ _/ _ \| \| | /_\ | _ \ \ / /          ║");
                Console.WriteLine(@"  ║  | |) | | (__  | |  | | (_) | .` |/ _ \|   /\ V /           ║");
                Console.WriteLine(@"  ║  |___/___\___| |_| |___\___/|_|\_/_/ \_\_|_\ |_|            ║");
                Console.WriteLine(@"  ╠═════════════════════════════════════════════════════════════╣");

                string dbHeader = $"[ WORDS DATABASE ]";
                string entryCount = $"{sortedEntries.Count} RECORDS";
                int padding = 61 - 4 - dbHeader.Length - entryCount.Length;

                Console.Write(@"  ║  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(dbHeader);
                Console.Write(new string(' ', Math.Max(0, padding)));
                Console.Write(entryCount);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"  ║");
                Console.WriteLine(@"  ╚═════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                // --- 2. GET CURRENT PAGE ITEMS ---
                var pageItems = sortedEntries.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();

                // --- 3. PRINT ITEMS ---
                foreach (var entry in pageItems)
                {
                    // WORD AND TYPE
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"    > WORD: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(entry.Key.ToUpper());
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"  [{entry.Value.type.ToUpper()}]");

                    // DEFINITION
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"     > DEF : ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    List<string> wrappedDef = WordWrap(entry.Value.definition, 50);
                    for (int i = 0; i < wrappedDef.Count; i++)
                    {
                        if (i == 0) Console.WriteLine(wrappedDef[i]);
                        else Console.WriteLine($"            {wrappedDef[i]}");
                    }

                    // EXAMPLE
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"      EX  : ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    List<string> wrappedEx = WordWrap($"\"{entry.Value.example}\"", 50);
                    for (int i = 0; i < wrappedEx.Count; i++)
                    {
                        if (i == 0) Console.WriteLine(wrappedEx[i]);
                        else Console.WriteLine($"            {wrappedEx[i]}");
                    }

                    // DIVIDER
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(@"  ───────────────────────────────────────────────────────────────");
                }

                // --- 4. PAGINATION FOOTER ---
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  [ PAGE {currentPage} OF {totalPages} ]");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    [N] Next Page  |  [P] Prev Page  |  [Q] Quit / Return");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n    > COMMAND : ");
                Console.ResetColor();

                // --- 5. INPUT HANDLING ---
                string input = Console.ReadLine()?.Trim().ToUpper();

                switch (input)
                {
                    case "N":
                    case "NEXT":
                        if (currentPage < totalPages) currentPage++;
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("    [!] End of database reached.");
                            System.Threading.Thread.Sleep(800);
                        }
                        break;
                    case "P":
                    case "PREV":
                    case "PREVIOUS":
                        if (currentPage > 1) currentPage--;
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("    [!] Already at the beginning of the database.");
                            System.Threading.Thread.Sleep(800);
                        }
                        break;
                    case "Q":
                    case "QUIT":
                    case "BACK":
                    case "EXIT":
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("    Exiting...");
                        System.Threading.Thread.Sleep(500);
                        viewing = false; // Exits the loop
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("    [!] Invalid command. Use N, P, or Q.");
                        System.Threading.Thread.Sleep(800);
                        break;
                }
            }
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
