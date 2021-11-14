# GSimpleWinSleepPreventer
Утилита для предотвращения засыпания Windows.

Утилита использует [SetThreadExecutionState](https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate) (kernel32).

**SetThreadExecutionState** используется для остановки тайм-аута машины и перехода в режим ожидания / выключения устройства отображения.

Команда для помощи:

```shell
GSimpleWinSleepPreventer.exe -h
```

**Можно установить несколько ручных режимов EXECUTION_STATE:**

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

Можно использовать несколько режимов:

```shell
GSimpleWinSleepPreventer.exe -es continuous display system
```

**Вы можете установить один из быстрых режимов:**

1. **monitor** - предотвратить только отключение монитора. Используются режимы: `ES_CONTINUOUS` и `ES_DISPLAY_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -m
```

2. **sleep** - предотвратить переход в спящий режим (не влияет на монитор). Используются режимы: `ES_CONTINUOUS` и `ES_AWAYMODE_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -s
```

3. **awake** - включить режим отсутствия и предотвратить таймаут простоя сна. Используются режимы: `ES_CONTINUOUS` и `ES_SYSTEM_REQUIRED`.

```shell
GSimpleWinSleepPreventer.exe -a
```
