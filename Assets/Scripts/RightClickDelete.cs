using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RightClickDelete : MonoBehaviour, IPointerClickHandler
{
    [Header("A gomb, amit aktiválni kell")]
    
    private Button collectorBtn;


    void Update()
    {
        if (Input.GetMouseButtonDown(1) && collectorBtn != null) // jobb egérgomb
        {

            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            // Egyszerû 2D raycast
            RaycastHit2D[] hitList = Physics2D.RaycastAll(mousePos, Vector2.zero,0.1f);
            foreach(RaycastHit2D hit in hitList) {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    collectorBtn.interactable = true;
                    collectorBtn.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void SetSelectedButton(Button btn)
    {
        collectorBtn = btn;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked " + gameObject.name);
    }
}
