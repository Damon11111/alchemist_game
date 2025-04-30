using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
                isMoving = true;

                // 这里可以加角色转身（可选）
                Vector3 lookDir = (targetPosition - transform.position).normalized;
                lookDir.y = 0; // 保持水平旋转
                transform.forward = lookDir;
            }
        }

        // 移动角色
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 播放跑步动画
            if (animator != null)
                animator.SetBool("IsMove", true);

            // 到达目标点
            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                isMoving = false;

                // 播放待机动画
                if (animator != null)
                    animator.SetBool("IsMove", false);
            }
        }
        else
        {
            // 如果没有在动，播放待机动画
            if (animator != null)
                animator.SetBool("IsMove", false);
        }
    }
}
