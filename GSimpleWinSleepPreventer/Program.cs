using System;
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
            /// Away mode should be used only by media-recording and media-distribution applications that must perform critical background processing on desktop computers while the computer appears to be sleeping. See Remarks.
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

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine($"Simple Windows Sleep Preventer by Gennadius (Gennady Zykov). Version {Assembly.GetExecutingAssembly().GetName().Version}.\n");
            bool isShowHelpOnly = false;
            if (args.Length == 1)
            {
                switch (args[0])
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
                    case "--help":
                    case "-h":
                        PrintHelp();
                        break;
                    default:
                        Console.WriteLine($"{args[0]} is an unknown command.");
                        isShowHelpOnly = true;
                        break;
                }
            }
            else
            {
                isShowHelpOnly = true;
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
            Console.WriteLine("Help:");
            Console.WriteLine("  --monitor, -m    Prevent only monitor powerdown.");
            Console.WriteLine("  --sleep, -s      Prevent Idle-to-Sleep (monitor not affected).");
            Console.WriteLine("  --awake, -a      Keep system awake.");
            Console.WriteLine("  --help, -h       Show help.");
        }

        /// <summary>
        /// Disable away mode and allow powerdown.
        /// </summary>
        static void AllowPowerdown()
        {
            Console.WriteLine("Allow Powerdown ON.");
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        /// <summary>
        /// Prevent only monitor powerdown.
        /// </summary>
        static void PreventMonitorPowerdown()
        {
            Console.WriteLine("Prevent Monitor Powerdown ON.");
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }

        /// <summary>
        /// Prevent Idle-to-Sleep (monitor not affected).
        /// </summary>
        static void PreventSleep()
        {
            Console.WriteLine("Prevent Sleep ON.");
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        /// <summary>
        /// Enable away mode and prevent the sleep idle time-out.
        /// </summary>
        static void KeepSystemAwake()
        {
            Console.WriteLine("Keep System Awake ON.");
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }
    }
}
