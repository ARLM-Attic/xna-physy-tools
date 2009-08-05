/********************************
* PhysX.cs                      *
* Created on: 08 March 09       *
* Last Modified on: 29 July 09  *
*********************************/
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaPhysXLoader.Cameras;
using StillDesign.PhysX;


namespace XnaPhysXLoader
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public sealed class PhysX
    {
        #region fields

        private bool _bCCDEnabled;
        private Camera _camera;
        private BasicEffect _visualizationEffect;
        private GraphicsDevice graphicsDevice;
        private static readonly PhysX instance = new PhysX();
        
        #endregion

        #region Public Properties

        public Core Core { get; private set; }

        public Scene Scene { get; private set; }

        public ControllerManager ControllerManager { get; private set; }

        public event EventHandlerItem<EventArgs> OnGravityChanged;

        #endregion

        private PhysX()
        {
        }

        public static PhysX Instance
        {
            get { return instance; }
        }


        /// <summary>
        /// Allows physics to initialize itself and its parameters
        /// </summary>
        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
           _visualizationEffect = new BasicEffect(this.graphicsDevice, null) {VertexColorEnabled = true};
            _bCCDEnabled = false;
            //_visualizationEffect.EnableDefaultLighting();

            var coreDesc = new CoreDescription();
            var output = new UserOutput();

            Core = new Core(coreDesc, output);

//#if DEBUG
            //_core.SetParameter( PhysicsParameter.VisualizationScale, 2.0f );
            Core.SetParameter(PhysicsParameter.VisualizationScale, 1.0f);
            Core.SetParameter(PhysicsParameter.VisualizeCollisionShapes, true);
            Core.SetParameter(PhysicsParameter.VisualizeClothMesh, true);
            Core.SetParameter(PhysicsParameter.VisualizeJointLocalAxes, true);
            Core.SetParameter(PhysicsParameter.VisualizeJointLimits, true);
            Core.SetParameter(PhysicsParameter.VisualizeFluidPosition, true);
            Core.SetParameter(PhysicsParameter.VisualizeFluidEmitters, false); // Slows down rendering a bit to much
            Core.SetParameter(PhysicsParameter.VisualizeForceFields, true);
            Core.SetParameter(PhysicsParameter.VisualizeSoftBodyMesh, true);
            Core.SetParameter(PhysicsParameter.DefaultSleepLinearVelocitySquared, 2.0f*2.0f);
            Core.SetParameter(PhysicsParameter.DefaultSleepAngularVelocitySquared, 2.0f*2.0f);
            Core.SetParameter(PhysicsParameter.VisualizeCollisionFaceNormals,true);
//#endif

            //aLk's Data INIT
            Core.SetParameter(PhysicsParameter.SkinWidth, 0.01f);
            Core.SetParameter(PhysicsParameter.VisualizeActorAxes, true);
            //aLk's Data END

            var sceneDesc = new SceneDescription
                                {
                                    SimulationType = SimulationType.Software,
                                    Gravity = new Vector3(0.0f, -9.81f, 0.0f)
                                };

            Scene = Core.CreateScene(sceneDesc);

            ControllerManager = Scene.CreateControllerManager();

            //Lesson 05: Materials
            //Material defaultMat = _scene.Materials[0];

            //defaultMat.Restitution = 0.25f;
            //defaultMat.StaticFriction = 0.5f;
            //defaultMat.DynamicFriction = 0.5f;
            //end lesson

            // Connect to the remote debugger if its there
            Core.Foundation.RemoteDebugger.Connect("localhost");
            Core.SetParameter(PhysicsParameter.ContinuousCollisionDetection, true);

        }

        public void SetGravity(string cmd, string[] args)
        {
            try
            {
                var gravity = new Vector3(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]));
                SetGravity(gravity);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Restart the physics world
        /// </summary>
        public void RestarPhysics()
        {
            Scene.FetchResults(SimulationStatus.RigidBodyFinished, true);
            Core.Dispose();
            Initialize(graphicsDevice);
        }

        /// <summary>
        /// Allows physics to process the simulation
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(float totalMillis)
        {
            // Update Physics
            Scene.Simulate((float) totalMillis/1000.0f);
            //_scene.Simulate( 1.0f / 60.0f );
            Scene.FlushStream();
            Scene.FetchResults(SimulationStatus.RigidBodyFinished, true);
        }

        /// <summary>
        /// Allows physics to draw itself information, only if DebugMode state is activated
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw()
        {
            _camera = CamerasManager.Instance.GetActiveCamera();

            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);


            _visualizationEffect.World = Matrix.Identity;
            _visualizationEffect.View = _camera.View;
            _visualizationEffect.Projection = _camera.Projection;

            DebugRenderable data = Scene.GetDebugRenderable();

            _visualizationEffect.Begin();

            foreach (EffectPass pass in _visualizationEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                if (data.PointCount > 0)
                {
                    DebugPoint[] points = data.GetDebugPoints();

                    graphicsDevice.DrawUserPrimitives(PrimitiveType.PointList, points, 0, points.Length);
                }

                if (data.LineCount > 0)
                {
                    DebugLine[] lines = data.GetDebugLines();

                    var vertices = new VertexPositionColor[data.LineCount*2];
                    for (int x = 0; x < data.LineCount; x++)
                    {
                        DebugLine line = lines[x];

                        vertices[x*2 + 0] = new VertexPositionColor(line.Point0, Int32ToColor(line.Color));
                        vertices[x*2 + 1] = new VertexPositionColor(line.Point1, Int32ToColor(line.Color));
                    }

                    graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, lines.Length);
                }

                if (data.TriangleCount > 0)
                {
                    DebugTriangle[] triangles = data.GetDebugTriangles();

                    var vertices = new VertexPositionColor[data.TriangleCount*3];
                    for (int x = 0; x < data.TriangleCount; x++)
                    {
                        DebugTriangle triangle = triangles[x];

                        vertices[x*3 + 0] = new VertexPositionColor(triangle.Point0, Int32ToColor(triangle.Color));
                        vertices[x*3 + 1] = new VertexPositionColor(triangle.Point1, Int32ToColor(triangle.Color));
                        vertices[x*3 + 2] = new VertexPositionColor(triangle.Point2, Int32ToColor(triangle.Color));
                    }

                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, triangles.Length);
                }

                pass.End();
            }

            _visualizationEffect.End();

        }

        #region PhysXParameters

        /// <summary>
        /// Sets the Skin with, that controls the percentage of intersection between two actors permited.
        /// </summary>
        /// <param name="value"></param>
        public void SetSkinWidth(float value)
        {
            Core.SetParameter(PhysicsParameter.SkinWidth, value);
        }

        /// <summary>
        /// Activate or Deactivate Continuous Collision Detection. Default: disabled
        /// </summary>
        public void ToggleCCD()
        {
            _bCCDEnabled = !_bCCDEnabled;
            Core.SetParameter(PhysicsParameter.ContinuousCollisionDetection, _bCCDEnabled);
        }

        /// <summary>
        /// Change some of properties of your simulation debug view like, what you want to see?
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public void SetPhysXDebugParameters(PhysicsParameter param, float value)
        {
            Core.SetParameter(param, value);
        }

        public void SetPhysXDebugParameters(PhysicsParameter param, bool value)
        {
            Core.SetParameter(param, value);
        }

        /// <summary>
        /// Configure values of each physics iteration
        /// </summary>
        /// <param name="timeStep">Maximum size of a substep. Range: (0,inf) </param>
        /// <param name="maxIterations">Maximum number of iterations to divide a timestep into. </param>
        /// <param name="timeSpetIsFixed">Method to use for timestep (either variable time step or fixed)</param>
        public void SetTimeConfiguration(float timeStep, int maxIterations, bool timeSpetIsFixed)
        {
            TimestepMethod timeMethod;
            if (timeSpetIsFixed)
            {
                timeMethod = TimestepMethod.Fixed;
            }
            else
            {
                timeMethod = TimestepMethod.Variable;
            }

            var time = new SceneTimingInformation();
            time.MaximumTimestep = timeStep;
            time.MaximumIterations = maxIterations;
            time.TimestepMethod = timeMethod;
            Scene.TimingInformation = time;
        }

        public void SetGravity(Vector3 gravity)
        {
            Scene.Gravity = gravity;
            if (OnGravityChanged != null)
                OnGravityChanged(null, null);
        }

        protected void Dispose(bool disposing)
        {
            //while (Scene.ControllerManagers[0].Controllers.Count>0)
            //{
            //    Scene.ControllerManagers[0].Controllers[0].Dispose();
            //} 

            //while(Scene.Actors.Count>0)
            //{
            //    while (Scene.Actors[0].Shapes.Count>0)
            //    {
            //        Scene.Actors[0].Shapes[0].Dispose();
            //    }
            //    Scene.Actors[0].Dispose();
            //}

            Scene.FetchResults(SimulationStatus.RigidBodyFinished, true);
            Scene.Dispose();
            Core.Dispose();
        }

        #endregion


        /// <summary>
        /// Convert form color in Int format (0-255) to XNA Color format
        /// </summary>
        public static Color Int32ToColor(int color)
        {
            var a = (byte)((color & 0xFF000000) >> 32);
            var r = (byte)((color & 0x00FF0000) >> 16);
            var g = (byte)((color & 0x0000FF00) >> 8);
            var b = (byte)((color & 0x000000FF) >> 0);

            return new Color(r, g, b, a);
        }

    }

}