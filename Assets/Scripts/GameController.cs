using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Material whiteMaterial;
    public Material grayMaterial;
    public Material blueMaterial;
    public Material redMaterial;
    public Material yellowMaterial;
    public Material greenMaterial;
    public Material purpleMaterial;
    public Material cyanMaterial;
    public Material staticMaterial;

    public PhysicsMaterial2D purpleBouncy;

    public bool redShift = true;
    public bool greenShift = true;
    public bool blueShift = true;

    IDictionary<string, float> standardBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };
    IDictionary<string, float> blueBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 35},
                        {"gravity", -0.6f},
                        {"drag", 0.1f}
                    };
    IDictionary<string, float> greenBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 0},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };
    IDictionary<string, float> redBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 1},
                        {"gravity", 0},
                        {"drag", 0.1f}
                    };
    IDictionary<string, float> yellowBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 5},
                        {"gravity", 0},
                        {"drag", 1000}
                    };
    IDictionary<string, float> cyanBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 9},
                        {"pickup?", 1},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };
    IDictionary<string, float> purpleBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };

    private GameObject[] whiteBlocks;
    private GameObject[] blueBlocks;
    private GameObject[] greenBlocks;
    private GameObject[] redBlocks;
    private GameObject[] yellowBlocks;
    private GameObject[] cyanBlocks;
    private GameObject[] purpleBlocks;

    // Start is called before the first frame update
    void Start()
    {
        whiteBlocks = GameObject.FindGameObjectsWithTag("WhiteBlockChild");
        blueBlocks = GameObject.FindGameObjectsWithTag("BlueBlockChild");
        greenBlocks = GameObject.FindGameObjectsWithTag("GreenBlockChild");
        redBlocks = GameObject.FindGameObjectsWithTag("RedBlockChild");
        yellowBlocks = GameObject.FindGameObjectsWithTag("YellowBlockChild");
        cyanBlocks = GameObject.FindGameObjectsWithTag("CyanBlockChild");
        purpleBlocks = GameObject.FindGameObjectsWithTag("PurpleBlockChild");
    }

    void shift(GameObject[] blocks, IDictionary<string, float> blockBlueprint, Material newMaterial, PhysicsMaterial2D physicsMaterial = null){
        foreach(GameObject blockChild in blocks){
            var block = blockChild.transform.parent.gameObject;
            var rb = block.GetComponent<Rigidbody2D>();

            //color
            block.GetComponent<MeshRenderer>().material = newMaterial;
            //layer
            block.layer = (int)blockBlueprint["layer"];
            //pickup
            if (blockBlueprint["pickup?"] == 1){
                block.tag = "pickup";
            }
            else {block.tag = "Untagged";
            }
            //optional physics material
            block.GetComponent<BoxCollider2D>().sharedMaterial = physicsMaterial;
            //physics settings
            rb.mass = blockBlueprint["mass"];
            rb.gravityScale = blockBlueprint["gravity"];
            rb.drag = blockBlueprint["drag"];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Red shift
        if(Input.GetButtonDown("RedShift") && redShift){

            var red = new Color(1, 0.6f, 0.6f);
            Camera.main.backgroundColor = red;

            //white to red
            shift(whiteBlocks, redBlockDict, redMaterial);
            //blue to purple
            shift(blueBlocks, purpleBlockDict, purpleMaterial, purpleBouncy);
            //green to yellow
            shift(greenBlocks, yellowBlockDict, yellowMaterial);
            //red to red
            shift(redBlocks, redBlockDict, redMaterial);
            //cyan to gray
            shift(cyanBlocks, standardBlockDict, grayMaterial);
            //purple to purple
            shift(purpleBlocks, purpleBlockDict, purpleMaterial, purpleBouncy);
            //yellow to yellow
            shift(yellowBlocks, yellowBlockDict, yellowMaterial);
        }
        //Green shift
        if(Input.GetButtonDown("GreenShift") && greenShift){

            var green = new Color(0.6f, 1, 0.6f);
            Camera.main.backgroundColor = green;

            //white to green
            shift(whiteBlocks, greenBlockDict, greenMaterial);
            //red to yellow
            shift(redBlocks, yellowBlockDict, yellowMaterial);
            //blue to cyan
            shift(blueBlocks, cyanBlockDict, cyanMaterial);
            //green to green
            shift(greenBlocks, greenBlockDict, greenMaterial);
            //purple to gray
            shift(purpleBlocks, standardBlockDict, grayMaterial);
            //yellow to yellow
            shift(yellowBlocks, yellowBlockDict, yellowMaterial);
            //cyan to cyan
            shift(cyanBlocks, cyanBlockDict, cyanMaterial);
        }
        //Blue Shift
        if(Input.GetButtonDown("BlueShift") && blueShift){

            var blue = new Color(0.4f, 0.6f, 1);
            Camera.main.backgroundColor = blue;

            //white to blue
            shift(whiteBlocks, blueBlockDict, blueMaterial);
            //red to purple
            shift(redBlocks, purpleBlockDict, purpleMaterial, purpleBouncy);
            //green to cyan
            shift(greenBlocks, cyanBlockDict, cyanMaterial);
            //blue to blue
            shift(blueBlocks, blueBlockDict, blueMaterial);
            //yellow to gray
            shift(yellowBlocks, standardBlockDict, grayMaterial);
            //cyan to cyan
            shift(cyanBlocks, cyanBlockDict, cyanMaterial);
            //purple to purple
            shift(purpleBlocks, purpleBlockDict, purpleMaterial, purpleBouncy);
        }
        //Generic unshift 
        if(Input.GetButtonDown("UnShift")){

            var normalBackground = new Color(0.8f, 0.8f, 0.8f);
            Camera.main.backgroundColor = normalBackground;

            shift(whiteBlocks, standardBlockDict, whiteMaterial);
            shift(redBlocks, redBlockDict, redMaterial);
            shift(blueBlocks, blueBlockDict, blueMaterial);
            shift(greenBlocks, greenBlockDict, greenMaterial);
            shift(yellowBlocks, yellowBlockDict, yellowMaterial);
            shift(cyanBlocks, cyanBlockDict, cyanMaterial);
            shift(purpleBlocks, purpleBlockDict, purpleMaterial, purpleBouncy);
        }
    }
}
