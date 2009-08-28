/**************************
 * Created by:            *
 *      Vando Pereira     *
 * Date Created:          *
 *      01-08-2009        *
 * Last Revision:         *
 *      05-08-2009        *
 *************************/
#region File Description
//-----------------------------------------------------------------------------
// ModelViewerControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StillDesign.PhysX;
using XnaPhysXLoader;
using XnaPhysXLoader.Cameras;
using XnaPhysXLoader.Helpers;
using XNAPhysXTools;
using ButtonState=Microsoft.Xna.Framework.Input.ButtonState;
using Keys=Microsoft.Xna.Framework.Input.Keys;

#endregion

namespace WinFormsContentLoading
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    class ModelViewerControl : GraphicsDeviceControl
    {
        private Camera godCamera;
        private Camera fixedCamera;
        private Camera orthoCamera;
        private PhysX physX;
        private float previousTime;
        private Actor modelActor;
        private KeyboardState previousState = Keyboard.GetState();
        private MouseState previousMouseState = Mouse.GetState();
        private bool flipNormals;
        private PxMeshEncoder encoder;
        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public Model Model
        {
            get { return model; }

            set
            {
                model = value;
                previewed = false;

                if (modelActor != null)
                {
                    modelActor.Dispose();
                }

                if (model != null)
                {
                    MeasureModel();
                    //modelActor = ProcessModel(value, Vector3.Zero, 0);
                }

                
                
                
            }
        }

        Model model;


        // Cache information about the model size and position.
        Matrix[] boneTransforms;
        Vector3 modelCenter;
        float modelRadius;


        // Timer controls the rotation speed.
        Stopwatch timer;
        private bool previewed;


        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            physX = PhysX.Instance;
            // Start the animation timer.
            timer = Stopwatch.StartNew();

            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;

            godCamera = new GodCamera("godCamera", aspectRatio, Form.ActiveForm);
            fixedCamera = new DefferedCamera("fixedCamera", aspectRatio, Form.ActiveForm);
            orthoCamera = new OrthographicCamera("orthoCamera", 1, 1000, new Vector3(0, 50, 500), new Vector3(0, 0, 0),
                                                 aspectRatio, Form.ActiveForm);

            CamerasManager.Instance.Activate(godCamera);

            physX.Initialize(GraphicsDevice);

            PhysicsHelper.CreateGroundPlane();

            GraphicsDevice.RenderState.CullMode = CullMode.None;

            //PhysicsHelper.CreateBox(new Vector3(0, 50, 0), new Vector3(2), 100);

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public Actor ProcessModel(string fileName,Model model, Vector3 position, float orientation)
        {
            previewed = true;
            var actorDesc = new ActorDescription();

            if (fileName != null)
                encoder = new PxMeshEncoder(fileName);

            foreach (ModelMesh mesh in model.Meshes)
            {
                actorDesc.Shapes.Add(RetrievePhysicsTriangleMeshFromMesh(mesh,model.Root.Transform *
                                                                                       mesh.ParentBone.Transform,true,ref encoder));
            }
            actorDesc.GlobalPose = Matrix.CreateRotationY(orientation) * Matrix.CreateTranslation(position);
            //actorDesc.GlobalPose = modelSceneNode.Model.Root.Transform;
            return PhysX.Instance.Scene.CreateActor(actorDesc);
            //PhysicsHelper.SetActorCollisionGroup(modelSceneNode.Actor, CollisionGroup.GROUP_COLLIDABLE_NON_PUSHABLE);
        }


        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            var camera = CamerasManager.Instance.GetActiveCamera();
            // Clear to the default control background color.
            Color backColor = new Color(BackColor.R, BackColor.G, BackColor.B);
            float elapsedTime = timer.ElapsedMilliseconds - previousTime;
            previousTime = timer.ElapsedMilliseconds;

            GraphicsDevice.Clear(backColor);

            HandleInput();




            //// Compute camera matrices.
            //float rotation = (float)timer.Elapsed.TotalSeconds;

            //Vector3 eyePosition = modelCenter;

            //eyePosition.Z += modelRadius * 2;
            //eyePosition.Y += modelRadius;

            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            
            //float nearClip = modelRadius / 100;
            //float farClip = modelRadius * 100;

            //camera.NearPlane = modelRadius/100;
            //camera.FarPlane = modelRadius*100;

            Matrix world = Matrix.Identity;//Matrix.CreateRotationY(rotation);
            //Matrix view = Matrix.CreateLookAt(eyePosition, modelCenter, Vector3.Up);
            //Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspectRatio,
            //                                                    nearClip, farClip);
            //camera.CameraPosition = eyePosition;
            camera.ModelCenter = modelCenter;
            camera.AspectRatio = aspectRatio;

            camera.Update(elapsedTime);
            physX.Update(timer.ElapsedMilliseconds);

            if (model != null)
            {
                // Draw the model.
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = boneTransforms[mesh.ParentBone.Index] * world;
                        //effect.View = view;
                        //effect.Projection = projection;
                        
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;

                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.SpecularPower = 16;
                    }

                    mesh.Draw();
                }
            }

            physX.Draw();

            
        }

        private void HandleInput()
        {
            var states = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                var actor = PhysicsHelper.CreateSphere(CamerasManager.Instance.GetActiveCamera().Position, 1, 0.001f);
                actor.LinearVelocity = 50*CamerasManager.Instance.GetActiveCamera().Forward;
            }
            previousMouseState = mouseState;
            previousState = states;
        }


        /// <summary>
        /// Whenever a new model is selected, we examine it to see how big
        /// it is and where it is centered. This lets us automatically zoom
        /// the display, so we can correctly handle models of any scale.
        /// </summary>
        void MeasureModel()
        {
            // Look up the absolute bone transforms for this model.
            boneTransforms = new Matrix[model.Bones.Count];
            
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            // Compute an (approximate) model center position by
            // averaging the center of each mesh bounding sphere.
            modelCenter = Vector3.Zero;

            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere;
                Matrix transform = boneTransforms[mesh.ParentBone.Index];
                Vector3 meshCenter = Vector3.Transform(meshBounds.Center, transform);

                modelCenter += meshCenter;
            }

            modelCenter /= model.Meshes.Count;

            // Now we know the center point, we can compute the model radius
            // by examining the radius of each mesh bounding sphere.
            modelRadius = 0;

            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere;
                Matrix transform = boneTransforms[mesh.ParentBone.Index];
                Vector3 meshCenter = Vector3.Transform(meshBounds.Center, transform);

                float transformScale = transform.Forward.Length();
                
                float meshRadius = (meshCenter - modelCenter).Length() +
                                   (meshBounds.Radius * transformScale);

                modelRadius = Math.Max(modelRadius,  meshRadius);
            }
        }

        public bool Preview()
        {
            if(model == null)
                return false;

            try
            {
                ProcessModel(null, model, Vector3.Zero, 0);
            }
// ReSharper disable RedundantCatchClause
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return false;
#endif
            }
