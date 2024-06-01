using UnityEngine;
using Utils;

namespace Models.PlanetListUtils
{
    using System;
    using UnityEngine.Events;
    using System.Collections.Generic;
    
    public static class PlanetListDictionaries
    {
        private const string DecimalPlacesForPlanetCoordinates = "N2";
        private const float NoMass = 0f;
        
        public static Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>> 
            GetVariablePropertiesDictionary(GameObject currentPlanetModel)
        {
            return new Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>>
            {
                {
                    "Planet Mass",
                    new TwoObjectContainer<Func<string>, UnityAction<string>>(
                        () => currentPlanetModel.GetComponent<CelestialBody>().mass
                            .ToString(DecimalPlacesForPlanetCoordinates) + "_Earth masses",
                            
                        updatedData =>
                        {
                            var updatedMass = float.Parse(updatedData);

                            if (updatedMass > NoMass)
                            {
                                currentPlanetModel.GetComponent<CelestialBody>().mass = updatedMass;

                                Debug.Log("Changed mass to " + updatedMass);
                            }
                        })
                },
                {
                    "Planet X-Position", 
                    new TwoObjectContainer<Func<string>, UnityAction<string>>(
                        () => currentPlanetModel.transform.position.x
                            .ToString(DecimalPlacesForPlanetCoordinates),
                        updatedData =>
                        { 
                            var updatedX = float.Parse(updatedData);

                            var currentPlanetPosition = currentPlanetModel.transform.position;
                            var currentPlanetRotation = currentPlanetModel.transform.rotation;

                            currentPlanetPosition.x = updatedX;

                            currentPlanetModel.transform.SetPositionAndRotation(currentPlanetPosition, currentPlanetRotation);
                                
                            Debug.Log("Changed x-coordinate to " + updatedX);
                        })
                },
                {
                    "Planet Y-Position", 
                    new TwoObjectContainer<Func<string>, UnityAction<string>>(
                        () => currentPlanetModel.transform.position.y
                            .ToString(DecimalPlacesForPlanetCoordinates),
                        updatedData =>
                        { 
                            var updatedY = float.Parse(updatedData);

                            var currentPlanetPosition = currentPlanetModel.transform.position;
                            var currentPlanetRotation = currentPlanetModel.transform.rotation;

                            currentPlanetPosition.y = updatedY;

                            currentPlanetModel.transform.SetPositionAndRotation(currentPlanetPosition, currentPlanetRotation);
                                
                            Debug.Log("Changed y-coordinate to " + updatedY);
                        })
                },
                {
                    "Planet Z-Position", 
                    new TwoObjectContainer<Func<string>, UnityAction<string>>(
                        () => currentPlanetModel.transform.position.z
                            .ToString(DecimalPlacesForPlanetCoordinates),
                        updatedData =>
                        { 
                            var updatedZ = float.Parse(updatedData);

                            var currentPlanetPosition = currentPlanetModel.transform.position;
                            var currentPlanetRotation = currentPlanetModel.transform.rotation;

                            currentPlanetPosition.z = updatedZ;

                            currentPlanetModel.transform.SetPositionAndRotation(currentPlanetPosition, currentPlanetRotation);
                                
                            Debug.Log("Changed z-coordinate to " + updatedZ);
                        })
                }
            };
        }
    }
}