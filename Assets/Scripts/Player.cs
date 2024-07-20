using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float moveSpeed = 2.5f;

    [SerializeField]
    new Transform camera;

    [SerializeField]
    private float stepSize;
    private float stepNow = 0;

    GameBrain _brain;

    [SerializeField]
    private Vector2 cameraBobbingMinMax = new Vector2(-0.1f, 0.1f);

    private float cameraStart;

    bool cameraDown;

    Vector3 desiredCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _brain = Component.FindObjectOfType<GameBrain>();
        cameraDown = true;
        cameraStart = camera.localPosition.y;
        desiredCameraPos = new Vector3(camera.position.x, cameraStart + cameraBobbingMinMax.x, camera.position.z);
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 vel = transform.right * moveHorizontal * moveSpeed * Time.deltaTime + transform.forward * moveVertical * moveSpeed * Time.deltaTime;

        if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) != 0)
        {
            camera.localPosition = Vector3.MoveTowards(camera.localPosition, desiredCameraPos, (moveSpeed/200) * Time.deltaTime);
        }
        else {
            camera.localPosition = Vector3.MoveTowards(camera.localPosition, new Vector3(camera.localPosition.x, cameraStart, camera.localPosition.z), 0.5f * Time.deltaTime);
        }

        if (stepNow < stepSize)
        {
            stepNow += Mathf.Clamp01(Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical)) * (moveSpeed/100) * Time.deltaTime;

            if (stepNow >= stepSize / 2 && cameraDown)
            {
                cameraDown = false;
                desiredCameraPos.y = cameraStart + cameraBobbingMinMax.y;
            }
        }
        else {
            cameraDown = true;
            desiredCameraPos.y = cameraStart + cameraBobbingMinMax.x;

            stepNow = 0;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3))
            {
                Ground g = hitInfo.transform.GetComponent<Ground>();
                if (g != null)
                {
                    AudioClip[] footsteps = _brain.getFootstepsByGroundType(g.groundType);
                    _brain.PlaySoundAt(footsteps[Random.Range(0, footsteps.Length)], transform.position - new Vector3(0, transform.localScale.y, 0), new Vector2(0.75f, 1.25f), 0.2f);
                }
            }
        }

        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }
}