// ReSharper restore RedundantCatchClause
            

            return true;
        }

        public bool SaveMesh(string fileName)
        {
            if (model == null)
                return false;
            try
            {
                ProcessModel(fileName, model, Vector3.Zero, 0);
                encoder.Save();
            }
// ReSharper disable RedundantCatchClause
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return false;
#endif
            }
// ReSharper restore RedundantCatchClause
            
            
            return true;
        }

        /// <summary>
        /// This method take a name of a file, created by this app and create a PhysX actor with all shapes saved on file.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool LoadPxMesh(string name)
        {
            //Get the streams names from file
            var streams = PxMeshDecoder.Load(name);
            var actorDesc = new ActorDescription();

            try
            {
                for (int i = 0; i < streams.Count; i++)
                {
                    actorDesc.Shapes.Add(GetPxMeshFromFile(streams[i], Matrix.Identity)); //Load the PxMesh present in file
                }
                actorDesc.GlobalPose = Matrix.CreateRotationY(0) * Matrix.CreateTranslation(Vector3.Zero);
                //actorDesc.GlobalPose = modelSceneNode.Model.Root.Transform;
                modelActor = PhysX.Instance.Scene.CreateActor(actorDesc);

                return true;
            }
// ReSharper disable RedundantCatchClause
            catch (Exception)
            {
#if DEBUG                
                throw;
#else
                return false;
#endif
            }
// ReSharper restore RedundantCatchClause
            
        }

        /// <summary>
        /// Here is where the magic happens! It loads the physX file and get from it the triangle mesh.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="transforms"></param>
        /// <returns></returns>
        public static TriangleMeshShapeDescription GetPxMeshFromFile(string stream, Matrix transforms)
        {
            var fs = File.OpenRead(stream);
     
            TriangleMesh pMesh = PhysX.Instance.Core.CreateTriangleMesh(fs);
            fs.Close();
            File.Delete(stream);


            // Create TriangleMesh above code segment.
            pMesh.SaveToDescription();

            var tmsd = new TriangleMeshShapeDescription();
            tmsd.TriangleMesh = pMesh;
            tmsd.LocalPose = transforms;
            tmsd.MeshPagingMode = MeshPagingMode.Auto;
            return tmsd;
        }

        public static TriangleMeshShapeDescription RetrievePhysicsTriangleMeshFromMesh(ModelMesh mesh, Matrix transforms, bool flipNormals, ref PxMeshEncoder encoder)
        {
            Vector3[] vertices;
            VertexPositionNormalTexture[] meshVerts;
            var triangleMeshDesc = new TriangleMeshDescription();

            triangleMeshDesc.TriangleCount = mesh.MeshParts[0].PrimitiveCount;
            triangleMeshDesc.AllocateTriangles<int>(mesh.MeshParts[0].PrimitiveCount);

            meshVerts = new VertexPositionNormalTexture[mesh.MeshParts[0].NumVertices];
            if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
            {
                var indices = new short[mesh.IndexBuffer.SizeInBytes / 2];
                mesh.IndexBuffer.GetData(indices);
                foreach (int ui in indices)
                {
                    triangleMeshDesc.TriangleStream.Write(ui);
                }
            }
            else
            {
                var indices = new int[mesh.IndexBuffer.SizeInBytes / 4];
                mesh.IndexBuffer.GetData(indices);
                foreach (int ui in indices)
                {
                    triangleMeshDesc.TriangleStream.Write(ui);
                }
            }

            mesh.VertexBuffer.GetData(meshVerts);


            vertices = new Vector3[mesh.MeshParts[0].NumVertices];
            for (int i = 0; i < meshVerts.Length; i++)
            {
                vertices[i] = meshVerts[i].Position;
            }

            triangleMeshDesc.VertexCount = vertices.Length;


            triangleMeshDesc.AllocateVertices<Vector3>(vertices.Length);

            foreach (Vector3 vec in vertices)
            {
                triangleMeshDesc.VerticesStream.Write(vec);
            }

            if (flipNormals)
                triangleMeshDesc.Flags = MeshFlag.FlipNormals;



            var stream = new MemoryStream();
            Cooking.InitializeCooking(new ConsoleOutputStream());
            Cooking.CookTriangleMesh(triangleMeshDesc, stream);
            Cooking.CloseCooking();

            stream.Position = 0;
            if(encoder!=null)
                encoder.AddStream(stream);

            TriangleMesh pMesh = PhysX.Instance.Core.CreateTriangleMesh(stream);


            // Create TriangleMesh above code segment.
            pMesh.SaveToDescription();

            var tmsd = new TriangleMeshShapeDescription();
            tmsd.TriangleMesh = pMesh;
            tmsd.LocalPose = transforms;
            tmsd.MeshPagingMode = MeshPagingMode.Auto;
            return tmsd;
        }
    }
}
