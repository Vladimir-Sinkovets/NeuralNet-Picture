using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetPaintPicture
{
    class PointsArranger
    {
        public static double Radius = 10;
        public static double MinDistance = 0;
        public static int PointsCount = 40000;
        public static int seed = 123;
        public static bool makeRandomPointsWithControl = true;


        private static Random _rnd = new Random(seed);


        public static List<Point> CreatePointsMap(Bitmap bitmap)
        {
            DirectBitmap image = new DirectBitmap(bitmap);
            
            List<Point> points = new List<Point>();
            if(makeRandomPointsWithControl == true)
            {
                for (int i = 0; i < PointsCount; i++)
                    AddNewPointToList(points, image);
            }
            else
            {
                for (int i = 0; i < PointsCount; i++)
                    points.Add(GetRandomPoint(image));
            }
            return points;
        }


        private static void AddNewPointToList(List<Point> points, DirectBitmap image)
        {
            Point newPoint = GetRandomPoint(image);
            if (points.Count == 0)
            {
                points.Add(newPoint);
                return;
            }
            if (CheckPointPosition(image, points, newPoint))
            {
                points.Add(newPoint);
            }
        }
        private static Point GetRandomPoint(DirectBitmap image)
        {
            int x = _rnd.Next(7, image.Width - 7);
            int y = _rnd.Next(7, image.Height - 7);
            return new Point(x, y);
        }
        private static bool CheckPointPosition(DirectBitmap image, List<Point> points, Point newPoint)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Color pixelColor = image.GetPixel(newPoint.X, newPoint.Y);
                double minPossibleDistance = RGBDistance(pixelColor, Radius, MinDistance);
                if (pixelColor.A == 0)
                    return false;

                double distance = Distance(points[i], newPoint);
                if (distance <= minPossibleDistance)
                    return false;
            }
            return true;
        }
        private static double Distance(Point point1, Point point2)
        {
            int x = point2.X - point1.X;
            int y = point2.Y - point1.Y;
            return Math.Sqrt(x * x + y * y);
        }
        private static double RGBDistance(Color color, double radius, double minDistance)
        {
            return ((double)(color.R + color.G + color.B) / 765.0) * radius + minDistance;
        }
    }
}
