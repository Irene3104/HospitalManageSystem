using System;

namespace DotnetHospital.Services
{
    public static class ConsoleUI
    {
        // ===== HEADER PANELS =====

        // General header (with system title + optional section)
        public static void DrawHeader(string systemTitle, string sectionTitle = null,
                                      ConsoleColor systemColor = ConsoleColor.White,
                                      ConsoleColor sectionColor = ConsoleColor.Cyan)
        {
            Console.Clear();

            int innerWidth = Math.Max(systemTitle.Length, sectionTitle?.Length ?? 0) + 4;

            // Top border (left aligned)
            Console.Write('┌');
            Console.Write(new string('─', innerWidth));
            Console.WriteLine('┐');

            // System title
            Console.Write('│');
            WriteCentered(systemTitle, innerWidth, systemColor);
            Console.WriteLine('│');

            if (!string.IsNullOrEmpty(sectionTitle))
            {
                // Separator
                Console.Write('│');
                Console.Write(new string('-', innerWidth));
                Console.WriteLine('│');

                // Section title
                Console.Write('│');
                WriteCentered(sectionTitle, innerWidth, sectionColor);
                Console.WriteLine('│');
            }

            // Bottom border
            Console.Write('└');
            Console.Write(new string('─', innerWidth));
            Console.WriteLine('┘');

            Console.WriteLine();
        }

        // ===== MENUS =====
        public static void DrawMenu(string menuTitle, string[] options, ConsoleColor titleColor = ConsoleColor.Green, bool includeZeroExit = true)
        {
            Console.ForegroundColor = titleColor;
            Console.WriteLine(menuTitle);
            Console.ResetColor();

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            if (includeZeroExit)
            {
                Console.WriteLine("0. Exit");
            }
            Console.WriteLine();
        }

        // ===== MESSAGES =====
        public static void Log(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
        }

        public static void Error(string message)
        {
            Log("[ERROR] " + message, ConsoleColor.Red);
        }

        public static void Warning(string message)
        {
            Log("[WARNING] " + message, ConsoleColor.Yellow);
        }

        // ===== INPUT HELPERS =====
        public static string ReadPassword()
        {
            string pw = "";
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (pw.Length > 0)
                    {
                        pw = pw.Substring(0, pw.Length - 1); // .NET Framework 호환
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pw += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return pw;
        }

        public static void Pause(string msg = "Press any key to return...")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(msg);
            Console.ResetColor();
            Console.ReadKey(true);
        }

        // ===== TABLE HELPERS =====
        // Computes a flexible last-column width so that the header and rows fit the console width
        public static int ComputeFlexibleWidth(int fixedColumnsTotal, int separatorsCount, int min = 15, int max = 60)
        {
            int consoleWidth = 0;
            try { consoleWidth = Console.WindowWidth; } catch { consoleWidth = 0; }
            int sepWidth = separatorsCount * 3; // " | " per separator
            // Fallback target if console width cannot be obtained
            int fallback = fixedColumnsTotal + sepWidth + max;
            int target = consoleWidth > 0 ? Math.Max(0, consoleWidth - 2) : fallback; // small margin
            int remaining = target - (fixedColumnsTotal + sepWidth);
            return Math.Min(max, Math.Max(min, remaining));
        }

        // Safe truncation with ellipsis when content exceeds width
        public static string Truncate(string text, int width)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (width <= 0) return string.Empty;
            if (text.Length <= width) return text;
            if (width <= 3) return text.Substring(0, width);
            return text.Substring(0, width - 3) + "...";
        }

        // ===== INTERNAL =====
        private static void WriteCentered(string text, int width, ConsoleColor color)
        {
            int pad = Math.Max(0, (width - text.Length) / 2);
            int rightPad = Math.Max(0, width - pad - text.Length);

            Console.Write(new string(' ', pad));
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = prev;
            Console.Write(new string(' ', rightPad));
        }
    }
}
