using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using DirectorioAPI.Models;

namespace DirectorioClient;

public partial class MainWindow : Window
{
    private readonly HttpClient _http;
    private readonly ObservableCollection<Persona> _personas = new();

    public MainWindow()
    {
        InitializeComponent();
		System.Net.ServicePointManager.ServerCertificateValidationCallback += 
			(sender, cert, chain, sslPolicyErrors) => true;

        var baseUrl = new Uri("https://localhost:7209/"); // usa HTTP simple
		_http = new HttpClient { BaseAddress = baseUrl };


        // bind al DataGrid
        dataGrid.ItemsSource = _personas;

        // cargar inicial
        _ = LoadPersonasAsync();
    }

    private void Inputs_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        UpdateRegisterButtonState();
    }

    private void UpdateRegisterButtonState()
    {
        btnRegistrar.IsEnabled =
            !string.IsNullOrWhiteSpace(txtNombre.Text) &&
            !string.IsNullOrWhiteSpace(txtApellidoPaterno.Text) &&
            !string.IsNullOrWhiteSpace(txtIdentificacion.Text);
    }

    private async Task LoadPersonasAsync()
    {
        try
        {
            var lista = await _http.GetFromJsonAsync<Persona[]>("api/personas");
            _personas.Clear();
            if (lista != null)
            {
                foreach (var p in lista)
                    _personas.Add(p);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar personas: " + ex.Message);
        }
    }

    private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
    {
        var nueva = new Persona
        {
            Nombre = txtNombre.Text.Trim(),
            ApellidoPaterno = txtApellidoPaterno.Text.Trim(),
            ApellidoMaterno = string.IsNullOrWhiteSpace(txtApellidoMaterno.Text) ? null : txtApellidoMaterno.Text.Trim(),
            Identificacion = txtIdentificacion.Text.Trim()
        };

        // Validacion, UI ya desactiva boton
        if (string.IsNullOrWhiteSpace(nueva.Nombre) ||
            string.IsNullOrWhiteSpace(nueva.ApellidoPaterno) ||
            string.IsNullOrWhiteSpace(nueva.Identificacion))
        {
            MessageBox.Show("Faltan datos obligatorios.");
            return;
        }

        try
        {
            var resp = await _http.PostAsJsonAsync("api/personas", nueva);
            if (resp.IsSuccessStatusCode)
            {
                MessageBox.Show("Persona registrada con éxito.");
                // limpiar form
                txtNombre.Text = txtApellidoPaterno.Text = txtApellidoMaterno.Text = txtIdentificacion.Text = string.Empty;
                UpdateRegisterButtonState();
                await LoadPersonasAsync();
            }
            else
            {
                var detalle = await resp.Content.ReadAsStringAsync();
                MessageBox.Show($"Error al registrar: {resp.StatusCode}. {detalle}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al conectar con la API: " + ex.Message);
        }
    }

    private async void BtnRefrescar_Click(object sender, RoutedEventArgs e)
    {
        await LoadPersonasAsync();
    }

    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (dataGrid.SelectedItem is not Persona seleccionado)
        {
            MessageBox.Show("Selecciona una persona en la tabla.");
            return;
        }

        var confirm = MessageBox.Show($"Eliminar persona {seleccionado.Nombre} {seleccionado.ApellidoPaterno}?", "Confirmar", MessageBoxButton.YesNo);
        if (confirm != MessageBoxResult.Yes) return;

        try
        {
            var resp = await _http.DeleteAsync($"api/personas/{seleccionado.Id}");
            if (resp.IsSuccessStatusCode || resp.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                MessageBox.Show("Eliminada.");
                await LoadPersonasAsync();
            }
            else
            {
                var detalle = await resp.Content.ReadAsStringAsync();
                MessageBox.Show($"Error al eliminar: {resp.StatusCode}. {detalle}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al conectar con la API: " + ex.Message);
        }
    }
}
