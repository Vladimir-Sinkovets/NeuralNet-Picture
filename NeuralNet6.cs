using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetPaintPicture
{
    [Serializable]
    public class NeuralNet6
    {
        private Layer[] _layers;

        public NeuralNet6(params int[] layers)
        {
            _layers = new Layer[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                int nextLayer = 0;
                if ((i + 1) != layers.Length)
                {
                    nextLayer = layers[i + 1];
                }
                _layers[i] = new Layer(layers[i], nextLayer);
            }
        }

        public double[] Prediction(params double[] input)
        {
            for (int i = 0; i < _layers.Length; i++)
            {
                input = _layers[i].Calculate(input);
            }
            return _layers.Last().Values;
        }
        public double[] Training(double[] input, double[] answer, double learningRate)
        {
            double[] error = Subtraction(answer, Prediction(input));
            for (int i = _layers.Length - 1; i >= 0; i--)
            {
                error = _layers[i].FindErrors(error);
            }
            for (int i = _layers.Length - 2; i >= 0; i--)
            {
                Layer previousLayer = _layers[i + 1];
                _layers[i].Training(previousLayer.Errors,
                                    previousLayer.Values,
                                    learningRate);
            }
            return error;
        }
        private static double[] Subtraction(double[] arr1, double[] arr2)
        {
            double[] newArray = new double[arr1.Length];
            for (int i = 0; i < arr1.Length; i++)
            {
                newArray[i] = arr1[i] - arr2[i];
            }
            return newArray;
        }
        [Serializable]
        class Layer
        {
            private double[,] _weights; // [n, w]
            private readonly int _neuronsCount;
            private readonly int _weightsCount;
            private double[] _values;
            private double[] _errors;

            public double[] Errors
            {
                get => _errors;
            }
            public double[] Values
            {
                get => _values;
            }

            public Layer(int neuronsCount, int weightsCount)
            {
                _neuronsCount = neuronsCount;

                _weightsCount = weightsCount;
                CreateRandomWeights(neuronsCount, weightsCount);

                _values = new double[neuronsCount];
                for (int i = 0; i < _values.Length; i++)
                {
                    _values[i] = 0;
                }
            }

            public void Training(double[] errors, double[] nextLayerValues, double learningRate)
            {
                for (int w = 0; w < _weightsCount; w++)
                {
                    for (int n = 0; n < _neuronsCount; n++)
                    {
                        _weights[n, w] = _weights[n, w] + learningRate * errors[w] * ReLUDx(nextLayerValues[w]) * Values[n];
                    }
                }
            }
            public double[] Calculate(double[] input)
            {
                SetValues(input);
                double[] output = new double[_weightsCount];
                for (int w = 0; w < _weightsCount; w++)
                {
                    for (int n = 0; n < _neuronsCount; n++)
                    {
                        output[w] += input[n] * _weights[n, w];
                    }
                }
                ReLUArray(output);
                return output;
            }
            public double[] FindErrors(double[] nextLayerErrors)
            {
                _errors = new double[_neuronsCount];
                if (_weightsCount == 0)
                {
                    for (int i = 0; i < nextLayerErrors.Length; i++)
                    {
                        _errors[i] = nextLayerErrors[i];
                    }
                    return _errors;
                }

                for (int n = 0; n < _neuronsCount; n++)
                {
                    _errors[n] = 0;
                    for (int w = 0; w < _weightsCount; w++)
                    {
                        _errors[n] += _weights[n, w] * nextLayerErrors[w];
                    }
                }
                return _errors;
            }

            private void SetValues(double[] input)
            {
                for (int i = 0; i < _values.Length; i++)
                {
                    _values[i] = input[i];
                }
            }

            private static void ReLUArray(double[] output)
            {
                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = ReLU(output[i]);
                }
            }
            private static double ReLU(double x)
            {
                if (x >= 0 && x <= 1)
                    return x;
                else if (x > 1)
                    return 1f + 0.01f * (x - 1);
                else
                    return 0.01f * x;
            }
            private static double ReLUDx(double x)
            {
                if (x >= 0 && x <= 1)
                    return 1;
                else if (x > 1)
                    return 0.01f;
                else
                    return 0.01f;
            }

            private void CreateRandomWeights(int neuronsCount, int weightsCount)
            {
                Random rnd = new Random(100);
                _weights = new double[neuronsCount, weightsCount];
                for (int n = 0; n < neuronsCount; n++)
                {
                    for (int w = 0; w < weightsCount; w++)
                    {
                        _weights[n, w] = (double)rnd.NextDouble();
                    }
                }
            }
        }
    }
}
