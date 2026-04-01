using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(e => e.Miasto == "Warsaw")
            .Select(e => $"{e.NumerIndeksu}, {e.Imie} {e.Nazwisko}, {e.Miasto}");
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(e => $"{e.Email}");
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(e => e.Nazwisko)
            .ThenBy(e => e.Imie)
            .Select(e => $"{e.NumerIndeksu}, {e.Imie} {e.Nazwisko}");
    }
  /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
  
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        return DaneUczelni.Przedmioty
            .Where(e => e.Kategoria == "Analytics")
            .Select(e => $"{e.Nazwa}, {e.DataStartu}")
            .Take(1)
            .DefaultIfEmpty("Brak przedmiotu z kategorii Analytics");
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        bool istnieje = DaneUczelni.Zapisy.Any(e => !e.CzyAktywny);
        return new[] {istnieje ? "Tak" : "Nie"};
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        bool wszyscyMajaKatedry = DaneUczelni.Prowadzacy
            .All(e => !string.IsNullOrWhiteSpace(e.Katedra));
        return new[] {wszyscyMajaKatedry ? "Tak" : "Nie"};
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        int countCzyAktywny = DaneUczelni.Zapisy.Count(e => e.CzyAktywny);
        return  new[] {countCzyAktywny.ToString()};
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(e => e.Miasto)
            .Distinct()
            .OrderBy(e => e);

    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(e =>
                $"Data zapisu: {e.DataZapisu:yyyy-MM-dd}, StudentId: {e.StudentId}, PrzedmiotId: {e.PrzedmiotId}");
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        const int numerStrony = 2;
        const int rozmiarStrony = 2;

        return DaneUczelni.Przedmioty
            .OrderBy(e => e.Nazwa)
            .Skip((numerStrony - 1) * rozmiarStrony)
            .Take(rozmiarStrony)
            .Select(e => $"{e.Nazwa}, {e.Kategoria}");
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy,
                student => student.Id,
                zapisy => zapisy.StudentId,
                ((student, zapis) => $"{student.Imie}, {student.Nazwisko}, {zapis.DataZapisu:yyyy-MM-dd}")
            );
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Studenci
            .SelectMany(student => DaneUczelni.Zapisy.Where(zapis => zapis.StudentId == student.Id),
                (student, zapis) => new { Student = student, Zapis = zapis })
            .Join(DaneUczelni.Przedmioty,
                e => e.Zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (x, przedmiot) => $"{x.Student.Imie}, {x.Student.Nazwisko}, {przedmiot.Nazwa}");
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => przedmiot.Nazwa)
            .GroupBy(nazwaPrzedmiotu => nazwaPrzedmiotu)
            .OrderBy(grupa => grupa.Key)
            .Select(grupa => $"{grupa.Key}, {grupa.Count()}");

    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(e => e.OcenaKoncowa.HasValue)
            .Join(DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => new
                {
                    przedmiot.Nazwa,
                    Ocena = zapis.OcenaKoncowa!.Value
                })
            .GroupBy(p => p.Nazwa)
            .OrderBy(grupa => grupa.Key)
            .Select(grupa => $"{grupa.Key}, {grupa.Average(x => x.Ocena)}");
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                prowadzacy => prowadzacy.Id,
                przedmioty => przedmioty.ProwadzacyId,
                (prowadzacy, przedmioty) => $"{prowadzacy.Imie}, {prowadzacy.Nazwisko}, {przedmioty.Count()}");
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy
                    .Where(e => e.OcenaKoncowa.HasValue),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new
                {
                    student.Id,
                    student.Imie,
                    student.Nazwisko,
                    Ocena = zapis.OcenaKoncowa!.Value
                })
            .GroupBy(x => new {x.Id, x.Imie, x.Nazwisko})
            .OrderBy(grupa => grupa.Key.Nazwisko)
            .ThenBy(grupa => grupa.Key.Imie)
            .Select(grupa => $"{grupa.Key.Imie} {grupa.Key.Nazwisko}, {grupa.Max(x => x.Ocena)}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy.Where(e => e.CzyAktywny),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new
                {
                    student.Imie,
                    student.Nazwisko
                })
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Where(grupa => grupa.Count() > 1)
            .OrderBy(grupa => grupa.Key.Nazwisko)
            .ThenBy(grupa => grupa.Key.Nazwisko)
            .Select(grupa => $"{grupa.Key.Imie} {grupa.Key.Nazwisko}, {grupa.Count()}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .Join(
                DaneUczelni.Zapisy,
                przedmioty => przedmioty.Id,
                zapis => zapis.PrzedmiotId,
                (przedmiot, zapis) => new
                {
                    przedmiot.Nazwa,
                    zapis.OcenaKoncowa
                })
            .GroupBy(x => x.Nazwa)
            .Where(grupa => grupa.All(x => !x.OcenaKoncowa.HasValue))
            .Select(grupa => grupa.Key);
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                prowadzacy => prowadzacy.Id,
                przedmioty => przedmioty.ProwadzacyId,
                (prowadzacy, przedmioty) => new
                {
                    Prowadzacy = prowadzacy,
                    Oceny = przedmioty
                        .Join(
                            DaneUczelni.Zapisy,
                            przedmiot => przedmiot.Id,
                            zapis => zapis.PrzedmiotId,
                            (przedmiot, zapis) => zapis.OcenaKoncowa)
                        .Where(ocena => ocena.HasValue)
                        .Select(ocena => ocena!.Value)
                        .ToList()
                })
            .Select(x => x.Oceny.Any()
                ? $"{x.Prowadzacy.Imie} {x.Prowadzacy.Nazwisko},  {x.Oceny.Average()}"
                : $"{x.Prowadzacy.Imie} {x.Prowadzacy.Nazwisko},  brak ocen" );
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                studenci => studenci.Id,
                zapis => zapis.StudentId,
                (student, zapis) => student.Miasto)
            .GroupBy(miasto => miasto)
            .OrderByDescending(grupa => grupa.Count())
            .ThenBy(grupa => grupa.Key)
            .Select(grupa => $"{grupa.Key}: {grupa.Count()}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
