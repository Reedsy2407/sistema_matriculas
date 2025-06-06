﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MatriculasMODELS
{
    public class CursoO
    {
        [DisplayName("CURSO")]
        [Required]
        public int id_curso { get; set; }
        [DisplayName("NOMBRE")]
        [Required]
        public string? nom_curso { get; set; }
        [DisplayName("CREDITOS")]
        [Required]
        public int creditos_curso { get; set; }
        [DisplayName("CARRERA")]
        [Required]
        public int id_carrera { get; set; }
    }
}
