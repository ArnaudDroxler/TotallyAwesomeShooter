using System;
using UnityEngine;

// ADAPTED FROM https://github.com/Zinglish/quake3-movement-unity3d/blob/master/CPMPlayer.js
// C# version is not updated so i adapted JS version

namespace TAS
{
    [RequireComponent(typeof(CharacterController))]

    public class CharacterControls : MonoBehaviour
    {
        // ----------------
        //  Public settings
        // ----------------

        // player view camera
        public Camera m_camera;

        // Settings Classes
        [Serializable]
        public class ViewSettings
        {
            public float playerViewYOffset = 0.6f;
            public float xMouseSensitivity = 30.0f;
            public float yMouseSensitivity = 30.0f;

        }

        [Serializable]
        public class MovementSettings
        {
            public float moveSpeed = 7.0f;                // Ground move speed
            public float runAcceleration = 14.0f;         // Ground accel
            public float runDeacceleration = 10.0f;       // Deacceleration that occurs when running on the ground
            public float airAcceleration = 2.0f;          // Air accel
            public float airDecceleration = 2.0f;         // Deacceleration experienced when ooposite strafing
            public float airControl = 0.3f;               // How precise air control is
            public float sideStrafeAcceleration = 50.0f;  // How fast acceleration occurs to get up to sideStrafeSpeed when
            public float sideStrafeSpeed = 1.0f;          // What the max speed to generate when side strafing
            public float jumpSpeed = 8.0f;                // The speed at which the character's up axis gains when hitting jump
            public float moveScale = 1.0f;
            public float gravity = 20.0f;
            public float friction = 6.0f;
        }

        // ----------------
        //  Tools
        // ----------------
        public MovementSettings movementSettings = new MovementSettings();
        public ViewSettings viewSettings = new ViewSettings();

        private CharacterController m_controller;

        // Camera rotations
        private float rotX = 0.0f;
        private float rotY = 0.0f;
        
        private Vector3 playerVelocity = Vector3.zero;

        // Q3: players can queue the next jump just before he hits the ground
        private bool wishJump = false;

        // Player status
        private Vector3 spawnPosition;
        private Quaternion spawnRotation;
        private bool isDead;

        /*print() style */
        public GUIStyle style;

        // -----------------
        //  Methods
        // -----------------
        private void Start()
        {
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // setup camera
            m_camera = GetComponentInChildren<Camera>();
            m_camera.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + viewSettings.playerViewYOffset,
                transform.position.z);
                
            // setup controller
            m_controller = GetComponent<CharacterController>();

            // setup spawn point
            spawnPosition = transform.position;
            spawnRotation = m_camera.transform.rotation;

            // make player alive
            isDead = false;
            
        }

        private void Update()
        {
            
            // Ensure the cursor is locked
            if (Cursor.lockState == CursorLockMode.None)
            {
                if (Input.GetMouseButton(0))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            RotateView();

            //Jump detection here to not miss it
            QueueJump();

            // this should be in fixedUpdate but lags
            Vector2 input = GetInput();
            
            if (m_controller.isGrounded)
                GroundMove(input);
            else
                AirMove(input);

            // Ceiling  colision detection (no floating when hitting ceiling)
            // TODO
            // TODO
            // TODO

            // Move the character
            m_controller.Move(playerVelocity * Time.deltaTime);

            if (Input.GetKeyUp("x"))
                PlayerExplode();
            if (Input.GetButton("Fire1") && isDead)
                PlayerSpawn();
        }


        // -------------------
        //   Custom Methods
        // -------------------

        private void RotateView()
        {
            rotX -= Input.GetAxis("Mouse X") * viewSettings.xMouseSensitivity * 0.02f;
            rotY += Input.GetAxis("Mouse Y") * viewSettings.yMouseSensitivity * 0.02f;

            // Clamp the X rotation
            if (rotX < -90)
                rotX = -90;
            else if (rotX > 90)
                rotX = 90;

            transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
            m_camera.transform.rotation = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera
        }

        private Vector2 GetInput()
        {
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
            return input;
        }

        // Movement methods
        private void QueueJump()
        {
            if (Input.GetButtonDown("Jump"))
                wishJump = true;
            if (Input.GetButtonUp("Jump"))
                wishJump = false;
        }

        private void GroundMove(Vector2 input)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material.color = Color.gray;

            Vector3 wishDir;
            float wishSpeed;

            if (!wishJump)
                ApplyFriction(1.0f);
            else
                ApplyFriction(0);
            
            wishDir = new Vector3(input.x, 0, input.y);
            wishDir = transform.TransformDirection(wishDir);
            wishDir.Normalize();

            wishSpeed = wishDir.magnitude;
            wishSpeed *= movementSettings.moveSpeed;
            
            Accelerate(wishDir, wishSpeed, movementSettings.runAcceleration);


            playerVelocity.y = -1 * movementSettings.gravity * Time.deltaTime;

            if (wishJump)
            {
                playerVelocity.y = movementSettings.jumpSpeed;
                wishJump = false;
            }
        }

