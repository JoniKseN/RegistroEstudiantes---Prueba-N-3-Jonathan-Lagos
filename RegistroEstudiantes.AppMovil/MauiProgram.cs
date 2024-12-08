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
            Registrar();
            return builder.Build();
        }

        public static void Registrar()
        {
            FirebaseClient client = new FirebaseClient("https://registroestudiantesprueba-default-rtdb.firebaseio.com/");

            var cursos = client.Child("Curso").OnceAsync<Curso>();

            if (cursos.Result.Count == 0)

            {
                client.Child("Curso").PostAsync(new Curso { Nombre = "Enseñanza Media" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "Enseñaza Basica" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "Universitario" });

            }

        }
    }
}
