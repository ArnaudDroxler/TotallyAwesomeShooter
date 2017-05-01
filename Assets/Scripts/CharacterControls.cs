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
            //public float sideStrafeAcceleration = 50.0f;  // How fast acceleration occurs to get up to sideStrafeSpeed when
            //public float sideStrafeSpeed = 1.0f;          // What the max speed to generate when side strafing
            public float jumpSpeed = 8.0f;                // The speed at which the character's up axis gains when hitting jump
            public float WallJumpSpeed = 10.0f;
            public float moveScale = 1.0f;
            public float gravity = 20.0f;
            public float friction = 6.0f;
        }

        // ----------------
        //  Tools
        // ----------------
        // player view camera
        public Camera m_camera;

        public MovementSettings movementSettings = new MovementSettings();
        public ViewSettings viewSettings = new ViewSettings();
        
        public CharacterController m_controller;

        // Camera rotations
        private float rotX = 0.0f;
        private float rotY = 0.0f;
        
        public Vector3 playerVelocity = Vector3.zero;

        // Q3: players can queue the next jump just before he hits the ground
        private bool wishJump = false;

        // Player status
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;

        public Vector3 reSpawnPosition;
        public float reSpawnRotY;

        public float timer = 0.0f;
        public bool timerOk = false;

        /*print() style */
        public GUIStyle style;

        private float fallZone = -50.0f;
        

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

            // Move the character
            m_controller.Move(playerVelocity * Time.deltaTime);

            if(m_controller.transform.position.y < fallZone)
                PlayerReSpawn();
            if (Input.GetKeyUp("r"))
                PlayerReSpawn();

            if (timerOk)
                timer += Time.deltaTime * 1000;
        }
        
        // -------------------
        //   Custom Methods
        // -------------------

        public void SetupPlayer(Transform t)
        {
            spawnPosition = t.position;
            spawnPosition.y += 2;
            spawnRotation = t.rotation;
            reSpawnRotY = t.rotation.eulerAngles.y;
            PlayerSpawn();
        }

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

        // -------------------
        // Movement methods
        // -------------------

        public void QueueJump()
        {
            if (Input.GetButtonDown("Jump"))
                wishJump = true;
            if (Input.GetButtonUp("Jump"))
                wishJump = false;
        }

        private void GroundMove(Vector2 input)
        {
            // Debug coloring in gray when on the ground
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
            
            // reset gravity ( <1 = isGrounded flickering)
            playerVelocity.y = - 1;

            if (wishJump)
            {
                playerVelocity.y = movementSettings.jumpSpeed;
                wishJump = false;
            }
        }

        private void AirMove(Vector2 input)
        {
            // Debug coloring in red when on the ground
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

            Accelerate(wishDir, wishSpeed, accel);

            // No air control if angle too big
            float yVel = playerVelocity.y;
            playerVelocity.y = 0;
            if (Vector3.Angle(wishDir, playerVelocity) < 45)
                AirControl(input, wishDir, wishSpeed2);

            playerVelocity.y = yVel - movementSettings.gravity * Time.deltaTime;
        }

        private void AirControl(Vector2 input, Vector3 wishDir, float wishSpeed)
        {
            float zspeed;
            float speed;
            float dot;
            float k;


            
            // Good air control if moving only forward or backward
            if (Mathf.Abs(input.y) > 0 && Mathf.Abs(input.x) < 0.001)
            {
                zspeed = playerVelocity.y;
                playerVelocity.y = 0;

                /* Next two lines are equivalent to idTech's VectorNormalize() */
                speed = playerVelocity.magnitude;
                playerVelocity.Normalize();
                
                playerVelocity.x = playerVelocity.x + wishDir.x * movementSettings.airControl;
                playerVelocity.y = playerVelocity.y + wishDir.y * movementSettings.airControl;
                playerVelocity.z = playerVelocity.z + wishDir.z * movementSettings.airControl;

                playerVelocity.Normalize();

                playerVelocity.x *= speed;
                playerVelocity.y = zspeed; // Note this line
                playerVelocity.z *= speed;

                return;
            }
            // Mediocre if diagonals
            else if (Mathf.Abs(input.y) > 0 && Mathf.Abs(input.x) > 0)
            {
                zspeed = playerVelocity.y;
                playerVelocity.y = 0;

                /* Next two lines are equivalent to idTech's VectorNormalize() */
                speed = playerVelocity.magnitude;
                playerVelocity.Normalize();

                dot = Vector3.Dot(playerVelocity, wishDir);
                k = 32;
                k *= movementSettings.airControl * dot * dot * Time.deltaTime;

                // Change direction while slowing down
                if (dot > 0)
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

            else
            {

            }
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

        // ------------------
        // Player interactions
        // ------------------

        private void OnControllerColliderHit(ControllerColliderHit hit)
        { 
            // reset y velocity if hit ceiling
            if((m_controller.collisionFlags & CollisionFlags.Above) != 0 && playerVelocity.y > 0)
            {
                playerVelocity.y = -movementSettings.gravity * Time.deltaTime;
            }

            // walljumps
            if (!m_controller.isGrounded && hit.normal.y < 0.1f)
            {
                if (wishJump)
                { 
                    playerVelocity += hit.normal * movementSettings.jumpSpeed;
                    playerVelocity.y += movementSettings.WallJumpSpeed;
                }
            }   
        }

        public void jumpPad(float jumpForce, Vector3 normal)
        {
            playerVelocity = normal * jumpForce;
        }
        

        // -----------------
        //  Player actions
        // -----------------

        private void PlayerReSpawn()
        {
            transform.position = reSpawnPosition;
            rotX = 0.0f;
            rotY = reSpawnRotY;
            playerVelocity = Vector3.zero;
            
        }

        private void PlayerSpawn()
        {
            transform.position = spawnPosition;
            m_camera.transform.rotation = spawnRotation;
            rotX = 0.0f;
            rotY = spawnRotation.eulerAngles.y;
            timer = 0.0f;
            playerVelocity = Vector3.zero;

        }


        // -----------------
        //  Rocket jumps
        // -----------------
        public void RocketForce(float power, Vector3 origin, float radius)
        {
            Vector3 tossDirection;
            tossDirection = transform.position - origin;

            // -0.5 for the capsule radius
            float distance = tossDirection.magnitude - 0.5f; 
            float force = power / radius * (radius - distance);
            
            tossDirection.Normalize();
            playerVelocity += tossDirection * force;
        }

        // ----------------
        // Activate gun
        // ----------------
        public void activeGun()
        {
            GetComponent<Shoot>().enabled = true;
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }

        // ---------------
        // End of game
        // ---------------
        public void endGame()
        {
            timerOk = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame();
        }
    }
}   