using BazaCzesciSamochodowych.Models;
using System.Windows;

namespace BazaCzesciSamochodowych;

public partial class CzescWindow : Window
{
    public Czesc Czesc { get; private set; }

    public CzescWindow()
    {
        InitializeComponent();
        Czesc = new Czesc();
    }

    public CzescWindow(Czesc czesc)
    {
        InitializeComponent();
        Czesc = czesc;
        txtTytul.Text = "EDYTUJ CZĘŚĆ";

        txtNazwa.Text = czesc.Nazwa;
        txtProducent.Text = czesc.Producent;
        txtMarka.Text = czesc.Marka;
        txtModel.Text = czesc.Model;
        txtRok.Text = czesc.Rok.ToString();
        txtNumer.Text = czesc.NumerCzesci;
        txtCena.Text = czesc.Cena.ToString();
        txtStan.Text = czesc.StanMagazynowy.ToString();
    }

    private void Zapisz_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNazwa.Text) ||
            string.IsNullOrWhiteSpace(txtProducent.Text) ||
            string.IsNullOrWhiteSpace(txtMarka.Text))
        {
            MessageBox.Show("Uzupełnij nazwę, producenta i markę.",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(txtRok.Text, out int rok) || rok < 1900 || rok > 2100)
        {
            MessageBox.Show("Podaj poprawny rok.",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(txtCena.Text, out decimal cena) || cena < 0)
        {
            MessageBox.Show("Podaj poprawną cenę.",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(txtStan.Text, out int stan) || stan < 0)
        {
            MessageBox.Show("Podaj poprawny stan magazynowy.",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Czesc.Nazwa = txtNazwa.Text.Trim();
        Czesc.Producent = txtProducent.Text.Trim();
        Czesc.Marka = txtMarka.Text.Trim();
        Czesc.Model = txtModel.Text.Trim();
        Czesc.Rok = rok;
        Czesc.NumerCzesci = txtNumer.Text.Trim();
        Czesc.Cena = cena;
        Czesc.StanMagazynowy = stan;

        DialogResult = true;
        Close();
    }

    private void Anuluj_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
