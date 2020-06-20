using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class PreSegmentationForm : Form
    {
        List<WSIHistoObject> objects = new List<WSIHistoObject>();

        double[] background_color = new double[] { 0, 0, 0 };

        private bool horizontal = true; // decides wether segments are ordered horizontal or vertical
        private bool descending = true; // decides wether segments are ordered asc or desc

        private List<string> WsiPaths;

        private Object thislock = new Object();

        public PreSegmentationForm(List<string> wsiPaths)
        {
            InitializeComponent();

            WsiPaths = wsiPaths;
        }

        private void PreSegmentationForm_Load(object sender, EventArgs e)
        {
            InitializeButtonsAndScrolls();
            EnableButtons();
        }

        private void PreSegmentationForm_Paint(object sender, PaintEventArgs e)
        {
            lock (thislock)
            {
                int height = 0;
                int currentColumn = 0;
                for (int i = 0; i < objects.Count; i++)
                {
                    // arrange wsi objects side by side
                    int maxColumns = this.panel1.Width / objects[0].Width;
                    objects[i].Location = new Point(objects[i].Width * currentColumn, height - panel1.VerticalScroll.Value);
                    currentColumn++;
                    // new row if number of columns has reached max number of columns
                    if (currentColumn >= maxColumns)
                    {
                        currentColumn = 0;
                        height += objects[i].Height;
                    }
                }
            }
        }

        private void InitializeButtonsAndScrolls()
        {
            hScrollBarThreshold.Minimum = 0;
            hScrollBarThreshold.Maximum = 255;
            hScrollBarThreshold.Value = 220;

            hScrollBarMaxContourSize.Minimum = 0;
            hScrollBarMaxContourSize.Maximum = 50000; // Todo: calculate by image size

            hScrollBarMinContourSize.Minimum = 0;
            hScrollBarMinContourSize.Maximum = 50000;

            trackBarResolutuion.Minimum = 0;
            trackBarResolutuion.Maximum = 9;
            trackBarResolutuion.Value = 7;

            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 9;
            numericUpDown1.Value = trackBarResolutuion.Value;

            textBox1.Text = "stack-name";
        }

        private void EnableButtons()
        {
            buttonCreateStack.Enabled = buttonProceed.Enabled = objects.Count > 0;
        }

        private void radioButtonNone_CheckedChanged(object sender, EventArgs e)
        {
            this.descending = true;
            this.horizontal = true;
        }

        private void radioButtonHorzDesc_CheckedChanged(object sender, EventArgs e)
        {
            this.descending = true;
            this.horizontal = true;
        }

        private void radioButtonHorzAsc_CheckedChanged(object sender, EventArgs e)
        {
            this.descending = false;
            this.horizontal = true;
        }

        private void radioButtonVertDesc_CheckedChanged(object sender, EventArgs e)
        {
            this.descending = true;
            this.horizontal = false;
        }

        private void radioButtonVertAsc_CheckedChanged(object sender, EventArgs e)
        {
            this.descending = false;
            this.horizontal = false;
        }

        private void buttonSegment_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<List<Rectangle>> obj_contours = new List<List<Rectangle>>();
            
            foreach (string path in WsiPaths)
            {
                try
                {
                    Image<Bgr, byte> image = WSIExtraction.Extraction.ExtractImageFromWSI(path, 7);
                    List<Rectangle> extracted = WSIExtraction.Extraction.ExtractObjects(image, this.hScrollBarThreshold.Value, 1000, out Image<Bgr, byte> image_debug);
                    obj_contours.Add(extracted);
                    AddObjectsFromContours(image, extracted, path, 7);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(ex.Message);
                    return;
                } finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            EnableButtons();
            this.Invalidate();
        }

        private void AddObjectsFromContours(Image<Bgr, Byte> image, List<Rectangle> contours, string wsi_path, int resolution_level)
        {
            // order bboxes from top to button / left to right (horizontal descending)
            if (this.horizontal == true && this.descending == true)
            {
                // sort by x coordinate
                contours.Sort((x, y) => x.Left.CompareTo(y.Left));
                // than sort by y coordinate
                contours.Sort((x, y) => x.Top.CompareTo(y.Top));
            }
            if (this.horizontal == true && this.descending == false)
            {
                contours.Sort((x, y) => x.Left.CompareTo(y.Left));
                contours.Sort((x, y) => y.Top.CompareTo(x.Top));
            }
            if (this.horizontal == false && this.descending == true)
            {
                contours.Sort((x, y) => y.Left.CompareTo(x.Left));
                contours.Sort((x, y) => x.Top.CompareTo(y.Top));
            }
            if (this.horizontal == true && this.descending == false)
            {
                contours.Sort((x, y) => y.Left.CompareTo(x.Left));
                contours.Sort((x, y) => y.Top.CompareTo(x.Top));
            }

            for (int j = 0; j < contours.Count; j++)
            {
                Rectangle contour = contours[j];
                WSIHistoObject obj = new WSIHistoObject();

                using (Image<Bgr, Byte> subimage = image.Copy(new Rectangle(contour.Left, contour.Top, contour.Width, contour.Height)))
                {
                    obj.PictureBox_SetImage(subimage.Bitmap, resolution_level);
                }

                int layerFactor = (int)(Math.Pow(2.0, (double)resolution_level));
                obj.objRectangle = new Rectangle(contour.X * layerFactor, contour.Y * layerFactor, contour.Width * layerFactor, contour.Height * layerFactor);
                obj.wsiImagePath = wsi_path;
                obj.labelTitle.Text = System.IO.Path.GetFileNameWithoutExtension(wsi_path) + "_" + j.ToString();
                obj.AllowDrop = true;
                obj.DragDrop += new System.Windows.Forms.DragEventHandler(this.wsiHistoObject_DragDrop);
                obj.DragOver += new System.Windows.Forms.DragEventHandler(this.wsiHistoObject_DragOver);
                obj.MouseDown += new System.Windows.Forms.MouseEventHandler(this.wsiHistoObject_MouseDown);
                obj.labelTitle.DragDrop += new System.Windows.Forms.DragEventHandler(this.wsiHistoObjectChild_DragDrop);
                obj.labelTitle.DragOver += new System.Windows.Forms.DragEventHandler(this.wsiHistoObjectChild_DragOver);
                obj.labelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.wsiHistoObjectChild_MouseDown);
                obj.PictureBox_AddDragDropHandler(new System.Windows.Forms.DragEventHandler(this.wsiHistoObjectChild_DragDrop));
                obj.PictureBox_AddDragOverHandler(new System.Windows.Forms.DragEventHandler(this.wsiHistoObjectChild_DragOver));
                obj.PictureBox_AddMouseDownHandler(new System.Windows.Forms.MouseEventHandler(this.wsiHistoObjectChild_MouseDown));
                obj.buttonRemove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.wsiHistoObject_ButtonRem_Click);

                objects.Add(obj);
                this.panel1.Controls.Add(obj);
            }
        }

        private void wsiHistoObject_ButtonRem_Click(object sender, MouseEventArgs e)
        {
            // remove object from form and objects list
            WSIHistoObject obj = (WSIHistoObject)((Button)sender).Parent;
            panel1.Controls.Remove(obj);
            objects.Remove(obj);
            EnableButtons();
            // repaint form so the control disappears
            this.Invalidate();
        }

        private void wsiHistoObjectChild_MouseDown(object sender, MouseEventArgs e)
        {
            wsiHistoObject_MouseDown(((Control)sender).Parent, e);
        }

        private void wsiHistoObjectChild_DragOver(object sender, DragEventArgs e)
        {
            wsiHistoObject_DragOver(((Control)sender).Parent, e);
        }

        private void wsiHistoObjectChild_DragDrop(object sender, DragEventArgs e)
        {
            wsiHistoObject_DragDrop(((Control)sender).Parent, e);
        }

        private void wsiHistoObject_MouseDown(object sender, MouseEventArgs e)
        {
            ((WSIHistoObject)sender).DoDragDrop(sender, DragDropEffects.Move);
        }

        private void wsiHistoObject_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void wsiHistoObject_DragDrop(object sender, DragEventArgs e)
        {
            // get index of the target object
            int index = objects.IndexOf((WSIHistoObject)sender);
            if (index < 0) index = objects.Count - 1;
            WSIHistoObject data = (WSIHistoObject)e.Data.GetData(typeof(WSIHistoObject));
            // move source object to target position
            objects.Remove(data);
            objects.Insert(index, data);
            // repaint form so rearangement becomes visible
            this.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBarResolutuion.Value = (int)numericUpDown1.Value;
        }

        private void trackBarResolutuion_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBarResolutuion.Value;
        }

        private void buttonCreateStack_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Path.Combine(ApplicationContext.OutputPath, textBox1.Text));
            for (int i = 0; i < objects.Count; i++)
            {
                WSIHistoObject obj = objects[i];
                SaveROIAsPNG(obj, (int)numericUpDown1.Value);
            }

            WriteStackV2((int)numericUpDown1.Value);
        }

        private string SaveROIAsPNG(WSIHistoObject histoObject, int resolution)
        {
            string path = null;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using(VMscope.InteropCore.ImageStreaming.IStreamingImage image = VMscope.VirtualSlideAccess.Sdk.GetImage(histoObject.wsiImagePath))
                {
                    int layerFactor = (int)(Math.Pow(2.0, resolution));
                    // is bitmap loaded correctly?
                    using (Bitmap bmp = image.GetImagePart(histoObject.objRectangle.X, histoObject.objRectangle.Y, histoObject.objRectangle.Width, histoObject.objRectangle.Height, (int)(histoObject.objRectangle.Width / layerFactor), (int)(histoObject.objRectangle.Height / layerFactor)))
                    {
                        /*using(BitmapViewer viewer = new BitmapViewer(bmp))
                        {
                            viewer.ShowDialog();
                        }*/

                        // flip image so it fits the thumbnail
                        for (int k = 0; k < histoObject.NrOfRotations % 4; k++)
                        {
                            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        if (histoObject.HorizontalFlip)
                            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        // write image to file
                        path = Path.Combine(ApplicationContext.OutputPath, textBox1.Text, (histoObject.labelTitle.Text + ".png"));
                        bmp.Save(path, ImageFormat.Png);
                    
                        Console.WriteLine("saved image " + histoObject.labelTitle.Text);
                        bmp.Dispose();
                    }
                    image.Dispose();
                }

                return path;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message);
                return path;
            } finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void WriteStack(int threshold)
        {
            StreamWriter file = new StreamWriter(Path.Combine(ApplicationContext.OutputPath, this.textBox1.Text, "stack.json"), false);
            file.WriteLine("{");
            file.WriteLine("  \"stack\": [");
            bool is_first_stack = true;
            {
                if (is_first_stack)
                {
                    is_first_stack = false;
                    file.WriteLine("    {");
                }
                else
                {
                    file.WriteLine("    ,{");
                }

                file.WriteLine("      \"stackname\": \"" + this.textBox1.Text.Replace("\\", "\\\\") + "\",");
                file.WriteLine("      \"threshold\": " + (threshold.ToString().Replace("\\", "\\\\") + ","));
                file.WriteLine("      \"backgroundcolor\": [");
                file.WriteLine("        " + (int)(Math.Round(background_color[0])) + ",");
                file.WriteLine("        " + (int)(Math.Round(background_color[1])) + ",");
                file.WriteLine("        " + (int)(Math.Round(background_color[2])));
                file.WriteLine("      ],");
                file.WriteLine("      \"resolutionlevel\": " + ((int)this.numericUpDown1.Value).ToString() + ",");
                file.WriteLine("      \"section\": [");
                bool is_first_section = true;

                foreach (WSIHistoObject obj in objects)
                {
                    if (is_first_section)
                    {
                        is_first_section = false;
                    }
                    else
                    {
                        file.WriteLine(",");
                    }
                    file.WriteLine("        {");
                    file.WriteLine("          \"path\": \"" + obj.labelTitle.Text.Replace("\\", "\\\\") + ".png" + "\",");
                    file.WriteLine("          \"wsi_source_path\": \"" + obj.wsiImagePath.Replace("\\", "\\\\") + "\",");
                    file.WriteLine("          \"rotationclockwise\": " + ((int)(obj.NrOfRotations % 4) * 90).ToString() + ",");
                    file.WriteLine("          \"X\": " + obj.objRectangle.X + ",");
                    file.WriteLine("          \"Y\": " + obj.objRectangle.Y + ",");
                    file.WriteLine("          \"width\": " + obj.objRectangle.Width + ",");
                    file.WriteLine("          \"height\": " + obj.objRectangle.Height);
                    file.Write("        }");
                }
                file.WriteLine("");
                file.WriteLine("      ]");


                file.WriteLine("    }");

            }
            file.WriteLine("  ]");
            file.WriteLine("}");
            file.Close();
            // back to normal cursor
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Done");
        }

        private void WriteStackV2(int threshold)
        {
            WSIExtraction.Stack stack = new WSIExtraction.Stack();
            stack.Stackname = this.textBox1.Text;
            stack.Threshold = threshold;
            List<int> backgroundColor = new List<int>();
            backgroundColor.Add((int)(Math.Round(background_color[0])));
            backgroundColor.Add((int)(Math.Round(background_color[1])));
            backgroundColor.Add((int)(Math.Round(background_color[2])));
            stack.Backgroundcolor = backgroundColor;
            stack.Resolutionlevel = (int)this.numericUpDown1.Value;

            List<WSIExtraction.Slice> slices = new List<WSIExtraction.Slice>();

            foreach (WSIHistoObject obj in objects)
            {
                WSIExtraction.Slice slice = new WSIExtraction.Slice();
                slice.Path = obj.labelTitle.Text + ".png";
                slice.WsiSourcePath = obj.wsiImagePath;
                slice.X = obj.objRectangle.X;
                slice.Y = obj.objRectangle.Y;
                slice.Width = obj.objRectangle.Width;
                slice.Height = obj.objRectangle.Height;
                slices.Add(slice);
            }

            stack.Section = slices;

            ReadWriteUtils.SerializeObjectToJSON(stack, Path.Combine(ApplicationContext.OutputPath, this.textBox1.Text, "stack.json"));

            // back to normal cursor
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Done");
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            List<string> toRegistrate = new List<string>();
            Directory.CreateDirectory(Path.Combine(ApplicationContext.OutputPath, textBox1.Text));
            for (int i = 0; i < objects.Count; i++)
            {
                WSIHistoObject obj = objects[i];
                toRegistrate.Add(SaveROIAsPNG(obj, (int)numericUpDown1.Value));
            }

            WriteStackV2((int)numericUpDown1.Value);

            this.Hide();
            using(RegistrationForm form = new RegistrationForm(toRegistrate))
            {
                form.ShowDialog();
            }
            this.Close();
        }
    }
}
