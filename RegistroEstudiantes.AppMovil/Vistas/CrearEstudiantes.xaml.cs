using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;
using System.Collections.ObjectModel;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class CrearEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroestudiantesprueba-default-rtdb.firebaseio.com/");
    public List<Curso> Cursos { get; set; }

    public CrearEstudiantes()
    {
        InitializeComponent();
        ListarCursos();
        BindingContext = this;
    }

    private void ListarCursos()
    {
        var cursos = client.Child("Curso").OnceAsync<Curso>();
        Cursos = cursos.Result.Select(x => x.Object).ToList();
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {

        if (string.IsNullOrWhiteSpace(primerNombreEntry.Text) ||
            string.IsNullOrWhiteSpace(primerApellidoEntry.Text) ||
            string.IsNullOrWhiteSpace(correoEntry.Text) ||
            cursoPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
            return;
        }

        Curso curso = cursoPicker.SelectedItem as Curso;

        var estudiante = new Estudiantes
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            CursoAlumno = cursoAlumnoEntry.Text,
            Edad = int.TryParse(edadEntry.Text, out int edad) ? edad : 0, 
            FechaInicio = fechaInicioPicker.Date,
            Curso = curso,
            Estado = EstadoSwitch.IsToggled 
        };

        try
        {
            await client.Child("Estudiantes").PostAsync(estudiante);
            await DisplayAlert("Éxito", $"El estudiante {estudiante.PrimerNombre} {estudiante.PrimerApellido} fue guardado correctamente.", "OK");

            if (Navigation.NavigationStack.LastOrDefault() is ListarEstudiantes listarPage)
            {
                listarPage.CargarLista(); 
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al guardar el estudiante: {ex.Message}", "OK");
        }
    }
}
