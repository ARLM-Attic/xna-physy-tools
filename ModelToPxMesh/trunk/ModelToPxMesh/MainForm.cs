#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelToPxMesh;
using StillDesign.PhysX;
using XnaPhysXLoader;

#endregion

namespace WinFormsContentLoading
{
    /// <summary>
    /// Custom form provides the main user interface for the program.
    /// In this sample we used the designer to fill the entire form with a
    /// ModelViewerControl, except for the menu bar which provides the
    /// "File / Open..." option.
    /// </summary>
    public partial class MainForm : Form
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;


        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            contentBuilder = new ContentBuilder();

            contentManager = new ContentManager(modelViewerControl.Services,
                                                contentBuilder.OutputDirectory);


            /// Automatically bring up the "Load Model" dialog when we are first shown.
            //this.Shown += OpenMenuClicked;
        }


        /// <summary>
        /// Event handler for the Exit menu option.
        /// </summary>
        void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Event handler for the Open menu option.
        /// </summary>
        void OpenMenuClicked(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Model";

            fileDialog.Filter = "Model Files (*.fbx;*.x)|*.fbx;*.x|" +
                                "FBX Files (*.fbx)|*.fbx|" +
                                "X Files (*.x)|*.x|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadModel(fileDialog.FileName);
            }
        }


        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>
        void LoadModel(string fileName)
        {
            Cursor = Cursors.WaitCursor;

            // Unload any existing model.
            modelViewerControl.Model = null;
            contentManager.Unload();

            // Tell the ContentBuilder what to build.
            contentBuilder.Clear();
            contentBuilder.Add(fileName, "Model", null, "ModelProcessor");

            // Build this new model data.
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                modelViewerControl.Model = contentManager.Load<Model>("Model");
            }
            else
            {
                // If the build failed, display an error message.
                MessageBox.Show(buildError, "Error");
            }
            Cursor = Cursors.Arrow;
        }

        private void OnCheckedChange(object sender, EventArgs e)
        {
            var obj = sender as ToolStripMenuItem;
            if(obj.Checked)
            {
                PhysX.Instance.SetPhysXDebugParameters(PhysicsParameter.VisualizeCollisionFaceNormals,true);
            }
            else
            {
                PhysX.Instance.SetPhysXDebugParameters(PhysicsParameter.VisualizeCollisionFaceNormals, false);
            }
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            new About().Show(this);   
        }

        private void OnPreviewPhysX(object sender, EventArgs e)
        {
            if(!modelViewerControl.Preview())
            {
                MessageBox.Show(this, "Please load a model before trying to create a PhysX Mesh", "No Model Loaded",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void OnSavePhysXMesh(object sender, EventArgs e)
        {

            SaveFileDialog fileDialog = new SaveFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../..");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Save PhysX Mesh";

            fileDialog.Filter = "PhysX Mesh Files (*.pxmsh;*.bin)|*.pxmsh;*.bin|" +
                                "All Files (*.*)|*.*";
            fileDialog.FileName = "mesh_" + DateTime.Now.Millisecond + ".pxmsh";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!modelViewerControl.SaveMesh(fileDialog.FileName))
                {
                    MessageBox.Show(this, "Please load a model before trying to create a PhysX Mesh", "No Model Loaded",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void OnOpenPhysXDescription(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load PhysX Mesh Description";

            fileDialog.Filter = "PhysX Mesh Files (*.bin;*.pxmsh)|*.bin;*.pxmsh|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadPhysXMesh(fileDialog.FileName);
            }
        }

        private void LoadPhysXMesh(string name)
        {
            if(!modelViewerControl.LoadPxMesh(name))
            {
                MessageBox.Show(this, "Is not possible to load your file, please check if it correct", "Error loading file",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnCullBoth(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;

            if (item.Checked)
            {
                modelViewerControl.GraphicsDevice.RenderState.CullMode = CullMode.None;
                miCullBack.Checked = false;
                miCullFront.Checked = false;
            }


        }

        private void OnCullFront(object sender, EventArgs e)
        {

            var item = sender as ToolStripMenuItem;

            if (item.Checked)
            {
                modelViewerControl.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
                miCullBack.Checked = false;
                miCullBoth.Checked = false;
            }
        }

        private void OnCullBack(object sender, EventArgs e)
        {

            var item = sender as ToolStripMenuItem;

            if (item.Checked)
            {
                modelViewerControl.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                miCullBoth.Checked = false;
                miCullFront.Checked = false;
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            new QuickHelp().Show(this);
        }
    }
}
