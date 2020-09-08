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
        /// <summary>
        /// Read opencv image from file.
        /// </summary>
        /// <typeparam name="T">color space / type</typeparam>
        /// <typeparam name="D">pixel data tyoe</typeparam>
        /// <param name="filename">filename</param>
        /// <returns>image</returns>
        public static Image<T, D> ReadOpenCVImageFromFile<T, D>(string filename) where T : struct, IColor where D : new()
        {
            Image<T, D> image = null;
            try
            {
                image = new Image<T, D>(filename);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return image;
        }

        /// <summary>
        /// Writes an unsigned mat to file.
        /// </summary>
        /// <param name="filename">filename</param>
        /// <param name="image">image</param>
        public static void WriteUMatToFile(string filename, UMat image)
        {
            CvInvoke.Imwrite(filename, image);
        }

        /// <summary>
        /// Reads an ITK image from file.
        /// </summary>
        /// <param name="file">filename</param>
        /// <returns>image</returns>
        public static sitk.Image ReadITKImageFromFile(string file)
        {
            return sitk.SimpleITK.ReadImage(file);
        }

        /// <summary>
        /// Reads an ITK image from file, specifying the output pixel type.
        /// </summary>
        /// <param name="file">filename</param>
        /// <param name="outputType">output pixel type</param>
        /// <returns>image</returns>
        public static sitk.Image ReadITKImageFromFile(string file, sitk.PixelIDValueEnum outputType)
        {
            sitk.ImageFileReader reader = new sitk.ImageFileReader();
            reader.SetFileName(file);
            reader.SetOutputPixelType(outputType);
            return reader.Execute();
        }

        public static sitk.Image ReadITKImageAsGrayscaleFromFile(string file, sitk.PixelIDValueEnum outputType, ColorChannel channel)
        {
            sitk.ImageFileReader reader = new sitk.ImageFileReader();
            reader.SetFileName(file);
            reader.SetOutputPixelType(outputType);
            sitk.Image temp = reader.Execute();

            sitk.Image result = null;
            sitk.VectorIndexSelectionCastImageFilter rgbVector = new sitk.VectorIndexSelectionCastImageFilter();
            switch (channel)
            {
                case ColorChannel.R: result = rgbVector.Execute(temp, 0, sitk.PixelIDValueEnum.sitkFloat32); break;
                case ColorChannel.G: result = rgbVector.Execute(temp, 1, sitk.PixelIDValueEnum.sitkFloat32); break;
                case ColorChannel.B: result = rgbVector.Execute(temp, 2, sitk.PixelIDValueEnum.sitkFloat32); break;
            }

            return result;
        }

        /// <summary>
        /// Write a bitmap as Png.
        /// </summary>
        /// <param name="bmp">bitmap</param>
        /// <param name="outputFileName">output filename</param>
        public static void WriteBitmapAsPng(Bitmap bmp, string outputFileName)
        {
            WriteBitmapAsType(bmp, outputFileName, ImageFormat.Png);
        }

        /// <summary>
        /// Write a bitmap as image format type.
        /// </summary>
        /// <param name="bmp">bitmap</param>
        /// <param name="outputFileName">output filename</param>
        /// <param name="formatType">image format type</param>
        public static void WriteBitmapAsType(Bitmap bmp, string outputFileName, ImageFormat formatType)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    bmp.Save(memory, formatType);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// Convert ITK image to openCV image.
        /// </summary>
        /// <typeparam name="T">color space / type</typeparam>
        /// <typeparam name="D">pixel datatype</typeparam>
        /// <param name="image">itk image</param>
        /// <returns>opencv image</returns>
        public static Image<T, D> ConvertSitkImageToOpenCv<T, D>(sitk.Image image) where T : struct, IColor where D : new()
        {
            string filename = Path.GetTempPath() + "\\temp_image.png";
            WriteSitkImage(image, filename);
            return ReadOpenCVImageFromFile<T, D>(filename);
        }

        /// <summary>
        /// Write ITK image to file.
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="outputFileName">output filename</param>
        public static void WriteSitkImage(sitk.Image img, string outputFileName)
        {
            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(outputFileName);
            writer.Execute(img);
            writer.Dispose();
        }
        
        [Obsolete("Unused at the moment.")]
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

        /// <summary>
        /// Read fixed point set from file.
        /// </summary>
        /// <param name="filenamePointSets">filename</param>
        /// <returns>dictionary with point sets</returns>
        public static Dictionary<int, CoordPoint> ReadFixedPointSet(string filenamePointSets)
        {
            if (File.Exists(filenamePointSets))
            {
                string content = File.ReadAllText(filenamePointSets);
                //string[] temp = content.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                string[] sPoints = content.Split('\n').Skip(2).Where(it => !string.IsNullOrWhiteSpace(it)).ToArray();
                Dictionary<int, CoordPoint> points = new Dictionary<int, CoordPoint>();

                int i = 0;
                foreach (string sPoint in sPoints)
                {
                    double[] coords = sPoint.Replace('.', ',').Split(' ').Where(it => it != null).Select(it => double.Parse(it)).ToArray();
                    points.Add(i, new CoordPoint(coords[0], coords[1]));
                    i++;
                }
                return points;
            }
            return null;
        }

        /// <summary>
        /// Read transformed point sets from file.
        /// </summary>
        /// <param name="filenameTransformedFilename">filename</param>
        /// <returns>dictionary of point sets</returns>
        public static Dictionary<int, CoordPoint> ReadTransformedPointSets(string filenameTransformedFilename)
        {
            if (File.Exists(filenameTransformedFilename))
            {
                string content = File.ReadAllText(filenameTransformedFilename);
                string[] lines = content.Split('\n');

                Dictionary<int, CoordPoint> transformedPoints = new Dictionary<int, CoordPoint>();

                int i = 0;
                foreach (string line in lines)
                {
                    List<string> entries = line.Split(';').ToList();
                    string transformedPoint = entries.Where(it => it.Contains("OutputPoint")).FirstOrDefault();
                    if(transformedPoint != null)
                    {
                        transformedPoints.Add(i, ExractCoordsFromString(transformedPoint));
                    }
                    i++;
                }
                return transformedPoints;
            }

            return null;
        }

        /// <summary>
        /// Extract coord points from a string of form "[ x.xx y.yy ]"
        /// </summary>
        /// <param name="sCoord">coord string</param>
        /// <returns>point as CoordPoint</returns>
        private static CoordPoint ExractCoordsFromString(string sCoord)
        {
            string[] temp = sCoord.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            double[] coords = temp[1].Replace('.', ',').Split(' ').Where(it => it != null && it != "").Select(it => double.Parse(it)).ToArray();
            return new CoordPoint(coords[0], coords[1]);
        }

        /// <summary>
        /// Serialize an object to JSON and save to file.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="toSerialize">object to serialize</param>
        /// <param name="filename">filename</param>
        public static void SerializeObjectToJSON<T>(T toSerialize, string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, toSerialize);
            }
        }

        /// <summary>
        /// Serialize object list to JSON and save to file.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="toSerialize">object list to serialize</param>
        /// <param name="filename">filename</param>
        public static void SerializeObjectListToJSON<T>(List<T> toSerialize, string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, toSerialize);
            }
        }

        /// <summary>
        /// Deserialize an object list from JSON-file.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="filename">filename</param>
        /// <returns>list of deserialized objects</returns>
        public static List<T> DeserializeObjectListFromJSON<T>(string filename)
        {
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<T> data = (List<T>)serializer.Deserialize(file, typeof(List<T>));
                return data;
            }
        }

        /// <summary>
        /// Deserialize an object from JSON-file.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="filename">filename</param>
        /// <returns>deserialized object</returns>
        public static T DeserializeObjectFromJSON<T>(string filename)
        {
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                T data = (T)serializer.Deserialize(file, typeof(T));
                return data;
            }
        }

        /// <summary>
        /// Get output directory from registration parameters and optional iteration.
        /// </summary>
        /// <param name="parameters">params</param>
        /// <param name="i">iteration</param>
        /// <returns>output directory</returns>
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
