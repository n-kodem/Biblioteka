# Opis aplikacji:

Aplikacja biblioteki pozwala na zarządzanie katalogiem książek dostępnych, listą czytelników, oraz wypożyczeniami w przykładowej bibliotece.

# Funkcjonalności:

- Wyszukiwanie książek po id, tytule, autorze lub gatunku
- Wyszukiwanie czytelników po id, imieniu, nazwisku, emailu, płci i adresie
- Wyszukiwanie wyporzyczeń po imieniu, nazwisku, książce, dacie wypożyczenia i dacie oddania
- Dodawanie nowych pozycji do bazy
- Obsługa wypożyczeń i zwrotów książek
- Wyświetlanie Książek, Czytelników i Wyporzyczeń w formie tabeli

# Technologie:

Aplikacja została napisana w języku C# z wykorzystaniem technologii WPF wraz z pluginem Material Design do tworzenia interfejsu użytkownika. Do przechowywania danych została użyta baza danych MS SQL.
Instalacja i uruchomienie

1. Pobierz plik zip programu dostępny w zakładce release.
2. Rozpakuj skompresowany plik.
3. Uruchom plik wykonywalny .exe programu.

# Użytkowanie:
Po uruchomieniu aplikacji przed użytkownikiem wyświetli się panel logowania, należy tam wpisać dane logowania, a następnie kliknąć przycisk zaloguj się (dane domyślne *login*:**admin**, *hasło*: **admin**)

Po zalogowaniu się, użytkownik zostanie przeniesiony do głównego okna programu. Z górnego menu można wybrać interesującą nas opcję (np. wyszukiwanie książek, dodawanie nowych pozycji itp.).

Aby wyszukać książkę, należy wpisać odpowiednie słowo kluczowe w polu wyszukiwania, wyszukiwanie rozpocznie się automatycznie. Rezultaty wyszukiwania zostaną wyświetlone w tabeli panelu.

# Diagram ERD bazy danych:
```mermaid
erDiagram
    books ||--o{ borrowings : "can be"
    books {
        primary_key_int id
        string tytul
        string autor
        string gatunek
    }
    
    borrowings {
        primary_key_int id
        foreign_key_int bookId
        foreign_key_int userId
        string borrowDate
        string returnDate
    }
    readers ||--o{ borrowings : has
    readers {
        primary_key_int id
        string imie
        string nazwisko
        string email
        string plec
        string adres
    }
    users{
        primary_key_int id
        string login
        string haslo
        string decoder
        bool isAdmin
    }
```
# Schemat UI
```mermaid
graph TD
    A[Panel Logowania] <--> B[Panel główny]
    B <--> C(Użytkownik wybiera kontrolkę)
    C <-->|1| D[Baza biblioteki]
    C <-->|2| E[Baza czytelników]
    C <-->|3| F[Wypożyczenia]
    C <-->|4| G[Ustawienia]
    C <-->|5| H[Wypożyczenie książki]
    C <-->|6| I[Dodaj książkę]
    C <-->|7| J[Dodaj czytelnika]
    C <-->|8| K[Dodaj użytkownika]
```
# Testy:
## Sprawdzenie, czy użytkownik może się zalogować z nieprawidłowymi danymi
![Image alt text](/testy_zdjecia/login_inv.png "Invalid login")
## Sprawdzenie, czy użytkownik może dodać książki
![Image alt text](/testy_zdjecia/adding_book.png "Adding book")
## Sprawdzenie, czy użytkownik może dodać czytelników
![Image alt text](/testy_zdjecia/adding_user.png "Adding user")
## Sprawdzenie, czy program prawidłowo wyświetla książki
![Image alt text](/testy_zdjecia/bookbase_view.png "Bookbase view")
## Sprawdzenie, czy program prawidłowo wyświetla wypożyczenia
![Image alt text](/testy_zdjecia/borrowings_view.png "Borrowings view")
## Sprawdzenie, czy program prawidłowo czytelników książki
![Image alt text](/testy_zdjecia/readers_view.png "Readers view")
## Sprawdzenie, czy program prawidłowo wyświetla ustawienia
![Image alt text](/testy_zdjecia/settings_view.png "Settings view")

