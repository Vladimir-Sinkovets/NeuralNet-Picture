using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetPaintPicture
{
    class PicturePainter
    {
        static Random r = new Random(1);

        public static int minLengthLine = 2;
        public static int valueForReplaceZero = 1;
        public static bool useSkips = false;
        public static bool shouldReplaceZero = true;
        public static int minLength = 0;
        public static int maxLength = 10;


        public static Bitmap DrawLinePicture(List<Point> points, NeuralNet6 net, Bitmap originalPicture)
        {
            Bitmap newPicture = new Bitmap(originalPicture.Width, originalPicture.Height);

            foreach (var point in points)
            {
                Color color = originalPicture.GetPixel(point.X, point.Y);
                int angle = DefineAngle(point, originalPicture, net);
                int brightness = color.R + color.G + color.B;
                DrawLineOnPicture(point, newPicture, angle, brightness);
            }
            return newPicture;
        }

        private static int DefineAngle(Point point, Bitmap originalPicture, NeuralNet6 net)
        {
            double[] input = GetInputFromPicture(point, originalPicture);
            double[] output = net.Prediction(input);
            int angle = GetAngleFromOutput(output);
            return angle;
        }
        private static int GetAngleFromOutput(double[] output)
        {
            int[] angles = new int[13] { 0, 15, 30, 45, 60, 75, 90, 105, 120, 135, 150, 165, -1 };
            int index = FindIndexOfMaxElement(output);
            if (angles[index] == -1 && useSkips == false)
                return angles[r.Next(0, 11)];
            return angles[index];
        }
        private static int FindIndexOfMaxElement(double[] output)
        {
            int indexOfMaxElement = 0;
            double maxElement = output[0];
            for (int i = 1; i < output.Length; i++)
            {
                if (output[i] > maxElement)
                {
                    indexOfMaxElement = i;
                    maxElement = output[i];
                }
            }
            return indexOfMaxElement;
        }
        private static double[] GetInputFromPicture(Point point, Bitmap originalPicture)
        {
            double[] input = new double[50];
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    int index = y * 7 + x;
                    int xPicture = point.X - 3 + x;
                    int yPicture = point.Y - 3 + y;
                    Color c = originalPicture.GetPixel(xPicture, yPicture);
                    input[index] = (double)(c.R + c.G + c.B)/ 765d;
                }
            }
            input[49] = 1;
            return input;
        }
        private static void DrawLineOnPicture(Point point, Bitmap newPicture, int angle, int RColor)
        {
            if (angle == -1)
                return;
            Point[][] lines = new Point[12][]
            {
                new Point[11] { new Point(3, 3), new Point(4, 3), new Point(2, 3), new Point(5, 3), new Point(1, 3), new Point(6, 3), new Point(0, 3),      new Point(7, 3), new Point(-1, 3), new Point(8, 3), new Point(-2, 3), },
                new Point[11] { new Point(3, 3), new Point(4, 2), new Point(2, 4), new Point(5, 2), new Point(1, 4), new Point(6, 2), new Point(0, 4),      new Point(7, 1), new Point(-1, 5), new Point(8, 1), new Point(-2, 5), },
                new Point[11] { new Point(3, 3), new Point(4, 2), new Point(2, 4), new Point(5, 2), new Point(1, 4), new Point(6, 1), new Point(0, 5),      new Point(7, 0), new Point(-1, 6), new Point(8, 0), new Point(-2, 6), },
                new Point[11] { new Point(3, 3), new Point(4, 2), new Point(2, 4), new Point(5, 1), new Point(1, 5), new Point(6, 0), new Point(0, 6),      new Point(7, -1), new Point(-1, 7), new Point(8, -2), new Point(-2, 8), },
                new Point[11] { new Point(3, 3), new Point(4, 2), new Point(2, 4), new Point(4, 1), new Point(2, 5), new Point(5, 0), new Point(1, 6),      new Point(6, -1), new Point(0, 7), new Point(6, -2), new Point(0, 8), },
                new Point[11] { new Point(3, 3), new Point(4, 2), new Point(2, 4), new Point(4, 1), new Point(2, 5), new Point(4, 0), new Point(2, 6),      new Point(5, -1), new Point(1, 7), new Point(5, -2), new Point(1, 8), },
                          
                new Point[11] { new Point(3, 3), new Point(3, 2), new Point(3, 4), new Point(3, 1), new Point(3, 5), new Point(3, 0), new Point(3, 6),      new Point(3, -1), new Point(3, 7), new Point(3, -2), new Point(3, 8),},
                new Point[11] { new Point(3, 3), new Point(2, 2), new Point(4, 4), new Point(2, 1), new Point(4, 5), new Point(2, 0), new Point(4, 6),      new Point(1, -1), new Point(5, 7), new Point(1, -2), new Point(5, 8),},
                new Point[11] { new Point(3, 3), new Point(2, 2), new Point(4, 4), new Point(2, 1), new Point(4, 5), new Point(1, 0), new Point(5, 6),      new Point(0, -1), new Point(6, 7), new Point(0, -2), new Point(6, 8),},
                new Point[11] { new Point(3, 3), new Point(2, 2), new Point(4, 4), new Point(1, 1), new Point(5, 5), new Point(0, 0), new Point(6, 6),      new Point(-1, -1), new Point(7, 7), new Point(-2, -2), new Point(8, 8),},
                new Point[11] { new Point(3, 3), new Point(2, 2), new Point(4, 4), new Point(1, 2), new Point(5, 4), new Point(0, 1), new Point(6, 5),      new Point(-1, 0), new Point(7, 6), new Point(-2, 0), new Point(8, 6),},
                new Point[11] { new Point(3, 3), new Point(2, 2), new Point(4, 4), new Point(1, 2), new Point(5, 4), new Point(0, 2), new Point(6, 4),      new Point(-1, 1), new Point(7, 5), new Point(-2, 1), new Point(8, 5),},
            };
            Point[] line = lines[angle / 15];

            int lineLength = (int)((765 - RColor) / 765d * (maxLength - minLengthLine)) + minLengthLine;
            if (lineLength == 0 && shouldReplaceZero == true)
                lineLength = valueForReplaceZero;
            if (lineLength > line.Length)
                lineLength = line.Length;

            for (int i = 0; i < lineLength; i++)
            {
                int x = line[i].X + point.X - 3;
                int y = line[i].Y + point.Y - 3;
                if (x >= newPicture.Width - 5 || y >= newPicture.Height - 5 || x < 0 || y < 0)
                    continue;
                newPicture.SetPixel(x, y, Color.Black);
            }
        }

    }
}
