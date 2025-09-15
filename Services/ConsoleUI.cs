using System;
using System.Collections.Generic;
using System.Linq;
using DotnetHospital.Entities;

namespace DotnetHospital.Services
{
    /// <summary>
    /// Static utility class for console user interface operations
    /// Provides methods for headers, menus, messages, input handling, and table formatting
    /// </summary>
    public static class ConsoleUI
    {
        #region Table Width Constants

        /// <summary>
        /// Table types for different display contexts
        /// </summary>
        public enum TableType
        {
            DoctorDetails,
            PatientDetails,
            DoctorPatientList,
            AppointmentList
        }

        /// <summary>
        /// Standard column widths for consistent table formatting
        /// </summary>
        public static class TableWidths
        {
            // Basic column widths
            public const int Name = 20;
            public const int Email = 25;
            public const int Phone = 15;
            public const int Patient = 20;
            public const int Doctor = 20;
            
            // Flexible column width ranges
            public const int AddressMin = 20;
            public const int AddressMax = 60;
            public const int DescriptionMin = 15;
            public const int DescriptionMax = 60;
            
            // Separator and margin constants
            public const int SeparatorWidth = 3; // " | " width
            public const int ConsoleMargin = 2;  // Small margin for console width
        }

        /// <summary>
        /// Constants for table column separators and formatting
        /// </summary>
        public static class TableFormatting
        {
            public const string ColumnSeparator = " | ";
            public const char HeaderSeparator = '-';
            public const string Ellipsis = "...";
            public const int MinEllipsisWidth = 3;
        }

        /// <summary>
        /// Constants for input validation and user interaction
        /// </summary>
        public static class InputConstants
        {
            public const string BackCommand = "b";
            public const string PasswordMask = "*";
            public const int MinPasswordLength = 0;
            public const int MaxRetryAttempts = 3;
        }

        /// <summary>
        /// Constants for error handling and user feedback
        /// </summary>
        public static class ErrorMessages
        {
            public const string GenericError = "An unexpected error occurred. Please try again.";
            public const string InvalidInput = "Invalid input. Please check your entry and try again.";
            public const string DataNotFound = "The requested information could not be found.";
            public const string OperationFailed = "The operation could not be completed.";
            public const string SystemError = "A system error occurred. Please contact support.";
        }

        /// <summary>
        /// Constants for success messages and confirmations
        /// </summary>
        public static class SuccessMessages
        {
            public const string OperationCompleted = "Operation completed successfully!";
            public const string DataSaved = "Data has been saved successfully.";
            public const string DataLoaded = "Data loaded successfully.";
        }

        /// <summary>
        /// Gets appropriate column widths for different table types
        /// </summary>
        /// <param name="tableType">Type of table being displayed</param>
        /// <returns>Tuple containing (nameWidth, emailWidth, phoneWidth, addressWidth)</returns>
        public static (int nameWidth, int emailWidth, int phoneWidth, int addressWidth) GetColumnWidths(TableType tableType)
        {
            int nameWidth = TableWidths.Name;
            int emailWidth = TableWidths.Email;
            int phoneWidth = TableWidths.Phone;
            int addressWidth = ComputeFlexibleWidth(nameWidth + emailWidth + phoneWidth, 3, TableWidths.AddressMin, TableWidths.AddressMax);
            
            return (nameWidth, emailWidth, phoneWidth, addressWidth);
        }

        /// <summary>
        /// Gets appropriate column widths for patient-doctor tables
        /// </summary>
        /// <param name="tableType">Type of table being displayed</param>
        /// <returns>Tuple containing (patientWidth, doctorWidth, emailWidth, phoneWidth, addressWidth)</returns>
        public static (int patientWidth, int doctorWidth, int emailWidth, int phoneWidth, int addressWidth) GetPatientDoctorColumnWidths(TableType tableType)
        {
            int patientWidth = TableWidths.Patient;
            int doctorWidth = TableWidths.Doctor;
            int emailWidth = TableWidths.Email;
            int phoneWidth = TableWidths.Phone;
            int addressWidth = ComputeFlexibleWidth(patientWidth + doctorWidth + emailWidth + phoneWidth, 4, TableWidths.AddressMin, TableWidths.AddressMax);
            
            return (patientWidth, doctorWidth, emailWidth, phoneWidth, addressWidth);
        }

        /// <summary>
        /// Gets appropriate column widths for appointment tables
        /// </summary>
        /// <param name="tableType">Type of table being displayed</param>
        /// <returns>Tuple containing (doctorWidth, patientWidth, descriptionWidth)</returns>
        public static (int doctorWidth, int patientWidth, int descriptionWidth) GetAppointmentColumnWidths(TableType tableType)
        {
            int doctorWidth = TableWidths.Doctor;
            int patientWidth = TableWidths.Patient;
            int descriptionWidth = ComputeFlexibleWidth(doctorWidth + patientWidth, 2, TableWidths.DescriptionMin, TableWidths.DescriptionMax);
            
            return (doctorWidth, patientWidth, descriptionWidth);
        }

