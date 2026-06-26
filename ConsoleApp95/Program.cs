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

        }

        static void mainmenu()
        {
            bool try2 = false;
            while (!try2)
            {
                // ── Centering helper ────────────────────────────────────────────
                int uiWidth = 69;
                int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                string pad = new string(' ', leftPad);

                // ── Title Screen / Main Menu ─────────────────────────────────────
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(pad + @"╔═════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(pad + @"║            ___ ___ _    ___ ___ ___    _  _ _   _ _      _              ║");
                Console.WriteLine(pad + @"║           | __|_ _| |  |_ _| _ \_ _|__| || | | | | |    /_\             ║");
                Console.WriteLine(pad + @"║           | _| | || |__ | ||  _/| |___| __ | |_| | |__ / _ \            ║");
                Console.WriteLine(pad + @"║           |_| |___|____|___|_| |___|  |_||_|\___/|____/_/ \_\           ║");
                Console.WriteLine(pad + @"╠════════════════════╦════════════════════════════════════════════════════╣");
                Console.WriteLine(pad + @"║   +============+   ║                                                    ║");
                Console.WriteLine(pad + @"║   |            |   ║                                                    ║");
                Console.WriteLine(pad + @"║   O            |   ║          [ 1 ]  >  START / LOGIN                   ║");
                Console.WriteLine(pad + @"║  /|\           |   ║          [ 2 ]  >  CREATE ACCOUNT                  ║");
                Console.WriteLine(pad + @"║  / \           |   ║          [ 3 ]  >  EXIT GAME                       ║");
                Console.WriteLine(pad + @"║                |   ║                                                    ║");
                Console.WriteLine(pad + @"║                |   ║                                                    ║");
                Console.WriteLine(pad + @"║   =============╝   ║                                                    ║");
                Console.WriteLine(pad + @"╚════════════════════╩════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.Write($"{pad}Enter your command (1-3) : ");
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
                        Console.WriteLine($"{pad}Thank you for Playing!");
                        Thread.Sleep(1000);
                        Console.WriteLine($"{pad}Press any key to end the program...");
                        try2 = true;
                        Console.ReadKey();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine($"{pad}Invalid Option please enter either 1, 2 or 3");
                        try2 = false;
                        break;
                }
            }
        }

        static void login()
        {
            // ── Centering helper ────────────────────────────────────────────
            int uiWidth = 69;
            int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
            string pad = new string(' ', leftPad);

            // ── System Authentication ────────────────────────────────────────
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(pad + @"╔═════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine(pad + @"║                      [ SYSTEM AUTHENTICATION ]                          ║");
            Console.WriteLine(pad + @"╠═════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine(pad + @"║                                                                         ║");
            Console.WriteLine(pad + @"║           .----------.                                                  ║");
            Console.WriteLine(pad + @"║          /   ______   \                                                 ║");
            Console.WriteLine(pad + @"║         |   /      \   |                                                ║");
            Console.WriteLine(pad + @"║         |  |        |  |    AWAITING USER CREDENTIALS...                ║");
            Console.WriteLine(pad + @"║        _|  |________|  |_                                               ║");
            Console.WriteLine(pad + @"║       /                  \   =====================================      ║");
            Console.WriteLine(pad + @"║      |   .------------.   |                                             ║");
            Console.WriteLine(pad + @"║      |   |    ______  |   |  * UNAUTHORIZED ACCESS PROHIBITED *         ║");
            Console.WriteLine(pad + @"║      |   |    |    |  |   |                                             ║");
            Console.WriteLine(pad + @"║      |   |    |    O  |   |  =====================================      ║");
            Console.WriteLine(pad + @"║      |   |    |   /|\ |   |                                             ║");
            Console.WriteLine(pad + @"║      |   |    |   / \ |   |  PLEASE ENTER YOUR LOGIN DATA BELOW.        ║");
            Console.WriteLine(pad + @"║      |   |   /______\ |   |                                             ║");
            Console.WriteLine(pad + @"║      |   '------------'   |                                             ║");
            Console.WriteLine(pad + @"║       \__________________/                                              ║");
            Console.WriteLine(pad + @"║                                                                         ║");
            Console.WriteLine(pad + @"╚═════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // --- INPUT FIELDS ---
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{pad} > USERNAME : ");
            Console.ResetColor();
            string usern = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{pad}> PASSWORD : ");
            Console.ResetColor();
            string pass = "";
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, pass.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write('*');
                }
            }
            Console.WriteLine();
            if (usern == "" || pass == "")
            {
                Console.Clear();
                WarningPopup("EMPTY DATA", "Username and Password cannot be empty.");
                return;
            }
            if (!File.Exists("login.txt"))
            {
                Console.Clear();
                WarningPopup("DATABASE NOT FOUND", "No accounts found. Please register first.");
                return;
            }
            string[] lines = File.ReadAllLines("login.txt");
            bool userExists = lines.Any(line => line.Split(',')[0] == usern);

            if (!userExists)
            {
                Console.Clear();
                WarningPopup("USER NOT RECOGNIZED", "Account not found. Please register first.");
                return;
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
                // ── Centering helper ────────────────────────────────────────────
                int uiWidth2 = 69;
                int leftPad2 = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                string pad2 = new string(' ', leftPad2);

                // ── Access Granted Screen ────────────────────────────────────────
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine(pad2 + @"                        ___");
                Console.WriteLine(pad2 + @"                       /   \");
                Console.WriteLine(pad2 + @"                      /    /");
                Console.WriteLine(pad2 + @"                     /    /_");
                Console.WriteLine(pad2 + @"             _______/       `""--.   ");
                Console.WriteLine(pad2 + @"            (_________            )");
                Console.WriteLine(pad2 + @"            (_________            )");
                Console.WriteLine(pad2 + @"            (_________            )");
                Console.WriteLine(pad2 + @"            (_________            )");
                Console.WriteLine(pad2 + @"            (____________________/)");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(pad2 + @"╔═════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(pad2 + @"║                      [ A C C E S S   G R A N T E D ]                    ║");
                Console.WriteLine(pad2 + @"╚═════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine(pad2 + $"       Welcome back, {currentUser}!");
                Console.WriteLine(pad2 + $"       Current Points: {points}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine();
                Console.WriteLine(pad2 + @"       game loading...");
                Console.WriteLine();
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
            int uiWidth = 69;
            int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
            string pad = new string(' ', leftPad);
            Console.Beep(800, 200);
            Console.Beep(600, 300);

            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{pad}        ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{pad}        ║                 [ S Y S T E M  A L E R T ]             ║");
            Console.WriteLine($"{pad}        ╠════════════════════════════════════════════════════════╣");
            Console.WriteLine($"{pad}        ║                        _______                         ║");
            Console.WriteLine($"{pad}        ║                       /       \\                        ║");
            Console.WriteLine($"{pad}        ║                      /    !    \\                       ║");
            Console.WriteLine($"{pad}        ║                     /___________\\                      ║");
            Console.WriteLine($"{pad}        ║                                                        ║");
            int totalBoxInnerWidth = 56;
            int titlePadding = (totalBoxInnerWidth - errorTitle.Length) / 2;
            Console.Write($"{pad}        ║");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("".PadLeft(Math.Max(0, titlePadding)) + errorTitle + "".PadRight(Math.Max(0, totalBoxInnerWidth - titlePadding - errorTitle.Length)));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("║");
            int detailPadding = (totalBoxInnerWidth - errorDetail.Length) / 2;
            Console.Write($"{pad}        ║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("".PadLeft(Math.Max(0, detailPadding)) + errorDetail + "".PadRight(Math.Max(0, totalBoxInnerWidth - detailPadding - errorDetail.Length)));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("║"); // Fixed: Removed the breaking {pad} here

            Console.WriteLine($"{pad}        ║                                                        ║");
            Console.WriteLine($"{pad}        ║                   [  Tap to Continue  ]                ║");
            Console.WriteLine($"{pad}        ╚════════════════════════════════════════════════════════╝");

            Console.ResetColor();
            Console.WriteLine();
            // --- 3D DROP SHADOW EFFECT ---
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{pad}          ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀");

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
                int uiWidth = 71;
                int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                string pad = new string(' ', leftPad);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                // --- CUSTOM ID BADGE ASCII ART ---
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{pad}╔═════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"{pad}║                      [ NEW USER REGISTRATION ]                      ║");
                Console.WriteLine($"{pad}╠═════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine($"{pad}║                                                                     ║");
                Console.WriteLine($"{pad}║         [  NEW ACCOUNT  ]                                           ║");
                Console.WriteLine($"{pad}║          _______________                                            ║");
                Console.WriteLine($"{pad}║         |  ___________  |                                           ║");
                Console.WriteLine($"{pad}║         | |     _     | |          INPUT CREDENTIALS BELOW.         ║");
                Console.WriteLine($"{pad}║         | |    ( )    | |                                           ║");
                Console.WriteLine($"{pad}║         | |   /| |\\   | |                                           ║");
                Console.WriteLine($"{pad}║         | |  /_\\_/_\\  | |  ====================================     ║");
                Console.WriteLine($"{pad}║         | |___________| |        * AWAITING NEW PLAYER  *           ║");
                Console.WriteLine($"{pad}║         |    _______    |  ====================================     ║");
                Console.WriteLine($"{pad}║         |   |_______|   |                                           ║");
                Console.WriteLine($"{pad}║         |_______________|                                           ║");
                Console.WriteLine($"{pad}║                                                                     ║");
                Console.WriteLine($"{pad}╚═════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                Console.WriteLine(pad + "type '/' to go back");
                // --- INPUT FIELDS (Perfectly aligned with the box left margin) ---
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{pad}> USERNAME         : ");
                Console.ResetColor();
                string newuser = Console.ReadLine();
                if (newuser == "/")
                {
                    Console.Clear();
                    mainmenu();
                    Console.Clear();
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{pad}> PASSWORD         : ");
                Console.ResetColor();
                ConsoleKeyInfo key;
                string newpass = "";
                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace && newpass.Length > 0)
                    {
                        newpass = newpass.Substring(0, newpass.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        newpass += key.KeyChar;
                        Console.Write('*');
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.Write($"{pad}> CONFIRM PASSWORD : ");
                Console.ResetColor();
                string confpass = "";
                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace && newpass.Length > 0)
                    {
                        confpass = confpass.Substring(0, confpass.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        confpass += key.KeyChar;
                        Console.Write('*');
                    }
                }
                // --- PAUSE ---
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{pad}Loading...");
                System.Threading.Thread.Sleep(600);
                Console.ResetColor();

                Console.WriteLine($"{pad}=======================================================================");

                if (string.IsNullOrWhiteSpace(newuser) || string.IsNullOrWhiteSpace(newpass) || string.IsNullOrWhiteSpace(confpass))
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
                    string[] newaccount = { newuser + "," + confpass };
                    File.AppendAllLines("login.txt", newaccount);
                    Console.Clear();

                    // Define the dimensions of the loading block
                    int blockWidth = 39;  // Width of the "=======================================" line
                    int blockHeight = 5;  // 3 lines for the box + 1 empty line + 1 line for the progress bar

                    // Calculate centering coordinates
                    int targetRow = Math.Max(0, (Console.WindowHeight - blockHeight) / 2);
                    int totalLeftPad = Math.Max(0, (Console.WindowWidth - blockWidth) / 2);
                    string centerPad = new string(' ', totalLeftPad);

                    // Set cursor to the start row for vertical centering
                    Console.SetCursorPosition(0, targetRow);

                    // Print Header (Cyan)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{centerPad}=======================================");
                    Console.WriteLine($"{centerPad}   [   CREATING NEW ACCOUNT   ]   "); // Adjusted spaces so it is centered within the 39-character border
                    Console.WriteLine($"{centerPad}=======================================");
                    Console.ResetColor();

                    // Add a blank line for spacing
                    Console.WriteLine();

                    // Print Progress Bar
                    Console.Write($"{centerPad}PROCESSING: [");
                    Console.ForegroundColor = ConsoleColor.Green;
                    for (int i = 0; i < 20; i++)
                    {
                        Console.Write("█");
                        System.Threading.Thread.Sleep(100);
                    }
                    Console.ResetColor();
                    Console.WriteLine("] 100%");
                }

                System.Threading.Thread.Sleep(500);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n\n\n\n\n");
                Console.WriteLine($"{pad}                            * * *");
                Console.WriteLine($"{pad}                          * \\    |   /   *");
                Console.WriteLine($"{pad}                         * - -  [ OK ] - - *");
                Console.WriteLine($"{pad}                          * /    |   \\   *");
                Console.WriteLine($"{pad}                            * * *");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{pad}╔═════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"{pad}║                      [ REGISTRATION SUCCESSFUL ]                    ║");
                Console.WriteLine($"{pad}╚═════════════════════════════════════════════════════════════════════╝");

                Console.ResetColor();
                string successMsg = $"Account created for: {newuser}";
                int msgPadding = Math.Max(0, (uiWidth - successMsg.Length) / 2);
                Console.WriteLine($"\n{new string(' ', leftPad + msgPadding)}{successMsg}");

                string continueMsg = "Tap to Continue...";
                int continuePadding = Math.Max(0, (uiWidth - continueMsg.Length) / 2);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n{new string(' ', leftPad + continuePadding)}{continueMsg}");
                Console.ResetColor();

                Console.ReadKey();
                regis = true; // Breaks the loop
                Console.Clear();
                mainmenu();
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
                int uiWidth = 75;
                int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                string pad = new string(' ', leftPad);
                // Format the player/points row to fit exactly inside the box
                // The inner width between ║ and ║ is 73 chars
                string playerSection = playerText.PadRight(38); // left column
                string pointsSection = pointsText.PadLeft(20);  // right column
                string innerRow = $"  {playerSection}{pointsSection}";

                // Trim or pad innerRow to exactly 73 chars so the closing ║ lines up
                innerRow = innerRow.PadRight(73).Substring(0, 73);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(pad + @"╔═════════════════════════════════════════════════════════════════════════╗");
                Console.Write(pad + @"║");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(innerRow);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"║");
                Console.WriteLine(pad + @"╠════════════════════╦════════════════════════════════════════════════════╣");
                Console.WriteLine(pad + @"║   .-----------.    ║                                                    ║");
                Console.WriteLine(pad + @"║   |   GAME    |    ║                                                    ║");
                Console.WriteLine(pad + @"║   |   MENU    |    ║        [ 1 ]  >  PLAY HANGMAN                      ║");
                Console.WriteLine(pad + @"║   |___________|    ║        [ 2 ]  >  READING COMPREHENSION             ║");
                Console.WriteLine(pad + @"║      _    _        ║        [ 3 ]  >  VIEW LEADERBOARD                  ║");
                Console.WriteLine(pad + @"║    _| |  | |_      ║        [ 4 ]  >  WORD DICTIONARY                   ║");
                Console.WriteLine(pad + @"║   |_(A)  (B)_|     ║        [ 5 ]  >  LOGOUT & RETURN                   ║");
                Console.WriteLine(pad + @"║     |_|  |_|       ║                                                    ║");
                Console.WriteLine(pad + @"║                    ║      >>  SELECT YOUR NEXT CHALLENGE  <<            ║");
                Console.WriteLine(pad + @"║                    ║                                                    ║");
                Console.WriteLine(pad + @"╚════════════════════╩════════════════════════════════════════════════════╝");
                Console.WriteLine();
                // --- INPUT PROMPT ---
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{pad}Enter your command (1-5) : ");
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
            int uiWidth = 71; // Total box width (69 internal + 2 borders)
            int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
            string pad = new string(' ', leftPad);

            // --- BUBBLE SORT (WITH DATA VALIDATION SAFEGUARDS) ---
            for (int i = 0; i < leaderboard.Length - 1; i++)
            {
                for (int j = 0; j < leaderboard.Length - 1 - i; j++)
                {
                    if (string.IsNullOrWhiteSpace(leaderboard[j]) || string.IsNullOrWhiteSpace(leaderboard[j + 1])) continue;

                    try
                    {
                        string[] partsA = leaderboard[j].Split(',');
                        string[] partsB = leaderboard[j + 1].Split(',');

                        if (partsA.Length < 2 || partsB.Length < 2) continue;

                        int pointsA = int.Parse(partsA[1].Trim());
                        int pointsB = int.Parse(partsB[1].Trim());

                        if (pointsA < pointsB)
                        {
                            string temp = leaderboard[j];
                            leaderboard[j] = leaderboard[j + 1];
                            leaderboard[j + 1] = temp;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();
            // --- HEADER ROW (Every internal line is exactly 69 characters long) ---
            Console.WriteLine($"{pad}╔═════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{pad}║              ___________                                            ║");
            Console.WriteLine($"{pad}║             '._==_==_=_.'       [ GLOBAL LEADERBOARD ]              ║");
            Console.WriteLine($"{pad}║             .-\\:      /-.       ======================              ║");
            Console.WriteLine($"{pad}║            | (|:.     |) |           TOP PLAYERS                    ║");
            Console.WriteLine($"{pad}║             '-|:.     |-'                                           ║");
            Console.WriteLine($"{pad}║               \\::.   //                                             ║");
            Console.WriteLine($"{pad}║                '::. .'                                              ║");
            Console.WriteLine($"{pad}║                 ) (                                                 ║");
            Console.WriteLine($"{pad}║               _.' '._                                               ║");
            Console.WriteLine($"{pad}║              `\"\"\"\"\"\"\"`                                              ║");
            Console.WriteLine($"{pad}╠═════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"{pad}║   RANK   │            USERNAME            │         TOTAL SCORE     ║");
            Console.WriteLine($"{pad}╠══════════╪════════════════════════════════╪═════════════════════════╣");

            Console.ResetColor();

            // --- PLAYERS (TOP 10 ONLY) ---
            int rank = 1;
            bool hasPlayers = false;

            for (int i = 0; i < leaderboard.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(leaderboard[i]))
                {
                    hasPlayers = true;
                    string[] parts = leaderboard[i].Split(',');

                    string name = parts[0].Trim();
                    string pts = (parts.Length > 1 ? parts[1].Trim() : "0") + " PTS";
                    if (name.Length > 26) name = name.Substring(0, 23) + "...";
                    string rankCell = $"  #{rank}".PadRight(10);
                    string nameCell = $"  {name}".PadRight(32);
                    int scorePadding = (25 - pts.Length) / 2;
                    string scoreCell = "".PadLeft(scorePadding) + pts + "".PadRight(25 - scorePadding - pts.Length);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{pad}║");
                    if (rank == 1) Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (rank == 2) Console.ForegroundColor = ConsoleColor.Gray;
                    else if (rank == 3) Console.ForegroundColor = ConsoleColor.DarkRed;
                    else Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(rankCell);
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("│");

                    if (rank <= 3) { /* keep rank color or go white for name */ }
                    Console.Write(nameCell);
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("│");
                    Console.Write(scoreCell);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("║");
                    rank++;
                    if (rank > 10) break;// limits the rankings to only 10
                }
            }
            if (!hasPlayers)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{pad}║");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("    [ NO RECORDS FOUND. BE THE FIRST TO SET A HIGH SCORE! ]    ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("║");
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{pad}╚══════════╧════════════════════════════════╧═════════════════════════╝");
            Console.WriteLine();
            string backMsg = "Tap to go back";
            int backPadding = (uiWidth - backMsg.Length) / 2;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{new string(' ', leftPad + backPadding)}{backMsg}");
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
            Console.WriteLine("_|_"); ;
        }

        static void HangmanMenu(string currentUser, Dictionary<string, (string type, string definition, string example)> wordDictionary)
        {

            bool exithangmanmenu = false;
            //CALLL OUT POINT HERE
            Console.Clear();
            while (!exithangmanmenu)
            {

                for (int wrongGuesses = 1; wrongGuesses <= 6;)
                {

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    int uiWidth = 63; // Total width of the box layout (61 internal + 2 borders)
                    int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                    string pad = new string(' ', leftPad);

                    string playerText = $"> USER: {currentUser}";
                    string pointsText = $"> SCORE: {points} PTS";

                    // 61 total internal width minus 4 spaces (2 on left padding, 2 on right padding)
                    int totalInnerWidth = 61;
                    int availableTextSpace = totalInnerWidth - 4;
                    int spaces = availableTextSpace - playerText.Length - pointsText.Length;
                    if (spaces < 0) spaces = 0;

                    // Outer Box Theme Color
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    // --- HEADER ROW ---
                    Console.WriteLine($"{pad}╔═════════════════════════════════════════════════════════════╗");
                    Console.Write($"{pad}║  ");

                    // Header Text Highlights
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(playerText);
                    Console.Write(new string(' ', spaces));
                    Console.Write(pointsText);

                    // Close Header Row
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("  ║"); // Removed the extra {pad} here

                    // --- MENU BODY ---
                    Console.WriteLine($"{pad}╠════════════╦════════════════════════════════════════════════╣");
                    Console.WriteLine($"{pad}║  +======+  ║                                                ║");
                    Console.WriteLine($"{pad}║  |      |  ║                                                ║");
                    Console.WriteLine($"{pad}║  O      |  ║             [ 1 ] > EASY (BEGINNER)            ║");
                    Console.WriteLine($"{pad}║ /|\\     |  ║             [ 2 ] > NORMAL (OPERATIVE)         ║"); // Used string interpolation smoothly
                    Console.WriteLine($"{pad}║ / \\     |  ║             [ 3 ] > HARD (NIGHTMARE)           ║");
                    Console.WriteLine($"{pad}║         |  ║             [ 4 ] > RETURN                     ║");
                    Console.WriteLine($"{pad}║         |  ║                                                ║");
                    Console.WriteLine($"{pad}║ ========╩= ║              »» SELECT DIFFICULTY ««           ║");
                    Console.WriteLine($"{pad}╚════════════╩════════════════════════════════════════════════╝");
                    Console.ResetColor(); // Good practice so subsequent inputs look normal
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{pad} Enter command (1-4) : ");
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
                            calcu();
                            break;

                        case "2":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n    Loading Normal Mode...");
                            System.Threading.Thread.Sleep(600);
                            points = HangmanNormal(currentUser, points, wordDictionary);
                            calcu();
                            break;

                        case "3":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n    Loading Hard Mode...");
                            System.Threading.Thread.Sleep(800);
                            points = HangmanHard(currentUser, points, wordDictionary);
                            calcu();
                            break;

                        case "4":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\n    Returning...");
                            System.Threading.Thread.Sleep(800);
                            gamemenu(wordDictionary);
                            return;

                        default:
                            Console.Clear();
                            Console.WriteLine("INVALID OPTION", "Please enter 0, 1, 2, or 3.");
                            exithangmanmenu = false;
                            continue;
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
                Console.WriteLine(@"  ║               Guess the right word by letter                ║");
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
                    Console.Clear();
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
                    Console.Clear();
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
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Enter '0' to go back to the gamemenu");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    > ENTER A LETTER : ");
                Console.ResetColor();
                string input = Console.ReadLine()?.ToLower();
                if (input == "0")
                {
                    gamemenu(wordDictionary);
                    playing = false;
                }
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
                Console.WriteLine(@"  ║               Guess the right word by letter                ║");
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
                    Console.Clear();
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
                    Console.Clear();
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
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Enter '0' to go back to gamemenu");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    > ENTER A LETTER : ");
                    Console.ResetColor();
                    string input = Console.ReadLine()?.ToLower();
                    if (input == "0")
                    {
                        gamemenu(wordDictionary);
                        playing = false;
                    }
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
                Console.WriteLine(@"  ║               Guess the right word by letter                ║");
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
                    points += 20; // Normal Mode Reward!
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
                    Console.WriteLine("\n                  REWARD: [ + 20 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Press ANY KEY to return to the Hangman Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    return points;
                }

                if (wrongGuesses >= 6)
                {
                    points -= 8;
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
                    Console.WriteLine("\n                  PENALTY: [ - 8 POINTS ]");
                    Console.ResetColor();
                    Console.WriteLine("\n\n          Press ANY KEY to return to the Hangman Menu...");
                    Console.ReadKey();
                    Console.Clear();
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
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Enter '0' to go back to gamemenu");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("    ▶ ENTER A LETTER : ");
                    Console.ResetColor();
                    string input = Console.ReadLine()?.ToLower();
                    if (input == "0")
                    {
                        gamemenu(wordDictionary);
                        playing = false;
                    }
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
                int uiWidth = 75;
                int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                string pad = new string(' ', leftPad);

                // ── Reading Comprehension Difficulty Menu ───────────────────────
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(pad + @"╔═════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(pad + @"║               [ READING COMPREHENSION ]                                 ║");
                Console.WriteLine(pad + @"╠════════════════════╦════════════════════════════════════════════════════╣");
                Console.WriteLine(pad + @"║   .-----------.    ║                                                    ║");
                Console.WriteLine(pad + @"║   |           |    ║      CHOOSE YOUR LEVEL OF DIFFICULTY               ║");
                Console.WriteLine(pad + @"║   |           |    ║                                                    ║");
                Console.WriteLine(pad + @"║   |___________|    ║      [ 1 ]  .  EASY LEVEL                          ║");
                Console.WriteLine(pad + @"║    ____|||||___    ║      [ 2 ]  .  MEDIUM LEVEL                        ║");
                Console.WriteLine(pad + @"║   |___________|    ║      [ 3 ]  .  HARD LEVEL                          ║");
                Console.WriteLine(pad + @"║                    ║      [ 4 ]  .  EXIT                                ║");
                Console.WriteLine(pad + @"║                    ║                                                    ║");
                Console.WriteLine(pad + @"║   =============    ║                                                    ║");
                Console.WriteLine(pad + @"╚════════════════════╩════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{pad}Enter your choice (1-4) : ");
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
                        Console.WriteLine(pad + "\n    [!] Invalid Option pls enter either 1, 2, 3 or 4");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                        try4 = false;
                        continue;
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

                int questionIndex = 0;
                while (questionIndex < questions.Count)
                {
                    var q = questions[questionIndex];

                    int uiWidth = 65;
                    int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                    string pad = new string(' ', leftPad);
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    string playerText = $"> USER: {currentUser}";
                    string pointsText = $"> SCORE: {points} PTS";
                    int spaces = 61 - 4 - playerText.Length - pointsText.Length;
                    if (spaces < 0) spaces = 0;

                    Console.Clear();
                    Console.WriteLine(pad + @"╔═════════════════════════════════════════════════════════════╗");
                    Console.Write(pad + @"║  ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(playerText);
                    Console.Write(new string(' ', spaces));
                    Console.Write(pointsText);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(@"  ║");
                    Console.WriteLine(pad + @"╠═════════════════════════════════════════════════════════════╣");
                    Console.WriteLine(pad + @"║                  [ READING COMPREHENSION ]                  ║");
                    Console.WriteLine(pad + @"╚═════════════════════════════════════════════════════════════╝");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(pad + $"       [ THE STORY : {title.ToUpper()} ]");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(pad + @"─────────────────────────────────────────────────────────────");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Gray;
                    List<string> wrappedStory = WordWrap(text, 60);
                    foreach (string line in wrappedStory)
                        Console.WriteLine(pad + "  " + line);
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(pad + @"─────────────────────────────────────────────────────────────");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(pad + @"[ QUESTION ]");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    List<string> wrappedQuestion = WordWrap($"Q: {q.Prompt}", 60);
                    foreach (var line in wrappedQuestion)
                        Console.WriteLine(pad + "  " + line);
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (var choice in q.Choices)
                        Console.WriteLine(pad + "    " + choice);
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(pad + @"─────────────────────────────────────────────────────────────");
                    Console.WriteLine(pad + "[ Type '0' to exit ]");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(pad + "> ENTER YOUR ANSWER (A/B/C/D) : ");
                    Console.ResetColor();
                    string input = Console.ReadLine()?.Trim().ToUpper();

                    if (input == "0")
                    {
                        goBackToStory = true;
                        gamemenu(wordDictionary);
                        break;
                    }

                    // ── Invalid input: do NOT advance, just re-show the same question ──
                    if (input != "A" && input != "B" && input != "C" && input != "D")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(pad + "\n\t\t\t input. Please enter A, B, C, or D.");
                        Console.ResetColor();
                        Console.WriteLine(pad + "  Press any key to try again...");
                        Console.ReadKey();
                        continue; // loops back to the same question
                    }

                    if (input[0] == q.Answer)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n {pad}[✓] CORRECT! The answer is {q.Answer}. (+{pointsIfPass} PTS)");
                        Console.ResetColor();
                        points += pointsIfPass;
                        score++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        points -= pointsifNot;
                        Console.WriteLine($"\n {pad}[X] INCORRECT! The correct answer was {q.Answer}. (-{pointsifNot} PTS)");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"\n {pad} Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();

                    questionIndex++; // only advance when a valid answer was given
                }
                if (!goBackToStory)
                {
                    Console.Clear();
                    int uiWidth = 71;
                    int leftPad = Math.Max(0, (Console.WindowWidth - uiWidth) / 2);
                    string pad = new string(' ', leftPad);
                    int topPad = Math.Max(0, (Console.WindowHeight - 12) / 2);
                    Console.SetCursorPosition(0, topPad);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(pad + @"╔═════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine(pad + @"║                                                                     ║");
                    Console.WriteLine(pad + @"║                  [ O V E R A L L   R E S U L T ]                    ║");
                    Console.WriteLine(pad + @"║                                                                     ║");
                    Console.WriteLine(pad + @"╠═════════════════════════════════════════════════════════════════════╣");
                    Console.WriteLine(pad + @"║                                                                     ║");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(pad + $"║   STORY TITLE  :  {title.PadRight(50)}║");
                    Console.WriteLine(pad + $"║   SCORE        :  {(score + " / " + questions.Count + " Correct").PadRight(50)}║");
                    Console.WriteLine(pad + $"║   TOTAL POINTS :  {(points.ToString() + " PTS").PadRight(50)}║");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(pad + @"║                                                                     ║");
                    Console.WriteLine(pad + @"╚═════════════════════════════════════════════════════════════════════╝");
                    if (score == questions.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(pad + @"                    STATUS: EXCELLENT ★");
                    }
                    else if (score > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(pad + @"                    STATUS: GOOD JOB");
                    }
                    else if (score == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(pad + @"                    STATUS: NICE TRY");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(pad + @"                    STATUS: FAILED");
                    }
                    Console.WriteLine(pad + "Press any key to back");
                    Console.ResetColor();
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
