/*********************************
 * PhysicsHelper.cs              *
 * Created On: 02-2009           *
 * *                             *
 * Last Modified on: 08-04-2009  *
 * Copyright 3DGamingStuff 2009  *
 * *******************************/

using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StillDesign.PhysX;
using XnaPhysXLoader.Cameras;

namespace XnaPhysXLoader.Helpers
{
    /// <summary>
    /// This class provides some usefull functions related with physics
    /// </summary>
    public static class PhysicsHelper
    {
        //public static void SetActorCollisionGroup(Actor actor, CollisionGroup group)
        //{
        //    actor.Group = (short) group;
        //    foreach (Shape shape in actor.Shapes)
        //    {
        //        shape.Group = (short) group;
        //    }
        //}

        public static Vector3 ApplyForceToActor(Actor actor, Vector3 forceDir, float gForceStrenght, float elapsedTime)
        {
            Vector3 forceVec = gForceStrenght*forceDir*elapsedTime*0.001f;
            actor.AddForce(forceVec, ForceMode.Force);
            return forceVec;
        }

        public static void RenderActors(ReadOnlyList<Actor> actors,GraphicsDevice graphicsDevice)
        {
            foreach (Actor actor in actors)
            {
                DrawActor(actor, new Vector3(0.8f, 0.8f, 1),graphicsDevice);
            }
        }

        private static void DrawActor(Actor actor, Vector3 actualColor,GraphicsDevice graphicsDevice)
        {
            foreach (Shape shape in actor.Shapes)
            {
                switch (shape.Type)
                {
                    case ShapeType.Box:
                        //DrawBox((BoxShape) shape, actualColor);
                        break;
                    case ShapeType.Capsule:
                        //DrawCapsule((CapsuleShape) shape, actualColor);
                        break;
                    case ShapeType.Plane:
                        //DrawPlane((PlaneShape) shape);
                        break;
                    case ShapeType.Sphere:
                        //drawSphere((SphereShape) shape, actualColor);
                        break;
                    case ShapeType.Heightfield:
                        drawHeightfield((HeightFieldShape) shape,graphicsDevice);
                        break;
                }
            }
        }

        private static void drawHeightfield(HeightFieldShape heightFieldShape, GraphicsDevice device)
        {
            VertexPositionNormalTexture[] cubeVertices;
            VertexDeclaration basicEffectVertexDeclaration;
            VertexBuffer vertexBuffer;
            var white = new Color(1, 1, 1, 1);
            int vertexNum = (heightFieldShape.HeightField.NumberOfRows - 1)*
                            (heightFieldShape.HeightField.NumberOfColumns - 1)*3*2;
            var vertices = new VertexPositionColor[vertexNum];
            int triangleIndex;
            Vector3 pos;
            int vertexIndex = 0;
            HeightFieldShape.GetTriangleResult triangle;
            Camera _camera = CamerasManager.Instance.GetActiveCamera();

            for (int row = 0; row < heightFieldShape.HeightField.NumberOfRows - 1; row++)
            {
                for (int column = 0; column < heightFieldShape.HeightField.NumberOfColumns - 1; column++)
                {
                    triangleIndex = 2*(row*heightFieldShape.HeightField.NumberOfColumns + column);

                    triangle = heightFieldShape.GetTriangle(triangleIndex);


                    pos = new Vector3(triangle.WorldTriangle.Vertex2.X, triangle.WorldTriangle.Vertex2.Y,
                                      triangle.WorldTriangle.Vertex2.Z);
                    vertices[vertexIndex++] = new VertexPositionColor(pos, white);

                    pos = new Vector3(triangle.WorldTriangle.Vertex1.X, triangle.WorldTriangle.Vertex1.Y,
                                      triangle.WorldTriangle.Vertex1.Z);
                    vertices[vertexIndex++] = new VertexPositionColor(pos, white);

                    pos = new Vector3(triangle.WorldTriangle.Vertex0.X, triangle.WorldTriangle.Vertex0.Y,
                                      triangle.WorldTriangle.Vertex0.Z);
                    vertices[vertexIndex++] = new VertexPositionColor(pos, white);

                    triangleIndex++;

                    triangle = heightFieldShape.GetTriangle(triangleIndex);

                    pos = new Vector3(triangle.WorldTriangle.Vertex2.X, triangle.WorldTriangle.Vertex2.Y,
                                      triangle.WorldTriangle.Vertex2.Z);
                    vertices[vertexIndex++] = new VertexPositionColor(pos, white);

                    pos = new Vector3(triangle.WorldTriangle.Vertex1.X, triangle.WorldTriangle.Vertex1.Y,
                                      triangle.WorldTriangle.Vertex1.Z);
                    vertices[vertexIndex++] = new VertexPositionColor(pos, white);

                    pos = new Vector3(triangle.WorldTriangle.Vertex0.X, triangle.WorldTriangle.Vertex0.Y,
                                      triangle.WorldTriangle.Vertex0.Z);
                    vertices[vertexIndex++] = new VertexPositionColor(pos, white);
                }
            }
            //inicio do código do cubo 
            //InitializeCube();

            basicEffectVertexDeclaration = new VertexDeclaration(
                device, VertexPositionNormalTexture.VertexElements);

            var basicEffect = new BasicEffect(device, null);
            basicEffect.Alpha = 1.0f;
            basicEffect.DiffuseColor = new Vector3(0.2f, 0.0f, 1.0f);
            basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            //basicEffect.DirectionalLight0.Enabled = true;
            //basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
            //basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            //basicEffect.DirectionalLight0.SpecularColor = Vector3.One;

            //basicEffect.DirectionalLight1.Enabled = true;
            //basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
            //basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            //basicEffect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

            //basicEffect.LightingEnabled = true;

            basicEffect.World = Matrix.CreateScale(1, 1, 1);
            basicEffect.View = _camera.View;
            basicEffect.Projection = _camera.Projection;

            //NoNameEngine.Instance.Device.Clear(Color.SteelBlue);
            //NoNameEngine.Instance.Device.RenderState.CullMode = CullMode.CullClockwiseFace;

            device.VertexDeclaration = basicEffectVertexDeclaration;

            vertexBuffer = new VertexBuffer(
                device,
                VertexPositionColor.SizeInBytes*vertexNum,
                BufferUsage.None
                );

            vertexBuffer.SetData(vertices);

            device.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);


