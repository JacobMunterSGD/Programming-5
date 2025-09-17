using UnityEngine;

public class PickupCubes : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit hit;

        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), out hit, Mathf.Infinity))
        //{
        //    print(hit);
        //    Destroy(hit.transform.gameObject);
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Destroy(hit.transform.gameObject);
        }

        //Camera.main.ScreenToWorldPoint(transform.position);
    }
}