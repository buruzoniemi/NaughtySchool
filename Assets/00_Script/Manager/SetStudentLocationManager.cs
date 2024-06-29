using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStudentLocationManager : MonoBehaviour
{
    [SerializeField] Vector3[] Location;
    [SerializeField] Vector3[] Scale;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 設定
    public void SetStudentLocation(int playerID, SetStudentLocation StudentObject)
    {
        StudentObject.SetLocation(Location[playerID]);
        StudentObject.SetScale(Scale[playerID]);
    }
}
