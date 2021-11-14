using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;

namespace GSimpleWinSleepPreventer.Service
{
    public partial class GSimpleWinSleepPreventerService : ServiceBase
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        const string _eventSourceName = "GSimpleWinSleepPreventerService";
        const string _logName = "GSimpleWinSleepPreventerServiceLog";

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

        readonly Dictionary<string, EXECUTION_STATE> ExecutionStateMap = new Dictionary<string, EXECUTION_STATE>()
        {
            { "away", EXECUTION_STATE.ES_AWAYMODE_REQUIRED },
            { "continuous", EXECUTION_STATE.ES_CONTINUOUS },
            { "display", EXECUTION_STATE.ES_DISPLAY_REQUIRED },
            { "system", EXECUTION_STATE.ES_SYSTEM_REQUIRED }
        };

        public GSimpleWinSleepPreventerService()
        {
            InitializeComponent();

            eventLog = new EventLog();
            if (!EventLog.SourceExists(_eventSourceName))
            {
                EventLog.CreateEventSource(_eventSourceName, _logName);
            }
            eventLog.Source = _eventSourceName;
            eventLog.Log = _logName;
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry($"Starting Simple Windows Sleep Preventer by Gennadius (Gennady Zykov). Version {Assembly.GetExecutingAssembly().GetName().Version}.\n");
            bool isSuccess = true;
            if (args.Length == 0)
            {
                eventLog.WriteEntry("No arguments set. Will use PreventSleep mode.", EventLogEntryType.Warning);
                args = new string[] { "-s" };
            }
            if (args.Length == 1)
            {
                isSuccess = ParseSingleParam(args[0]);
            }
            else
            {
                isSuccess = ParseMultipleParams(args);
            }

            if (isSuccess)
            {
                eventLog.WriteEntry("Executing...");
            }
            else
            {
                eventLog.WriteEntry("Failed.", EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Stopped.");
        }

        bool ParseSingleParam(string arg)
        {
            bool isSuccess = true;
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
                default:
                    eventLog.WriteEntry($"{arg} is an unknown command.", EventLogEntryType.Error);
                    isSuccess = false;
                    break;
            }
            return isSuccess;
        }

        bool ParseMultipleParams(string[] args)
        {
            bool isSuccess = true;
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
                        eventLog.WriteEntry($"{args[i]} is an unknown key.", EventLogEntryType.Error);
                        isSuccess = false;
                        break;
                    }
                }
                if (isSuccess)
                {
                    string logMessagePartMessage = $"Manual set of the execution states [{string.Join(", ", args.Skip(1))}]";
                    InternalSetThreadExecutionState(logMessagePartMessage, executionStateMode);
                }
            }
            else
            {
                eventLog.WriteEntry($"{args[0]} is an unknown command.", EventLogEntryType.Error);
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Disable away mode and allow powerdown.
        /// </summary>
        void AllowPowerdown()
        {
            InternalSetThreadExecutionState("Allow Powerdown",
                EXECUTION_STATE.ES_CONTINUOUS);
        }

        /// <summary>
        /// Prevent only monitor powerdown.
        /// </summary>
        void PreventMonitorPowerdown()
        {
            InternalSetThreadExecutionState("Prevent Monitor Powerdown",
                EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }

        /// <summary>
        /// Prevent Idle-to-Sleep (monitor not affected).
        /// </summary>
        void PreventSleep()
        {
            InternalSetThreadExecutionState("Prevent Sleep",
                EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        /// <summary>
        /// Enable away mode and prevent the sleep idle time-out.
        /// </summary>
        void KeepSystemAwake()
        {
            InternalSetThreadExecutionState("Keep System Awake",
                EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        void InternalSetThreadExecutionState(string logMessagePart, EXECUTION_STATE executionStates)
        {
            try
            {
                SetThreadExecutionState(executionStates);
                eventLog.WriteEntry($"{logMessagePart} ON.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"An error has occurred while setting system state.\n{ex}", EventLogEntryType.Error);
            }
        }
    }
}
