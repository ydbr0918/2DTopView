using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) target = go.transform;
            else return; // 아직 Player가 없다면 리턴
        }
        Vector3 targetPos = target.position;
        targetPos.z = -10f;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

}
