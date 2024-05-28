using System.Collections;
using TMPro;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// Handles the Planet List animation on button click
    /// </summary>
    public class PlanetListAnimator : MonoBehaviour
    {
        private Transform _planetListContainerTransform;
        private bool _listIsOpen;

        [SerializeField] private Vector3 moveDistance;

        [SerializeField] private float moveSpeed = 1.0f;


        public TextMeshProUGUI buttonText;

        /// <summary>
        /// Getter / Setter methods for the Transform that will contain the Planet List Items
        /// </summary>
        public Transform PlanetListContainerTransform { private get; set; }

        /// <summary>
        /// Getter / Setter methods to read the List Is Open boolean outside of this Class
        /// </summary>
        public bool ListIsOpen { get; private set; }

        /// <summary>
        /// Called on Object Instantiation, sets the PlanetListContainerTransform to the Transform of the current Object
        /// </summary>
        private void Start()
        {
            PlanetListContainerTransform = transform;
        }

        /// <summary>
        /// Called on button click, starts a new Coroutine where the list will be moved to the new position
        /// </summary>
        public void OnArrowClick()
        {
            Debug.Log("Arrow clicked");
            if (ListIsOpen)
            {
                StartCoroutine(MovePanel(moveDistance));
                ListIsOpen = false;
                buttonText.text = "↑";
            }
            else
            {
                StartCoroutine(MovePanel(-moveDistance));
                ListIsOpen = true;
                buttonText.text = "↓";
            }
        }

        /// <summary>
        /// Creates a nice moving animation for the Planet List Container using multi threading
        /// </summary>
        /// 
        /// <param name="vectorToTarget">
        /// The vector pointing towards the target.
        /// Vector length should be equal to the distance between the points
        /// </param>
        /// 
        /// <returns>
        /// An IEnumerator, which allows for this method to be called in parallel
        /// </returns>
        private IEnumerator MovePanel(Vector3 vectorToTarget)
        {
            var startPosition = PlanetListContainerTransform.position;
            var endPosition = startPosition + vectorToTarget;

            var currentTime = 0.0f;

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime * moveSpeed;
                PlanetListContainerTransform.position = Vector3.Lerp(startPosition, endPosition, currentTime);
                yield return null;
            }

            PlanetListContainerTransform.position = endPosition;
        }
    }
}