using System;
using UnityEngine;

namespace RCRCoreLib.Core.Building
{
    public class ObjectDrag: MonoBehaviour
    {
        private Vector3 startPos;
        private float deltaX, deltaY;

        private void Start()
        {
            startPos = UnityEngine.Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(startPos);

            deltaX = startPos.x - transform.position.x;
            deltaY = startPos.y - transform.position.y;
        }

        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Vector3 pos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY);

            Vector3Int cellPos = BuildingSystem.Instance.gridLayout.WorldToCell(pos);
            transform.position = BuildingSystem.Instance.gridLayout.CellToLocalInterpolated(cellPos);
        }

        private void LateUpdate()
        {
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                gameObject.GetComponent<PlaceableObject>().CheckPlacement();
                Destroy(this);
            }
        }
    }
}