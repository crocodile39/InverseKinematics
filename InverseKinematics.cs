using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    [SerializeField] Transform pivotOne;
    [SerializeField] Transform pivotTwo;

    [SerializeField] Transform pivotThree;
    [SerializeField] Transform pivotFour;

    [SerializeField] float legLength;
    [SerializeField] float bodyHeight;


    private float x;

    public bool findPos;
    public bool findPos2;
    public GameObject posObject;
    public GameObject posObject2;
    private Vector2 footTarget;
    private Vector2 footTarget2;

    private bool animate;


    private bool returningOne;
    private bool returningTwo;

    private Vector2 lastPos;
    [SerializeField] LayerMask floorLayer;

    // Start is called before the first frame update
    void Start()
    {
        float y2 = bodyHeight * bodyHeight;
        float h2 = legLength * legLength;

        h2 *= h2;

        float x2 = h2 - y2;
        x = Mathf.Sqrt(x2);

        FindFootPos();
        StartCoroutine(WaitSecondLeg());
    }

    IEnumerator WaitSecondLeg()
    {
        yield return new WaitForSeconds(1);
        FindFootPos2();
        animate = true;
    }

    // Update is called once per frame
    void Update()
    {
       

        float c = Vector2.Distance(transform.position, footTarget);
        float cS = c * c;
        float a = legLength;
        float aS = a * a;
        float b = legLength;
        float bS = b * b;
        float top = (cS) - (aS) - (bS);
        float topBottom = top / (2 * a * b);
        float radians = Mathf.Acos(topBottom);
        float C = radians * Mathf.Rad2Deg;



        pivotTwo.localEulerAngles = new(0, 0, C);


        top = (a * a) - (b * b) - (c * c);
        topBottom = top / (2 * b * c);
        radians = Mathf.Acos(topBottom);
        float A = radians * Mathf.Rad2Deg;
        float theta = Mathf.Asin(bodyHeight / c);
        theta *= Mathf.Rad2Deg;
        theta += A;
        pivotOne.localEulerAngles = new(0, 0, theta);


        if (animate)
        {
            float c2 = Vector2.Distance(transform.position, footTarget2);
            float cS2 = c2 * c2;
            float top2 = (cS2) - (aS) - (bS);
            float topBottom2 = top2 / (2 * a * b);
            float radians2 = Mathf.Acos(topBottom2);
            float C2 = radians2 * Mathf.Rad2Deg;
            pivotFour.localEulerAngles = new(0, 0, C2);


            top2 = (a * a) - (b * b) - (c2 * c2);
            topBottom2 = top2 / (2 * b * c2);
            radians2 = Mathf.Acos(topBottom2);
            float A2 = radians2 * Mathf.Rad2Deg;
            float theta2 = Mathf.Asin(bodyHeight / c2);
            theta2 *= Mathf.Rad2Deg;
            theta2 += A2;
            pivotThree.localEulerAngles = new(0, 0, theta2);
        }


        float speed = Mathf.Abs(transform.position.x - lastPos.x);


        if (Vector2.Distance(transform.position, footTarget) > 1.5f * legLength)
        {
            if (returningOne)
            {
                FindFootPos();
                returningOne = false;
            }
            else
            {
                returningOne = true;
            }
        }
        if (returningOne)
        {
            footTarget.x += speed * 2;
        }
        Debug.Log(returningOne);

        if (Vector2.Distance(transform.position, footTarget2) > 1.5f * legLength)
        {
            if (returningTwo)
            {
                FindFootPos2();
                returningTwo = false;
            }
            else
            {
                returningTwo = true;
            }
        }
        if (returningTwo)
        {
            footTarget2.x += speed * 2;
        }

        lastPos = transform.position;
    }

    void FindFootPos()
    {
        findPos = false;

        Vector2 pos = new(transform.position.x + (x * .75f), transform.position.y - bodyHeight);
        Vector2 thisPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, pos - thisPosition, Mathf.Infinity, floorLayer);

        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            transform.position = new(transform.position.x, hit.point.y + bodyHeight);

            footTarget = hit.point;

            Instantiate(posObject, footTarget, Quaternion.identity);
        }
        else
        {
            footTarget = pos;

            Instantiate(posObject, pos, Quaternion.identity);
        }
        //footTarget = pos;
        //Instantiate(posObject, pos, Quaternion.identity);

    }

    void FindFootPos2()
    {
        findPos2 = false;

        Vector2 pos2 = new(transform.position.x + (x * .75f), transform.position.y - bodyHeight);
        footTarget2 = pos2;
        Instantiate(posObject2, pos2, Quaternion.identity);
    }

}
