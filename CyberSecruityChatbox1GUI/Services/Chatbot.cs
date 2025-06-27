using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class Chatbot
    {
        private string _userName;
        private bool _isRunning;
        private string _userInterest = "";
        private List<string> _previousTopics = new List<string>();

        private readonly ConsoleColor _botColor = ConsoleColor.Yellow;
        private readonly ConsoleColor _headerColor = ConsoleColor.Cyan;
        private readonly ConsoleColor _warningColor = ConsoleColor.Red;
        private readonly ConsoleColor _tipColor = ConsoleColor.Green;

        // Keyword to Response mapping (Unit 1)
        private readonly Dictionary<string, Action> _keywordActions;

        // Random phishing tips (Unit 2)
        private readonly List<string> _phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Never click suspicious links, especially from unknown senders.",
            "Check the email sender’s address carefully — small spelling differences can indicate fraud.",
            "Hover over links before clicking to see where they really lead.",
            "Report phishing attempts to phishing@sabric.co.za."
        };

        public Chatbot()
        {
            _keywordActions = new Dictionary<string, Action>
            {
                { "password", ShowPasswordTips },
                { "phish", ShowRandomPhishingTip },
                { "scam", ShowRandomPhishingTip },
                { "privacy", ShowPrivacyTips },
                { "social", ShowSocialMediaSafety },
                { "media", ShowSocialMediaSafety },
                { "brows", ShowBrowsingSafety },
                { "internet", ShowBrowsingSafety }
            };
        }

        public void Run()
        {
            InitializeChatbot();
            MainInteractionLoop();
            ShowExitMessage();
        }

        private void InitializeChatbot()
        {
            _isRunning = true;
            SetupConsole();
            ShowWelcomeScreen();
            GetUserName();
        }

        private void SetupConsole()
        {
            Console.Title = "SA Cybersecurity Chatbot";
            Console.WindowWidth = Math.Min(100, Console.LargestWindowWidth);
            Console.Clear();
        }

        private void ShowWelcomeScreen()
        {
            DisplayAsciiArt();
            Console.WriteLine("\nWelcome to the South African Cybersecurity Awareness Assistant!");
            Console.WriteLine("I'm here to help you stay safe online.\n");
        }

        private void DisplayAsciiArt()
        {
            Console.ForegroundColor = _headerColor;
            Console.WriteLine(@"
 
_________        ___.                 _________                         .__  __          
\_   ___ \___.__.\_ |__   ___________/   _____/ ____   ___________ __ __|__|/  |_ ___.__.
/    \  \<   |  | | __ \_/ __ \_  __ \_____  \_/ __ \_/ ___\_  __ \  |  \  \   __<   |  |
\     \___\___  | | \_\ \  ___/|  | \/        \  ___/\  \___|  | \/  |  /  ||  |  \___  |
 \______  / ____| |___  /\___  >__| /_______  /\___  >\___  >__|  |____/|__||__|  / ____|
        \/\/          \/     \/             \/     \/     \/                      \/     
            ");
            Console.ResetColor();
        }

        private void GetUserName()
        {
            do
            {
                Console.Write("Before we begin, what should I call you? ");
                _userName = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(_userName))
                {
                    Console.WriteLine("Please enter a valid name.");
                }

            } while (string.IsNullOrWhiteSpace(_userName));

            Console.WriteLine($"\nHello, {_userName}! Let's learn about cybersecurity.\n");
        }

        private void MainInteractionLoop()
        {
            while (_isRunning)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{_userName}, what would you like to know about? (type 'help' for options) ");
                Console.ResetColor();

                var input = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;
                ProcessInput(input);
            }
        }

        private void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ShowDefaultResponse();
                return;
            }

            // Sentiment detection (Unit 5)
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                TypeResponse("It's completely understandable to feel that way. Cyber threats are real, but you're taking the right step by learning!");
            }
            else if (input.Contains("curious") || input.Contains("interested"))
            {
                TypeResponse("Curiosity is the first step to being cyber smart!");
            }
            else if (input.Contains("frustrated") || input.Contains("angry"))
            {
                TypeResponse("I'm here to help, don't worry! Let's tackle cybersecurity one step at a time.");
            }

            // Exit
            if (input.Contains("exit") || input.Contains("quit"))
            {
                _isRunning = false;
                return;
            }

            // Help
            if (input.Contains("help"))
            {
                ShowHelpMenu();
                return;
            }

            // Greeting
            if (input.Contains("hello") || input.Contains("hi"))
            {
                TypeResponse($"Hi {_userName}! How can I assist you today?");
                return;
            }

            // Memory recognition (Unit 4)
            if (input.Contains("i'm interested in"))
            {
                var interest = input.Replace("i'm interested in", "").Trim();
                if (!string.IsNullOrWhiteSpace(interest))
                {
                    _userInterest = interest;
                    TypeResponse($"Great! I'll remember that you're interested in {interest}. It's a crucial part of cybersecurity.");
                    return;
                }
            }

            // Reuse memory
            if (input.Contains("remind me") && !string.IsNullOrEmpty(_userInterest))
            {
                TypeResponse($"Earlier you mentioned you're interested in {_userInterest}. Let's dive into that!");
                return;
            }

            // Keyword recognition
            bool foundMatch = false;
            foreach (var keyword in _keywordActions.Keys)
            {
                if (input.Contains(keyword))
                {
                    _keywordActions[keyword].Invoke();
                    _previousTopics.Add(keyword);
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                ShowDefaultResponse();
            }
        }

        // Randomized phishing tip (Unit 2)
        private void ShowRandomPhishingTip()
        {
            var rand = new Random();
            var tip = _phishingTips[rand.Next(_phishingTips.Count)];
            TypeResponse("⚠️ Phishing Tip:");
            Console.ForegroundColor = _warningColor;
            Console.WriteLine("• " + tip + "\n");
            Console.ResetColor();
        }

        private void ShowPasswordTips()
        {
            TypeResponse("💻 Important Password Safety Tips:");
            Console.ForegroundColor = _tipColor;
            Console.WriteLine("• Use at least 12 characters with a mix of letters, numbers, and symbols");
            Console.WriteLine("• Never reuse passwords across different sites");
            Console.WriteLine("• Consider a password manager like Bitwarden or LastPass");
            Console.WriteLine("• Enable two-factor authentication (2FA)");
            Console.WriteLine("• Change passwords after any data breach");
            Console.WriteLine("• Use https://haveibeenpwned.com to check breaches\n");
            Console.ResetColor();
        }

        private void ShowPrivacyTips()
        {
            TypeResponse("🔒 Privacy Protection Tips:");
            Console.ForegroundColor = _tipColor;
            Console.WriteLine("• Review app permissions regularly");
            Console.WriteLine("• Limit what you share on public platforms");
            Console.WriteLine("• Enable encryption on your devices");
            Console.WriteLine("• Use privacy-focused tools like DuckDuckGo or ProtonMail\n");
            Console.ResetColor();
        }

        private void ShowBrowsingSafety()
        {
            TypeResponse("🌐 Safe Browsing Practices:");
            Console.ForegroundColor = _tipColor;
            Console.WriteLine("• Always check for 🔒 and 'https://' before entering sensitive info");
            Console.WriteLine("• Use updated browsers with security features");
            Console.WriteLine("• Install reputable antivirus software");
            Console.WriteLine("• Be cautious with public WiFi - use a VPN");
            Console.WriteLine("• Test internet safety using MyBroadband\n");
            Console.ResetColor();
        }

        private void ShowSocialMediaSafety()
        {
            TypeResponse("📱 Social Media Security Tips:");
            Console.ForegroundColor = _tipColor;
            Console.WriteLine("• Review privacy settings regularly");
            Console.WriteLine("• Be wary of 'too good to be true' offers");
            Console.WriteLine("• Don't overshare personal information");
            Console.WriteLine("• Watch for fake profiles - SA romance scams are common\n");
            Console.ResetColor();
        }

        private void ShowHelpMenu()
        {
            TypeResponse("🔍 I can help with these topics:");
            Console.WriteLine("- 'Password safety' - Creating secure credentials");
            Console.WriteLine("- 'Phishing tips' - Spotting scam attempts");
            Console.WriteLine("- 'Privacy' - Protecting your data");
            Console.WriteLine("- 'Browsing safety' - Secure internet use");
            Console.WriteLine("- 'Social media' - Avoiding oversharing");
            Console.WriteLine("- Say 'I'm interested in privacy' to save your preferences");
            Console.WriteLine("- Type 'exit' to leave the chat\n");
        }

        private void ShowDefaultResponse()
        {
            TypeResponse("I'm not sure I understand. Can you try rephrasing?");
            Console.WriteLine("- Type 'help' to see available topics");
            Console.WriteLine("- Ask about 'passwords', 'phishing', 'privacy', or 'social media'\n");
        }

        private void TypeResponse(string message)
        {
            Console.ForegroundColor = _botColor;
            Console.Write("Bot: ");
            Console.ResetColor();

            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(20);
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Console.Write(message.Substring(message.IndexOf(c) + 1));
                    break;
                }
            }
            Console.WriteLine();
        }

        private void ShowExitMessage()
        {
            Console.ForegroundColor = _headerColor;
            Console.WriteLine($"\nThank you, {_userName}, for learning about cybersecurity!");
            Console.WriteLine("Remember to stay vigilant online in South Africa.");
            Console.WriteLine("Report cybercrime to https://www.cert.gov.za\n");
            Console.ResetColor();
        }
    }
}
