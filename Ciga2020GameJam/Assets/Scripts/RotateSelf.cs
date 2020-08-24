using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        // this.transform.Rotate(this.transform.up, Time.deltaTime * 30f);
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 30, 0) * Time.deltaTime;
    }
}
