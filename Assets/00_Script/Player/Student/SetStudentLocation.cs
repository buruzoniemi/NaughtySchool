using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetStudentLocation : MonoBehaviour
{
    [SerializeField] Vector3 location;
    [SerializeField] Vector3 scale;
    [SerializeField] PlayerInput playerInput;
    private int userID;
    private GameObject parent;
    private SetStudentLocationManager manager;
    // Start is called before the first frame update
    void Start()
    {
        //userID = playerInput.user.index;
        parent = this.gameObject;
        parent.transform.rotation = Quaternion.Euler(0, 180, 0);
        //manager = GameObject.Find("StudentManager").GetComponent<SetStudentLocationManager>();

        //manager.SetStudentLocation(userID,this);
        //parent.transform.position = location;
        //parent.transform.localScale = scale;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLocation(Vector3 location)
    {
        parent.transform.position = location;
        parent.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // 仮マップ用の一時的なやつ
    public void SetScale(Vector3 scale)
    {
        parent.transform.localScale = scale;
    }

}