        private void AirMove(Vector2 input)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material.color = Color.red;


            Vector3 wishDir;
            float accel;
            float wishSpeed;

            // CmdScale is fucked up, insane air speed when using it
            float scale = 1.0f;

            wishDir = new Vector3(input.x, 0, input.y);
            wishDir = transform.TransformDirection(wishDir);

            wishSpeed = wishDir.magnitude;
            wishSpeed *= movementSettings.moveSpeed;

            wishDir.Normalize();
            wishSpeed *= scale;

            float wishSpeed2 = wishSpeed;
            if (Vector3.Dot(playerVelocity, wishDir) < 0)
                accel = movementSettings.airDecceleration;
            else
                accel = movementSettings.airAcceleration;

            // If the player is ONLY strafing left or right
            if (input.x == 0 && input.y != 0)
            {
                if (wishSpeed > movementSettings.sideStrafeSpeed)
                    wishSpeed = movementSettings.sideStrafeSpeed;
                accel = movementSettings.sideStrafeAcceleration;
            }

            Accelerate(wishDir, wishSpeed, accel);

            AirControl(input, wishDir, wishSpeed2);

            playerVelocity.y -= movementSettings.gravity * Time.deltaTime;
        }

        private void AirControl(Vector2 input, Vector3 wishDir, float wishSpeed)
        {
            float zspeed;
            float speed;
            float dot;
            float k;

            // Can't control movement if not moving forward or backward
            if (Mathf.Abs(input.x) < 0.001 || Mathf.Abs(wishSpeed) < 0.001)
                return;

            zspeed = playerVelocity.y;
            playerVelocity.y = 0;

            /* Next two lines are equivalent to idTech's VectorNormalize() */
            speed = playerVelocity.magnitude;
            playerVelocity.Normalize();

            dot = Vector3.Dot(playerVelocity, wishDir);
            k = 32;
            k *= movementSettings.airControl * dot * dot * Time.deltaTime;

            // Change direction while slowing down
            //if (dot > 0)
            {
                playerVelocity.x = playerVelocity.x * speed + wishDir.x * k;
                playerVelocity.y = playerVelocity.y * speed + wishDir.y * k;
                playerVelocity.z = playerVelocity.z * speed + wishDir.z * k;

                playerVelocity.Normalize();
            }

            playerVelocity.x *= speed;
            playerVelocity.y = zspeed; // Note this line
            playerVelocity.z *= speed;
        }

        private void Accelerate(Vector3 wishDir, float wishSpeed, float accel)
        {
            float addSpeed;
            float accelSpeed;
            float currentSpeed;

            currentSpeed = Vector3.Dot(playerVelocity, wishDir);
            addSpeed = wishSpeed - currentSpeed;
            if (addSpeed <= 0)
                return;

            accelSpeed = accel * Time.deltaTime * wishSpeed;
            if (accelSpeed > addSpeed)
                accelSpeed = addSpeed;

            playerVelocity.x += accelSpeed * wishDir.x;
            playerVelocity.z += accelSpeed * wishDir.z;
        }

        private void ApplyFriction(float t)
        {
            Vector3 vec = playerVelocity; // Equivalent to: VectorCopy();
            float speed;
            float newspeed;
            float control;
            float drop;

            vec.y = 0.0f;
            speed = vec.magnitude;
            drop = 0.0f;

            /* Only if the player is on the ground then apply friction */
            if (m_controller.isGrounded)
            {
                control = speed < movementSettings.runDeacceleration ? movementSettings.runDeacceleration : speed;
                drop = control * movementSettings.friction * Time.deltaTime * t;
            }

            newspeed = speed - drop;
            if (newspeed < 0)
                newspeed = 0;
            if (speed > 0)
                newspeed /= speed;

            playerVelocity.x *= newspeed;
            // playerVelocity.y *= newspeed;
            playerVelocity.z *= newspeed;
        }

        // -----------------
        //  SpeedOmeter
        // -----------------
        private void OnGUI()
        {
            Vector3 ups = m_controller.velocity;
            ups.y = 0;
            GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups", style);
        }
       


        // -----------------
        //  Player actions
        // -----------------
        private void PlayerExplode()
        {
            isDead = true;
        }

        private void PlayerSpawn()
        {
            transform.position = spawnPosition;
            m_camera.transform.rotation = spawnRotation;
            rotX = 0.0f;
            rotY = 0.0f;
            playerVelocity = Vector3.zero;
            isDead = false;
        }


        // -----------------
        //  Rocket jumps
        // -----------------
        public void RocketForce(float power, Vector3 origin, float radius)
        {
            Vector3 tossDirection;
            tossDirection = transform.position - origin;
            float force = tossDirection.magnitude * (radius /power);
            Vector3 tossVector = tossDirection / force;
            playerVelocity += tossVector;

            Debug.Log(tossVector);
        }
    }
}   