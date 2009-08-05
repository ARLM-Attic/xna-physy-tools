using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace XnaPhysXLoader.Cameras
{
    /// <summary>
    /// Camera that uses an orthographic projection
    /// </summary>
    public class OrthographicCamera : Camera
    {
        /// <summary>
        /// Creates a camera using an orthographic projection
        /// </summary>
        /// <param name="width">Width of the projection volume</param>
        /// <param name="height">Height of the projection volume</param>
        /// <param name="nearClip">Distance to near clip plane</param>
        /// <param name="farClip">Distance to far clip plane</param>
        public OrthographicCamera(string id, float xMin, float xMax, float yMin, float yMax, float nearClip,
                                  float farClip, Vector3 position, Vector3 look, float aspectRatio, Form mainWindow)
            : base(id,aspectRatio,mainWindow)
        {
            nearPlane = nearClip;
            farPlane = farClip;
            Matrix.CreateOrthographicOffCenter(xMin, xMax, yMin, yMax, nearClip, farClip, out projection);

            view = Matrix.CreateLookAt(position, look, Vector3.Up);
            //Matrix.CreateOrthographic(width, height, nearClip, farClip, out projection);
            view = Matrix.Identity;
        }

        public OrthographicCamera(string id, float nearClip, float farClip, Vector3 position, Vector3 look, float aspectRatio, Form mainWindow)
            : base(id, aspectRatio,mainWindow)
        {
            nearPlane = nearClip;
            farPlane = farClip;

            Matrix.CreateOrthographic(200, 200, nearClip, farClip, out projection);
            //this.view = view;
            view = Matrix.CreateLookAt(position, look, Vector3.Up);
        }

        public override Vector3 CalculateVelocity()
        {
            return Vector3.Zero;
        }

        public void SetViewMatrix(ref Matrix viewMatrix)
        {
            view = viewMatrix;
        }


        public void UpdateViewMatrix(Vector3 position, Vector3 look)
        {
            view = Matrix.CreateLookAt(position, look, Vector3.Up);
        }
    }
}