            // This code would go between a NoNameEngine.Instance.Device 
            // BeginScene-EndScene block.
            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                //this.NoNameEngine.Instance.Device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, vertexNum / 3);

               device.DrawPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    vertexNum/3
                    );

                //NoNameEngine.Instance.Device.DrawPrimitives(
                //     PrimitiveType.TriangleList,
                //     0,
                //     12
                // );


                pass.End();
            }
            basicEffect.End();
        }


        //public static void drawSphere(SphereShape sphereShape, Vector3 color)
        //{
        //    // Draw the Box
        //    {
        //        Camera _camera = CamerasManager.Instance.GetActiveCamera();
        //        var _sphere = ResourceManager.Instance.Get<Model>("Sphere");
        //        var transforms = new Matrix[_sphere.Bones.Count];
        //        _sphere.CopyAbsoluteBoneTransformsTo(transforms);

        //        foreach (ModelMesh mesh in _sphere.Meshes)
        //        {
        //            foreach (BasicEffect effect in mesh.Effects)
        //            {
        //                effect.DiffuseColor = color;
        //                effect.EnableDefaultLighting();
        //                effect.World = Matrix.CreateScale(sphereShape.Radius, sphereShape.Radius, sphereShape.Radius)*
        //                               transforms[mesh.ParentBone.Index]*sphereShape.GlobalPose;
        //                effect.View = _camera.View;
        //                effect.Projection = _camera.Projection;
        //            }

        //            mesh.Draw();
        //        }
        //    }
        //}

        //private static void DrawBox(BoxShape shape, Vector3 color)
        //{
        //    // Draw the Box
        //    {
        //        Camera _camera = CamerasManager.Instance.GetActiveCamera();
        //        var _box = ResourceManager.Instance.Get<Model>("Cube");
        //        var transforms = new Matrix[_box.Bones.Count];
        //        _box.CopyAbsoluteBoneTransformsTo(transforms);

        //        foreach (ModelMesh mesh in _box.Meshes)
        //        {
        //            foreach (BasicEffect effect in mesh.Effects)
        //            {
        //                effect.DiffuseColor = color;
        //                effect.EnableDefaultLighting();

        //                effect.World = Matrix.CreateScale(shape.Dimensions.X, shape.Dimensions.Y, shape.Dimensions.Z)*
        //                               transforms[mesh.ParentBone.Index]*shape.GlobalPose;
        //                effect.View = _camera.View;
        //                effect.Projection = _camera.Projection;
        //            }

        //            mesh.Draw();
        //        }
        //    }
        //}


        //private static void DrawCapsule(CapsuleShape shape, Vector3 color)
        //{

        //    {
        //        Camera _camera = CamerasManager.Instance.GetActiveCamera();
        //        var _capsule = ResourceManager.Instance.Get<Model>("Capsule");
        //        var transforms = new Matrix[_capsule.Bones.Count];
        //        _capsule.CopyAbsoluteBoneTransformsTo(transforms);

        //        foreach (ModelMesh mesh in _capsule.Meshes)
        //        {
        //            foreach (BasicEffect effect in mesh.Effects)
        //            {
        //                effect.DiffuseColor = color;
        //                effect.EnableDefaultLighting();

        //                effect.World = Matrix.CreateScale(shape.Radius, shape.Height, shape.Radius)*
        //                               transforms[mesh.ParentBone.Index]*shape.GlobalPose;
        //                effect.View = _camera.View;
        //                effect.Projection = _camera.Projection;
        //            }

        //            mesh.Draw();
        //        }
        //    }
        //}

        //private static void DrawPlane(PlaneShape shape)
        //{
        //    {
        //        Camera _camera = CamerasManager.Instance.GetActiveCamera();
        //        var _plane = ResourceManager.Instance.Get<Model>("Plane");
        //        var transforms = new Matrix[_plane.Bones.Count];
        //        _plane.CopyAbsoluteBoneTransformsTo(transforms);

        //        foreach (ModelMesh mesh in _plane.Meshes)
        //        {
        //            foreach (BasicEffect effect in mesh.Effects)
        //            {
        //                effect.EnableDefaultLighting();

        //                effect.World = Matrix.CreateScale(10240.0f, 1.0f, 10240.0f)*transforms[mesh.ParentBone.Index]*
        //                               shape.GlobalPose;
        //                effect.View = _camera.View;
        //                effect.Projection = _camera.Projection;
        //            }

        //            mesh.Draw();
        //        }
        //    }
        //}

        public static Actor RetrievePhysicsTriggerFromModel(Model model, Vector3 position, float orientation)
        {
            var actorDesc = new ActorDescription();
            foreach (ModelMesh mesh in model.Meshes)
            {
                TriangleMeshShapeDescription shape = RetrievePhysicsTriangleMeshFromMesh(mesh,
                                                                                         model.Root.Transform*
                                                                                         mesh.ParentBone.Transform,true);
                shape.Flags |= ShapeFlag.TriggerOnLeave;
                actorDesc.Shapes.Add(shape);
            }
            //actorDesc.Flags = 0;
            actorDesc.GlobalPose = Matrix.CreateRotationY(orientation)*Matrix.CreateTranslation(position);
            //actorDesc.GlobalPose = modelSceneNode.Model.Root.Transform;
            return PhysX.Instance.Scene.CreateActor(actorDesc);
        }

        public static Actor CreateGroundPlane()
        {
            // Create a plane with default descriptor
            var planeDesc = new PlaneShapeDescription();
            var actorDesc = new ActorDescription();
            actorDesc.Shapes.Add(planeDesc);
            return PhysX.Instance.Scene.CreateActor(actorDesc);
        }

        public static Actor CreateBox(Vector3 pos, Vector3 boxDim, float density)
        {
            var actorDesc = new ActorDescription();
            var bodyDesc = new BodyDescription();

            var boxShapeDesc = new BoxShapeDescription
            {
                Dimensions = new Vector3(boxDim.X, boxDim.Y, boxDim.Z),
                //LocalPose = Matrix.CreateTranslation(0, boxDim.Y, 0)
            };

            actorDesc.Shapes.Add(boxShapeDesc);
            actorDesc.GlobalPose = Matrix.CreateTranslation(pos);

            if (density > 0)
            {
                actorDesc.BodyDescription = bodyDesc;
                actorDesc.Density = density;
            }

            return PhysX.Instance.Scene.CreateActor(actorDesc);
        }

        public static TriangleMeshShapeDescription RetrievePhysicsTriangleMeshFromMesh(ModelMesh mesh, Matrix transforms,bool flipNormals)
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

            if(flipNormals)
                triangleMeshDesc.Flags = MeshFlag.FlipNormals;



            var stream = new MemoryStream();
            Cooking.InitializeCooking(new ConsoleOutputStream());
            Cooking.CookTriangleMesh(triangleMeshDesc, stream);
            Cooking.CloseCooking();

            stream.Position = 0;

            TriangleMesh pMesh = PhysX.Instance.Core.CreateTriangleMesh(stream);


            // Create TriangleMesh above code segment.
            pMesh.SaveToDescription();

            var tmsd = new TriangleMeshShapeDescription();
            tmsd.TriangleMesh = pMesh;
            tmsd.LocalPose = transforms;
            tmsd.MeshPagingMode = MeshPagingMode.Auto;
            return tmsd;
        }

        

        public static Actor CreateSphere(Vector3 pos, float radius,
                                                    float density)
        {
            // Add a single-shape actor to the scene
            var actorDesc = new ActorDescription();
            var bodyDesc = new BodyDescription();

            // The actor has one shape, a sphere
            var sphereDesc = new SphereShapeDescription(radius);
            //{
            //    LocalPose = Matrix.CreateTranslation(new Vector3(0, radius, 0))
            //};
            actorDesc.Shapes.Add(sphereDesc);

            if (density > 0)
            {
                actorDesc.BodyDescription = bodyDesc;
                actorDesc.Density = density;
            }
            actorDesc.GlobalPose = Matrix.CreateTranslation(pos);

            return PhysX.Instance.Scene.CreateActor(actorDesc);

        }

    }
}