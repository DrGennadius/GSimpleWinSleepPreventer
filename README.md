# GSimpleWinSleepPreventer
A utility for preventing Windows from falling asleep.

The utility uses SetThreadExecutionState (kernel32).

**It is possible to set several modes:**

1. Prevent only monitor powerdown.
2. Prevent Idle-to-Sleep (monitor not affected).
3. Enable away mode and prevent the sleep idle time-out.
