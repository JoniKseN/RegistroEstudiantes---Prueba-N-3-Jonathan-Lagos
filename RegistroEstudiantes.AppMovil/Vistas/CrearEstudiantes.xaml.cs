using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

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
        var cursos= client.Child("Curso").OnceAsync<Curso>();
		Cursos= cursos.Result.Select(x=>x.Object).ToList();
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
	{
		Curso curso = cursoPicker.SelectedItem as Curso;

		var estudiante = new Estudiantes
		{
			PrimerNombre = primerNombreEntry.Text,
			SegundoNombre = segundoNombreEntry.Text,
			PrimerApellido = primerApellidoEntry.Text,
			SegundoApellido = segundoApellidoEntry.Text,
			CorreoElectronico = correoEntry.Text,
			Edad = int.Parse(edadEntry.Text),
			FechaInicio = fechaInicioPicker.Date,
			Curso = curso
		};
		try
		{
            await client.Child("Estudiantes").PostAsync(estudiante);
			await DisplayAlert("Exito", $"El estudiante{estudiante.PrimerNombre} {estudiante.PrimerApellido} fue guardado correctamente", "OK");
			await Navigation.PopAsync();
        }
		catch (Exception ex) 
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
		
	}
}