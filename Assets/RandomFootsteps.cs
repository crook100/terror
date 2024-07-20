using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFootsteps : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 2.5f;

    [SerializeField]
    private float stepSize;
    private float stepNow = 0;

    GameBrain _brain;

    public Vector3 desired_position;

    // Start is called before the first frame update
    void Start()
    {
        _brain = Component.FindObjectOfType<GameBrain>();
        desired_position = transform.position;
        desired_position.x += Random.Range(-7, 7);
        desired_position.z += Random.Range(-7, 7);
    }

    // Update is called once per frame
    void Update()
    {
        if( Vector3.Distance(transform.position, desired_position) > 0.1 ) 
        {
            transform.position = Vector3.MoveTowards(transform.position, desired_position, moveSpeed * Time.deltaTime);
        }
        else { Destroy(gameObject); }

        if (stepNow < stepSize)
        {
            stepNow += Time.deltaTime;
        }
        else
        {
            stepNow = 0;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3))
            {
                Ground g = hitInfo.transform.GetComponent<Ground>();
                if (g != null)
                {
                    AudioClip[] footsteps = _brain.getFootstepsByGroundType(g.groundType);
                    _brain.PlaySoundAt(footsteps[Random.Range(0, footsteps.Length)], transform.position - new Vector3(0, transform.localScale.y, 0), new Vector2(0.75f, 1.25f), 0.1f);
                }
            }
        }

    }
}
