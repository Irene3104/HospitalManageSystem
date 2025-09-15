using System;

namespace DotnetHospital.Services
{
    /// <summary>
    /// Static utility class for console user interface operations
    /// Provides methods for headers, menus, messages, input handling, and table formatting
    /// </summary>
    public static class ConsoleUI
    {
        #region Header Panels

        /// <summary>
        /// Draws a bordered header with system title and optional section title
        /// </summary>
        /// <param name="systemTitle">Main system title</param>
        /// <param name="sectionTitle">Optional section title</param>
        /// <param name="systemColor">Color for system title</param>
        /// <param name="sectionColor">Color for section title</param>
        public static void DrawHeader(string systemTitle, string sectionTitle = null,
                                      ConsoleColor systemColor = ConsoleColor.White,
                                      ConsoleColor sectionColor = ConsoleColor.Cyan)
        {
            Console.Clear();

            int innerWidth = Math.Max(systemTitle.Length, sectionTitle?.Length ?? 0) + 4;

            // Draw top border
            Console.Write('┌');
            Console.Write(new string('─', innerWidth));
            Console.WriteLine('┐');

            // Draw system title
            Console.Write('│');
            WriteCentered(systemTitle, innerWidth, systemColor);
            Console.WriteLine('│');

            if (!string.IsNullOrEmpty(sectionTitle))
            {
                // Draw separator line
                Console.Write('│');
                Console.Write(new string('-', innerWidth));
                Console.WriteLine('│');

                // Draw section title
                Console.Write('│');
                WriteCentered(sectionTitle, innerWidth, sectionColor);
                Console.WriteLine('│');
            }

            // Draw bottom border
            Console.Write('└');
            Console.Write(new string('─', innerWidth));
            Console.WriteLine('┘');

            Console.WriteLine();
        }

        #endregion

        #region Menu Operations

        /// <summary>
        /// Draws a numbered menu with options
        /// </summary>
        /// <param name="menuTitle">Title of the menu</param>
        /// <param name="options">Array of menu options</param>
        /// <param name="titleColor">Color for the menu title</param>
        /// <param name="includeZeroExit">Whether to include "0. Exit" option</param>
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

        #endregion

        #region Message Display

        /// <summary>
        /// Logs a message with specified color
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="color">Color for the message</param>
        public static void Log(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prev;
        }

        /// <summary>
        /// Displays an error message in red
        /// </summary>
        /// <param name="message">Error message to display</param>
        public static void Error(string message)
        {
            Log("[ERROR] " + message, ConsoleColor.Red);
        }

        /// <summary>
        /// Displays a warning message in yellow
        /// </summary>
        /// <param name="message">Warning message to display</param>
        public static void Warning(string message)
        {
            Log("[WARNING] " + message, ConsoleColor.Yellow);
        }

        #endregion

        #region Input Helpers

        /// <summary>
        /// Reads password input with masked characters
        /// </summary>
        /// <returns>Password string entered by user</returns>
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
                        pw = pw.Substring(0, pw.Length - 1); // .NET Framework compatible
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

        /// <summary>
        /// Pauses execution and waits for user input
        /// </summary>
        /// <param name="msg">Message to display before pausing</param>
        public static void Pause(string msg = "Press any key to return...")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(msg);
            Console.ResetColor();
            Console.ReadKey(true);
        }

        #endregion

        #region Table Helpers

        /// <summary>
        /// Computes flexible column width to fit console width
        /// </summary>
        /// <param name="fixedColumnsTotal">Total width of fixed columns</param>
        /// <param name="separatorsCount">Number of column separators</param>
        /// <param name="min">Minimum column width</param>
        /// <param name="max">Maximum column width</param>
        /// <returns>Calculated column width</returns>
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

        /// <summary>
        /// Truncates text with ellipsis if it exceeds specified width
        /// </summary>
        /// <param name="text">Text to truncate</param>
        /// <param name="width">Maximum width</param>
        /// <returns>Truncated text with ellipsis if needed</returns>
        public static string Truncate(string text, int width)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (width <= 0) return string.Empty;
            if (text.Length <= width) return text;
            if (width <= 3) return text.Substring(0, width);
            return text.Substring(0, width - 3) + "...";
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Writes centered text within specified width
        /// </summary>
        /// <param name="text">Text to center</param>
        /// <param name="width">Total width for centering</param>
        /// <param name="color">Color for the text</param>
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

        #endregion
    }
}
