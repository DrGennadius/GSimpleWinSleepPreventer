using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GSimpleWinSleepPreventer
{
    class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            /// <summary>
            /// Enables away mode. This value must be specified with ES_CONTINUOUS.
            /// Away mode should be used only by media-recording and media-distribution applications that must perform critical background processing on desktop computers while the computer appears to be sleeping.
            /// </summary>
            ES_AWAYMODE_REQUIRED = 0x00000040,

            /// <summary>
            /// Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
            /// </summary>
            ES_CONTINUOUS = 0x80000000,

            /// <summary>
            /// Forces the display to be on by resetting the display idle timer.
            /// </summary>
            ES_DISPLAY_REQUIRED = 0x00000002,

            /// <summary>
            /// Forces the system to be in the working state by resetting the system idle timer.
            /// </summary>
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        readonly static Dictionary<string, EXECUTION_STATE> ExecutionStateMap = new Dictionary<string, EXECUTION_STATE>()
        {
            { "away", EXECUTION_STATE.ES_AWAYMODE_REQUIRED },
            { "continuous", EXECUTION_STATE.ES_CONTINUOUS },
            { "display", EXECUTION_STATE.ES_DISPLAY_REQUIRED },
            { "system", EXECUTION_STATE.ES_SYSTEM_REQUIRED }
        };

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine($"Simple Windows Sleep Preventer by Gennadius (Gennady Zykov). Version {Assembly.GetExecutingAssembly().GetName().Version}.\n");
            bool isShowHelpOnly = false;
            if (args.Length == 1)
            {
                isShowHelpOnly = ParseSingleParam(args[0]);
            }
            else if (args.Length == 0)
            {
                isShowHelpOnly = true;
            }
            else
            {
                isShowHelpOnly = ParseMultipleParams(args);
            }

            if (isShowHelpOnly)
            {
                PrintHelp();
            }
            else
            {
                Console.WriteLine("Type Enter to allow powerdown.");
                Console.ReadLine();
                AllowPowerdown();
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Help.");
            Console.WriteLine("\nQuick modes:");
            Console.WriteLine("  --monitor, -m    Prevent only monitor powerdown.");
            Console.WriteLine("  --sleep, -s      Prevent Idle-to-Sleep (monitor not affected).");
            Console.WriteLine("  --awake, -a      Keep system awake.");
            Console.WriteLine("  --full, -f       Uses all modes (display, system, away).");
            Console.WriteLine("  --help, -h       Show help.");
            Console.WriteLine("\nManually set several modes:");
            Console.WriteLine("  --ExecutionState, -es [modes]  Available modes: continuous, display, system, away.");
            Console.WriteLine("      continuous - Informs the system that the state being set should remain in");
            Console.WriteLine("                   effect until the next call that uses ES_CONTINUOUS and one of");
            Console.WriteLine("                   the other state flags is cleared.");
            Console.WriteLine("      display    - Forces the display to be on by resetting the display idle timer.");
            Console.WriteLine("      system     - Forces the system to be in the working state by resetting the");
            Console.WriteLine("                   system idle timer.");
            Console.WriteLine("      away       - Enables away mode. This value must be specified with ES_CONTINUOUS.");
            Console.WriteLine("                   Away mode should be used only by media-recording and");
            Console.WriteLine("                   media-distribution applications that must perform critical");
            Console.WriteLine("                   background processing on desktop computers while the computer");
            Console.WriteLine("                   appears to be sleeping.");
        }

        static bool ParseSingleParam(string arg)
        {
            bool isShowHelpOnly = false;
            switch (arg)
            {
                case "--monitor":
                case "-m":
                    PreventMonitorPowerdown();
                    break;
                case "--sleep":
                case "-s":
                    PreventSleep();
                    break;
                case "--awake":
                case "-a":
                    KeepSystemAwake();
                    break;
                case "--full":
                case "-f":
                    FullPrevent();
                    break;
                case "--help":
                case "-h":
                    isShowHelpOnly = true;
                    break;
                default:
                    Console.WriteLine($"{arg} is an unknown command.");
                    isShowHelpOnly = true;
                    break;
            }
            return isShowHelpOnly;
        }

        static bool ParseMultipleParams(string[] args)
        {
            bool isShowHelpOnly = false;
            if (args[0] == "--ExecutionState" || args[0] == "-es")
            {
                EXECUTION_STATE executionStateMode = 0;
                for (int i = 1; i < args.Length; i++)
                {
                    if (ExecutionStateMap.ContainsKey(args[i]))
                    {
                        executionStateMode |= ExecutionStateMap[args[i]];
                    }
                    else
                    {
                        Console.WriteLine($"{args[i]} is an unknown key.");
                        isShowHelpOnly = true;
                        break;
                    }
                }
                if (!isShowHelpOnly)
                {
                    string consoleMessage = $"Manual set of the execution states [{string.Join(", ", args.Skip(1))}]";
                    InternalSetThreadExecutionState(consoleMessage, executionStateMode);
                }
            }
            else
            {
                Console.WriteLine($"{args[0]} is an unknown command.");
                isShowHelpOnly = true;
            }
            return isShowHelpOnly;
        }

        /// <summary>
        /// Disable away mode and allow powerdown.
        /// </summary>
        static void AllowPowerdown()
        {
            InternalSetThreadExecutionState("Allow Powerdown",
                EXECUTION_STATE.ES_CONTINUOUS);
        }

        /// <summary>
        /// Prevent only monitor powerdown.
        /// </summary>
        static void PreventMonitorPowerdown()
        {
            InternalSetThreadExecutionState("Prevent Monitor Powerdown",
                EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }

        /// <summary>
        /// Prevent Idle-to-Sleep (monitor not affected).
        /// </summary>
        static void PreventSleep()
        {
            InternalSetThreadExecutionState("Prevent Sleep",
                EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        /// <summary>
        /// Enable away mode and prevent the sleep idle time-out.
        /// </summary>
        static void KeepSystemAwake()
        {
            InternalSetThreadExecutionState("Keep System Awake",
                EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        /// <summary>
        /// Full Prevent. Uses all modes (display, system, away) in continuous.
        /// </summary>
        static void FullPrevent()
        {
            InternalSetThreadExecutionState("Full Prevent",
                EXECUTION_STATE.ES_CONTINUOUS 
                | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                | EXECUTION_STATE.ES_SYSTEM_REQUIRED
                | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        static void InternalSetThreadExecutionState(string consoleMessagePart, EXECUTION_STATE executionStates)
        {
            try
            {
                SetThreadExecutionState(executionStates);
                Console.WriteLine($"{consoleMessagePart} ON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred while setting system state.\n{ex}");
            }
        }
    }
}
