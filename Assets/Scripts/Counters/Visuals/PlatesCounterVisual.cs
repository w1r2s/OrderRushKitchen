using System.Collections.Generic;
using UnityEngine;

namespace OrderRushKitchen.Counters
{

    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private Transform CounterTopPoint;
        [SerializeField] private Transform PlateVisualPrefab;

        [SerializeField] private PlatesCounter platesCounter;

        private List<GameObject> plateVisualGameObjectList;

        private void Awake()
        {
            plateVisualGameObjectList = new List<GameObject>();
        }
        private void Start()
        {
            platesCounter.onPlateSpawned += PlatesCounter_onPlateSpawned;
            platesCounter.onPlateRemoved += PlatesCounter_onPlateRemoved;
        }

        private void PlatesCounter_onPlateRemoved(object sender, System.EventArgs e)
        {
            GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
            plateVisualGameObjectList.Remove(plateGameObject);
            Destroy(plateGameObject);
        }

        private void PlatesCounter_onPlateSpawned(object sender, System.EventArgs e)
        {
           Transform PlateVisualTransform = Instantiate(PlateVisualPrefab, CounterTopPoint);

            float plateOffsetY = 0.1f;
            PlateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

            plateVisualGameObjectList.Add(PlateVisualTransform.gameObject);
        }
    }

}
