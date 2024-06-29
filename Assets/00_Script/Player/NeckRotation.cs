using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckRotation : MonoBehaviour
{
    // 令和のクソスクリプトSeason2
    public void EnableRotation()
    {
        transform.rotation = Quaternion.Euler(30.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    public void DisableRotation()
    {
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
