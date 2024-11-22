using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;
    private float moveSpeed = 10f;
    private float cameraSpeed = 10f;
    private float jumpPower = 5f;
    private bool isJump = false;
    private GameObject previousSphere; // 前回生成した球体を保持する変数

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //前に進む
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * moveSpeed);
        }
        //後ろに進む
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * moveSpeed);
        }
        //左に進む
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * moveSpeed);
        }
        //右に進む
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * moveSpeed);
        }
        //ジャンプ
        if (Input.GetKey(KeyCode.Space) && !isJump)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }

        // Fキーを押した時に赤い球体をカメラの前に生成
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 前回生成した球体が存在する場合、それを削除
            if (previousSphere != null)
            {
                Destroy(previousSphere);
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = hit.point;
                    sphere.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    Renderer renderer = sphere.GetComponent<Renderer>();
                    renderer.material.color = Color.red;

                    // 新しく生成した球体を保持
                    previousSphere = sphere;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = Vector3.zero;
        }

        //カメラの回転
        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X") * cameraSpeed;
            float y = Input.GetAxis("Mouse Y") * cameraSpeed;
            if (Mathf.Abs(x) > 0)
            {
                transform.RotateAround(transform.position, Vector3.up, x);
            }
            if (Mathf.Abs(y) > 0)
            {
                transform.RotateAround(transform.position, transform.right, -y);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }
}