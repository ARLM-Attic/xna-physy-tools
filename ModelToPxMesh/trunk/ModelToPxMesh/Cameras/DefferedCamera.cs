#region Using Statements

using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ButtonState=Microsoft.Xna.Framework.Input.ButtonState;
using Keys=Microsoft.Xna.Framework.Input.Keys;

#endregion

namespace XnaPhysXLoader.Cameras
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DefferedCamera : Camera
    {
        private float cameraArc = -30;
        private float cameraDistance = 300;
        private float cameraRotation;
        private GamePadState currentGamePadState;
        private KeyboardState currentKeyboardState;
        private Vector3 position;

        public DefferedCamera(string id,float aspectRatio,Form mainWindow) : base(id,aspectRatio,mainWindow)
        {
        }

        public float CameraArc
        {
            get { return cameraArc; }
            set { cameraArc = value; }
        }

        public float CameraRotation
        {
            get { return cameraRotation; }
            set { cameraRotation = value; }
        }

        public float CameraDistance
        {
            get { return cameraDistance; }
            set { cameraDistance = value; }
        }

        public override Vector3 CalculateVelocity()
        {
            return Vector3.Zero;
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(float elapsedMillis)
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // TODO: Add your update code here



            // Check for input to rotate the camera up and down around the model.
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                currentKeyboardState.IsKeyDown(Keys.W))
            {
                cameraArc += elapsedMillis * 0.01f;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                currentKeyboardState.IsKeyDown(Keys.S))
            {
                cameraArc -= elapsedMillis * 0.01f;
            }

            cameraArc += currentGamePadState.ThumbSticks.Right.Y * elapsedMillis * 0.05f;

            // Limit the arc movement.
            if (cameraArc > 90.0f)
                cameraArc = 90.0f;
            else if (cameraArc < -90.0f)
                cameraArc = -90.0f;

            // Check for input to rotate the camera around the model.
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                currentKeyboardState.IsKeyDown(Keys.D))
            {
                cameraRotation += elapsedMillis * 0.01f;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                currentKeyboardState.IsKeyDown(Keys.A))
            {
                cameraRotation -= elapsedMillis * 0.01f;
            }

            cameraRotation += currentGamePadState.ThumbSticks.Right.X * elapsedMillis * 0.05f;

            // Check for input to zoom camera in and out.
            if (currentKeyboardState.IsKeyDown(Keys.Z))
                cameraDistance += elapsedMillis * 0.025f;

            if (currentKeyboardState.IsKeyDown(Keys.X))
                cameraDistance -= elapsedMillis * 0.025f;

            cameraDistance += currentGamePadState.Triggers.Left * elapsedMillis * 0.025f;
            cameraDistance -= currentGamePadState.Triggers.Right * elapsedMillis * 0.025f;

            // Limit the arc movement.
            if (cameraDistance > 11900.0f)
                cameraDistance = 11900.0f;
            else if (cameraDistance < 10.0f)
                cameraDistance = 10.0f;

            if (currentGamePadState.Buttons.RightStick == ButtonState.Pressed ||
                currentKeyboardState.IsKeyDown(Keys.R))
            {
                cameraArc = -30;
                cameraRotation = 0;
                cameraDistance = 100;
            }

            view = Matrix.CreateTranslation(0, -10, 0)*
                   Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation))*
                   Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc))*
                   Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                       ModelCenter, Vector3.Up);

            //view = Matrix.CreateLookAt(CameraPosition, ModelCenter, Vector3.Up);

            position = Vector3.Transform(Vector3.Zero, Matrix.Invert(view));

            projection = Matrix.CreatePerspectiveFieldOfView(1,
                                                             AspectRatio,
                                                             NearPlane,
                                                             FarPlane);

            base.Update(elapsedMillis);
        }
    }
}