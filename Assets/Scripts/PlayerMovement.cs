    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class PlayerMovement : MonoBehaviour
    {
    
        public MoveJoystick movementJoystick;
        public float playerSpeed = 5f;
        public float playerJump = 1500f;
        private Rigidbody2D rb;
        private float char_scale = 0.4f;
        Animator myAnimator;
        [SerializeField] 
        public GameObject jumpButton;
        public bool isGrounded;
        CapsuleCollider2D myCapsuleCollider;
        private bool jumpPressed = false;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            myCapsuleCollider = GetComponent<CapsuleCollider2D>();

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(movementJoystick.joystickVec.y != 0)
            {
                if (movementJoystick.joystickVec.x < 0)
                {
                    rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * 0);
                    transform.localScale = new Vector2(-char_scale, char_scale);
                    myAnimator.SetBool("isRunning", true);
                }
                else if (movementJoystick.joystickVec.x > 0)
                {
                    rb.velocity = new Vector2(Mathf.Abs(movementJoystick.joystickVec.x) * playerSpeed, movementJoystick.joystickVec.y * 0);
                    transform.localScale = new Vector2((char_scale), char_scale);
                    myAnimator.SetBool("isRunning", true);
                }
                
            }
            else
            {
                rb.velocity = Vector2.zero;
                myAnimator.SetBool("isRunning", false);
            }

            if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")) && jumpPressed)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerJump);
                //rb.AddForce(Vector2.up * playerJump, ForceMode2D.Impulse);
                jumpPressed = false;
                isGrounded = false;
                myAnimator.SetBool("isJumping", true);
            } 

            if (rb.velocity.y < -0.1f)
            {
                myAnimator.SetBool("isFallingDown", true);
                myAnimator.SetBool("isJumping", false);
            }
            else
            {
                myAnimator.SetBool("isFallingDown", false);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.gameObject.tag == "Platforms")
            {
                isGrounded = true;
                myAnimator.SetBool("isJumping", false);
            }
        }

        public void Jump()
        {   
            if (!jumpPressed)
                jumpPressed = true;
            
        }
    }