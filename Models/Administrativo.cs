namespace POOI_T1_Cruz.Models
{
    public class Administrativo : Empleado
    {
        public int anioIngreso { get; set; }
        public bool postGrado { get; set; }

        public int AniosServicio()
        {
            return DateTime.Now.Year - anioIngreso;
        }

        public decimal Incentivo()
        {
            if (postGrado)
            {
                return 500m;
            }

            return 0m;
        }

        public override decimal Bonificacion()
        {
            int aniosServicio = AniosServicio();

            if (aniosServicio < 5)
            {
                return 200m;
            }

            if (aniosServicio <= 10)
            {
                return 450m;
            }

            return 300m;
        }

        public override decimal MontoAPagar()
        {
            return SueldoBasico() + Escolaridad() + Bonificacion() + Incentivo();
        }
    }
}
