using UnityEngine;

public class PlacementValidator : MonoBehaviour
{
    public Collider2D placementArea; // inspectorban be�ll�that� vitrin collider
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    private SpriteRenderer sr;
    private Color originalColor;
    private Collider2D[] myCol2Ds;
    private bool isTriggered = false;
    private bool checkColor = true;
    public bool IsValid { get; private set; } = false;

    private int colNum = 0;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        myCol2Ds = GetComponents<Collider2D>();
    }

    void Update()
    {
        if (checkColor)
        {
            if (!isTriggered)
            {
                IsValid = InsideCol();
                sr.color = IsValid ? validColor : invalidColor;
            }
            else
            {
                IsValid = false;
                sr.color = invalidColor;
            }
        }
        else
        {
            IsValid = InsideCol();
            sr.color = IsValid ? originalColor : invalidColor;
            
        }
    }

    public void RestoreColor()
    {
        sr.color = originalColor;
        checkColor = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Collectable"))
        {
            isTriggered = true;
            colNum++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Collectable"))
        {
            colNum--;
            if (colNum == 0)
            {
                isTriggered = false;
            }
        }
    }
    // -------------------------
    // ✅ BELÜL VAN-E A PLACEMENT AREA-BAN?
    // -------------------------
    private bool InsideCol()
    {
        if (placementArea == null)
            return true;

        foreach (Collider2D myCol2D in myCol2Ds)
        {
            // mintavételi pontok a collider sarkainál és középen
            Vector2[] testPoints = new Vector2[]
            {
                myCol2D.bounds.min,
                myCol2D.bounds.max,
                new Vector2(myCol2D.bounds.min.x, myCol2D.bounds.max.y),
                new Vector2(myCol2D.bounds.max.x, myCol2D.bounds.min.y),
                myCol2D.bounds.center
            };

            foreach (Vector2 p in testPoints)
            {
                if (!IsPointInsidePlacement(p))
                    return false; // ha bármelyik pont kívül esik, invalid
            }
        }

        return true;
    }

    // -------------------------
    // ✅ PONT ELLENŐRZÉSE A PLACEMENT AREA-BAN
    // -------------------------
    private bool IsPointInsidePlacement(Vector2 point)
    {
        if (placementArea is EdgeCollider2D edge)
        {
            return IsPointInsideEdgeCollider(point, edge);
        }
        else if (placementArea is PolygonCollider2D poly)
        {
            return poly.OverlapPoint(point);
        }
        else
        {
            return placementArea.bounds.Contains(point);
        }
    }

    // -------------------------
    // 🧠 POINT IN POLYGON (EdgeCollider2D)
    // -------------------------
    private bool IsPointInsideEdgeCollider(Vector2 point, EdgeCollider2D edge)
    {
        Vector2[] pts = edge.points;
        int count = pts.Length;
        if (count < 3) return false;

        int intersections = 0;

        for (int i = 0; i < count; i++)
        {
            Vector2 a = edge.transform.TransformPoint(pts[i]);
            Vector2 b = edge.transform.TransformPoint(pts[(i + 1) % count]);

            if (((a.y > point.y) != (b.y > point.y)) &&
                (point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x))
            {
                intersections++;
            }
        }

        return (intersections % 2) != 0;
    }

    public void SetPlayedArea(Collider2D placementArea)
    {
        this.placementArea = placementArea;
    }


}
