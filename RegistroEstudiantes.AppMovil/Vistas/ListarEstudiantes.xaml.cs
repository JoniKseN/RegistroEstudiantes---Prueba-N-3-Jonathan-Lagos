using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class ListarEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroestudiantesprueba-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiantes> Lista { get; set; } = new ObservableCollection<Estudiantes>();

    public ListarEstudiantes()
    {
        InitializeComponent();
        BindingContext = this;
        CargarLista();
    }

    public async void CargarLista()
    {
        try
        {
            Lista.Clear(); 
            var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiantes>();
            var estudiantesActivos = estudiantes.Where(e => e.Object.Estado == true).ToList();

            foreach (var estudiante in estudiantesActivos)
            {
                Lista.Add(new Estudiantes
                {
                    Id = estudiante.Key,
                    PrimerNombre = estudiante.Object.PrimerNombre,
                    SegundoNombre = estudiante.Object.SegundoNombre,
                    PrimerApellido = estudiante.Object.PrimerApellido,
                    SegundoApellido = estudiante.Object.SegundoApellido,
                    CorreoElectronico = estudiante.Object.CorreoElectronico,
                    CursoAlumno = estudiante.Object.CursoAlumno,
                    Edad = estudiante.Object.Edad,
                    FechaInicio = estudiante.Object.FechaInicio,
                    Estado = estudiante.Object.Estado,
                    Curso = estudiante.Object.Curso
                });
            }

            listaCollection.ItemsSource = Lista; 
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar la lista: {ex.Message}", "OK");
        }
    }


    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();

        if (filtro.Length > 0)
        {
            listaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
        else
        {
            listaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoEstudianteBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearEstudiantes());
    }

    private async void EditarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiantes;

        if (estudiante != null && !string.IsNullOrEmpty(estudiante.Id))
        {
            await Navigation.PushAsync(new EditarEstudiantes(estudiante.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la información del estudiante", "OK");
        }
    }

    private async void desahbilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiantes;

        if (estudiante is null)
        {
            await DisplayAlert("Error", "No se pudo obtener la información del estudiante", "OK");
            return;
        }

        bool confirmacion = await DisplayAlert("Confirmación", $"¿Está seguro de deshabilitar al estudiante {estudiante.NombreCompleto}?", "Sí", "No");
        if (confirmacion)
        {
            try
            {
                estudiante.Estado = false; 
                await client.Child("Estudiantes").Child(estudiante.Id).PutAsync(estudiante);
                await DisplayAlert("Éxito", $"El estudiante {estudiante.NombreCompleto} ha sido deshabilitado con éxito.", "OK");

                CargarLista();
                filtroSearchBar_TextChanged(this, null);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al deshabilitar el estudiante: {ex.Message}", "OK");
            }
        }
    }
}
