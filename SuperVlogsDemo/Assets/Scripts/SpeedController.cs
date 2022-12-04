using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public Transform[] points;
    public float speed = 1f;
    [Range(1, 32)]
    public int sampleRate = 16;

    public AudioClip clip1;

    [System.Serializable]
    class SamplePoint//This creates our conceptual table of segments.
    {//This takes in our potential positions in the spline and distance between them when lerping.
        public float samplePosition;
        public float accumulatedDistance;

        public SamplePoint(float samplePosition, float distanceCovered)
        {
            this.samplePosition = samplePosition;
            this.accumulatedDistance = distanceCovered;
        }
    }

    List<List<SamplePoint>> table = new List<List<SamplePoint>>();

    float distance = 0f;
    float accumDistance = 0f;
    int currentIndex = 0;
    int currentSample = 0;



    // Start is called before the first frame update
    void Start()
    {
        
        //make sure there are 4 points, else disable the component
        if (points.Length < 4)
        {
            enabled = false;
        }

        int size = points.Length;

        //calculate the speed graph table
        Vector3 prevPos = points[0].position;
        for (int i = 0; i < size; ++i)
        {//This uses a segment object within our list as rows in the SamplePoint table. 
            List<SamplePoint> segment = new List<SamplePoint>();
            Vector3 p0 = points[(i - 1 + points.Length) % points.Length].position;//Each 4 points in the spline are added to
                                                                                  //the list of points.
                Vector3 p1 = points[i].position;
                Vector3 p2 = points[(i + 1) % points.Length].position;
                Vector3 p3 = points[(i + 2) % points.Length].position;

            //calculate samples
            segment.Add(new SamplePoint(0f, accumDistance));
            Vector3 previousPos = CatMullEquation(0, p0, p1, p2, p3);//To call each previous position.
            for (int sample = 1; sample <= sampleRate; ++sample)
            {
                //This creates each sample and stores them in individual segments.
                float t = (float)sample / sampleRate; //This is interpolation.
                Vector3 currentPos = CatMullEquation(t, p0, p1, p2, p3);//This shows the current position using the catmull-rom eq'n.
                Vector3 resultant = currentPos - previousPos;
                accumDistance += resultant.magnitude;//This calculates the movement progress and distance between each position the
                segment.Add(new SamplePoint(t, accumDistance));//object lerps on and then adds it to the table.
                previousPos = currentPos;
            }
            table.Add(segment);//The main purpose of this loop is to add a new row in the table for each 4 points' accumulated distance and 
        }                      //length between previous and current positions of our object on the spline.
    }

    // Update is called once per frame
    void Update()
    {
        distance += speed * Time.deltaTime;

        //check if we need to update our samples
        if (distance > table[currentIndex][currentSample + 1].accumulatedDistance)
        {
            //This updates sample and index indices
            //With a default rate, if current sample + 1 > sample rate, increment our currentindex, if the interval changes, reset the distance.
            //This increases the sample index by one, and if it goes beyond the range of the current list then increase the segment index and
            //reset sample index.
            if (currentSample >= sampleRate - 1)
            {
                if (currentIndex < table.Count - 1)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex = 0;
                    distance = 0;
                }
                Debug.Log(currentIndex);

                currentSample = 0;
            }
            else 
            {
                currentSample++;
            }
        }

        Vector3 p0 = points[(currentIndex - 1 + points.Length) % points.Length].position;//Within the points list, each point has their
        Vector3 p1 = points[currentIndex].position;                                      //own corresponding use of currentIndex to be applied
                                                                                         //to the process we have above in our if statement. 
        Vector3 p2 = points[(currentIndex + 1) % points.Length].position;
        Vector3 p3 = points[(currentIndex + 2) % points.Length].position;

        transform.position = CatMullEquation(GetAdjustedT(), p0, p1, p2, p3);//We have our object's position moving in accordance
                                                                             //to the equation with lerping t value and points.
    }

    public float GetAdjustedT()//This is a generic t value that can be manipulated in our Start method when calculating the object's
                               //lerping progress in terms of movement from a previous position to the current one and adding those values
                               //to our table as a new row.
    {                          
        SamplePoint current = table[currentIndex][currentSample];
        SamplePoint next = table[currentIndex][currentSample + 1];

        return Mathf.Lerp(current.samplePosition, next.samplePosition,
            (distance - current.accumulatedDistance) / (next.accumulatedDistance - current.accumulatedDistance)
        );
    }

    public Vector3 CatMullEquation(float currentPosition, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
    {//This is the equation we are using to create the curvature movement of our car.
        Vector3 q = 2f * point2;
        Vector3 w = point3 - point1;
        Vector3 e = 2f * point1 - 5f * point2 + 4f * point3 - point4;
        Vector3 r = -point1 + 3f * point2 - 3f * point3 + point4;

        Vector3 equation = 0.5f * (q + (w * currentPosition) + (e * currentPosition * currentPosition)
            + (r * currentPosition * currentPosition * currentPosition));

        return equation;
    }

    private void OnTriggerEnter(Collider other)//These are just some speed boosters the player can interact with as they move.
    {
        if (other.gameObject.name == "speedboost")
        {
            speed = 70;
            
        }
        if (other.gameObject.name == "speedboost3")
        {
            //transform.Rotate(0, 0, 180);
            GetComponent<Transform>().eulerAngles = new Vector3(-90, 0, 180);
        }
        if (other.gameObject.name == "speedboost4")
        {
            speed = 10;
            GetComponent<Transform>().eulerAngles = new Vector3(-90, 0, 90);
        }
        if (other.gameObject.name == "speedboost2")
        {
            speed = 30;
            GetComponent<Transform>().eulerAngles = new Vector3(-90, 0, 0);
        }
    }
}
