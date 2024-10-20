using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

namespace MG_Utilities
{
    public class CameraController : Singleton<CameraController>
    {
        #region SERIALIZABLE VARIABLES
        [Header("Camera Controller")]
        [Space(10)]

        [Header("Speed Settings")]
        [SerializeField] float moveSpeed;
        [Space(4)]
        [SerializeField] float edgeMoveModifier;
        [SerializeField] int edgeMoveZone;
        [Space(4)]
        [SerializeField] float panModifier;
        [SerializeField] float panRotationSpeed;
        [Space(4)]
        [SerializeField] float rotationSpeed;
        [Space(4)]
        [SerializeField] float zoomSpeed;

        [Header("Allowed Movements")]
        [Space(2)]
        [SerializeField] bool allowKeyboardMovement = true;
        [SerializeField] bool allowEdgeScrolling = false;
        [SerializeField] bool allowDragPan = true;
        [SerializeField] bool allowZoom = true;
        [SerializeField] bool allowRotation = true;
        [SerializeField] bool allowPanRotation = true;

        [Header("Zoom Options")]
        [Space(2)]
        [SerializeField] float followOffsetYMin = 2f;
        [SerializeField] float followOffsetYMax = 85f;
        [SerializeField] float zoomMinZ = -60f;
        
        [Header("Rotation Options")]
        [Space(2)]
        [SerializeField] float eulerRotationMin= 330;
        [SerializeField] float eulerRotationMax= 40;
        [SerializeField] float horizontalSpeedMultiplierRot = 3f;

        [Header("Unity Setup")]
        [Space(2)]
        [SerializeField] CinemachineCamera virtualCamera;
        [SerializeField] LayerMask groundLayerMask;
        [SerializeField] bool debug = true;
        [Space(2)]
        #endregion

        #region VARIABLES
        bool dragPanActive = false;
        bool rotationPanActive = false;
        Vector2 lastMousePosition = Vector2.zero;
        Vector2 lastMousePositionRotation = Vector2.zero;

        Vector3 followOffset;
        Vector3 zoomDirection;

        float rotateDirection = 0f;
        Vector2 keyboardMoveDirection = Vector2.zero;

        PlayerInputActions playerInputActions;
        Action<InputAction.CallbackContext> OnRotationPerformed;
        Action<InputAction.CallbackContext> OnRotationCancelled;
        Action<InputAction.CallbackContext> OnMovePerformed;
        Action<InputAction.CallbackContext> OnMoveCancelled;
        Action<InputAction.CallbackContext> OnDragPanStarted;
        Action<InputAction.CallbackContext> OnDragPanCancelled;
        Action<InputAction.CallbackContext> OnRotationPanStarted;
        Action<InputAction.CallbackContext> OnRotationPanCancelled;

        #endregion

        #region UNITY FUNCTIONS
        protected override void Awake()
        {
            base.Awake();
            followOffset = (virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow)
                .FollowOffset;
            
            zoomDirection = followOffset.normalized;

            playerInputActions = new PlayerInputActions();
            SetUpEventLambdas();
        }

        void OnEnable()
        {
            playerInputActions.Camera.Enable();

            playerInputActions.Camera.Rotate.performed += OnRotationPerformed;
            playerInputActions.Camera.Rotate.canceled += OnRotationCancelled;

            playerInputActions.Camera.Move.performed += OnMovePerformed;
            playerInputActions.Camera.Move.canceled += OnMoveCancelled;

            playerInputActions.Camera.DragPan.started += OnDragPanStarted;
            playerInputActions.Camera.DragPan.canceled += OnDragPanCancelled;

            playerInputActions.Camera.RotationPan.started += OnRotationPanStarted;
            playerInputActions.Camera.RotationPan.canceled += OnRotationPanCancelled;
        }

        void OnDisable()
        {
            playerInputActions.Camera.Rotate.performed -= OnRotationPerformed;
            playerInputActions.Camera.Rotate.canceled -= OnRotationCancelled;

            playerInputActions.Camera.Move.performed -= OnMovePerformed;
            playerInputActions.Camera.Move.canceled -= OnMoveCancelled;

            playerInputActions.Camera.DragPan.started -= OnDragPanStarted;
            playerInputActions.Camera.DragPan.canceled -= OnDragPanCancelled;
            playerInputActions.Camera.RotationPan.started -= OnRotationPanStarted;
            playerInputActions.Camera.RotationPan.canceled -= OnRotationPanCancelled;

            playerInputActions.Camera.Disable();
        }
        void Update()
        {
            HandleAllCameraMovement();
            HandleCameraRotation();
            HandlePanRotation();
            HandleCameraZoom();
        }

        #endregion

        #region CAMERA FUNCTIONS
        void HandleAllCameraMovement()
        {
            Vector3 moveDirection = Vector3.zero;

            if (allowKeyboardMovement)
                moveDirection += HandleCameraKeyboardMovement();
            if (allowEdgeScrolling)
                moveDirection += HandleEdgeScrolling();
            if (allowDragPan)
                moveDirection += HandleDragPan();

            transform.position += moveDirection * Time.deltaTime;
        }

