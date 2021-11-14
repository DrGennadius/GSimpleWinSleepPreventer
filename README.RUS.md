# GSimpleWinSleepPreventer
Утилита для предотвращения засыпания Windows.

Утилита использует [SetThreadExecutionState](https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate) (kernel32).

**SetThreadExecutionState** используется для остановки тайм-аута машины и перехода в режим ожидания / выключения устройства отображения.

## Портативное автономное приложение

Команда для помощи:

```shell
GSimpleWinSleepPreventer.exe -h
```

![image](https://user-images.githubusercontent.com/27915885/141684747-f0d35e9d-c04a-4d04-a4b4-d2a89f78b450.png)

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

![image](https://user-images.githubusercontent.com/27915885/141684828-e40b7e3e-7ed7-43d0-ac24-88a7ffc5b38b.png)

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

![image](https://user-images.githubusercontent.com/27915885/141684880-a91feced-723b-42a5-8092-534e2dd296d0.png)

## Windows Сервис

Можно установить сервис на систему.

![image](https://user-images.githubusercontent.com/27915885/141693790-989c57f0-2fea-4a1a-b8dc-6fe25e667f90.png)

![image](https://user-images.githubusercontent.com/27915885/141693810-444e2567-4c22-413f-b0c6-f1cdd0b4abbd.png)

Это позволит автоматически запускаться при старте системы.

Есть возможность смотреть логи в Event Viewer.

![image](https://user-images.githubusercontent.com/27915885/141693948-e1b7a275-c34d-4c59-80fd-c48fda3d2f98.png)
