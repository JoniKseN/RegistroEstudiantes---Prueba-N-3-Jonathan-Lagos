using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class EditarEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroestudiantesprueba-default-rtdb.firebaseio.com/");

    public ObservableCollection<string> ListaCurso { get; set; } = new ObservableCollection<string>();
    private Estudiantes estudianteActualizado = new Estudiantes();
    private readonly string estudianteId;

    public EditarEstudiantes(string idEstudiante)
    {
        InitializeComponent();
        BindingContext = this;
        estudianteId = idEstudiante;
        CargarListaCurso();
        CargarEstudiante(estudianteId);
    }

    private async void CargarListaCurso()
    {
        try
        {
            var cursos = await client.Child("Curso").OnceAsync<Curso>();
            ListaCurso.Clear();
            foreach (var curso in cursos)
            {
                ListaCurso.Add(curso.Object.Nombre);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar los cursos: {ex.Message}", "OK");
        }
    }

    private async void CargarEstudiante(string idEstudiante)
    {
        try
        {
            var estudiante = await client.Child("Estudiantes").Child(idEstudiante).OnceSingleAsync<Estudiantes>();

            if (estudiante != null)
            {
                EditPrimerNombreEntry.Text = estudiante.PrimerNombre;
                EditSegundoNombreEntry.Text = estudiante.SegundoNombre;
                EditPrimerApellidoEntry.Text = estudiante.PrimerApellido;
                EditSegundoApellidoEntry.Text = estudiante.SegundoApellido;
                EditCorreoEntry.Text = estudiante.CorreoElectronico;
                EditCursoAlumno.Text = estudiante.CursoAlumno;
                EditEdadEntry.Text = estudiante.Edad.ToString();
                EditEstadoSwitch.IsToggled = estudiante.Estado ?? false;
                EditCursoPicker.SelectedItem = estudiante.Curso?.Nombre;

                EditCursoPicker.SelectedItem = ListaCurso.FirstOrDefault(c => c == estudiante.Curso?.Nombre);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar el estudiante: {ex.Message}", "OK");
        }
    }

    private async void ActualizarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            
            if (string.IsNullOrWhiteSpace(EditPrimerNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditPrimerApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditCorreoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditCursoAlumno.Text) ||
                string.IsNullOrWhiteSpace(EditEdadEntry.Text) ||
                EditCursoPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            if (!int.TryParse(EditEdadEntry.Text, out var edad))
            {
                await DisplayAlert("Error", "La edad debe ser un número válido.", "OK");
                return;
            }

            estudianteActualizado.Id = estudianteId;
            estudianteActualizado.PrimerNombre = EditPrimerNombreEntry.Text.Trim();
            estudianteActualizado.SegundoNombre = EditSegundoNombreEntry.Text?.Trim();
            estudianteActualizado.PrimerApellido = EditPrimerApellidoEntry.Text.Trim();
            estudianteActualizado.SegundoApellido = EditSegundoApellidoEntry.Text.Trim();
            estudianteActualizado.CorreoElectronico = EditCorreoEntry.Text.Trim();
            estudianteActualizado.CursoAlumno = EditCursoAlumno.Text.Trim();
            estudianteActualizado.Edad = edad;
            estudianteActualizado.Estado = EditEstadoSwitch.IsToggled;
            estudianteActualizado.Curso = new Curso { Nombre = EditCursoPicker.SelectedItem.ToString() };

            await client.Child("Estudiantes").Child(estudianteActualizado.Id).PutAsync(estudianteActualizado);
            await DisplayAlert("Éxito", "El estudiante ha sido actualizado correctamente.", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar el estudiante: {ex.Message}", "OK");
        }
    }
}