        Vector3 HandleCameraKeyboardMovement()
        {
            Vector3 tempDir = ((transform.forward * keyboardMoveDirection.y) + (transform.right * keyboardMoveDirection.x)).normalized;
            tempDir *= moveSpeed;
            tempDir.y = 0;
            return tempDir;
        }
        Vector3 HandleEdgeScrolling()
        {
            if (dragPanActive) //to not move when dragging
                return Vector3.zero;

            Vector3 inputDirection = Vector3.zero;
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

            if (x <= edgeMoveZone && x >= 0) inputDirection.x = -1f;
            if (x >= Screen.width - edgeMoveZone && x <= Screen.width) inputDirection.x = 1f;
            if (y < edgeMoveZone && y >= 0) inputDirection.z = -1f;
            if (y >= Screen.height - edgeMoveZone && y <= Screen.height) inputDirection.z = 1f;

            Vector3 tempDir = ((transform.forward * inputDirection.z) + (transform.right * inputDirection.x)).normalized;
            return tempDir * (moveSpeed * edgeMoveModifier);
        }
        Vector3 HandleDragPan()
        {
            Vector2 mouseMovementDelta = Vector2.zero;

            if (dragPanActive)
            {
                mouseMovementDelta = lastMousePosition - (Vector2)Input.mousePosition; //to give correct direction

                lastMousePosition = Input.mousePosition;
            }

            Vector3 tempDir = ((transform.forward * mouseMovementDelta.y) + (transform.right * mouseMovementDelta.x)).normalized;
            tempDir = tempDir * (moveSpeed * panModifier);
            return tempDir.With(y: 0);
        }

        void HandleCameraRotation()
        {
            if (!allowRotation)
                return;

            transform.eulerAngles += Vector3.zero.With(y: rotateDirection * rotationSpeed * Time.deltaTime);
        }

        void HandlePanRotation()
        {
            if (!allowPanRotation)
                return;

            Vector2 mouseMovementRotationDelta = Vector2.zero;

            if (rotationPanActive)
            {
                mouseMovementRotationDelta = (Vector2)Input.mousePosition - lastMousePositionRotation;
                lastMousePositionRotation = Input.mousePosition;
            }

            //Handling X mouse axis rotation
            float xRotation = mouseMovementRotationDelta.normalized.x * panRotationSpeed * horizontalSpeedMultiplierRot * Time.deltaTime;

            //Handling Y mouse axis rotation
            float yRotation = mouseMovementRotationDelta.normalized.y * panRotationSpeed * Time.deltaTime;

            if(yRotation > 0 && transform.eulerAngles.x + yRotation > eulerRotationMax && transform.eulerAngles.x < eulerRotationMin)
            {
                yRotation = 0;
            }else if(yRotation < 0 && transform.eulerAngles.x + yRotation < eulerRotationMin && transform.eulerAngles.x > eulerRotationMax)
            {
                yRotation = 0;
            }            
            
            transform.eulerAngles += new Vector3(yRotation, xRotation, 0);
        }
        void HandleCameraZoom()
        {
            if (!allowZoom)
                return;

            zoomDirection = (virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow)
                .FollowOffset;

            if (zoomDirection.normalized == Vector3.zero)
            {
                if (debug)
                {
                    Debug.LogError("The zoom direction is Vector3.zero!");
                }
            }

            Vector3 cameraZoomVector = zoomDirection.normalized * (Time.deltaTime * zoomSpeed);

            if (Input.mouseScrollDelta.y > 0)
            {
                if (followOffset.y - cameraZoomVector.y <= followOffsetYMin)
                {
                    // If we go through the lower border, we instead clamp the vector magnitude to be equal to the border
                    float clampedY = Mathf.Abs(followOffset.y - followOffsetYMin);
                    float ratio = Mathf.Abs(clampedY / cameraZoomVector.y);
                    cameraZoomVector = new Vector3(cameraZoomVector.x * ratio, clampedY, cameraZoomVector.z * ratio);
                }

                followOffset -= cameraZoomVector;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                if (followOffset.y + cameraZoomVector.y >= followOffsetYMax)
                {
                    // Similarly, if we go through the upper border, we instead clamp the vector magnitude to be equal to the border
                    float clampedY = Mathf.Abs(followOffsetYMax - followOffset.y);
                    float ratio = Mathf.Abs(clampedY / cameraZoomVector.y);
                    cameraZoomVector = new Vector3(cameraZoomVector.x * ratio, clampedY, cameraZoomVector.z * ratio);
                }

                followOffset += cameraZoomVector;

                // simply limit Z axis zoom, rare edge case so don't worry about zoom deformation
                if (followOffset.z < zoomMinZ) { followOffset.z = zoomMinZ; }
            }

            if (followOffset != Vector3.zero)
            {
                (virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow)
                    .FollowOffset = followOffset;
            }

        }

        #endregion

        #region HELPER FUNCTIONS
        
        public Vector3 GetWorldMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,1000f,groundLayerMask))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        void SetUpEventLambdas()
        {
            OnRotationPerformed = context => rotateDirection = context.ReadValue<float>();
            OnRotationCancelled = context => rotateDirection = 0f;

            OnMovePerformed = context => keyboardMoveDirection = context.ReadValue<Vector2>();
            OnMoveCancelled = context => keyboardMoveDirection = Vector2.zero;

            OnDragPanStarted = context =>
            {
                dragPanActive = true;
                lastMousePosition = Input.mousePosition;
            };
            OnDragPanCancelled = context => dragPanActive = false;

            OnRotationPanStarted = context =>
            {
                rotationPanActive = true;
                lastMousePositionRotation = Input.mousePosition;
            };
            OnRotationPanCancelled = context => rotationPanActive = false;
        }

        #endregion
    }
}
