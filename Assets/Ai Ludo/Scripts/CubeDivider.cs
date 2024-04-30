using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeDivider : MonoBehaviour
{
    public GameObject cubePrefab; // Reference to the cube prefab
    private void Start()
    {
        DivideIntoTwoCubes(cubePrefab.transform.position);
    }
    public void DivideIntoTwoCubes(Vector3 originalPosition)
    {
        // Calculate the position for the first cube
        Vector3 firstCubePosition = originalPosition + new Vector3(-.3f, 0f, 0f);

        // Calculate the position for the second cube
        Vector3 secondCubePosition = originalPosition + new Vector3(.3f, 0f, 0f);

        // Create the first cube
        GameObject firstCube = Instantiate(cubePrefab, firstCubePosition, Quaternion.identity);

        // Create the second cube
        GameObject secondCube = Instantiate(cubePrefab, secondCubePosition, Quaternion.identity);

        // Optionally, you can set other properties of the cubes such as scale, rotation, etc.
        // For example:
        firstCube.transform.localScale = new Vector3(cubePrefab.transform.localScale.x/2f, cubePrefab.transform.localScale.y / 2f, cubePrefab.transform.localScale.z / 2f);
        secondCube.transform.localScale = new Vector3(cubePrefab.transform.localScale.x/2f, cubePrefab.transform.localScale.y / 2f, cubePrefab.transform.localScale.z / 2f);
        //secondCube.transform.localScale = new Vector3(.5f, .5f, .5f);
        //Vector3.Distance(firstCube.transform.position, secondCube.transform.position);
        //Debug.LogError(Vector3.Distance(firstCube.transform.position, secondCube.transform.position));
       // secondCube.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}