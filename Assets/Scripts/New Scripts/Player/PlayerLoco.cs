using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOT.Player
{
    public class PlayerLoco : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerAnimHandler animHandler;
        PlayerManager playerManager;
        CameraHandler cameraHandler;

        Transform cameraTransform;
        public Vector3 moveDirection;

        [HideInInspector] public Transform myTransform;
        public new Rigidbody rigidbody;
        public Transform normalCamera;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 5;
        [SerializeField] float sprintSpeed = 8;
        [SerializeField] float walkSpeed = 5;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float fallingSpeed = 15;


        [Header("Grounded and inAir Stats")]
        [SerializeField] float groundDetectionRayStartPoint = .5f;
        [SerializeField] float minimumDistanceNeedToStartFall = 1f;
        [SerializeField] float groundDirectionRayDistance = .2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTime = 0;


        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            animHandler = GetComponent<PlayerAnimHandler>();
            rigidbody = GetComponent<Rigidbody>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = CameraHandler.Instance;
            animHandler.Initialize();
            cameraTransform = Camera.main.transform;
            myTransform = transform;

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement       
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotationInput(float delta)
        {
            if (inputHandler.lockOnFlag)
            {
                if (inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDir = Vector3.zero;
                    targetDir = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDir += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDir.y = 0;
                    targetDir.Normalize();

                    if (targetDir == Vector3.zero)
                    {
                        targetDir = myTransform.forward;
                    }
                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion rotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * delta);
                    transform.rotation = rotation;
                }
                else
                {
                    Vector3 rotationDir = moveDirection;
                    rotationDir = cameraHandler.currentLockOnTarget.position - transform.position;
                    rotationDir.y = 0;
                    rotationDir.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDir);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * delta);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = inputHandler.moveAmount;
                targetDir = cameraTransform.forward * inputHandler.vertical;
                targetDir += cameraTransform.right * inputHandler.horizontal;
                targetDir.y = 0;
                targetDir.Normalize();

                if (targetDir == Vector3.zero)
                {
                    targetDir = myTransform.forward;
                }
                float rs = rotationSpeed;
                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion rotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);
                transform.rotation = rotation;
            }

        }

        public void HandleMovementInput(float delta)
        {

            if (inputHandler.rollFlag) return;
            if (playerManager.isInteracting) return;


            moveDirection = cameraTransform.forward * inputHandler.vertical;
            moveDirection += cameraTransform.right * inputHandler.horizontal;

            moveDirection.y = 0;
            moveDirection.Normalize();
            float speed = movementSpeed;

            if (inputHandler.moveAmount > 0.5f && inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= walkSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }

            }

            Vector3 projectVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectVelocity;

            if (inputHandler.lockOnFlag && !inputHandler.sprintFlag)
            {
                animHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                animHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }


            if (animHandler.canRotate)
            {
                HandleRotationInput(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animHandler.anim.GetBool(AnimHash.Interacting)) return;

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraTransform.forward * inputHandler.vertical;
                moveDirection += cameraTransform.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animHandler.PlayTargetAnimation(AnimHash.block, true);
                    moveDirection.y = 0;

                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    animHandler.PlayTargetAnimation(AnimHash.BackStab, true);
                }

            }

        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;

            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            Debug.DrawLine(origin, myTransform.forward * 0.5f, Color.black, 0.1f, false);
            if (Physics.Raycast(origin, myTransform.forward, out hit, .4f))
            {
                //Debug.Log("hit");
                moveDirection = Vector3.zero;
            }
            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);

                rigidbody.AddForce(moveDirection * fallingSpeed / 15f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + (dir * groundDirectionRayDistance);


            targetPosition = myTransform.position;


            Debug.DrawLine(origin, origin + Vector3.down * minimumDistanceNeedToStartFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, Vector3.down, out hit, minimumDistanceNeedToStartFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTime > 0.5f)
                    {
                        animHandler.PlayTargetAnimation(AnimHash.land, true);
                        inAirTime = 0;
                    }
                    else
                    {
                        animHandler.PlayTargetAnimation(AnimHash.None, false);
                        inAirTime = 0;
                    }
                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }
                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animHandler.PlayTargetAnimation(AnimHash.jump, true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }


        }

        #endregion

    }
}
