using Microsoft.ML.Data;


namespace Sessions_app.Models
{
    public class PacienteData
    {
        [LoadColumn(0)] public float Idade { get; set; }
        [LoadColumn(1)] public float Peso { get; set; }
        [LoadColumn(2)] public float Altura { get; set; }
        [LoadColumn(3)] public string Sexo { get; set; }

        [LoadColumn(4)]
        public bool ResultadoClinico { get; set; }
    }
}
