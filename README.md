# GSimpleWinSleepPreventer
A utility for preventing Windows from falling asleep.

See doc for Russian: [README.RUS.md](https://github.com/DrGennadius/GSimpleWinSleepPreventer/blob/master/README.RUS.md).

The utility uses [SetThreadExecutionState](https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate) (kernel32).

**SetThreadExecutionState** is used to stop the machine timing out and entering standby/switching the display device off.

## Portable standalone application

Command for help:

```shell
GSimpleWinSleepPreventer.exe -h
```

![image](https://user-images.githubusercontent.com/27915885/141684747-f0d35e9d-c04a-4d04-a4b4-d2a89f78b450.png)

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

![image](https://user-images.githubusercontent.com/27915885/141684828-e40b7e3e-7ed7-43d0-ac24-88a7ffc5b38b.png)

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

4. **full** - enable full prevent. Uses all modes (display, system, away) in continuous: `ES_CONTINUOUS`, `ES_DISPLAY_REQUIRED`, `ES_SYSTEM_REQUIRED` and `ES_AWAYMODE_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -f
```

![image](https://user-images.githubusercontent.com/27915885/141684880-a91feced-723b-42a5-8092-534e2dd296d0.png)
