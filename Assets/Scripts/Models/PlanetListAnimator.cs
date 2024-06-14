using TMPro;
using UnityEngine;
using System.Collections;

namespace Models
{
    /// <summary>
    /// Handles the Planet List animation on button click
    /// </summary>
    public class PlanetListAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform planetListContainerTransform;
        [SerializeField] private RectTransform planetListScrollViewTransform;
        
        [SerializeField] private Vector3 moveOffset;
        [SerializeField] private float moveSpeed = 1.0f;
        
        [SerializeField] private float widthRatioCorrection = .1f;
        
        private bool _listIsOpen;
        private bool _initialised;
        
        public TextMeshProUGUI buttonText;
        private Vector3 _moveDistanceImproved;

        /// <summary>
        /// Getter / Setter methods for the Transform that will contain the Planet List Items
        /// </summary>
        public RectTransform PlanetListContainerTransform { private get; set; }

        /// <summary>
        /// Getter / Setter methods for the Transform that will contain the Planet List Items
        /// </summary>
        private Vector3 MoveOffset { get; set; }

        /// <summary>
        /// Getter / Setter methods for the Transform that will contain the Planet List Items
        /// </summary>
        private float MoveSpeed { get; set; }

        /// <summary>
        /// Getter / Setter methods to read the List Is Open boolean outside of this Class
        /// </summary>
        public bool ListIsOpen { get; private set; }

        
        /// <summary>
        /// Called on Object Instantiation, sets the PlanetListContainerTransform to the Transform of the current Object
        /// </summary>
        private void Start()
        {
            PlanetListContainerTransform = planetListContainerTransform;
            MoveOffset = moveOffset;
            MoveSpeed = moveSpeed;
            ListIsOpen = _listIsOpen;
        }

        
        /// <summary>
        /// Called every frame, sets the PlanetListContainerTransform and the transform height variables, as soon as
        /// the transform is not null anymore
        /// </summary>
        private void Update()
        {
            if (PlanetListContainerTransform)
            {
                _moveDistanceImproved = MoveOffset;
                _moveDistanceImproved.y += planetListScrollViewTransform.rect.height * planetListScrollViewTransform.lossyScale.y;
            }
        }

        /// <summary>
        /// Called on button click, starts a new Coroutine where the list will be moved to the new position
        /// </summary>
        public void OnArrowClick()
        {
            Debug.Log(planetListScrollViewTransform.rect.height);
            if (ListIsOpen)
            {
                StartCoroutine(MovePanel(-_moveDistanceImproved));
                ListIsOpen = false;
                buttonText.text = "↑";
            }
            else
            {
                StartCoroutine(MovePanel(_moveDistanceImproved));
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
                currentTime += Time.deltaTime * MoveSpeed;
                PlanetListContainerTransform.position = Vector3.Lerp(startPosition, endPosition, currentTime);
                yield return null;
            }

            PlanetListContainerTransform.position = endPosition;
        }
    }
}