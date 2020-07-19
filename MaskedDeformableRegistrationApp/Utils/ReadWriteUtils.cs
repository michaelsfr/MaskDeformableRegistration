using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using MaskedDeformableRegistrationApp.Registration;

namespace MaskedDeformableRegistrationApp.Utils
{
    public static class ReadWriteUtils
    {
        public static Image<T, D> ReadOpenCVImageFromFile<T, D>(string file) where T : struct, IColor where D : new()
        {
            Image<T, D> image = null;
            try
            {
                image = new Image<T, D>(file);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return image;
        }

        public static void WriteUMatToFile(string filename, UMat image)
        {
            CvInvoke.Imwrite(filename, image);
        }

        public static sitk.Image RescaleImageToFloat(sitk.Image img)
        {
            sitk.RescaleIntensityImageFilter filter = new sitk.RescaleIntensityImageFilter();
            filter.SetOutputMinimum(0);
            filter.SetOutputMaximum(1);
            return filter.Execute(img);
        }

        public static sitk.Image ReadITKImageFromFile(string file)
        {
            return sitk.SimpleITK.ReadImage(file);
        }

        public static sitk.Image ReadITKImageFromFile(string file, sitk.PixelIDValueEnum outputType)
        {
            sitk.ImageFileReader reader = new sitk.ImageFileReader();
            reader.SetFileName(file);
            reader.SetOutputPixelType(outputType);
            return reader.Execute();
        }

        public static void WriteBitmapAsPng(Bitmap bmp, string outputFileName)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    bmp.Save(memory, ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void WriteSitkImage(sitk.Image img, string outputFileName)
        {
            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(outputFileName);
            writer.Execute(img);
            writer.Dispose();
            //img.Dispose();
        }
        
        public static Dictionary<string, string> GetParameterDictionaryFromFile(string filename)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                if (line.StartsWith("(") && line.EndsWith(")"))
                {
                    string newline = line.Substring(1, line.Length - 1);
                    string[] param = newline.Split(' ');

                    if (param.Length == 2)
                    {
                        // TODO more than 1 parameter
                        dict.Add(param[0], param[1]);
                    }

                }
            }

            return dict;
        }

        public static Tuple<List<CoordPoint>, List<CoordPoint>> ReadFixedAndTransformedPointSets(string filename)
        {
            Tuple<List<CoordPoint>, List<CoordPoint>> result = null;

            if (File.Exists(filename))
            {
                string content = File.ReadAllText(filename);
                string[] lines = content.Split('\n');

                List<CoordPoint> inputPoints = new List<CoordPoint>();
                List<CoordPoint> transformedPoints = new List<CoordPoint>();

                foreach (string line in lines)
                {
                    List<string> entries = line.Split(';').ToList();
                    string inputPoint = entries.Where(it => it.Contains("InputPoint")).FirstOrDefault();
                    string transformedPoint = entries.Where(it => it.Contains("OutputPoint")).FirstOrDefault();
                    if(inputPoint != null && transformedPoint != null)
                    {
                        inputPoints.Add(ExractCoordsFromString(inputPoint));
                        transformedPoints.Add(ExractCoordsFromString(transformedPoint));
                    }
                }
                result = new Tuple<List<CoordPoint>, List<CoordPoint>>(inputPoints, transformedPoints);
            }

            return result;
        }

        private static CoordPoint ExractCoordsFromString(string sCoord)
        {
            string[] temp = sCoord.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            double[] coords = temp[1].Replace('.', ',').Split(' ').Where(it => it != null && it != "").Select(it => double.Parse(it)).ToArray();
            return new CoordPoint(coords[0], coords[1]);
        }
             
        public static List<CoordPoint> ReadPointSetsFromFileOLD(string filenamePointSets)
        {
            if(File.Exists(filenamePointSets))
            {
                string content = File.ReadAllText(filenamePointSets);
                string[] temp = content.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                string[] sPoints = temp[1].Split('\n');
                List<CoordPoint> points = new List<CoordPoint>();

                foreach (string sPoint in sPoints)
                {
                    double[] coords = sPoint.Replace('.', ',').Split(' ').Where(it => it != null).Select(it => double.Parse(it)).ToArray();
                    points.Add(new CoordPoint(coords[0], coords[1]));
                }
                return points;
            }
            return null;
        }

        public static void SerializeObjectToJSON<T>(T toSerialize, string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, toSerialize);
            }
        }

        public static void SerializeObjectListToJSON<T>(List<T> toSerialize, string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, toSerialize);
            }
        }

        public static List<T> DeserializeObjectListFromJSON<T>(string filename)
        {
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<T> data = (List<T>)serializer.Deserialize(file, typeof(List<T>));
                return data;
            }
        }

        public static T DeserializeObjectFromJSON<T>(string filename)
        {
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                T data = (T)serializer.Deserialize(file, typeof(T));
                return data;
            }
        }

        public static string GetOutputDirectory(RegistrationParameters parameters, int i = -1)
        {
            string path = Path.Combine(ApplicationContext.OutputPath, parameters.SubDirectory);
            if (i != -1)
            {
                path = Path.Combine(path, i.ToString());
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
