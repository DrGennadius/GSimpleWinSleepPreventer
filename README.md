# GSimpleWinSleepPreventer
A utility for preventing Windows from falling asleep.

See doc for Russian: README.RUS.md.

The utility uses [SetThreadExecutionState](https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate) (kernel32).

**SetThreadExecutionState** is used to stop the machine timing out and entering standby/switching the display device off.

Command for help:

```shell
GSimpleWinSleepPreventer.exe -h
```

**You can set the several manual modes of EXECUTION_STATE:**

1. **away** (`ES_AWAYMODE_REQUIRED`)

```shell
GSimpleWinSleepPreventer.exe -es away
```

2. **continuous** (`ES_CONTINUOUS`)

```shell
GSimpleWinSleepPreventer.exe -es continuous
```

3. **display** (`ES_DISPLAY_REQUIRED`)

```shell
GSimpleWinSleepPreventer.exe -es display
```

4. **system** (`ES_SYSTEM_REQUIRED`)

```shell
GSimpleWinSleepPreventer.exe -es system
```

You can use multiple modes:

```shell
GSimpleWinSleepPreventer.exe -es continuous display system
```

**You can set one of the quick modes:**

1. **monitor** - prevent only monitor powerdown. Modes in use: `ES_CONTINUOUS` and `ES_DISPLAY_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -m
```

2. **sleep** - prevent Idle-to-Sleep (monitor not affected). Modes in use: `ES_CONTINUOUS` and `ES_AWAYMODE_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -s
```

3. **awake** - enable away mode and prevent the sleep idle time-out. Modes in use: `ES_CONTINUOUS` and `ES_SYSTEM_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -a
```
