using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    public Camera cam;
    public Collider2D placementArea;
    private GameObject previewObj;
    private GameObject selectedPrefab;
    private Button selectedButton;
    private Color defaultButtonColor;
    private Button selectedBtn;

    void Update()
    {
        // Jobb egér → megszakítjuk a kijelölést
        if (Input.GetMouseButtonDown(1))
        {
            CancelSelection();
            return;
        }

        if (previewObj == null) return;

        // Egér pozíció → világpozíció (2D-ben z=0)
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        previewObj.transform.position = mousePos;

        // Validátor lekérdezése
        PlacementValidator validator = previewObj.GetComponent<PlacementValidator>();

        // Lerakás bal egérrel
        if (validator != null && validator.IsValid && Input.GetMouseButtonDown(0))
        {
            // végleges tárgy létrehozása
            GameObject placed = Instantiate(selectedPrefab, previewObj.transform.position, Quaternion.identity);

            if (selectedBtn != null)
            {
                selectedBtn.interactable = false;
                selectedBtn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                placed.GetComponent<RightClickDelete>().SetSelectedButton(selectedBtn);
            }
            GetComponent<LevelManager>().CheckCollectables();


            // eredeti szín visszaállítása
            placed.GetComponent<PlacementValidator>()?.RestoreColor();
            placed.GetComponent<PlacementValidator>().SetPlayedArea(placementArea);

            placed.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Collider2D[] colliders = placed.GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = false;
            }
            CancelSelection(); // preview megszűnik
        }
    }

    public void SelectPrefab(GameObject prefab)
    {
        CancelSelection();

        selectedPrefab = prefab;
        previewObj = Instantiate(selectedPrefab);
        previewObj.GetComponent<PlacementValidator>().SetPlayedArea(placementArea);
        // UI gomb színkezelés
        selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (selectedButton != null)
        {
            defaultButtonColor = selectedButton.image.color;
            selectedButton.image.color = Color.green;
        }
    }

    public void SetSelectedButton(Button btn)
    {
        selectedBtn = btn;
    }

    private void CancelSelection()
    {
        if (previewObj != null) Destroy(previewObj);
        if (selectedButton != null)
        {
            selectedButton.image.color = defaultButtonColor;
            selectedButton = null;
        }
        selectedPrefab = null;
        previewObj = null;
    }

}