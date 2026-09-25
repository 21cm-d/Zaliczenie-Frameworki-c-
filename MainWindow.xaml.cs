using BazaCzesciSamochodowych.Data;
using BazaCzesciSamochodowych.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace BazaCzesciSamochodowych;

public partial class MainWindow : Window
{
    private readonly AppDbContext db = new();

    public MainWindow()
    {
        InitializeComponent();
        db.Database.EnsureCreated();
        DodajDanePrzykladowe();
        WczytajCzesci();
    }

    private void DodajDanePrzykladowe()
    {
        if (db.Czesc.Any())
            return;

        db.Czesc.AddRange(
            new Czesc
            {
                Nazwa = "Klocki hamulcowe",
                Producent = "ATE",
                Marka = "BMW",
                Model = "E90",
                Rok = 2008,
                NumerCzesci = "13.0460-7184.2",
                Cena = 189.99m,
                StanMagazynowy = 8
            },
            new Czesc
            {
                Nazwa = "Filtr oleju",
                Producent = "MANN",
                Marka = "Volkswagen",
                Model = "Golf V",
                Rok = 2007,
                NumerCzesci = "HU 719/7 X",
                Cena = 39.90m,
                StanMagazynowy = 15
            },
            new Czesc
            {
                Nazwa = "Tarcza hamulcowa",
                Producent = "TRW",
                Marka = "Mercedes",
                Model = "C220",
                Rok = 2012,
                NumerCzesci = "DF 4782",
                Cena = 249.00m,
                StanMagazynowy = 6
            }
        );

        db.SaveChanges();
    }

    private void WczytajCzesci(string? filtr = null)
    {
        var czesci = db.Czesc.AsNoTracking().ToList();

        if (!string.IsNullOrWhiteSpace(filtr))
        {
            filtr = filtr.ToLower();

            czesci = czesci.Where(c =>
                c.Nazwa.ToLower().Contains(filtr) ||
                c.Producent.ToLower().Contains(filtr) ||
                c.Marka.ToLower().Contains(filtr) ||
                c.Model.ToLower().Contains(filtr) ||
                c.NumerCzesci.ToLower().Contains(filtr)
            ).ToList();
        }

        gridCzesci.ItemsSource = czesci;
        lblLiczba.Text = $"Liczba wyświetlanych części: {czesci.Count}";
    }

    private void txtSzukaj_TextChanged(object sender, TextChangedEventArgs e)
    {
        WczytajCzesci(txtSzukaj.Text);
    }

    private void Dodaj_Click(object sender, RoutedEventArgs e)
    {
        var okno = new CzescWindow();
        if (okno.ShowDialog() == true)
        {
            db.Czesc.Add(okno.Czesc);
            db.SaveChanges();
            WczytajCzesci(txtSzukaj.Text);
        }
    }

    private void Edytuj_Click(object sender, RoutedEventArgs e)
    {
        if (gridCzesci.SelectedItem is not Czesc wybrana)
        {
            MessageBox.Show("Najpierw wybierz część z tabeli.", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var czesc = db.Czesc.Find(wybrana.Id);
        if (czesc == null)
            return;

        var okno = new CzescWindow(czesc);
        if (okno.ShowDialog() == true)
        {
            db.SaveChanges();
            WczytajCzesci(txtSzukaj.Text);
        }
    }

    private void Usun_Click(object sender, RoutedEventArgs e)
    {
        if (gridCzesci.SelectedItem is not Czesc wybrana)
        {
            MessageBox.Show("Najpierw wybierz część z tabeli.", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var wynik = MessageBox.Show(
            $"Czy na pewno chcesz usunąć część „{wybrana.Nazwa}”?",
            "Potwierdzenie",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (wynik == MessageBoxResult.Yes)
        {
            var czesc = db.Czesc.Find(wybrana.Id);
            if (czesc != null)
            {
                db.Czesc.Remove(czesc);
                db.SaveChanges();
                WczytajCzesci(txtSzukaj.Text);
            }
        }
    }

    private void Odswiez_Click(object sender, RoutedEventArgs e)
    {
        txtSzukaj.Clear();
        WczytajCzesci();
    }

    protected override void OnClosed(EventArgs e)
    {
        db.Dispose();
        base.OnClosed(e);
    }
}
