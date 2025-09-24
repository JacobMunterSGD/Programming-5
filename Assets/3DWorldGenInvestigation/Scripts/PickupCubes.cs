using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PickupCubes : MonoBehaviour
{

    [Header("Runtime Variables")]
    [SerializeField] List<GameObject> selection = new();

    [SerializeField] bool hasSelection;
    [SerializeField] Vector3 averageSelectionPosition;

    Vector3 lastMousePos;

    [Header("Parameters")] // variables to change how it's used - come up with better name later
	[SerializeField] int selectedRaiseHeight;
    [SerializeField] float minMouseMovement;

    [SerializeField] float selectionRadius;

    [SerializeField] LayerMask selectionLayer;

    // TEMP
    Transform currentRaycastHitSpot;

	private void Start()
	{
		hasSelection = false;
	}

	void Update()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

			GameObject hitGameObject = hit.transform.gameObject;

            // select new geometry
			if (Vector3.Distance(lastMousePos, Input.mousePosition) > minMouseMovement)
            {
                if (!hasSelection)
                {
					lastMousePos = Input.mousePosition;
					HighlightSelection(hitGameObject);
				}
                else
                {
                    MoveSelection(hitGameObject.transform.position);
				}
			}

			if (Input.GetMouseButtonDown(0))
            {
                if (selection.Count <= 0) return;

                if (hasSelection) PlaceSelection();
                else GrabSelection();
			}
            
		}
    }

    void GrabSelection()
    {
        hasSelection = true;

		foreach (GameObject GO in selection)
        {
            GO.transform.position += new Vector3(0, selectedRaiseHeight, 0);
        }
    }

    void PlaceSelection()
    {
		foreach (GameObject GO in selection)
		{
			RaycastHit _hit;
			float yValue = GO.transform.position.y;
			if (Physics.Raycast(GO.transform.position, Vector3.down, out _hit, Mathf.Infinity, selectionLayer))
			{
				yValue = _hit.transform.position.y + 1;
			}

			GO.transform.position = new Vector3(GO.transform.position.x, yValue, GO.transform.position.z);

		}

		hasSelection = false;
	}

    void MoveSelection(Vector3 _moveToSpot)
    {
        averageSelectionPosition = GetAverageSelectionPosition();

		Vector3 moveBy = _moveToSpot - averageSelectionPosition;
        moveBy.y = 0;

        foreach (GameObject GO in selection)
        {
			RaycastHit _hit;
            float yValue = GO.transform.position.y;
			if (Physics.Raycast(GO.transform.position, Vector3.down, out _hit, Mathf.Infinity, selectionLayer))
			{
				yValue = _hit.transform.position.y + selectedRaiseHeight;
			}

			GO.transform.position += moveBy;
            GO.transform.position = new Vector3(GO.transform.position.x, yValue, GO.transform.position.z);
            
		}
    }

    void HighlightSelection(GameObject newCube)
    {
        if (selection.Count >= 0)
        {
            foreach (GameObject GO in selection)
            {
                ChangeHighlight(false, GO);
			}

            selection.Clear();
        }

        // add cubes to list based on the grab radius (right now just add one cube)
        RaycastHit[] allSpheres = Physics.SphereCastAll(newCube.transform.position, selectionRadius, Vector3.forward, selectionRadius, selectionLayer);

        currentRaycastHitSpot = newCube.transform;

		foreach (RaycastHit hit in allSpheres)
		{
			GameObject _tempCube = hit.transform.gameObject;
			selection.Add(_tempCube);
			ChangeHighlight(true, _tempCube);
		}

		averageSelectionPosition = GetAverageSelectionPosition();
	}

    Vector3 GetAverageSelectionPosition()
    {
        Vector3 _tempAveragePos = Vector3.zero;

		foreach (GameObject GO in selection)
        {
            _tempAveragePos += GO.transform.position;
		}

		_tempAveragePos /= selection.Count;

        return _tempAveragePos;
	}

    void ChangeHighlight(bool isBeingHighlighted, GameObject _gameObject)
    {
        Color originalColour = _gameObject.GetComponent<CubeInfoGameobject>().cubeInfo.Colour;

        MeshRenderer mr = _gameObject.GetComponent<MeshRenderer>();

		if (isBeingHighlighted)
        {
            mr.material.color = new Color(originalColour.r + .2f, originalColour.g + .2f, originalColour.b + .2f);
		}
        else
        {
            mr.material.color = originalColour;
		}

    }

}