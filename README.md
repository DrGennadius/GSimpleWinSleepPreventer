# GSimpleWinSleepPreventer
A utility for preventing Windows from falling asleep.

The utility uses [SetThreadExecutionState](https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate) (kernel32).

**SetThreadExecutionState** is used to stop the machine timing out and entering standby/switching the display device off.

**You can set the several manual modes of EXECUTION_STATE:**

1. **away** (ES_AWAYMODE_REQUIRED)
2. **continuous** (ES_CONTINUOUS)
3. **display** (ES_DISPLAY_REQUIRED)
4. **system** (ES_SYSTEM_REQUIRED)

**You can set one of the quick modes:**

1. **monitor** - prevent only monitor powerdown.
2. **sleep** - prevent Idle-to-Sleep (monitor not affected).
3. **awake** - enable away mode and prevent the sleep idle time-out.
