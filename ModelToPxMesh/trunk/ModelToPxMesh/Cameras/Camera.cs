using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaPhysXLoader.Cameras
{
    public abstract class Camera
    {
        #region fields

        private Vector3 actualPosition;
        private float aspectRatio;
        private bool enabled;
        protected float farPlane = 1000.0f;
        private float fieldOfView = MathHelper.PiOver4;
        protected Vector3 forward;
        private BoundingFrustum frustum;
        private string id;
        protected Vector3 lookAt;
        protected float nearPlane = 1f;
        private float pitch;
        protected Matrix projection;
        private Quaternion rotation = Quaternion.Identity;
        protected Vector3 up = Vector3.Up;
        protected Matrix view;
        protected Form window;

        #region Properties

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix ViewProjectionMatrix
        {
            get { return Matrix.Multiply(view, projection); }
        }

        public Vector3 Position
        {
            get { return actualPosition; }
            set { actualPosition = value; }
        }

        public Vector3 Forward
        {
            get { return forward; }
        }

        /// <summary>
        /// The camera's rotation.
        /// </summary>
        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// The camera's field of view in radians.
        /// </summary>
        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                fieldOfView = value;
                Perspective(fieldOfView, aspectRatio, NearPlane, FarPlane);
            }
        }

        /// <summary>
        /// The near clipping plane.
        /// </summary>
        public virtual float NearPlane
        {
            get { return nearPlane; }
            set
            {
                nearPlane = value;
                Perspective(fieldOfView, aspectRatio, nearPlane, farPlane);
            }
        }

        /// <summary>
        /// The far clipping plane.
        /// </summary>
        public virtual float FarPlane
        {
            get { return farPlane; }
            set
            {
                farPlane = value;
                Perspective(fieldOfView, aspectRatio, nearPlane, farPlane);
            }
        }

        /// <summary>
        /// The camera's frustum.
        /// </summary>
        public BoundingFrustum Frustum
        {
            get
            {
                if (frustum == null)
                    frustum = new BoundingFrustum(Matrix.Multiply(view, projection));
                return frustum;
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public String Id
        {
            get { return id; }
        }

        public Vector3 Up
        {
            get { return up; }
        }

        public Vector3 Velocity
        {
            get { return CalculateVelocity(); }
        }

        public Vector3 ModelCenter { get; set; }
        public Vector3 CameraPosition { get; set; }

        #endregion

        public bool Jittering { get; set; }

        public float Pitch
        {
            get { return pitch; }
            set
            {
                pitch = MathHelper.Clamp(value,
                                         -MathHelper.ToRadians(89), MathHelper.ToRadians(89));
            }
        }

        public float Yaw { get; set; }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                Perspective(fieldOfView, aspectRatio, NearPlane, FarPlane);
            }
        }

        #endregion

        protected Camera(string id,float aspectRatio, Form mainWindow)
        {
            this.id = id;

            CamerasManager.Instance.AddCamera(this);
            view = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            //projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, _engine.Game.GraphicsDevice.Viewport.AspectRatio, NearPlane, FarPlane);

            window = mainWindow;

            CenterCursor();

            
        }

        protected void CenterCursor()
        {

            Mouse.SetPosition(window.Width/2, window.Height/2);
        }

        #region Public Methods

        public abstract Vector3 CalculateVelocity();

        /// <summary>
        /// Aims the camera at a point in space.
        /// </summary>
        /// <param name="target">The point in space the camara will face.</param>
        /// <param name="up">A normal indicating the up direction.</param>
        public void LookAt(Vector3 target, Vector3 up)
        {
            view = Matrix.CreateLookAt(actualPosition, target, up);

            rotation = Quaternion.CreateFromRotationMatrix(Matrix.Invert(view));

            forward = target - actualPosition;
            forward.Normalize();
        }

        public virtual void Update(float elapsedMillis)
        {
            frustum = new BoundingFrustum(Matrix.Multiply(view, projection));
        }

        protected virtual BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(actualPosition, 0);
        }

        public void Perspective(float fovx, float aspect, float znear, float zfar)
        {
            fieldOfView = fovx;
            aspectRatio = aspect;
            nearPlane = znear;
            farPlane = zfar;

            float aspectInv = 1.0f/aspect;
            float e = 1.0f/(float) Math.Tan(MathHelper.ToRadians(fovx)/2.0f);
            float fovy = 2.0f*(float) Math.Atan(aspectInv/e);
            float xScale = 1.0f/(float) Math.Tan(0.5f*fovy);
            float yScale = xScale/aspectInv;

            projection.M11 = xScale;
            projection.M12 = 0.0f;
            projection.M13 = 0.0f;
            projection.M14 = 0.0f;

            projection.M21 = 0.0f;
            projection.M22 = yScale;
            projection.M23 = 0.0f;
            projection.M24 = 0.0f;

            projection.M31 = 0.0f;
            projection.M32 = 0.0f;
            projection.M33 = (zfar + znear)/(znear - zfar);
            projection.M34 = -1.0f;

            projection.M41 = 0.0f;
            projection.M42 = 0.0f;
            projection.M43 = (2.0f*zfar*znear)/(znear - zfar);
            projection.M44 = 0.0f;
        }

        public virtual void Draw()
        {
            throw new NotImplementedException();
        }


        public void GetWorldMatrix(out Matrix worldMatrix)
        {
            worldMatrix = Matrix.Invert(view);
        }

        public void GetViewMatrix(out Matrix viewMatrix)
        {
            viewMatrix = view;
        }

        #endregion
    }
}