        #endregion

        #region Table Display Methods

        /// <summary>
        /// Creates and displays a table header with specified column names and widths
        /// </summary>
        /// <param name="columnNames">Array of column header names</param>
        /// <param name="columnWidths">Array of corresponding column widths</param>
        private static void DisplayTableHeader(string[] columnNames, int[] columnWidths)
        {
            if (columnNames == null || columnWidths == null)
                throw new ArgumentNullException("Column names and widths cannot be null");
            
            if (columnNames.Length != columnWidths.Length)
                throw new ArgumentException("Column names and widths arrays must have the same length");

            if (columnNames.Length == 0)
                throw new ArgumentException("At least one column must be specified");

            try
            {
                var headerParts = new string[columnNames.Length];
                for (int i = 0; i < columnNames.Length; i++)
                {
                    headerParts[i] = (columnNames[i] ?? "").PadRight(Math.Max(columnWidths[i], 1));
                }
                
                string header = string.Join(TableFormatting.ColumnSeparator, headerParts);
                Console.WriteLine(header);
                Console.WriteLine(new string(TableFormatting.HeaderSeparator, header.Length));
            }
            catch (Exception ex)
            {
                Error($"Failed to display table header: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a formatted table row with specified values and widths
        /// </summary>
        /// <param name="values">Array of values to display</param>
        /// <param name="columnWidths">Array of corresponding column widths</param>
        /// <returns>Formatted table row string</returns>
        private static string CreateTableRow(string[] values, int[] columnWidths)
        {
            if (values == null || columnWidths == null)
                throw new ArgumentNullException("Values and widths cannot be null");
            
            if (values.Length != columnWidths.Length)
                throw new ArgumentException("Values and widths arrays must have the same length");

            try
            {
                var rowParts = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    rowParts[i] = (values[i] ?? "").PadRight(Math.Max(columnWidths[i], 1));
                }
                
                return string.Join(TableFormatting.ColumnSeparator, rowParts);
            }
            catch (Exception ex)
            {
                Error($"Failed to create table row: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Displays a table with doctor details
        /// </summary>
        /// <param name="doctors">List of doctors to display</param>
        /// <param name="title">Table title</param>
        public static void DisplayDoctorTable(IEnumerable<Doctor> doctors, string title = "Doctors")
        {
            if (doctors == null)
            {
                Error(ErrorMessages.DataNotFound);
                return;
            }

            var doctorList = doctors.ToList();
            if (doctorList.Count == 0)
            {
                Warning("No doctors found.");
                return;
            }

            try
            {
                var (nameWidth, emailWidth, phoneWidth, addressWidth) = GetColumnWidths(TableType.DoctorDetails);
                
                // Display table header
                string[] columnNames = { "Name", "Email Address", "Phone", "Address" };
                int[] columnWidths = { nameWidth, emailWidth, phoneWidth, addressWidth };
                DisplayTableHeader(columnNames, columnWidths);

                // Display table rows
                foreach (var doctor in doctorList)
                {
                    if (doctor == null) continue; // Skip null doctors
                    
                    var formattedAddress = Truncate(doctor.GetFormattedAddress() ?? "", addressWidth);
                    string[] values = { 
                        doctor.DisplayName ?? "Unknown", 
                        doctor.Email ?? "", 
                        doctor.Phone ?? "", 
                        formattedAddress 
                    };
                    Console.WriteLine(CreateTableRow(values, columnWidths));
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failed to display doctor table: {ex.Message}", true);
            }
        }

        /// <summary>
        /// Displays a table with patient-doctor information
        /// </summary>
        /// <param name="patients">List of patients to display</param>
        /// <param name="doctors">List of doctors for lookup</param>
        /// <param name="title">Table title</param>
        public static void DisplayPatientDoctorTable(IEnumerable<Patient> patients, IEnumerable<Doctor> doctors, string title = "Patients")
        {
            var patientList = patients.ToList();
            var doctorList = doctors.ToList();
            
            if (patientList.Count == 0)
            {
                Warning("No patients found.");
                return;
            }

            var (patientWidth, doctorWidth, emailWidth, phoneWidth, addressWidth) = GetPatientDoctorColumnWidths(TableType.DoctorPatientList);
            
            // Display table header
            string[] columnNames = { "Patient", "Doctor", "Email Address", "Phone", "Address" };
            int[] columnWidths = { patientWidth, doctorWidth, emailWidth, phoneWidth, addressWidth };
            DisplayTableHeader(columnNames, columnWidths);
            
            // Display table rows
            foreach (var patient in patientList)
            {
                var doctor = doctorList.FirstOrDefault(d => d.Id == patient.DoctorId);
                var doctorName = doctor?.DisplayName ?? "Not assigned";
                var formattedAddress = Truncate(patient.GetFormattedAddress(), addressWidth);
                string[] values = { patient.Name, doctorName, patient.Email, patient.Phone, formattedAddress };
                Console.WriteLine(CreateTableRow(values, columnWidths));
            }
        }

        /// <summary>
        /// Displays a table with appointment information
        /// </summary>
        /// <param name="appointments">List of appointments to display</param>
        /// <param name="doctors">List of doctors for lookup</param>
        /// <param name="patients">List of patients for lookup</param>
        /// <param name="title">Table title</param>
        public static void DisplayAppointmentTable(IEnumerable<Appointment> appointments, IEnumerable<Doctor> doctors, IEnumerable<Patient> patients, string title = "Appointments")
        {
            var appointmentList = appointments.ToList();
            var doctorList = doctors.ToList();
            var patientList = patients.ToList();
            
            if (appointmentList.Count == 0)
            {
                Warning("No appointments found.");
                return;
            }

            var (doctorWidth, patientWidth, descriptionWidth) = GetAppointmentColumnWidths(TableType.AppointmentList);
            
            // Display table header
            string[] columnNames = { "Doctor", "Patient", "Description" };
            int[] columnWidths = { doctorWidth, patientWidth, descriptionWidth };
            DisplayTableHeader(columnNames, columnWidths);
            
            // Display table rows
            foreach (var appointment in appointmentList)
            {
                var doctor = doctorList.FirstOrDefault(d => d.Id == appointment.DoctorId);
                var patient = patientList.FirstOrDefault(p => p.Id == appointment.PatientId);
                var doctorName = doctor?.DisplayName ?? "Unknown Doctor";
                var patientName = patient?.Name ?? "Unknown Patient";
                var note = Truncate(appointment.Note ?? string.Empty, descriptionWidth);
                string[] values = { doctorName, patientName, note };
                Console.WriteLine(CreateTableRow(values, columnWidths));
            }
        }

        #endregion

        #region Error Handling Methods

        /// <summary>
        /// Executes an operation with comprehensive error handling
        /// </summary>
        /// <param name="operation">The operation to execute</param>
        /// <param name="operationName">Name of the operation for error messages</param>
        /// <param name="showRetryOption">Whether to show retry option on failure</param>
        /// <returns>True if operation succeeded, false otherwise</returns>
        public static bool ExecuteWithErrorHandling(Action operation, string operationName = "Operation", bool showRetryOption = true)
        {
            int attempts = 0;
            while (attempts < InputConstants.MaxRetryAttempts)
            {
                try
                {
                    operation();
                    return true;
                }
                catch (ArgumentException ex)
                {
                    Error($"Invalid input: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Error($"Operation not allowed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    LogError(ex, operationName);
                }

                attempts++;
                if (attempts < InputConstants.MaxRetryAttempts && showRetryOption)
                {
                    if (!AskForRetry(operationName))
                        return false;
                }
            }

            Error($"Failed to complete {operationName} after {InputConstants.MaxRetryAttempts} attempts.");
            return false;
        }

        /// <summary>
        /// Executes an operation with error handling and returns a result
        /// </summary>
        /// <typeparam name="T">Type of result to return</typeparam>
        /// <param name="operation">The operation to execute</param>
        /// <param name="operationName">Name of the operation for error messages</param>
        /// <param name="defaultValue">Default value to return on failure</param>
        /// <returns>Result of operation or default value on failure</returns>
        public static T ExecuteWithErrorHandling<T>(Func<T> operation, string operationName = "Operation", T defaultValue = default(T))
        {
            try
            {
                return operation();
            }
            catch (ArgumentException ex)
            {
                Error($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Error($"Operation not allowed: {ex.Message}");
            }
            catch (Exception ex)
            {
                LogError(ex, operationName);
            }

            return defaultValue;
        }

        /// <summary>
        /// Logs detailed error information for debugging
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        /// <param name="operationName">Name of the operation that failed</param>
        private static void LogError(Exception exception, string operationName)
        {
            // In a real application, this would log to a file or database
            // For now, we'll show a user-friendly message
            Error($"{ErrorMessages.OperationFailed} ({operationName})");
            
            // In development, you might want to show more details
            #if DEBUG
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Debug Info: {exception.GetType().Name}: {exception.Message}");
            Console.ResetColor();
            #endif
        }

        /// <summary>
        /// Asks user if they want to retry a failed operation
        /// </summary>
        /// <param name="operationName">Name of the operation that failed</param>
        /// <returns>True if user wants to retry, false otherwise</returns>
        private static bool AskForRetry(string operationName)
        {
            Console.WriteLine();
            Console.Write($"Would you like to retry {operationName}? (y/n): ");
            var response = Console.ReadLine()?.Trim().ToLower();
            return response == "y" || response == "yes";
        }

        /// <summary>
        /// Displays a user-friendly error message with optional retry option
        /// </summary>
        /// <param name="message">Error message to display</param>
        /// <param name="showRetry">Whether to show retry option</param>
        public static void ShowError(string message, bool showRetry = false)
        {
            Error(message);
            if (showRetry)
            {
                Console.Write("Would you like to try again? (y/n): ");
                var response = Console.ReadLine()?.Trim().ToLower();
                if (response == "y" || response == "yes")
                {
                    Console.WriteLine("Please try again...");
                }
            }
        }

        /// <summary>
        /// Displays a success message with optional pause
        /// </summary>
        /// <param name="message">Success message to display</param>
        /// <param name="pauseAfter">Whether to pause after displaying message</param>
        public static void ShowSuccess(string message, bool pauseAfter = true)
        {
            Log(message, ConsoleColor.Green);
            if (pauseAfter)
            {
                Pause();
            }
        }

        #endregion

        #region Input Validation Methods

        /// <summary>
        /// Gets and validates a numeric ID input from user
        /// </summary>
        /// <param name="prompt">Prompt message to display</param>
        /// <param name="allowBack">Whether to allow 'b' for back</param>
        /// <returns>Valid ID or -1 if cancelled/back</returns>
        public static int GetValidIdInput(string prompt, bool allowBack = true)
        {
            while (true)
            {
                DisplayInputPrompt(prompt, allowBack);
                var input = Console.ReadLine();
                
                if (IsBackCommand(input, allowBack))
                    return -1;
                
                if (IsEmptyInput(input))
                    continue;
                
                if (TryParseId(input, out int id))
                    return id;
            }
        }

        /// <summary>
        /// Displays input prompt with optional back command hint
        /// </summary>
        private static void DisplayInputPrompt(string prompt, bool allowBack)
        {
            Console.Write(prompt);
            if (allowBack)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" ('{InputConstants.BackCommand}' to go back)");
                Console.ResetColor();
            }
            Console.Write(": ");
        }

        /// <summary>
        /// Checks if input is a back command
        /// </summary>
        private static bool IsBackCommand(string input, bool allowBack)
        {
            return allowBack && !string.IsNullOrWhiteSpace(input) && 
                   input.Trim().Equals(InputConstants.BackCommand, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if input is empty and shows error if so
        /// </summary>
        private static bool IsEmptyInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Error(ErrorMessages.InvalidInput + " ID cannot be empty.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to parse input as ID and shows error if invalid
        /// </summary>
        private static bool TryParseId(string input, out int id)
        {
            if (int.TryParse(input, out id))
                return true;
            
            Error(ErrorMessages.InvalidInput + " Please enter digits only.");
            return false;
        }

        /// <summary>
        /// Gets and validates a required text input from user
        /// </summary>
        /// <param name="prompt">Prompt message to display</param>
        /// <param name="fieldName">Name of the field for error messages</param>
        /// <returns>Valid non-empty text input</returns>
        public static string GetRequiredTextInput(string prompt, string fieldName = "Field")
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                {
                    Error($"{fieldName} cannot be empty.");
                    continue;
                }
                
                return input.Trim();
            }
        }

        /// <summary>
        /// Gets and validates an optional text input from user
        /// </summary>
        /// <param name="prompt">Prompt message to display</param>
        /// <returns>Text input or empty string</returns>
        public static string GetOptionalTextInput(string prompt)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            return input?.Trim() ?? string.Empty;
        }

        #endregion

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
                    Console.Write(InputConstants.PasswordMask);
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
            
            int separatorWidth = separatorsCount * TableWidths.SeparatorWidth;
            
            // Fallback target if console width cannot be obtained
            int fallbackTarget = fixedColumnsTotal + separatorWidth + max;
            int targetWidth = consoleWidth > 0 ? Math.Max(0, consoleWidth - TableWidths.ConsoleMargin) : fallbackTarget;
            
            int remainingWidth = targetWidth - (fixedColumnsTotal + separatorWidth);
            return Math.Min(max, Math.Max(min, remainingWidth));
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
            return text.Substring(0, width - TableFormatting.MinEllipsisWidth) + TableFormatting.Ellipsis;
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
