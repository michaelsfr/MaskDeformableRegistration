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

namespace MaskedDeformableRegistrationApp.Utils
{
    public static class ReadWriteUtils
    {
        public static Image<Bgr, byte> ReadOpenCVImageFromFile(string file)
        {
            Image<Bgr, byte> image = null;
            try
            {
                image = new Image<Bgr, byte>(file);
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
    }
}
