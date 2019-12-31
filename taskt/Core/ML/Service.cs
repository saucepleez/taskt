using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.ML.Models;

namespace taskt.Core.ML
{
    public class Service
    {
        public TrainingResponse Train(TrainingRequest request)
        {
            //Todo: Load dataset here optionally?


            if (request.Dataset.Count == 0)
            {
                throw new Exception("No data loaded");
            }

            var response = new TrainingResponse();
            response.Request = request;

            response.Started = DateTime.Now;

            //log dataset as metadata
            var jsonDataset = Newtonsoft.Json.JsonConvert.SerializeObject(request.Dataset);

            //create ml context for training
            var mlContext = new MLContext(seed: 0);
            var dataView = mlContext.Data.LoadFromEnumerable(request.Dataset);

            //create test train split
            DataOperationsCatalog.TrainTestData dataSplitView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            IDataView trainData = dataSplitView.TrainSet;
            IDataView testData = dataSplitView.TestSet;

            //create pipeline for training
            IEstimator<ITransformer> mlPipeline = BuildTrainingPipeline(mlContext);

            //train model
            ITransformer mlModel = TrainModel(mlContext, trainData, mlPipeline);

            response.Finished = DateTime.Now;

            //save model
            var modelFileName = SaveModelFile(mlContext, mlModel, trainData.Schema);
            response.ModelName = modelFileName;


            //return file data if requested
            if (request.ReturnFileData)
            {
                var modelZip = File.ReadAllBytes(modelFileName);
                var base64String = Convert.ToBase64String(modelZip);
                response.ModelData = base64String;
            }

            response.Result = "Training Completed Successfully";

            //return model file name
            return response;
        }

        public PredictionOutput Predict(ScoringRequest request)
        {   
            MLContext mlContext = new MLContext();
            var prediction = mlContext.Model.CreatePredictionEngine<MultiClassInput, PredictionOutput>(request.Model);        
            PredictionOutput result = prediction.Predict(new MultiClassInput() { Statement = request.Input });
            return result;             
        }

        public ITransformer LoadModel(string base64string)
        {
            var mlContext = new MLContext(seed: 0);
            var bytes = Convert.FromBase64String(base64string);
            Stream stream = new MemoryStream(bytes);
            var mlModel = mlContext.Model.Load(stream, out DataViewSchema inputSchema);
            return mlModel;
        }

        #region MLdotNet
        private static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                                      .Append(mlContext.Transforms.Text.FeaturizeText("Statement_tf", "Statement"))
                                      .Append(mlContext.Transforms.CopyColumns("Features", "Statement_tf"))
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);

            // Set the training algorithm
            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                                      .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }
        private static ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(trainingDataView);
        }

        private static string SaveModelFile(MLContext mlContext, ITransformer mlModel, DataViewSchema modelInputSchema)
        {
            //create guid for model name
            if (!System.IO.Directory.Exists("Data\\Models"))
            {
                System.IO.Directory.CreateDirectory("Data\\Models");
            }

            var modelPath = $"Data\\Models\\{Guid.NewGuid()}.zip";

            //save model
            mlContext.Model.Save(mlModel, modelInputSchema, modelPath);

            //return model path back
            return modelPath;
        }


        #endregion

    }
}
