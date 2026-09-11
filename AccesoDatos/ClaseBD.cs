namespace AccesoDatos
{
    public class Region
    {
        public int IdRegion { get; set; }
        public required string Nombre { get; set; }
    }

    public class Comuna
    {
        public int IdComuna { get; set; }
        public required string Nombre { get; set; }
        public int IdRegion { get; set; }

       /*xml*/
        public required InformacionGeografica InformacionAdicional { get; set; }
    }

    public class InformacionGeografica
    {
        public decimal Superficie { get; set; }
        public int Poblacion { get; set; }
        public decimal Densidad { get; set; }
    }

}
