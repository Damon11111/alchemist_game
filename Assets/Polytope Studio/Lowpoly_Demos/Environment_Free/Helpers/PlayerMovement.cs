using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public float speed = 6f;            // 前后移动速度
    public float rotationSpeed = 120f;  // 左右旋转速度
    public float gravity = -9.81f;      // 重力
    public float jumpHeight = 1.5f;     // 跳跃高度

    private Vector3 velocity; 
    private bool isGrounded; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. 检测是否着地
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            // 轻微压住角色贴地，避免一直触发isGrounded=false
            velocity.y = -2f; 
        }

        // 2. 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 左右方向键
        float vertical   = Input.GetAxis("Vertical");   // W/S 或 上下方向键

        // 3. 水平旋转：根据horizontal输入左右转向
        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

        // 4. 前后移动：角色面向的方向是 transform.forward
        Vector3 move = transform.forward * vertical * speed;

        // 5. 判断是否在移动（只检测vertical，也可改成检测move.magnitude > 0.1f）
        bool isMoving = Mathf.Abs(vertical) > 0.1f;

        // 6. 驱动Animator参数，让它播放Idle或Walk
        animator.SetBool("IsMove", isMoving);

        // 7. 用CharacterController移动
        controller.Move(move * Time.deltaTime);

        // 8. 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 9. 施加重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 测试日志
        Debug.Log("PlayerMovement is Running!");
    }
}
