using System;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

namespace MixedReality.Toolkit.Input
{
    public class EyeTracker : MonoBehaviour
    {
        [SerializeField]
        private GazeInteractor gazeInteractor;

        [SerializeField]
        private GameObject hitPointDisplayPrefab;

        private GameObject hitPointDisplayer;

        [SerializeField]
        private GameObject objectOfInterest;

        private StreamWriter trackerData;

        private void Awake()
        {
            var trackerDataPath = Path.Combine(Application.persistentDataPath, "eyetracking.csv");
            trackerData = new StreamWriter(trackerDataPath);
            trackerData.AutoFlush = true;
        }
        // Start is called before the first frame update
        void Start()
        {
            hitPointDisplayer = Instantiate(hitPointDisplayPrefab);
        }

        // Update is called once per frame
        void Update()
        {
            var ray = new Ray(gazeInteractor.rayOriginTransform.position,
                gazeInteractor.rayOriginTransform.forward * 3);
            if (Physics.Raycast(ray, out var hit))
            {

                if (hit.collider.gameObject == objectOfInterest)
                {
                    hitPointDisplayer.transform.position = hit.point;
                    WriteTrackingPoint(hit.point);
                }

            }
        }

        private void WriteTrackingPoint(Vector3 hitPoint)
        {
            var relativePoint =
                objectOfInterest.transform.position - hitPoint;
            trackerData.WriteLine(FormattableString.Invariant(
                $"{relativePoint.x}, {relativePoint.y}, {relativePoint.z}"));
        }

        private void OnDestroy() { trackerData.Close(); }
    }
}
