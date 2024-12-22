using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            ActualizarCurso();
            ActualizarEstudiantes();
            return builder.Build();
        }

        public static async Task ActualizarCurso()
        {
            FirebaseClient client = new FirebaseClient("https://registroestudiantesprueba-default-rtdb.firebaseio.com/");

            var cursos = await client.Child("Curso").OnceAsync<Curso>();

            if (cursos.Count == 0)

            {
                await client.Child("Curso").PostAsync(new Curso { Nombre = "Enseñaza Basica" });
                await client.Child("Curso").PostAsync(new Curso { Nombre = "Enseñanza Media" });
                await client.Child("Curso").PostAsync(new Curso { Nombre = "Universitario" });

            }
            else
            {
                foreach (var curso in cursos)
                {
                    if(curso.Object.Estado == null)
                    {
                        var cursoActualizado = curso.Object;
                        cursoActualizado.Estado = true;

                        await client.Child("Curso").Child(curso.Key).PutAsync(cursoActualizado);

                    }
                }
            }

        }

        public static async Task ActualizarEstudiantes()
        {
            FirebaseClient client = new FirebaseClient("https://registroestudiantesprueba-default-rtdb.firebaseio.com/");

            var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiantes>();
            foreach(var estudiante in estudiantes)
            {
                if (estudiante.Object.Estado == null)
                {
                    var estudianteActualizado = estudiante.Object;
                    estudianteActualizado.Estado= true;
                    
                    await client.Child("Estudiantes").Child(estudiante.Key).PutAsync(estudianteActualizado);
                }
                                          
            }
        }
    }
}
