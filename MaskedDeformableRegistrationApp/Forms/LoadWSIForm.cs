using MaskedDeformableRegistrationApp.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaskedDeformableRegistrationApp
{
    public partial class LoadWSIForm : Form
    {
        private List<string> Filenames { get; set; } = new List<string>();

        private const string _dialogFilter = "WSI files (*.svs)|*.svs|(*.vsf)|*.vsf|(*.ndpi)|*.ndpi";
        private const string _dirDialogFilter = "*.svs|*.vsf|*.ndpi";

        // REMOVE LATER
        private const string _initDir = @"D:\Dokumente\Master Informatik\Masterthesis\Bildmaterial\BoneMarrow\I16_4837";

        public LoadWSIForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeTreeList();
            EnableButtons();
        }

        private void InitializeTreeList()
        {
            treeView1.AllowDrop = true;
            treeView1.ItemDrag += new ItemDragEventHandler(treeView1_ItemDrag);
            treeView1.DragEnter += new DragEventHandler(treeView1_DragEnter);
            treeView1.DragOver += new DragEventHandler(treeView1_DragOver);
            treeView1.DragDrop += new DragEventHandler(treeView1_DragDrop);
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode != null && targetNode.Index + 1 <= treeView1.Nodes.Count)
            {
                draggedNode.Remove();
                treeView1.Nodes.Insert(targetNode.Index + 1, draggedNode);

                Filenames.Remove(draggedNode.Name);
                int index = Filenames.IndexOf(targetNode.Name);
                Filenames.Insert(index + 1, draggedNode.Name);
            }
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
            treeView1.SelectedNode = treeView1.GetNodeAt(targetPoint);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void buttonAddSlices_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = _dialogFilter;
            openFileDialog1.InitialDirectory = _initDir;
            DialogResult result = openFileDialog1.ShowDialog();

            if(result == DialogResult.OK)
            {
                foreach (string path in openFileDialog1.FileNames)  {
                    Filenames.Add(path);
                    string name = Path.GetFileName(path);
                    treeView1.Nodes.Add(path, name);
                }
            }
            EnableButtons();
        }

        private void buttonLoadDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = _initDir;
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                Filenames.Clear();
                treeView1.Nodes.Clear();
                string path = folderBrowserDialog1.SelectedPath;
                string[] files = GetFiles(path, _dirDialogFilter, SearchOption.AllDirectories);
                foreach (string filePath in files)
                {
                    Filenames.Add(filePath);
                    string name = Path.GetFileName(filePath);
                    treeView1.Nodes.Add(filePath, name);
                }
            }
            EnableButtons();
        }
        public string[] GetFiles(string sourceFolder, string filter, SearchOption searchOption)
        {
            ArrayList alFiles = new ArrayList();
            string[] MultipleFilters = filter.Split('|');

            foreach (string FileFilter in MultipleFilters)
            {
                alFiles.AddRange(Directory.GetFiles(sourceFolder, FileFilter, searchOption));
            }

            return (string[])alFiles.ToArray(typeof(string));
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            if(Filenames != null)
            {
                this.Hide();
                using (PreSegmentationForm seg = new PreSegmentationForm(Filenames))
                {
                    seg.ShowDialog();
                }
                this.Close();
            }
            
        }

        private void buttonRemoveSlice_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                int nodeIndex = treeView1.SelectedNode.Index;
                treeView1.Nodes.RemoveAt(nodeIndex);
                Filenames.Remove(treeView1.SelectedNode.Name);
                EnableButtons();
            }
        }

        private void buttonClearList_Click(object sender, EventArgs e)
        {
            Filenames.Clear();
            treeView1.Nodes.Clear();
            EnableButtons();
        }

        private void EnableButtons()
        {
            bool hasNodes = treeView1.Nodes.Count > 0;
            buttonProceed.Enabled = buttonRemoveSlice.Enabled = buttonClearList.Enabled = hasNodes;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string path = e.Node.Name;

            if (path != null)
            {
                using (VMscope.InteropCore.ImageStreaming.IStreamingImage image = VMscope.VirtualSlideAccess.Sdk.GetImage(path))
                {
                    double ratioWidth = (double)image.Size.Width / (double)image.Size.Height;
                    double ratioHeight = (double)image.Size.Height / (double)image.Size.Width;
                    int newWidth = image.Size.Width > image.Size.Height ? pictureBox1.Width : (int)(ratioWidth * pictureBox1.Width);
                    int newHeight = image.Size.Width > image.Size.Height ? (int)(ratioHeight * pictureBox1.Height) : pictureBox1.Height;
                    Bitmap bmp = image.GetImagePart(0, 0, image.Size.Width, image.Size.Height, newWidth, newHeight);

                    pictureBox1.Image = bmp;
                }
            }
        }

    }
}
