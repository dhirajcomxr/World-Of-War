using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOT.Player;
using WOT.Core;

namespace WOT
{
    public class CameraHandler : Singleton<CameraHandler>
    {
        private Transform myTransform;
        private InputHandler myInputHandler;
        private PlayerManager playerManager;

        private Vector3 camFollowVelocity = Vector3.zero;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayer;
        private LayerMask enviromentLayer;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.1f;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockPivotPosition = 2.25f;
        public float unlockedPivotPosition = 2.25f;
        [Space(5), Header("Lock Character Data")]


        public Transform nearestLockOnTarget;
        public Transform currentLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;

        public float lockOnArea = 25;
        public float maximumLockOnDistance = 30;

        List<CharacterManager> availableTargets = new List<CharacterManager>();


        [System.Obsolete]
        private void Awake()
        {
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayer = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            myInputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }
        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Environment");
        }
        public void followTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref camFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (!myInputHandler.lockOnFlag && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;

                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                Quaternion lerpRotation =
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                float velocity = 0;
                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        public void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - myTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayer))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }
            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            float shortDistance = Mathf.Infinity;
            // float shortestDistanceOfLeftTarget = Mathf.Infinity;
            // float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] collider = Physics.OverlapSphere(targetTransform.position, lockOnArea);
            for (int i = 0; i < collider.Length; i++)
            {
                CharacterManager character = collider[i].GetComponent<CharacterManager>();
                if (character != null)
                {
                    RaycastHit hit;
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    if (character.transform.root != targetTransform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);
                            if (hit.transform.gameObject.layer == enviromentLayer)
                            {
                                Debug.Log("This is not Layer");
                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }

                    }
                }
            }
            for (int i = 0; i < availableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);
                if (distanceFromTarget < shortDistance)
                {
                    shortDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[i].lockOnTransform;
                }
                //Tutorial Number = 25(P2)
                /* if (myInputHandler.lockOnFlag)
                 {
                     Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[i].transform.position);
                     var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[i].transform.position.x;
                     var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[i].transform.position.x;

                     if(relativeEnemyPosition.x > 0 && distanceFromLeftTarget< shortestDistanceOfLeftTarget)
                     {
                         shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                         leftLockTarget = availableTargets[i].lockOnTransform;
                     }
                     if (relativeEnemyPosition.x < 0 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                     {
                         shortestDistanceOfRightTarget = distanceFromRightTarget;
                         rightLockTarget = availableTargets[i].lockOnTransform;
                     }
                 }*/
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}