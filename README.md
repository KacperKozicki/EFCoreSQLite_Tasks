# Aplikacja EFCore_Tasks

Aplikacja **EFCore_Tasks** to przykładowa aplikacja desktopowa, która umożliwia zarządzanie zadaniami oraz ich punktami. Aplikacja została napisana w języku C# przy użyciu platformy .NET i technologii Entity Framework Core.

## Wymagania

Aby uruchomić aplikację **EFCore_Tasks**, musisz mieć zainstalowane następujące elementy:

- Visual Studio (wersja 2019 lub nowsza)
- .NET 5.0 SDK lub nowszy

## Instrukcje instalacji

1. Pobierz folder z plikiem instalacyjnym aplikacji **EFCore_Tasks** dla systemu Windows z repozytorium o nazwie [SETUP](https://github.com/USER/SETUP).

2. Uruchom pobrany plik instalacyjny.

3. Postępuj zgodnie z krokami instalatora, akceptując warunki licencji i wybierając lokalizację instalacji.

4. Mogą zostać zgłoszone ostrzeżenia o szkodliwym oprogramowaniu (proszę je zignorować).

5. Po zakończeniu instalacji, otwórz ścieżkę aplikacji **EFCore_Tasks** i uruchom jako administrator plik `EFCoreSQLite_Tasks.GUI.exe`.

## Instrukcje użytkowania

1. Po uruchomieniu aplikacji zostanie wyświetlone okno logowania. Aby zalogować się, wprowadź poprawne dane logowania (login i hasło) i kliknij przycisk "Zaloguj". Istniejące konta (login, hasło):
   - admin, admin
   - kierownik, kierownik
   - pracownik, pracownik
   - gosc, gosc

2. Po poprawnym zalogowaniu zostanie otwarte okno główne aplikacji, które zawiera listę zadań dla danego użytkownika (wyjątkiem są konta admin oraz kierownik). Możesz dodawać nowe zadania, usuwać istniejące zadania oraz edytować ich szczegóły. Dodawanie użytkowników dotyczy konta z uprawnieniami.

3. Po wybraniu zadania z listy i kliknięciu na nie, zostanie otwarte okno ze szczegółami zadania. Możesz dodawać, usuwać i edytować punkty zadania oraz monitorować postęp zadania.

4. W celu zamknięcia aplikacji, kliknij przycisk "X" w prawym górnym rogu okna lub wybierz opcję "Zamknij" z menu kontekstowego aplikacji.

