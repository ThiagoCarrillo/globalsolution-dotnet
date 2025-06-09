using Microsoft.ML.Data;
using System.Text.Json.Serialization;

namespace Sessions_app.Models
{
    public class PacientePredicao
    {
        [ColumnName("Score")]
        public float Probabilidade { get; set; }
    }
}
