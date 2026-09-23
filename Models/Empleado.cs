namespace POOI_T1_Cruz.Models
{
    public class Empleado
    {
        public string idEmpleado { get; set; } = "";
        public string nomapeEmpleado { get; set; } = "";
        public string categoriaEmpleado { get; set; } = "";
        public int nHijos { get; set; }
        public string tipoContrato { get; set; } = "";

        public decimal SueldoBasico()
        {
            if (categoriaEmpleado == "E1")
            {
                return 5500m;
            }

            if (categoriaEmpleado == "E2")
            {
                return 2500m;
            }

            if (categoriaEmpleado == "E3")
            {
                return 2200m;
            }

            return 1700m;
        }
        public decimal Escolaridad()
        {
            return nHijos * 108m;
        }
        public virtual decimal Bonificacion()
        {
            if (tipoContrato == "Indefinido")
            {
                return SueldoBasico() * 0.15m;
            }

            if (tipoContrato == "Contratado")
            {
                return SueldoBasico() * 0.10m;
            }

            return 0m;
        }
        public virtual decimal MontoAPagar()
        {
            return SueldoBasico() + Escolaridad() + Bonificacion();
        }
    }

}
