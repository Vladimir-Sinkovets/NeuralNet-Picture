using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetPaintPicture
{
    class PictureNeuralFilter
    {
        private const int epochCount = 10;
        private const double learningRate = 0.001;

        private static Random rnd = new Random(123);
        private static string fileName = "NeuralNet.soap";
        private NeuralNet6 net;

        public PictureNeuralFilter()
        {
            net = LoadSoapNeuralNet();
            //_net = new NeuralNet6(50, 40, 13);
            //NeuralNetTraining(dataPath, _net);
            //SaveSoapNeuralNet(_net);
        }
        public Bitmap CreatePicture(Bitmap originalPicture)
        {
            List<Point> pointsMap = PointsArranger.CreatePointsMap(originalPicture);
            Bitmap newPicture = PicturePainter.DrawLinePicture(pointsMap, net, originalPicture);
            return newPicture;
        }

        private static NeuralNet6 LoadSoapNeuralNet()
        {
            SoapFormatter formatter = new SoapFormatter();
            NeuralNet6 net = null;
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                net = (NeuralNet6)formatter.Deserialize(fs);
            }
            return net;
        }
        private static void SaveSoapNeuralNet(NeuralNet6 net)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, net);
            }
        }


        private static void NeuralNetTraining(string dataPath, NeuralNet6 net)
        {
            List<double[]> input = new List<double[]>();
            List<double[]> output = new List<double[]>();

            StreamReader reader = new StreamReader(dataPath);
            ReadFile(input, output, reader);

            Train(net, input, output);
        }
        private static void Train(NeuralNet6 net, List<double[]> input, List<double[]> output)
        {

            for (int e = 0; e <= epochCount; e++)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    net.Training(input[i], output[i], learningRate);
                }
            }
            SaveSoapNeuralNet(net);
        }
        private static void ReadFile(List<double[]> input, List<double[]> output, StreamReader reader)
        {
            string str = "";
            while ((str = reader.ReadLine()) != null)
            {
                AddToInputList(input, str);
                str = reader.ReadLine();
                AddToOutputList(output, str);
            }
        }
        private static void AddToInputList(List<double[]> input, string str)
        {
            string[] inputString = str.Split(' ');
            double[] inputdouble = new double[inputString.Length + 1];

            for (int j = 0; j < inputdouble.Length - 1; j++)
            {
                inputdouble[j] = Convert.ToSingle(inputString[j]) / 255;
            }
            inputdouble[inputdouble.Length - 1] = 1;
            input.Add(inputdouble);
        }
        private static void AddToOutputList(List<double[]> output, string outputString)
        {
            switch (outputString)
            {
                case "0":
                    output.Add(new double[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
                    break;
                case "15":
                    output.Add(new double[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
                    break;
                case "30":
                    output.Add(new double[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
                    break;
                case "45":
                    output.Add(new double[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
                    break;
                case "60":
                    output.Add(new double[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, });
                    break;
                case "75":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, });
                    break;
                case "90":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, });
                    break;
                case "105":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, });
                    break;
                case "120":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, });
                    break;
                case "135":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, });
                    break;
                case "150":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, });
                    break;
                case "165":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, });
                    break;
                case "-1":
                    output.Add(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, });
                    break;

            }
        }
    }
}