using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ButtonState=Microsoft.Xna.Framework.Input.ButtonState;
using Keys=Microsoft.Xna.Framework.Input.Keys;

namespace XnaPhysXLoader.Cameras
{
    public class GodCamera : Camera
    {
        private Vector3 _velocity = Vector3.Zero;
        private Matrix cameraRotation;
        private KeyboardState previousState;
        private MouseState previousMouseState;

        public GodCamera(string id,float aspectRatio,Form mainWindow) : base(id,aspectRatio,mainWindow)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
                                                             aspectRatio, NearPlane,
                                                             FarPlane);
            cameraRotation = Matrix.Identity;
            previousState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

        }

        public override void Update(float elapseMillis)
        {
            //System.Diagnostics.Debug.WriteLine("elapsedMillis: " + elapseMillis);
            KeyboardState states = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            var mouseCenter = new Vector2(window.Width/2, window.Height/2);
            Vector2 delta = cursorPosition - mouseCenter;
            Vector2 deltaDampened = delta*0.0015f;

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                CenterCursor();

                if (previousMouseState.RightButton == ButtonState.Released)
                {
                    Cursor.Hide();
                    previousState = states;
                    previousMouseState = mouseState;
                    return;
                }
                else
                {
                    // Should perhaps extract the yaw and pitch from the current direction of the camera
                    Yaw -= deltaDampened.X;
                    Pitch -= deltaDampened.Y;
                    //
                }
                    
            }
            else if (previousMouseState.RightButton == ButtonState.Pressed)
            {
                Cursor.Show();
            }


            Vector3 forward = Matrix.Invert(View).Forward;
            Vector3 position = Matrix.Invert(View).Translation;

            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed)
            {
                cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0.0f);         
            }

            if(states.IsKeyDown(Keys.Escape) && previousState.IsKeyUp(Keys.Escape))
            {
                Yaw = 0;
                Pitch = 0;
                cameraRotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0.0f);
                position = Vector3.Zero;
            }
                

            Vector3 newForward = Vector3.TransformNormal(Vector3.Forward, cameraRotation);

            var elapsed = (float)(elapseMillis / 1000.0);
                // Elapsed time since last frame in seconds
            const float speed = 0.02f; // 20 distance units per second
            float distance = speed*(1/elapsed); // d = vt

            // The amount of movement * the direction of movement, then rotate that along the direction we are looking
            Vector3 translateDirection = Vector3.Zero;

            

            
            if (states.IsKeyDown(Keys.W)) // Forwards
                translateDirection += Vector3.TransformNormal(Vector3.Forward, cameraRotation)*1.5f;

            if (states.IsKeyDown(Keys.S)) // Backwards
                translateDirection += Vector3.TransformNormal(Vector3.Backward, cameraRotation)*1.5f;

            if (states.IsKeyDown(Keys.A)) // Left
                translateDirection += Vector3.TransformNormal(Vector3.Left, cameraRotation)*1.5f;

            if (states.IsKeyDown(Keys.D)) // Right
                translateDirection += Vector3.TransformNormal(Vector3.Right, cameraRotation)*1.5f;

            Vector3 newPosition = position;
            _velocity = Vector3.Normalize(translateDirection)*distance;
            if (translateDirection.LengthSquared() > 0)
                newPosition += _velocity;

            Position = newPosition;
            base.forward = newForward;
            view = Matrix.CreateLookAt(newPosition, newPosition + newForward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(1,
                                                             AspectRatio,
                                                             NearPlane,
                                                             FarPlane);
            base.forward = Matrix.Invert(View).Forward;
            base.Update(elapseMillis);
            previousState = states;
            previousMouseState = mouseState;
        }

        public override Vector3 CalculateVelocity()
        {
            return _velocity;
        }
    }
}