::кодування
chcp 65001
::2 і 3 завдання
if not exist %1 (
    echo File not found.
    set opened=створено
) else (
    echo File found.
    set opened=відкрито
)

::4 завдання
echo %date% %TIME:~0,5% >%1
echo Файл %1 %opened% >>%1

::5 завдання
start w32tm /resync
echo Оновлений час: %date% %TIME:~0,5% >>%1

::6 завдання
tasklist >>%1
taskkill /im %3

::7 завдання
if "%ERRORLEVEL%" EQU "0" (
   echo Файл %3 успішно завершено >>%1
) else (
   echo Помилка. Файл %3 не завершено>>%1
)

::8 і 9 завдання
echo Видалення файлів, які мають розширення .TMP, або їх ім’я починається на «temp» в %2:>>%1
set deleted=0
for %%f in ("%2\*.tmp", "%2\temp*.*") do (
   echo %%f >>%1
   set /a deleted=deleted+1
   del %%f
)
echo Видалено %deleted% файл(и)>>%1

::10 і 11 завдання
set zippath=%4\%DATE:~6,4%-%DATE:~3,2%-%DATE:~0,2%_%TIME:~0,2%-%TIME:~3,2%.zip
"C:\Program Files\7-Zip\7z.exe" a -tzip "%zippath%" "%~2\*.*"
if "%ERRORLEVEL%" EQU "0" (
   echo Створено архів %zippath%>>%1
) else (
   echo Архів не створено>>%1
)

::12 і 13 завдання
ForFiles /p %4 /s /d -1 /c "cmd /c echo @file">>%1
if "%ERRORLEVEL%" EQU "0" (
   echo Вчорашній архів існує>>%1
) else (
   echo Вчорашній архів відсутній>>%1
   echo %date% %TIME:~0,5%>>"email.txt"
   echo Вчорашній архів відсутній>>"email.txt"
)

::14 завдання
ForFiles /p %4 /s /d -30 /c "cmd /c del @file"
if "%ERRORLEVEL%" EQU "0" (
   echo Всі архіви старші за 30 днів видалено>>%1
) else (
   echo Архівів старших за 30 днів не знайдено>>%1
)

::15 завдання
ping "google.com"
if "%ERRORLEVEL%" EQU "0" (
   echo Підключення до інтернету присутнє>>%1
) else (
   echo Підключення до інтернету відсутнє>>%1
)

::16 завдання
ipconfig | findstr /i "%5"
if %errorlevel% equ 0 (
  echo IP-адреса знайдена, комп'ютер вимкнено>>%1
  shutdown /s
) else (
  echo IP-адреса не знайдена>>%1
)

::17 завдання
echo Список комп'ютерів в мережі:>>%1
arp -a>>%1

::18 завдання
for /f %%i in ('findstr /R [0-9].[0-9].[0-9].[0-9] ipon.txt') do (
    ipconfig | findstr /i "%%i">nul
)
if %errorlevel% equ 1 (
    echo Не усі IP-адреси з ipon.txt наявні у системі>>%1
    echo Не усі IP-адреси з ipon.txt наявні у системі>>"email.txt"
)

::19 завдання
if %~z1 GTR %6 (
    echo log.log занадто великий, %~z1 байт>>%1
    echo log.log занадто великий, %~z1 байт>>"email.txt"
)

::20 завдання
wmic logicaldisk get size,freespace,caption |findstr.exe /r /c:".*">>%1

::21 завдання
set systeminfoPath=systeminfoPath-%DATE:~6,4%-%DATE:~3,2%-%DATE:~0,2%_%TIME:~0,2%-%TIME:~3,2%.txt
systeminfo >%systeminfoPath%

