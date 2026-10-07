using System.ComponentModel.DataAnnotations;

namespace POOI_T2_CruzAlcas.Models
{
    public class Alumno
    {
        [Required(ErrorMessage = "Ingrese el DNI.")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El DNI debe contener exactamente 8 dígitos.")]
        [Display(Name = "DNI")]
        public string dni { get; set; }

        [Required(ErrorMessage = "Ingrese los nombres.")]
        [StringLength(80, ErrorMessage = "Máximo 80 caracteres.")]
        [Display(Name = "Nombres")]
        public string nombres { get; set; }

        [Required(ErrorMessage = "Ingrese los apellidos.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Apellidos")]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "Ingrese la carrera.")]
        [StringLength(120, ErrorMessage = "Máximo 120 caracteres.")]
        [Display(Name = "Carrera")]
        public string carrera { get; set; }

        [Range(1, 10, ErrorMessage = "El ciclo debe estar entre 1 y 10.")]
        [Display(Name = "Ciclo")]
        public int ciclo { get; set; }

        public Alumno() { }

        public Alumno(string dni, string nombres, string apellidos, string carrera, int ciclo)
        {
            this.dni = dni;
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.carrera = carrera;
            this.ciclo = ciclo;
        }
    }
}
