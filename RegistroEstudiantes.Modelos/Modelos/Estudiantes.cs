﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Modelos.Modelos
{
    public class Estudiantes
    {
        public string? PrimerNombre {  get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido {get; set; }
        public string? SegundoApellido {  get; set; }
        public string? CorreoElectronico { get; set; }
        public int? Edad {  get; set; }

        public DateTime FechaInicio { get; set; }

        public Curso Curso { get; set; }

        public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";

    }
}
