using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject[] collectablesList;
    public Collider2D placementArea;
    public Animator anim;
    public Color originalColor;
    [Header("A szöveg, amit frissítünk")]
    public TMP_Text countdownText;
    [Header("Mettõl kezdõdjön a visszaszámlálás")]
    public int startCount = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectablesList = GameObject.FindGameObjectsWithTag("UICollectables");
    }

    public void CheckCollectables()
    {
        foreach (GameObject collectable in collectablesList)
        {
            if (collectable.active == true)
            {
                return;
            }
        }
        GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (GameObject collectableObject in collectableObjects)
        {
            collectableObject.GetComponent<RightClickDelete>().enabled = false;
        }


            StartCoroutine(CountdownCoroutine());

    }


    private IEnumerator CountdownCoroutine()
    {
        int count = startCount;
        bool isWin = true;

        while (count > 0)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();

            yield return new WaitForSeconds(1f);
            count--;
        }
        GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (GameObject collectableObject in collectableObjects)
        {
            Color color = collectableObject.GetComponent<SpriteRenderer>().color;
            if (color.r != originalColor.r && color.g != originalColor.b && color.b != originalColor.b)
            {
                Debug.Log("original: " + originalColor.r + " - " + originalColor.g + " - " + originalColor.b + " - " + originalColor.a);

                Debug.Log(color.r + " - " + color.g + " - " + color.b + " - " + color.a);
                isWin = false ;
                collectableObject.GetComponent<RightClickDelete>().enabled = true;
            }
        }
        if (countdownText != null)
            countdownText.text = "0";

        if (isWin)
        {
            anim.Play("levelEndAnimUp");
            Debug.Log("you win!");
        }
        // 0-t is kiírhatunk, ha szeretnénk
    }





}
