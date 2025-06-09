using Microsoft.ML;
using Sessions_app.Models;

namespace Sessions_app.Service
{
    public class PacienteMLService
    {
        private readonly string _caminhoModelo;
        private readonly MLContext _mlContext;
        private ITransformer _modeloTreinado;

        public PacienteMLService()
        {
            _mlContext = new MLContext();
            _caminhoModelo = Path.Combine(Directory.GetCurrentDirectory(), "MLModels", "ModeloPaciente.zip");
            CarregarOuTreinar();
        }

        private void CarregarOuTreinar()
        {
            if (File.Exists(_caminhoModelo))
            {
                using var stream = new FileStream(_caminhoModelo, FileMode.Open, FileAccess.Read);
                _modeloTreinado = _mlContext.Model.Load(stream, out _);
            }
            else
            {
                Treinar();
            }
        }

        private void Treinar()
        {
            // Carrega CSV
            var dadosTrain = _mlContext.Data.LoadFromTextFile<PacienteData>(
                path: Path.Combine("Data", "pacientes-train.csv"),
                hasHeader: true, separatorChar: ',');

            // Pipeline: OneHot em string + Concatenação + FastTree
            var pipeline = _mlContext.Transforms
   // Copia a coluna ResultadoClinico para o nome interno "Label":
   .CopyColumns(outputColumnName: "Label", inputColumnName: nameof(PacienteData.ResultadoClinico))

   // Codifica o Sexo como one-hot:
   .Append(_mlContext.Transforms.Categorical.OneHotEncoding(
       outputColumnName: "SexoEncoded",
       inputColumnName: nameof(PacienteData.Sexo)))

   // Concatena todas as features numéricas e a codificação de Sexo em um único vetor "Features":
   .Append(_mlContext.Transforms.Concatenate(
       outputColumnName: "Features",
       nameof(PacienteData.Idade),
       nameof(PacienteData.Peso),
       nameof(PacienteData.Altura),
       "SexoEncoded" /*, …outras colunas se houver*/))

   // Usa o trainer FastTree para classificação binária, apontando explicitamente para "Label" e "Features":
   .Append(_mlContext.BinaryClassification.Trainers.FastTree(
       new Microsoft.ML.Trainers.FastTree.FastTreeBinaryTrainer.Options
       {
           LabelColumnName = "Label",
           FeatureColumnName = "Features",
           // Você pode ajustar hiperparâmetros aqui, ex:
           NumberOfLeaves = 20,
           NumberOfTrees = 100,
           MinimumExampleCountPerLeaf = 10
       }));

            _modeloTreinado = pipeline.Fit(dadosTrain);

            // Salvar
            Directory.CreateDirectory(Path.GetDirectoryName(_caminhoModelo));
            _mlContext.Model.Save(_modeloTreinado, dadosTrain.Schema, _caminhoModelo);
        }

        public PacientePredicao Prever(PacienteData input)
        {
            var engine = _mlContext.Model.CreatePredictionEngine<PacienteData, PacientePredicao>(_modeloTreinado);
            return engine.Predict(input);
        }
    }
}
